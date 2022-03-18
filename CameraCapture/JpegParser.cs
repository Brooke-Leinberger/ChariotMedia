using System.Text.RegularExpressions;
namespace CameraCapture;


public class JpegParser
{
    private byte[] image;
    private byte[] marker;

    public JpegParser(byte[] image)
    {
        marker = new byte[4];
        this.image = image;
    }
    
    public void parse_marker(int index)
    {
        /*
         FFD8 : SOI 
         FFD9 : EOI
         FF00 : <someshit>
         FF01 : <somemoreshit>
         FFD0 - FFD8 : <evenmoreshit>
        */
        
        for (int i = 0; i < 4; i++)
            marker[i] = image[index + i];

        //start of image
        string hex = Convert.ToHexString(marker);
        if (Regex.IsMatch(hex, "(FF)(00|01|D[0-9]).{4}"))
        {
            //These markers don't have length; skip past 
            marker[2] = 0;
            marker[3] = 0;
        }
    }

    public int[] get_resolution()
    {
        return new int[] {256 * image[9] + image[10], 256 * image[7] + image[8]};
    }

    private int header_length()
    {
        //parse through header, until Start of Scan
        int index = 0;
        parse_marker(index);
        if (Convert.ToHexString(marker) != "FFD80000")
            return -1; //Not a valid jpeg

        index += 2;


        while (true)
        {
            string hex_debug = Convert.ToString(index, 16);
            parse_marker(index);
            if (marker[0] != 255)
                return -2; //should be a marker
            if (marker[1] == 218)
                return index;
            index += 256 * marker[2] + marker[3] + 2; //offset by length signifier + 2
        }
    }

    private int scan_length(int header_length)
    {
        for (int i = header_length; i < image.Length; i++)
        {
            if (image[i] == 255 && image[i + 1] == 217)
                return i + 2;
        }

        return -1;
    }
    
    public int find_length()
    {
        return scan_length(header_length());
    }

}