// See https://aka.ms/new-console-template for more information

using Unosquare.PiGpio.NativeEnums;
using Unosquare.PiGpio.NativeMethods;


class CommandProtocol
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

    private static char DecimalToHex(int val)
    {
        if (val > 15 || val < 0)
            return '\0';

        int inc = 48;
        if (val > 10)
            inc = 55;

        return (char) (val + inc);
    }
    

    private static string ByteToHex(byte val)
    {
        int left  = val >> 4; //extract left  4 bits with bitshift
        int right = val & 15; //extract right 4 bits by and-ing val with 00001111 (15)

        return new string(new char[] {DecimalToHex(left), DecimalToHex(right)});
    }

    private static string ByteToHex(byte[] arr)
    {
        string result = "";
        foreach (byte val in arr)
            result += ByteToHex(val);

        return result;
    }

    static byte[] GenerateCommandSequence(UIntPtr handle, Subsystem sys, SystemFunction func, double[] values)
    {
        //setup buffer and header
        byte[] buffer = new byte[values.Length + 4];
        buffer[0] = 255;
        buffer[1] = (byte) sys;
        buffer[2] = (byte) func;
        buffer[3] = (byte) buffer.Length;
        
        //copy values into write buffer
        for (int i = 0; i < values.Length; i++)
            buffer[i + 4] = (byte)values[i];

        return buffer;
    }
    
}
