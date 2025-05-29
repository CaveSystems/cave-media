// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

/// <summary>in fraction of 1024</summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct fb_chroma
{
    public uint redx;
    public uint greenx;
    public uint bluex;
    public uint whitex;
    public uint redy;
    public uint greeny;
    public uint bluey;
    public uint whitey;
};
