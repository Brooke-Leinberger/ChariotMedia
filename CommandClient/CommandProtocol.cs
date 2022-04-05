// See https://aka.ms/new-console-template for more information

using Unosquare.PiGpio.NativeEnums;
using Unosquare.PiGpio.NativeMethods;


public class CommandProtocol
{
    /*
     * NOTE: RESERVED: 255 (START COMMAND)
     * MULTI-BYTE PARAMETERS MUST TAKE INTO ACCOUNT THERE ARE ONLY 255 OPTIONS, NOT 256
     */

    public enum Subsystem
    {
        Core = 0,
        Drive = 1,
        Payload1 = 2,
        Payload2 = 3,
        Payload3 = 4,
        Payload4 = 5,
    }

    public enum SystemFunction
    {
        Initialize = 0, 
        Kill = 1,
        Update = 2
    }

    public const int HEADER_SIZE = 6;

    private static char DecimalToHex(int val)
    {
        if (val > 15 || val < 0)
            return '\0';

        int inc = 48;
        if (val > 9)
            inc = 55;

        return (char) (val + inc);
    }
    

    private static string ByteToHex(byte val)
    {
        int left  = val >> 4; //extract left  4 bits with bitshift
        int right = val & 15; //extract right 4 bits by and-ing val with 00001111 (15)

        return new string(new char[] {DecimalToHex(left), DecimalToHex(right)});
    }

    public static string ByteToHex(byte[] arr)
    {
        string result = "";
        foreach (byte val in arr)
            result += ByteToHex(val);

        return result;
    }

    public static byte[] GenerateCommandSequence(Subsystem sys, SystemFunction func, byte[] values)
    {
        //setup buffer and header
        byte[] buffer = new byte[values.Length + 6];
        buffer[0] = 255;
        buffer[1] = (byte) sys;
        buffer[2] = (byte) func;
        buffer[3] = (byte) values.Length;
        buffer[4] = (byte) ((int)sys + (int)func + values.Length);
        
        
        //copy values into write buffer
        int parity = 0;
        for (int i = 0; i < values.Length; i++)
        {
            buffer[i + HEADER_SIZE] = values[i];
            parity += values[i];
        }

        buffer[5] = (byte)(parity % 255);

        return buffer;
    }
    
}
