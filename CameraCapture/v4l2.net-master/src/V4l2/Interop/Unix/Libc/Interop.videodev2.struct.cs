// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;
using Iot.Device.Media;

public class Constants
{
    public const int VIDEO_MAX_FRAME = 32;
    public const int VIDEO_MAX_PLANES = 8;
}

public struct V4l2FrameBuffer
{
    public IntPtr Start;
    public uint Length;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_capability
{
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string driver;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string card;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string bus_info;
    public uint version;
    public uint capabilities;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] reserved;
}

public enum v4l2_ctrl_type : uint
{
    V4L2_CTRL_TYPE_INTEGER = 1,
    V4L2_CTRL_TYPE_BOOLEAN = 2,
    V4L2_CTRL_TYPE_MENU = 3,
    V4L2_CTRL_TYPE_BUTTON = 4,
    V4L2_CTRL_TYPE_INTEGER64 = 5,
    V4L2_CTRL_TYPE_CTRL_CLASS = 6,
    V4L2_CTRL_TYPE_STRING = 7,
    V4L2_CTRL_TYPE_BITMASK = 8,
    V4L2_CTRL_TYPE_INTEGER_MENU = 9,
    V4L2_CTRL_COMPOUND_TYPES = 0x0100,
    V4L2_CTRL_TYPE_U8 = 0x0100,
    V4L2_CTRL_TYPE_U16 = 0x0101,
    V4L2_CTRL_TYPE_U32 = 0x0102,
};

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_queryctrl
{
    public VideoDeviceValueType id;
    public v4l2_ctrl_type type;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string name;
    public int minimum;
    public int maximum;
    public int step;
    public int default_value;
    public uint flags;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] reserved;
};

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_control
{
    public VideoDeviceValueType id;
    public int value;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_fmtdesc
{
    public uint index;
    public v4l2_buf_type type;
    public uint flags;
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 32)]
    public string description;
    public PixelFormat pixelformat;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] reserved;
}

public enum v4l2_buf_type : uint
{
    V4L2_BUF_TYPE_VIDEO_CAPTURE = 1,
    V4L2_BUF_TYPE_VIDEO_OUTPUT = 2,
    V4L2_BUF_TYPE_VIDEO_OVERLAY = 3,
    V4L2_BUF_TYPE_VBI_CAPTURE = 4,
    V4L2_BUF_TYPE_VBI_OUTPUT = 5,
    V4L2_BUF_TYPE_SLICED_VBI_CAPTURE = 6,
    V4L2_BUF_TYPE_SLICED_VBI_OUTPUT = 7,
    V4L2_BUF_TYPE_VIDEO_OUTPUT_OVERLAY = 8,
    V4L2_BUF_TYPE_VIDEO_CAPTURE_MPLANE = 9,
    V4L2_BUF_TYPE_VIDEO_OUTPUT_MPLANE = 10,
    V4L2_BUF_TYPE_SDR_CAPTURE = 11,
    V4L2_BUF_TYPE_SDR_OUTPUT = 12,
    V4L2_BUF_TYPE_META_CAPTURE = 13,
    V4L2_BUF_TYPE_META_OUTPUT = 14,
    V4L2_BUF_TYPE_PRIVATE = 0x80,
  //  V4L2_BUF_TYPE_STREAMING = 0x04000000
}

public enum v4l2_field : uint
{
    V4L2_FIELD_ANY = 0,
    V4L2_FIELD_NONE = 1,
    V4L2_FIELD_TOP = 2,
    V4L2_FIELD_BOTTOM = 3,
    V4L2_FIELD_INTERLACED = 4,
    V4L2_FIELD_SEQ_TB = 5,
    V4L2_FIELD_SEQ_BT = 6,
    V4L2_FIELD_ALTERNATE = 7,
    V4L2_FIELD_INTERLACED_TB = 8,
    V4L2_FIELD_INTERLACED_BT = 9,
}

public enum v4l2_colorspace : uint
{
    V4L2_COLORSPACE_DEFAULT = 0,
    V4L2_COLORSPACE_SMPTE170M = 1,
    V4L2_COLORSPACE_SMPTE240M = 2,
    V4L2_COLORSPACE_REC709 = 3,
    V4L2_COLORSPACE_BT878 = 4,
    V4L2_COLORSPACE_470_SYSTEM_M = 5,
    V4L2_COLORSPACE_470_SYSTEM_BG = 6,
    V4L2_COLORSPACE_JPEG = 7,
    V4L2_COLORSPACE_SRGB = 8,
    V4L2_COLORSPACE_ADOBERGB = 9,
    V4L2_COLORSPACE_BT2020 = 10,
    V4L2_COLORSPACE_RAW = 11,
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_pix_format
{
    public uint width;
    public uint height;
    public PixelFormat pixelformat;
    public v4l2_field field;
    public uint bytesperline;
    public uint sizeimage;
    public v4l2_colorspace colorspace;
    public uint priv;
    public uint flags;

    [StructLayout(LayoutKind.Explicit)]
    public struct m_union
    {
        [FieldOffset(0)]
        public uint ycbcr_enc;
        
        [FieldOffset(0)]
        public uint hsv_enc;
    }

    //[MarshalAs(UnmanagedType.Struct
    public m_union union;
    
    public uint quantization;
    public uint xfer_func;
}


[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_plane_pix_format
{
    public uint sizeimage;
    public uint bytesperline;
    //[MarshalAs(UnmanagedType.ByValArray)]
    public fixed short reserved[6];
}



[StructLayout(LayoutKind.Sequential)]
public struct v4l2_pix_format_mplane
{
    public uint width;
    public uint height;
    public PixelFormat pixelformat;
    public v4l2_field field;
    public v4l2_colorspace colorspace;
    
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_plane_pix_format plane_fmt;

    public byte num_planes;
    public byte flags;

    
    [StructLayout(LayoutKind.Explicit)]
    public struct m_union
    {
        [FieldOffset(0)]
        public uint ycbcr_enc;
        
        [FieldOffset(0)]
        public uint hsv_enc;
    }

    //[MarshalAs(UnmanagedType.Struct
    public m_union union;
    
    public byte quantization;
    public byte xfer_func;
    public uint bytesperline;
    public uint sizeimage;
    public uint priv;
}


[StructLayout(LayoutKind.Sequential)]
public struct v4l2_rect
{
    public int left;
    public int top;
    public uint width;
    public uint height;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_clip
{
    public v4l2_rect c;
    public v4l2_clip* next;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_window
{
    public v4l2_rect w;
    public v4l2_field field;
    public uint chromakey;
    public v4l2_clip* clips;
    public uint clipcount;
    public void* bitmap;
    public byte global_alpha;
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_vbi_format
{
    public uint sampling_rate;
    public uint offset;
    public uint samples_per_line;
    public uint sample_format;
    public fixed uint start[2];
    public fixed uint count[2];
    public uint flags;
    public fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_sliced_vbi_format
{
    public ushort service_set;
    public fixed ushort service_lines[48];
    public uint io_size;
    public fixed uint reserved[2];
}

[StructLayout(LayoutKind.Sequential)]
public unsafe struct v4l2_sdr_format
{
    public PixelFormat pixelformat;
    public uint buffersize;
    public fixed byte reserved[24];
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_meta_format
{
    public uint dataformat;
    public uint buffersize;
}

[StructLayout(LayoutKind.Explicit, Pack = 1)]
public unsafe struct fmt
{
    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_pix_format pix;
    
    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_pix_format_mplane pix_mp;

    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_window win;
    
    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_vbi_format vbi;

    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_sliced_vbi_format sliced;
    
    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_sdr_format sdr;
    
    [FieldOffset(0)]
    //[MarshalAs(UnmanagedType.Struct
    public v4l2_meta_format meta;
    
    [FieldOffset(0)]
    public fixed byte raw[200];
}

[StructLayout(LayoutKind.Explicit)]
public unsafe struct v4l2_format
{
    [FieldOffset(0)]
    public v4l2_buf_type type;
    [FieldOffset(8)]
    public fmt fmt;
}

[StructLayout(LayoutKind.Sequential, Size = 208)]
public struct v4l2_format_aligned
{
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_fract
{
    public uint numerator;
    public uint denominator;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_cropcap
{
    public v4l2_buf_type type;
    public v4l2_rect bounds;
    public v4l2_rect defrect;
    public v4l2_fract pixelaspect;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_crop
{
    public v4l2_buf_type type;
    public v4l2_rect c;
}

public enum v4l2_memory : uint
{
    V4L2_MEMORY_MMAP = 1,
    V4L2_MEMORY_USERPTR = 2,
    V4L2_MEMORY_OVERLAY = 3,
    V4L2_MEMORY_DMABUF = 4,
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_requestbuffers
{
    public uint count;
    public v4l2_buf_type type;
    public v4l2_memory memory;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] reserved;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_timeval
{
    public uint tv_sec;
    public uint tv_usec;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_timecode
{
    public uint type;
    public uint flags;
    public byte frames;
    public byte seconds;
    public byte minutes;
    public byte hours;
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] userbits;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_buffer
{
    public uint index;
    public v4l2_buf_type type;
    public uint bytesused;
    public uint flags;
    public v4l2_field field;

    [StructLayout(LayoutKind.Sequential)]
    public struct timeval
    {
        public long tv_sec;
        public long tv_usec;
    }
    public timeval timestamp;

    public v4l2_timecode timecode;
    public uint sequence;
    public v4l2_memory memory;

	[StructLayout(LayoutKind.Explicit)]
		public struct m_union
		{
			[FieldOffset(0)]
			public uint offset;
			[FieldOffset(0)]
			public uint userptr;
            [FieldOffset(0)]
            private unsafe void* planes;
            [FieldOffset(0)]
            private short fd;
        }
		public m_union m;

    public uint length;
    public uint reserved2;
    
    [StructLayout(LayoutKind.Explicit)]
    public struct m_union2
    {
        [FieldOffset(0)]
        public int request_fd;
        [FieldOffset(0)]
        public uint reserved;
    }
    public m_union2 m2;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_frmsizeenum
{
    public uint index;
    public PixelFormat pixel_format;
    public v4l2_frmsizetypes type;

    [StructLayout(LayoutKind.Explicit)] 
    public class m_union
    {
        [FieldOffset(0)] 
        public v4l2_frmsize_discrete discrete;
        [FieldOffset(0)] 
        public v4l2_frmsize_stepwise stepwise;
    }

    public m_union union;

    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public uint[] reserved;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_frmsize_discrete
{
    public uint width;
    public uint height;
}

[StructLayout(LayoutKind.Sequential)]
public struct v4l2_frmsize_stepwise
{
    public uint min_width;
    public uint max_width;
    public uint step_width;
    public uint min_height;
    public uint max_height;
    public uint step_height;
};

public enum v4l2_frmsizetypes : uint
{
    V4L2_FRMSIZE_TYPE_DISCRETE = 1,
    V4L2_FRMSIZE_TYPE_CONTINUOUS = 2,
    V4L2_FRMSIZE_TYPE_STEPWISE = 3,
}