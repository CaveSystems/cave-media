// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_MODE_IS
{
    UNKNOWN = 0,
    DETAILED = 1,
    STANDARD = 2,
    VESA = 4,
    CALCULATED = 8,
    FIRST = 16,
    FROM_VAR = 32,
}
