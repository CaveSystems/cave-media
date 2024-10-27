// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_SYNC
{
    /// <summary>horizontal sync high active</summary>
    HOR_HIGH_ACT = 1,

    /// <summary>vertical sync high active</summary>
    VERT_HIGH_ACT = 2,

    /// <summary>external sync</summary>
    EXT = 4,

    /// <summary>composite sync high active</summary>
    COMP_HIGH_ACT = 8,

    /// <summary>broadcast video timings</summary>
    BROADCAST = 16,

    /// <summary>sync on green</summary>
    ON_GREEN = 32,
}
