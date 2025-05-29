// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct fb_videomode
{
    /// <summary>optional</summary>
    /// <remarks>const char* name;</remarks>
    [MarshalAs(UnmanagedType.LPStr)]
    public string name;

    /// <summary>optional</summary>
    public uint refresh;

    public uint xres;
    public uint yres;
    public uint pixclock;
    public uint left_margin;
    public uint right_margin;
    public uint upper_margin;
    public uint lower_margin;
    public uint hsync_len;
    public uint vsync_len;
    public uint sync;
    public uint vmode;
    public uint flag;
};
