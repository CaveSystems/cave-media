// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FBINFO_HWACCEL
{
    NONE = 0x0000,
    /// <summary>required</summary>
    COPYAREA = 0x0100,
    /// <summary>required</summary>
    FILLRECT = 0x0200,
    /// <summary>required</summary>
    IMAGEBLIT = 0x0400,
    /// <summary>optional</summary>
    ROTATE = 0x0800,
    /// <summary>optional</summary>
    XPAN = 0x1000,
    /// <summary>optional</summary>
    YPAN = 0x2000,
    /// <summary>optional</summary>
    YWRAP = 0x4000,
}
