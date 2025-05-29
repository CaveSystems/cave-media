// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_cmap
{
    /// <summary>First entry</summary>
    public uint start;
    /// <summary>Number of entries</summary>
    public uint len;
    /// <summary>Red values</summary>
    public UIntPtr red;
    public UIntPtr green;
    public UIntPtr blue;
    /// <summary>transparency, can be NULL</summary>
    public UIntPtr transp;
};
