// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_AUX_VGA_PLANES
{
    /// <summary>16 color planes (EGA/VGA)</summary>
    VGA4 = 0,

    /// <summary>CFB4 in planes (VGA)</summary>
    CFB4 = 1,

    /// <summary>CFB8 in planes (VGA)</summary>
    CFB8 = 2,
}
