// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Diagnostics;
using System.Net;
using System.Net.Sockets;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Iot.Device.Media;

namespace CameraCapture
{
    class Program
    {
        public static void CompareType<T>()
        {
            
            var safe = Unsafe.SizeOf<T>();
            var marshal = Marshal.SizeOf<T>();
            
            Debug.WriteLine(typeof(T).Name + $"{safe.ToString()},{marshal.ToString()}");

            
        }
        
        //helper function for limiting bytes sent per packet
        private static int Clamp(int value, int upperBound, int lowerBound)
        {
            if (value < lowerBound)
                return lowerBound;
            else if (value > upperBound)
                return upperBound;
            
            return value;
        }

        //helper function for creating a length indicator before each frame is sent
        private static byte[] IntToBytes(int value, int size)
        {
            byte[] bytes = new byte[size];
            for (int i = 0; i < size; i++)
            {
                bytes[i] = (byte)(value % 256);
                value >>= 8;
            }
            return bytes;
        }

        static unsafe void Main(string[] args)
        {
            CompareType<v4l2_capability>();
            CompareType<v4l2_fmtdesc>();
            CompareType<v4l2_requestbuffers>();
            CompareType<int>();
            CompareType<v4l2_control>();
            CompareType<v4l2_queryctrl>();
            CompareType<v4l2_cropcap>();
            CompareType<v4l2_crop>();
            CompareType<v4l2_format>();
            CompareType<v4l2_format_aligned>();
            CompareType<v4l2_frmsizeenum>();
            CompareType<v4l2_buffer>();
            
            //Settings for capture
            VideoConnectionSettings settings = new VideoConnectionSettings(0)
            {
                CaptureSize = (1920, 1080),
                PixelFormat = PixelFormat.MJPEG,
                ExposureType = ExposureType.Auto
            };
            //Initialize capture device
            using VideoDevice device = VideoDevice.Create(settings);
            int port = 7777;
            
            Console.WriteLine("Starting Server...");
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipep);
            listener.Blocking = true;

            listener.Listen();
            Console.WriteLine("Server Started; Awaiting Clients...");
            Socket client = listener.Accept();
            Console.WriteLine($"Client Connected\nStarting Stream...");

            
            fixed (V4l2FrameBuffer* buffers = &(((UnixVideoDevice)device).ApplyFrameBuffers()[0]))
            {
                // Start data stream
                v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;
                var status = Interop.ioctl(((UnixVideoDevice) device).getFD(), (int) RawVideoSettings.VIDIOC_STREAMON,
                    new IntPtr(&type));
                
                //Send loop
                while(true)
                {
                    byte[] dataBuffer = ((UnixVideoDevice) device).GetFrameData(buffers); //create byte buffer with full frame stored
                    
                    int length = new JpegParser(dataBuffer).find_length(); //find length of databuffer
                    client.Send(IntToBytes(length, 3), 3, SocketFlags.None); //send length ahead of frame
                    
                    //send frame
                    for (int count = 0; count < length;)
                        count += client.Send(dataBuffer, count, Clamp(length-count, 1024, 0), SocketFlags.None);
                }

                // Close data stream
                Console.WriteLine("Closing Server...");
                status = Interop.ioctl(((UnixVideoDevice) device).getFD(), (int) RawVideoSettings.VIDIOC_STREAMOFF,
                    new IntPtr(&type));

                UnixVideoDevice.UnmappingFrameBuffers(buffers);
                client.Close();
                listener.Close();
                Console.WriteLine("Server Closed");
            }
        }
    }
}