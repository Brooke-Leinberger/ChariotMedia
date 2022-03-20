// See https://aka.ms/new-console-template for more information

using System.Collections;
using System.Net;
using System.Net.Sockets;
using SFML.Window;
using SFML.Graphics;


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

        Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        connection.Connect(IPAddress.Parse("192.168.1.10"), 7777);
        //Server.Connect(IPAddress.Parse("127.0.0.1"), 7777);
        RenderWindow window = new RenderWindow(new VideoMode(1920, 1080), "Pi Stream");

        byte[] handshake = new byte[3];
        while(true)
        {

            connection.Receive(handshake, 0, 3, SocketFlags.None);
            byte[] dataBuffer = new byte[256 * 256 * handshake[2] + 256 * handshake[1] + handshake[0]];
            Console.WriteLine("New File");
            
            int gros = 0;
            for (int count = 0; count < dataBuffer.Length; count += gros)
            {
                gros = connection.Receive(dataBuffer, count, dataBuffer.Length - count, SocketFlags.None);
                Console.WriteLine(count);
            }

            Drawable sprite = new Sprite(new Texture(new Image(dataBuffer)));
            
            window.Clear();
            window.Draw(sprite);
            window.Display();
        }
    }
}