// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_cmap_user
{
    /// <summary>First entry</summary>
    public uint start;

    /// <summary>Number of entries</summary>
    public uint len;

    /// <summary>Red values</summary>
    /// <remarks>__u16 __user* red;</remarks>
    public UIntPtr red;

    /// <remarks>__u16 __user* green;</remarks>
    public UIntPtr green;

    /// <remarks>__u16 __user* blue;</remarks>
    public UIntPtr blue;

    /// <summary>transparency, can be NULL</summary>
    /// <remarks>__u16 __user* transp;</remarks>
    public UIntPtr transp;
};
