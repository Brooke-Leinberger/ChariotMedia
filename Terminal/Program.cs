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
        //Server.Connect(IPAddress.Parse("192.168.58.169"), 7777);
        Server.Connect(IPAddress.Parse("127.0.0.1"), 7777);

        string path = "/home/juno/transfer_test.jpg";

        byte[] dataBuffer = new byte[1024];

        int file_size = 128000; //MAKE SURE THIS NUMBER MATCHES THE CLIENT

        for (int num = 0; num < 60; num++)
        {
            using FileStream fs = new FileStream($"/home/juno/transfers/test{num}.jpg", FileMode.Create);
            Console.WriteLine("New File");
            int count1 = 0;
            for (int count = 0; count < 125; count++)
            {
                int gros = Server.Receive(dataBuffer, 0, 1024, SocketFlags.None);
                fs.Write(dataBuffer, 0, gros);
                fs.Flush();
                count1 += gros;
            }

            fs.Dispose();
        }
    }
}