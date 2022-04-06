using System;
using System.Net;
using System.Net.Sockets;
using Unosquare.PiGpio;

namespace CommandServer;

public class Program
{
    static int Main(string[] args)
    {
        ControllerCommand capcom = new ControllerCommand(0);
        
        //Setup connection
        Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Controller pad = new Controller(0);
        
        if(args.Length != 0)
            connection.Connect(IPAddress.Parse(args[0]), 8080);
        
        else
            connection.Connect(IPAddress.Parse("192.168.37.190"), 8080);
            
        
        //connection.Send(capcom.DriveCommandSequence(CommandProtocol.SystemFunction.Initialize));
        //connection.Send(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Initialize));
        int val = 0, inc = 1;
        byte[] data = new byte[10], pull = new byte[4];
        int count = 0, result;
        while (true)
        {
            count++;
            
            //Wait for pull request from server
            result = 0;
            while(result < 4)
                result += connection.Receive(pull, 0, 4 - result, SocketFlags.None);
            
            data = capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Update);
            Console.WriteLine($"{count, 6}: {CommandProtocol.ByteToHex(data)}");

            /*
            if (val + inc is > 45 or < -45)
                inc *= -1;

            val += inc;
            
            
            //Console.ReadLine();
            Console.WriteLine($"Val: {val}, Inc: {inc}");

            connection.Send(CommandProtocol.GenerateCommandSequence(
                CommandProtocol.Subsystem.Payload1, 
                CommandProtocol.SystemFunction.Update,
                new byte[] {(byte)(90 + val), (byte)(90 - val)}
                ));
                */
            connection.Send(data);
        }
        
        //connection.Send(capcom.DriveCommandSequence(CommandProtocol.SystemFunction.Kill));
        //connection.Send(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Kill));
        connection.Close();
    }
}