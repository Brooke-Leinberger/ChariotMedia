using System;
namespace CommandServer;

public class Program
{
    static int Main()
    {
        Controller xbox = new Controller(0);
        
        for (;;)
        {
            xbox.Update();
            Console.WriteLine($"Right: {xbox.getRightStick()[0]}, {xbox.getRightStick()[1]}; Left: {xbox.getLeftStick()[0]}, {xbox.getLeftStick()[1]}");
        }
        
        return 0;
    }
}