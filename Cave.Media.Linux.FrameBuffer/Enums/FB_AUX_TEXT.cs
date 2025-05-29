// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_AUX_TEXT
{
    /// <summary>Monochrome text</summary>
    MDA = 0,

    /// <summary>CGA/EGA/VGA Color text</summary>
    CGA = 1,

    /// <summary>S3 MMIO fasttext</summary>
    S3_MMIO = 2,

    /// <summary>MGA Millenium I: text, attr, 14 reserved bytes</summary>
    MGA_STEP16 = 3,

    /// <summary>other MGAs: text, attr, 6 reserved bytes</summary>
    MGA_STEP8 = 4,

    /// <summary>8-15: SVGA tileblit compatible modes</summary>
    SVGA_GROUP = 8,

    /// <summary>lower three bits says step</summary>
    SVGA_MASK = 7,

    /// <summary>SVGA text mode: text, attr</summary>
    SVGA_STEP2 = 8,

    /// <summary>SVGA text mode: text, attr, 2 reserved bytes</summary>
    SVGA_STEP4 = 9,

    /// <summary>SVGA text mode: text, attr, 6 reserved bytes</summary>
    SVGA_STEP8 = 10,

    /// <summary>SVGA text mode: text, attr, 14 reserved bytes</summary>
    SVGA_STEP16 = 11,

    /// <summary>reserved up to 15</summary>
    SVGA_LAST = 15,
}
