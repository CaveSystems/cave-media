// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_tilearea
{
    /// <summary>source origin in the x-axis</summary>
    public uint sx;

    /// <summary>source origin in the y-axis</summary>
    public uint sy;

    /// <summary>destination origin in the x-axis</summary>
    public uint dx;

    /// <summary>destination origin in the y-axis</summary>
    public uint dy;

    /// <summary>number of tiles in the x-axis</summary>
    public uint width;

    /// <summary>number of tiles in the y-axis</summary>
    public uint height;
};
