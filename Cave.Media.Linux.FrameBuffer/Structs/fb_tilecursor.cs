// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_tilecursor
{
    /// <summary>cursor position in the x-axis</summary>
    public uint sx;

    /// <summary>cursor position in the y-axis</summary>
    public uint sy;

    /// <summary>0 = erase, 1 = draw</summary>
    public uint mode;

    /// <summary>see FB_TILE_CURSOR_*</summary>
    public uint shape;

    /// <summary>foreground color</summary>
    public uint fg;

    /// <summary>background color</summary>
    public uint bg;
};
