// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux.FrameBuffer.Enums;

[Flags]
public enum FB_VMODE
{
    /// <summary>non interlaced</summary>
    NONINTERLACED = 0,

    /// <summary>interlaced</summary>
    INTERLACED = 1,

    /// <summary>double scan</summary>
    DOUBLE = 2,

    /// <summary>interlaced: top line first</summary>
    ODD_FLD_FIRST = 4,

    MASK = 255,

    /// <summary>ywrap instead of panning</summary>
    YWRAP = 256,

    /// <summary>smooth xpan possible (internally used)</summary>
    SMOOTH_XPAN = 512,

    /// <summary>don't update x/yoffset</summary>
    CONUPDATE = 512,
}
