// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_tileblit
{
    /// <summary>origin in the x-axis</summary>
    public uint sx;

    /// <summary>origin in the y-axis</summary>
    public uint sy;

    /// <summary>number of tiles in the x-axis</summary>
    public uint width;

    /// <summary>number of tiles in the y-axis</summary>
    public uint height;

    /// <summary>foreground color</summary>
    public uint fg;

    /// <summary>background color</summary>
    public uint bg;

    /// <summary>number of tiles to draw</summary>
    public uint length;

    /// <summary>array of indices to tile map</summary>
    /// <remarks>__u32* indices;</remarks>
    public UIntPtr indices;
};
