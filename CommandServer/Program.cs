// See https://aka.ms/new-console-template for more information

using System;
using System.Net;
using System.Net.Sockets;
using System.IO.Ports;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using Unosquare.PiGpio;
using Unosquare.PiGpio.NativeEnums;
using Unosquare.PiGpio.NativeMethods;

class Program
{ 
    static int Main()
    {
        Console.WriteLine(Setup.GpioInitialise());
        UIntPtr arduino = Uart.SerOpen("/dev/ttyACM0", UartRate.BaudRate19200);

        int port = 8080;
        Console.WriteLine("Starting Server...");
        IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
        Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        listener.Bind(ipep);
        listener.Blocking = true;

        listener.Listen();
        Console.WriteLine("Server Started; Awaiting Clients...");
        Socket client = listener.Accept();
        Console.WriteLine($"Client Connected\nStarting Commands...");
        IO.GpioSetMode(SystemGpio.Bcm06, PinMode.Output);

        byte[] command = new byte[8];
        while (true)
        {
            command[0] = 0;
            while (command[0] != 255)
                client.Receive(command, 1, SocketFlags.None);
            
            //recieve header
            int result = 1;
            while (result != 4)
                result += client.Receive(command, 1, 4 - result, SocketFlags.None);

            result = 0;
            while (result < command[3])
                result += client.Receive(command, 4, command[3] - result, SocketFlags.None);

            if (command[1] == (byte) CommandProtocol.Subsystem.Core &&
                command[2] == (byte) CommandProtocol.SystemFunction.Kill)
            {
                if (command[3] == 0)
                    break;
                
                //TODO: determine what to do with kill arguments
                break;
            }
            
            Console.WriteLine(CommandProtocol.ByteToHex(command));
            
            //send command to arduino
            Uart.SerWrite(arduino, command, (uint)(command[3] + 4));

            /*
            result = 0;
            byte[] echo = new byte[20];
            int echoLen = command[3] + 4 + "ECHO: \n".Length;

            while (result < echoLen)
                result += Uart.SerRead(arduino, echo, (uint) echoLen);
                */
        }
        
        Setup.GpioTerminate();
        return 0;
    }
}