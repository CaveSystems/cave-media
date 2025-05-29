// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Enums;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_fix_screeninfo
{
    /// <summary>identification string eg "TT Builtin"</summary>
    /// <remarks>char id[16];</remarks>
    [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 16)]
    public string id;

    /// <summary>Start of frame buffer mem (physical address)</summary>
    public nuint smem_start;

    /// <summary>Length of frame buffer mem</summary>
    public uint smem_len;

    /// <summary>see FB_TYPE_*</summary>
    [MarshalAs(UnmanagedType.U4)]
    public FB_TYPE type;

    /// <summary>Interleave for interleaved Planes</summary>
    public uint type_aux;

    /// <summary>see FB_VISUAL_*</summary>
    [MarshalAs(UnmanagedType.U4)]
    public FB_VISUAL visual;

    /// <summary>zero if no hardware panning</summary>
    public ushort xpanstep;

    /// <summary>zero if no hardware panning</summary>
    public ushort ypanstep;

    /// <summary>zero if no hardware ywrap</summary>
    public ushort ywrapstep;

    /// <summary>length of a line in bytes</summary>
    public uint line_length;

    /// <summary>Start of Memory Mapped I/O (physical address)</summary>
    public nuint mmio_start;

    /// <summary>Length of Memory Mapped I/O</summary>
    public uint mmio_len;

    /// <summary>Indicate to driver which specific chip/card we have</summary>
    public uint accel;

    /// <summary>see FB_CAP_*</summary>
    [MarshalAs(UnmanagedType.U2)]
    public ushort capabilities;

    /// <summary>Reserved for future compatibility</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 2)]
    public ushort[] reserved;
};
