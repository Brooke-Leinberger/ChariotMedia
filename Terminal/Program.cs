// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Net;
using System.Net.Sockets;


class Program
{
    static int Clamp(int value, int upperBound, int lowerBound)
    {
        if (value < lowerBound)
            return lowerBound;
        if (value > upperBound)
            return upperBound;

        return value;
    }

    static void Main()
    {

        Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //Server.Connect(IPAddress.Parse("192.168.58.169"), 7777);
        Server.Connect(IPAddress.Parse("127.0.0.1"), 7777);

        int maxSize = 1843200;
        string path = "/home/juno/transfer_test.jpg";
        byte[] handshake = new byte[3];

        for (int num = 0; num < 5; num++)
        {

            Server.Receive(handshake, 0, 3, SocketFlags.None);
            byte[] dataBuffer = new byte[256 * 256 * handshake[2] + 256 * handshake[1] + handshake[0]];
            Console.WriteLine("New File");
            
            int gros = 0;
            for (int count = 0; count < dataBuffer.Length; count += gros)
            {
                gros = Server.Receive(dataBuffer, count, dataBuffer.Length - count, SocketFlags.None);
                Console.WriteLine(count);
            }

            using FileStream fs = new FileStream($"/home/juno/transfers/test{num}.jpg", FileMode.Create);
            fs.Write(dataBuffer, 0, dataBuffer.Length);
            fs.Flush();
            fs.Dispose();
        }
    }
}