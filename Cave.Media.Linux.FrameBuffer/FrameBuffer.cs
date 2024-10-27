// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1)]
struct LinuxFbBitfield
{
    /// <summary>beginning of bitfield</summary>
    public uint offset;

    /// <summary>length of bitfield</summary>
    public uint length;

    /// <summary>!= 0 : Most significant bit is right</summary>
    public uint msb_right;
};

[StructLayout(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
struct LinuxFbFixScreenInfo
{
    // char[]
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string Id;

    /// <summary>Start of frame buffer mem (physical address)</summary>
    public UIntPtr SmemStart;

    /// <summary>Length of frame buffer mem</summary>
    public uint SmemLen;

    public uint Type;

    /// <summary>Interleave for interleaved Planes</summary>
    public uint TypeAux;

    /// <summary>FB_VISUAL_</summary>
    public uint Visual;

    /// <summary>zero if no hardware panning</summary>
    public ushort Xpanstep;

    /// <summary>zero if no hardware panning</summary>
    public ushort Ypanstep;

    /// <summary>zero if no hardware ywrap</summary>
    public ushort Ywrapstep;

    /// <summary>length of a line in bytes</summary>
    public uint LineLength;

    /// <summary>Start of Memory Mapped I/O (physical address)</summary>
    public UIntPtr MmioStart;

    /// <summary>Length of Memory Mapped I/O</summary>
    public uint MmioLen;

    /// <summary>Indicate to driver which specific chip/card we have</summary>
    public uint Accel;

    /// <summary>see FB_CAP_*</summary>
    public ushort capabilities;

    /// <summary>Reserved for future compatibility</summary>
    public uint reserved;
};

[StructLayout(LayoutKind.Sequential)]
struct LinuxFbVarScreenInfo
{
    public uint xres;
    public uint yres;
    public uint xres_virtual;
    public uint yres_virtual;
    public uint xoffset;
    public uint yoffset;
    public uint bits_per_pixel;
    public uint grayscale;

    public LinuxFbBitfield red;
    public LinuxFbBitfield green;
    public LinuxFbBitfield blue;
    public LinuxFbBitfield transp;

    /// <summary>!= 0 Non standard pixel format</summary>
    public uint nonstd;

    /// <summary>see FB_ACTIVATE_*</summary>
    public uint activate;

    /// <summary>height of picture in mm</summary>
    public uint height;

    /// <summary>width of picture in mm</summary>
    public uint width;

    /// <summary>(OBSOLETE) see fb_info.flags</summary>
    public uint accel_flags;

    /// <summary>pixel clock in ps (pico seconds)</summary>
    public uint pixclock;

    /// <summary>time from sync to picture</summary>
    public uint left_margin;

    /// <summary>time from picture to sync</summary>
    public uint right_margin;

    /// <summary>time from sync to picture</summary>
    public uint upper_margin;

    public uint lower_margin;

    /// <summary>length of horizontal sync</summary>
    public uint hsync_len;

    /// <summary>length of vertical sync</summary>
    public uint vsync_len;

    /// <summary>see FB_SYNC_*</summary>
    public uint sync;

    /// <summary>see FB_VMODE_*</summary>
    public uint vmode;

    /// <summary>angle we rotate counter clockwise</summary>
    public uint rotate;

    /// <summary>colorspace for FOURCC-based modes</summary>
    public uint colorspace;

    /// <summary>Reserved for future compatibility</summary>
    public uint reserved1;

    public uint reserved2;
    public uint reserved3;
    public uint reserved4;
};
