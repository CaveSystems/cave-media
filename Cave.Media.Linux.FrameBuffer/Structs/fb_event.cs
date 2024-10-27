// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_event
{
    /// <remarks>struct fb_info* info;</remarks>
    public nuint info;

    /// <remarks>void* data;</remarks>
    public nuint data;
};
