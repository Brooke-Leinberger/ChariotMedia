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

    static void Main(string[] args)
    {
        //Setup connection
        Socket connection = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
        //connection.Connect(IPAddress.Parse("192.168.133.105"), 7777);
        //connection.Connect(IPAddress.Parse("127.0.0.1"), 7777);
        connection.Connect(IPAddress.Parse(args[0]), 7777);
        
        //setup display
        RenderWindow window = new RenderWindow(new VideoMode(1280, 480), "Pi Stream");

        byte[] handshake = new byte[3]; //for storing length of frame
        while(true)
        {
            connection.Receive(handshake, 0, 3, SocketFlags.None); //Recieve length of frame
            byte[] leftBuffer = new byte[256 * 256 * handshake[2] + 256 * handshake[1] + handshake[0]]; //create buffer to correct size
            
            connection.Receive(handshake, 0, 3, SocketFlags.None); //Recieve length of frame
            byte[] rightBuffer = new byte[256 * 256 * handshake[2] + 256 * handshake[1] + handshake[0]]; //create buffer to correct size
            

            //Recieve loop
            for (int count = 0, gros = 0; count < leftBuffer.Length; count += gros)
                gros = connection.Receive(leftBuffer, count, leftBuffer.Length - count, SocketFlags.None);
            
            //Recieve loop
            for (int count = 0, gros = 0; count < rightBuffer.Length; count += gros)
                gros = connection.Receive(rightBuffer, count, rightBuffer.Length - count, SocketFlags.None);

            //Display image
            Texture text = new Texture(new Image(rightBuffer));
            Drawable right = new Sprite(text);
            Drawable left = new Sprite(new Texture(new Image(leftBuffer)));
            RenderStates state = new RenderStates(text);
            state.Transform.Translate(640,0);
            window.Clear();
            window.Draw(left);
            window.Draw(right, state);
            window.Display();
        }
    }
}