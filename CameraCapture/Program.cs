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
            
            VideoConnectionSettings settings = new VideoConnectionSettings(0)
            {
                CaptureSize = (1920, 1080),
                PixelFormat = PixelFormat.MJPEG,
                ExposureType = ExposureType.Auto
            };
            using VideoDevice device = VideoDevice.Create(settings);

            // Send photos

            //device.Capture("/home/juno/test.jpg");
            
            int port = 7777;
            int file_size = 128000; //***CHANGES BASED ON RESOLUTION AND CAMERA***
            
            Console.WriteLine("Starting Server...");

            IPEndPoint ipep = new IPEndPoint(IPAddress.Any, port);
            Socket listener = new Socket(AddressFamily.InterNetwork, SocketType.Stream, ProtocolType.Tcp);
            listener.Bind(ipep);
            listener.Blocking = true;

            listener.Listen();
            Console.WriteLine("Server Started; Awaiting Clients...");
            Socket client = listener.Accept();
            string path = "/home/juno/captures";
            Console.WriteLine($"Client Connected; Local Images Saved at {path}\nStarting Stream...");

            
            fixed (V4l2FrameBuffer* buffers = &(((UnixVideoDevice)device).ApplyFrameBuffers()[0]))
            {
                // Start data stream
                v4l2_buf_type type = v4l2_buf_type.V4L2_BUF_TYPE_VIDEO_CAPTURE;
                var status = Interop.ioctl(((UnixVideoDevice) device).getFD(), (int) RawVideoSettings.VIDIOC_STREAMON,
                    new IntPtr(&type));
                
                for(int id = 0; id < 5; id++)
                {
                    byte[] dataBuffer = ((UnixVideoDevice) device).GetFrameData(buffers);
                    device.SaveFrame($"{path}/data.jpg", dataBuffer);
                    JpegParser parser = new JpegParser(dataBuffer);

                    int length = parser.find_length();
                    byte[] saveBuffer = new byte[length];
                    client.Send(IntToBytes(length, 3), 3, SocketFlags.None);
                    for (int count = 0; count < length;)
                    {
                        count += client.Send(dataBuffer, count, Clamp(length-count, 1024, 0), SocketFlags.None);
                    }

                    for (int i = 0; i < saveBuffer.Length; i++)
                        saveBuffer[i] = dataBuffer[i];
                    
                    device.SaveFrame($"{path}/save{id}.jpg", saveBuffer);
                    int[] res = parser.get_resolution();
                    Console.WriteLine($"Resolution: {res[0]}x{res[1]}");
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
            //device.SendSerializedFrame(client, "/home/pi/local_test.jng");
            
            
        }
    }
}