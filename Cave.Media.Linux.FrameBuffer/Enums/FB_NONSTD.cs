// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_NONSTD
{
    /// <summary>Hold-And-Modify (HAM)</summary>
    HAM = 1,

    /// <summary>order of pixels in each byte is reversed</summary>
    REV_PIX_IN_B = 2,
}
