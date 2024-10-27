// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_VISUAL
{
    /// <summary>Monochr. 1=Black 0=White</summary>
    MONO01 = 0,

    /// <summary>Monochr. 1=White 0=Black</summary>
    MONO10 = 1,

    /// <summary>True color</summary>
    TRUECOLOR = 2,

    /// <summary>Pseudo color (like atari)</summary>
    PSEUDOCOLOR = 3,

    /// <summary>Direct color</summary>
    DIRECTCOLOR = 4,

    /// <summary>Pseudo color readonly</summary>
    STATIC_PSEUDOCOLOR = 5,

    /// <summary>Visual identified by a V4L2 FOURCC</summary>
    FOURCC = 6,
}
