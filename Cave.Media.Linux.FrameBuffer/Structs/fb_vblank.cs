// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct fb_vblank
{
    /// <summary>FB_VBLANK flags</summary>
    public uint flags;

    /// <summary>counter of retraces since boot</summary>
    public uint count;

    /// <summary>current scanline position</summary>
    public uint vcount;

    /// <summary>current scandot position</summary>
    public uint hcount;

    /// <summary>reserved for future compatibility</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] reserved;
};
