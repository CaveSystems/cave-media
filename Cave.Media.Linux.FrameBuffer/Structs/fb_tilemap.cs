// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_tilemap
{
    /// <summary>width of each tile in pixels</summary>
    public uint width;

    /// <summary>height of each tile in scanlines</summary>
    public uint height;

    /// <summary>color depth of each tile</summary>
    public uint depth;

    /// <summary>number of tiles in the map</summary>
    public uint length;

    /// <summary>actual tile map: a bitmap array, packed to the nearest byte</summary>
    /// <remarks>const __u8* data;</remarks>
    public UIntPtr data;
};
