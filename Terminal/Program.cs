// See https://aka.ms/new-console-template for more information

using System.Net;
using System.Net.Sockets;

Socket Server = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
Server.Connect(IPAddress.Loopback, 7777);

string path = "/home/juno/transfer_test.jpg";

byte[] dataBuffer = new byte[1024];

using FileStream fs = new FileStream(path, FileMode.Create);

for(int count = 0; count < 1843789;)
{
    Server.Receive(dataBuffer);
    fs.Write(dataBuffer, 0, dataBuffer.Length);
    fs.Flush();
    count += dataBuffer.Length;
}