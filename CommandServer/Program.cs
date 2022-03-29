// See https://aka.ms/new-console-template for more information

using Unosquare.PiGpio.NativeEnums;
using Unosquare.PiGpio.NativeMethods;


class Program
{
    /*
     * Reserved commands:
     * 255: Start
     * 254: Kill
     */
    static void SendCommandSequence(UIntPtr handle, double[] values)
    {
        //setup buffer and header
        byte[] buffer = new byte[values.Length + 3];
        buffer[0] = 255;
        buffer[1] = (byte) buffer.Length;
        buffer[2] = 1;
        
        //copy values into write buffer
        for (int i = 0; i < values.Length; i++)
            buffer[i + 3] = (byte)values[i];
        
        //write buffer
        Spi.SpiWrite(handle, buffer);
    }

    static void SendKillSequence(UIntPtr handle) => Spi.SpiWrite(handle, new byte[] {254});

        static int Main()
    {
        Console.WriteLine("Hello, World!");
        UIntPtr comm = Spi.SpiOpen(SpiChannelId.SpiChannel0, 2000000, SpiFlags.Default);

        while (true)
        {
            string input = Console.ReadLine();
            if (input == null)
                return -1; //ensure input is not null

            if (input.ToLower() == "exit")
                break;

            double[] args = input.Split(',').Select(c => Double.Parse(c)).ToArray();
            SendCommandSequence(comm, args);
        }

        Spi.SpiClose(comm);
        return 0;
    }
}
