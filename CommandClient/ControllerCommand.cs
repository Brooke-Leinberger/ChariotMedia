namespace CommandServer;

public class ControllerCommand
{
    private int deviceIndex;
    private Controller pad;

    private int VisorTurnRate = 180;
    private int SteerRate = 45;

    public ControllerCommand(int index)
    {
        deviceIndex = index;
        pad = new Controller(index);
    }

    public byte[] DriveCommandSequence(CommandProtocol.SystemFunction func)
    {
        pad.Update();
        Int16[] stick = pad.getLeftStick();
        byte[] data = new byte[]
        {
            (byte) MapRange(stick[0], Int16.MinValue, Int16.MaxValue, -SteerRate, SteerRate),
            (byte) MapRange(stick[1], Int16.MinValue, Int16.MaxValue, 0, 100)
        };

        return CommandProtocol.GenerateCommandSequence(CommandProtocol.Subsystem.Drive, func, data);
    }

    public byte[] VisorCommandSequence(CommandProtocol.SystemFunction func)
    {
        pad.Update();
        byte[] data = pad.getRightStick().Select(c => 
            (byte)MapRange(c, Int16.MinValue, Int16.MaxValue, 0, 180)).ToArray();
        byte[] buff = CommandProtocol.GenerateCommandSequence(CommandProtocol.Subsystem.Payload1, func, data);
        Console.WriteLine(CommandProtocol.ByteToHex(buff));
        return buff;
    }
    
    
    public int MapRange(int val, int valLow, int valHigh, int mapLow, int mapHigh)
    {
        return (int)((val + mapLow - valLow) * ((double) (mapHigh - mapLow) / (double) (valHigh - valLow)));
    }

}