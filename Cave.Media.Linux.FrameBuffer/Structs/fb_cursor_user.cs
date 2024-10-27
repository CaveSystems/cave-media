// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Structs;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_cursor_user
{
    /// <summary>what to set</summary>
    public ushort set;

    /// <summary>cursor on/off</summary>
    public ushort enable;

    /// <summary>bitop operation</summary>
    public ushort rop;

    /// <summary>cursor mask bits</summary>
    /// <remarks>const char __user* mask;</remarks>
    public UIntPtr mask;

    /// <summary>cursor hot spot</summary>
    public fbcurpos hot;

    /// <summary>Cursor image</summary>
    public fb_image_user image;
};
