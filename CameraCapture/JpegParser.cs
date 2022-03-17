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
            //These markers don't have length
            marker[2] = 0;
            marker[3] = 0;
        }
    }
    
    public int find_length()
    {
        //parse through header, until Start of Scan
        int index = 0;
        parse_marker(index);
        index += 2;
        if (Convert.ToHexString(marker) != "FFD80000")
            return -1; //Not a valid jpeg
        


        for (; Convert.ToHexString(marker).Substring(0, 4) != "FFDA" ; parse_marker(index))
        {
            parse_marker(index);
            if (Convert.ToHexString(marker).Substring(0, 2) != "FF")
                return -2; //should be a marker
            index += 16 * marker[2] + marker[3] + 2; //offset by length signifier + 2
        }
        
        Console.WriteLine($"SOS: {Convert.ToString(index, 16)}");
        //brute force until end of file

        return -100;
    }

}