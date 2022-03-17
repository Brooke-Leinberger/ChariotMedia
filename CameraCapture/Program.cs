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
        
        private static int Clamp(int value, int upperBound, int lowerBound)
        {
            if (value < lowerBound)
                return lowerBound;
            else if (value > upperBound)
                return upperBound;
            
            return value;
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
            
            VideoConnectionSettings settings = new VideoConnectionSettings(0)
            {
                CaptureSize = (1920, 1080),
                PixelFormat = PixelFormat.MJPEG,
                ExposureType = ExposureType.Auto
            };
            using VideoDevice device = VideoDevice.Create(settings);

            // Send photos

            //device.Capture("/home/juno/test.jpg");
            
            Console.WriteLine("Starting Server...");
            int port = 7777;
            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipep);
            listener.Blocking = true;

            listener.Listen();
            Console.WriteLine("Server Started; Awaiting Clients...");
            Socket client = listener.Accept();
            string path = "/home/pi/captures";
            Console.WriteLine($"Client Connected; Local Images Saved at {path}\nStarting Stream...");

            
            fixed (V4l2FrameBuffer* buffers = &(((UnixVideoDevice)device).ApplyFrameBuffers()[0]))
            {
                // Start data stream
                v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;
                var status = Interop.ioctl(((UnixVideoDevice) device).getFD(), (int) RawVideoSettings.VIDIOC_STREAMON,
                    new IntPtr(&type));
                
                for(int id = 0; id < 60; id++)
                {
                    byte[] dataBuffer = ((UnixVideoDevice) device).GetFrameData(buffers);
                    Console.WriteLine(dataBuffer.Length);
                    
                    int buffer = 0;
                    for (int count = 0; count < dataBuffer.Length;)
                    {
                        buffer = Clamp(dataBuffer.Length - count, 1024, 0);
                        client.Send(dataBuffer, count, buffer, SocketFlags.None);
                        count += buffer;
                    }
                    
                    device.SaveFrame($"{path}/local{id}.jpg", dataBuffer);
                }
                
                

                // Close data stream
                status = Interop.ioctl(((UnixVideoDevice) device).getFD(), (int) RawVideoSettings.VIDIOC_STREAMOFF,
                    new IntPtr(&type));

                UnixVideoDevice.UnmappingFrameBuffers(buffers);
            }
            //device.SendSerializedFrame(client, "/home/pi/local_test.jng");
        }
    }
}