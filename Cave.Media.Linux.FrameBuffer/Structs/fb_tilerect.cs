// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_tilerect
{
    /// <summary>origin in the x-axis</summary>
    public uint sx;
    /// <summary>origin in the y-axis</summary>
    public uint sy;
    /// <summary>number of tiles in the x-axis</summary>
    public uint width;
    /// <summary>number of tiles in the y-axis</summary>
    public uint height;
    /// <summary>what tile to use: index to tile map</summary>
    public uint index;
    /// <summary>foreground color</summary>
    public uint fg;
    /// <summary>background color</summary>
    public uint bg;
    /// <summary>raster operation</summary>
    public uint rop;
};
