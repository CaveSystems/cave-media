// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FBIO : uint
{
    GET_VSCREENINFO = 0x4600,
    PUT_VSCREENINFO = 0x4601,
    GET_FSCREENINFO = 0x4602,
    GETCMAP = 0x4604,
    PUTCMAP = 0x4605,
    PAN_DISPLAY = 0x4606,
    CURSOR = 0x4608,
    GET_MONITORSPEC = 0x460C,
    PUT_MONITORSPEC = 0x460D,
    SWITCH_MONIBIT = 0x460E,
    GET_CON2FBMAP = 0x460F,
    PUT_CON2FBMAP = 0x4610,

    /// <summary>arg: 0 or vesa level + 1</summary>
    BLANK = 0x4611,

    GET_VBLANK = 0x4612,
    ALLOC = 0x4613,
    FREE = 0x4614,
    GET_GLYPH = 0x4615,
    GET_HWCINFO = 0x4616,
    PUT_MODEINFO = 0x4617,
    GET_DISPINFO = 0x4618,
    WAITFORVSYNC = 0x4620,
}
