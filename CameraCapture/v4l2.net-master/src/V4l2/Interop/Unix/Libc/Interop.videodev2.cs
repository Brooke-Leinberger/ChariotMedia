// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;

internal enum VideoSettings : int
{
    VIDIOC_QUERYCAP,
    VIDIOC_ENUM_FMT,
    VIDIOC_CROPCAP,
    VIDIOC_G_CROP,
    VIDIOC_S_CROP,
    VIDIOC_G_FMT,
    VIDIOC_S_FMT,
    VIDIOC_REQBUFS,
    VIDIOC_QUERYBUF,
    VIDIOC_STREAMON,
    VIDIOC_STREAMOFF,
    VIDIOC_QBUF,
    VIDIOC_DQBUF,
    VIDIOC_ENUM_FRAMESIZES,
    VIDIOC_G_CTRL,
    VIDIOC_S_CTRL,
    VIDIOC_QUERYCTRL,
}

/// <summary>
/// videodev2.h Request Definition
/// </summary>
internal class RawVideoSettings
{
    public static int VIDIOC_QUERYCAP = Interop._IOR('V', 0, typeof(v4l2_capability));
    public static int VIDIOC_ENUM_FMT = Interop._IOWR('V', 2, typeof(v4l2_fmtdesc));
    public static int VIDIOC_G_FMT = Interop._IOWR('V', 4, typeof(v4l2_format));
    public static int VIDIOC_S_FMT = Interop._IOWR('V', 5, typeof(v4l2_format));
    public static int VIDIOC_REQBUFS = Interop._IOWR('V', 8, typeof(v4l2_requestbuffers));
    public static int VIDIOC_QUERYBUF = Interop._IOWR('V', 9, typeof(v4l2_buffer));
    public static int VIDIOC_OVERLAY = Interop._IOW('V', 14, typeof(int));
    public static int VIDIOC_QBUF = Interop._IOWR('V', 15, typeof(v4l2_buffer));
    public static int VIDIOC_DQBUF = Interop._IOWR('V', 17, typeof(v4l2_buffer));
    public static int VIDIOC_STREAMON = Interop._IOW('V', 18, typeof(int));
    public static int VIDIOC_STREAMOFF = Interop._IOW('V', 19, typeof(int));
    public static int VIDIOC_G_CTRL = Interop._IOWR('V', 27, typeof(v4l2_control));
    public static int VIDIOC_S_CTRL = Interop._IOWR('V', 28, typeof(v4l2_control));
    public static int VIDIOC_QUERYCTRL = Interop._IOWR('V', 36, typeof(v4l2_queryctrl));
    public static int VIDIOC_G_INPUT = Interop._IOR('V', 38, typeof(int));
    public static int VIDIOC_S_INPUT = Interop._IOWR('V', 39, typeof(int));
    public static int VIDIOC_G_OUTPUT = Interop._IOR('V', 46, typeof(int));
    public static int VIDIOC_S_OUTPUT = Interop._IOWR('V', 47, typeof(int));
    public static int VIDIOC_CROPCAP = Interop._IOWR('V', 58, typeof(v4l2_cropcap));
    public static int VIDIOC_G_CROP = Interop._IOWR('V', 59, typeof(v4l2_crop));
    public static int VIDIOC_S_CROP = Interop._IOW('V', 60, typeof(v4l2_crop));
    public static int VIDIOC_TRY_FMT = Interop._IOWR('V', 64, typeof(v4l2_format));
    public static int VIDIOC_G_PRIORITY = Interop._IOR('V', 67, typeof(uint));
    public static int VIDIOC_S_PRIORITY = Interop._IOW('V', 68, typeof(uint));
    public static int VIDIOC_ENUM_FRAMESIZES = Interop._IOWR('V', 74, typeof(v4l2_frmsizeenum));
    public static int VIDIOC_PREPARE_BUF = Interop._IOWR('V', 93, typeof(v4l2_buffer));

    public static Dictionary<VideoSettings, int> VideoSettingsMap = new Dictionary<VideoSettings, int>()
    {
        {VideoSettings.VIDIOC_QUERYCAP, RawVideoSettings.VIDIOC_QUERYCAP},
        {VideoSettings.VIDIOC_ENUM_FMT, RawVideoSettings.VIDIOC_ENUM_FMT},
        {VideoSettings.VIDIOC_CROPCAP, RawVideoSettings.VIDIOC_CROPCAP},
        {VideoSettings.VIDIOC_G_CROP, RawVideoSettings.VIDIOC_G_CROP},
        {VideoSettings.VIDIOC_S_CROP, RawVideoSettings.VIDIOC_S_CROP},
        {VideoSettings.VIDIOC_G_FMT, RawVideoSettings.VIDIOC_G_FMT},
        {VideoSettings.VIDIOC_S_FMT, RawVideoSettings.VIDIOC_S_FMT},
        {VideoSettings.VIDIOC_REQBUFS, RawVideoSettings.VIDIOC_REQBUFS},
        {VideoSettings.VIDIOC_QUERYBUF, RawVideoSettings.VIDIOC_QUERYBUF},
        {VideoSettings.VIDIOC_STREAMON, RawVideoSettings.VIDIOC_STREAMON},
        {VideoSettings.VIDIOC_STREAMOFF, RawVideoSettings.VIDIOC_STREAMOFF},
        {VideoSettings.VIDIOC_QBUF, RawVideoSettings.VIDIOC_QBUF},
        {VideoSettings.VIDIOC_DQBUF, RawVideoSettings.VIDIOC_DQBUF},
        {VideoSettings.VIDIOC_ENUM_FRAMESIZES, RawVideoSettings.VIDIOC_ENUM_FRAMESIZES},
        {VideoSettings.VIDIOC_G_CTRL, RawVideoSettings.VIDIOC_G_CTRL},
        {VideoSettings.VIDIOC_S_CTRL, RawVideoSettings.VIDIOC_S_CTRL},
        {VideoSettings.VIDIOC_QUERYCTRL, RawVideoSettings.VIDIOC_QUERYCTRL},
    };
}