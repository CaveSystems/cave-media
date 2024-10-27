// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Enums;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_fillrect
{
    /// <summary>screen-relative</summary>
    public uint dx;
    public uint dy;
    public uint width;
    public uint height;
    public uint color;
    public ROP rop;
};
