// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_CUR_SET
{
    IMAGE = 0x01,
    POS = 0x02,
    HOT = 0x04,
    CMAP = 0x08,
    SHAPE = 0x10,
    SIZE = 0x20,
    ALL = 0xFF,
}
