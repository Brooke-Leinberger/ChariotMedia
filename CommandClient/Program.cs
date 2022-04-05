using System;
using System.Net;
using System.Net.Sockets;
using Unosquare.PiGpio;

namespace CommandServer;

public class Program
{
    static int Main(string[] args)
    {
        //ControllerCommand capcom = new ControllerCommand(0);
        
        //Setup connection
        Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Controller pad = new Controller(0);
        
        if(args.Length != 0)
            connection.Connect(IPAddress.Parse(args[0]), 8080);
        
        else
            connection.Connect(IPAddress.Parse("192.168.37.147"), 8080);
            
        
        //connection.Send(capcom.DriveCommandSequence(CommandProtocol.SystemFunction.Initialize));
        //connection.Send(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Initialize));

        while (true)
        {
            //Console.WriteLine(CommandProtocol.ByteToHex(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Update)));
            //connection.Send(capcom.DriveCommandSequence(CommandProtocol.SystemFunction.Update));
            
            //connection.Send(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Update));
            connection.Send(new byte[] {255, 2, 2, 2, 90, 90});
        }
        
        //connection.Send(capcom.DriveCommandSequence(CommandProtocol.SystemFunction.Kill));
        //connection.Send(capcom.VisorCommandSequence(CommandProtocol.SystemFunction.Kill));
        connection.Close();
    }
}