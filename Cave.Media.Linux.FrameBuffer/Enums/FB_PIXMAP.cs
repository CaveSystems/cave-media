// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_PIXMAP
{
    /// <summary>used internally by fbcon</summary>
    DEFAULT = 1,
    /// <summary>memory is in system RAM</summary>
    SYSTEM = 2,
    /// <summary>memory is iomapped</summary>
    IO = 4,
    /// <summary>set if GPU can DMA</summary>
    SYNC = 256,
}
