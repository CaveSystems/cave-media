// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_image
{
    /// <summary>Where to place image</summary>
    public uint dx;

    public uint dy;

    /// <summary>Size of image</summary>
    public uint width;

    public uint height;

    /// <summary>Only used when a mono bitmap</summary>
    public uint fg_color;

    public uint bg_color;

    /// <summary>Depth of the image</summary>
    public byte depth;

    /// <summary>Pointer to image data</summary>
    public nuint data;

    /// <summary>color map info</summary>
    public fb_cmap cmap;
};
