// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;

Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Server.Connect(IPAddress.Parse("129.21.74.166"), 7777);

string path = "/home/juno/transfer_test.jpg";

byte[] dataBuffer = new byte[1024];


for (int num = 0; ; num++)
{
    using FileStream fs = new FileStream($"/home/juno/transfers/test{num}.jpg", FileMode.Create);
    for (int count = 0; count < 1843789;)
    {
        Server.Receive(dataBuffer);
        fs.Write(dataBuffer, 0, dataBuffer.Length);
        fs.Flush();
        count += dataBuffer.Length;
    }
    fs.Dispose();
}