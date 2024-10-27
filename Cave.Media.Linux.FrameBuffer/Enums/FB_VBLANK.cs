// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_VBLANK
{
    /// <summary>currently in a vertical blank</summary>
    VBLANKING = 0x001,
    /// <summary>currently in a horizontal blank</summary>
    HBLANKING = 0x002,
    /// <summary>vertical blanks can be detected</summary>
    HAVE_VBLANK = 0x004,
    /// <summary>horizontal blanks can be detected</summary>
    HAVE_HBLANK = 0x008,
    /// <summary>global retrace counter is available</summary>
    HAVE_COUNT = 0x010,
    /// <summary>the vcount field is valid</summary>
    HAVE_VCOUNT = 0x020,
    /// <summary>the hcount field is valid</summary>
    HAVE_HCOUNT = 0x040,
    /// <summary>currently in a vsync</summary>
    VSYNCING = 0x080,
    /// <summary>verical syncs can be detected</summary>
    HAVE_VSYNC = 0x100,
}
