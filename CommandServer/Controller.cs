using System.Data;
using System.Runtime.InteropServices;

namespace CommandServer;

public class Controller
{

    [StructLayout(LayoutKind.Sequential)]
    public struct Xbox
    {
        public int count;
        public IntPtr arr;
    }
    
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z17GetControllerAxesPvP4xbox")]
    private static extern IntPtr GetControllerAxes(IntPtr pad);
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z20GetControllerPointeri")]
    private static extern IntPtr GetControllerPointer(int index);
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z14InitControllerv")]
    public static extern int InitController();
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z8CloseSDLPv")]
    public static extern int CloseController(IntPtr pad);
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z8CloseSDLPv")]
    public static extern IntPtr CreateXbox();
    
    [DllImport("libcontroller.so", CallingConvention = CallingConvention.StdCall, EntryPoint = "_Z8CloseSDLPv")]
    public static extern void destroyXbox(IntPtr xbox);
    
    
    private int SDL2Index;
    private IntPtr pad;

    private Int16[] leftStick, rightStick, triggers;

    public Controller(int index)
    {
        SDL2Index = index;
        pad = GetControllerPointer(SDL2Index);
        Update();
    }

    public void Update()
    {
        InitController();
        pad = GetControllerPointer(0);
        IntPtr arr = GetControllerAxes(pad);

        /*
        int[] arr = new int[] {0, 0, 0, 0, 0, 0};
        var result = Marshal.PtrToStructure<int[]>(box.arr);

        if (result is not null)
            arr = result;
            */

        leftStick = new Int16[] {Marshal.ReadInt16(arr, 0 * 2), Marshal.ReadInt16(arr, 1 * 2)};

        rightStick = new Int16[] {Marshal.ReadInt16(arr, 3 * 2), Marshal.ReadInt16(arr, 4 * 2)};

        triggers = new Int16[] {Marshal.ReadInt16(arr, 2 * 2), Marshal.ReadInt16(arr, 5 * 2)};
    }

    public Int16[] getLeftStick() => leftStick;

    public Int16[] getRightStick() => rightStick;

    public Int16[] getTriggers() => triggers;
}