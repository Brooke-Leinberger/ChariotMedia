// See https://aka.ms/new-console-template for more information

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
        Server.Connect(IPAddress.Parse("192.168.58.169"), 7777);
//Server.Connect(IPAddress.Parse("127.0.0.1"), 7777);

        string path = "/home/juno/transfer_test.jpg";

        byte[] dataBuffer = new byte[1024];

        int fileSize = 1843200;

        for (int num = 0; num < 60; num++)
        {
            using FileStream fs = new FileStream($"/home/juno/transfers/test{num}.jpg", FileMode.Create);
            Console.WriteLine("New File");
            for (int count = 0; count < fileSize;)
            {
                int gros = Server.Receive(dataBuffer, 0, Clamp(fileSize - count, 1024, 0), SocketFlags.None);
                //Console.WriteLine(gros);
                fs.Write(dataBuffer, 0, gros);
                fs.Flush();
                count += gros;
                Console.WriteLine(count);
            }

            fs.Dispose();
        }
    }
}