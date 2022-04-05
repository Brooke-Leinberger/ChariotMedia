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
    static int Main(string[] args)
    {
        string uart = "/dev/ttyACM0";
        if (args.Length > 0)
            uart = args[0];
            
        SerialPort arduino = new SerialPort(uart, 9600, Parity.Odd, 8, StopBits.One);
        arduino.Open();

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

        byte[] command = new byte[8];
        while (true)
        {
            command[0] = 0;
            while (command[0] != 255)
                client.Receive(command, 1, SocketFlags.None);
            
            //recieve header
            int result = 1;
            while (result != CommandProtocol.HEADER_SIZE)
                result += client.Receive(command, 1, CommandProtocol.HEADER_SIZE - result, SocketFlags.None);

            result = 0;
            while (result < command[3])
                result += client.Receive(command, CommandProtocol.HEADER_SIZE, command[3] - result, SocketFlags.None);

            if (command[1] == (byte) CommandProtocol.Subsystem.Core &&
                command[2] == (byte) CommandProtocol.SystemFunction.Kill)
            {
                if (command[3] == 0)
                    break;
                
                //TODO: determine what to do with kill arguments
                break;
            }
            
            //check parity
            Console.WriteLine(CommandProtocol.ByteToHex(command));
            if(command[4] != command[1] + command[2] + command[3])
                continue;

            int parity = 0;
            for (int i = 0; i < command[3]; i++)
                parity += (int) command[i + CommandProtocol.HEADER_SIZE];

            if (command[5] != parity % 255)
                continue;

            Console.WriteLine("WRITE");
            //send command to arduino
            arduino.Write(command, 0, command[3] + CommandProtocol.HEADER_SIZE);

        }
        
        arduino.Close();
        return 0;
    }
}