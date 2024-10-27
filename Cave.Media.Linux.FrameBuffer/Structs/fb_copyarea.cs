// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_copyarea
{
    public uint dx;
    public uint dy;
    public uint width;
    public uint height;
    public uint sx;
    public uint sy;
};
