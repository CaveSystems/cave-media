// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_TYPE : uint
{
    /// <summary>
    /// Packed Pixels
    /// </summary>
    PACKED_PIXELS = 0,
    /// <summary>
    /// Non interleaved planes
    /// </summary>
    PLANES = 1,
    /// <summary>
    /// Interleaved planes
    /// </summary>
    INTERLEAVED_PLANES = 2,
    /// <summary>
    /// Text/attributes
    /// </summary>
    TEXT = 3,
    /// <summary>
    /// EGA/VGA planes
    /// </summary>
    VGA_PLANES = 4,
    /// <summary>
    /// Type identified by a V4L2 FOURCC
    /// </summary>
    FOURCC = 5,
}
