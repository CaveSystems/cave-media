// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FBMON
{
    MAXTIMINGS = 0,
    VSYNCTIMINGS = 1,
    HSYNCTIMINGS = 2,
    DCLKTIMINGS = 3,
    IGNOREMON = 0x100,
}
