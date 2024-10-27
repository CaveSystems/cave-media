// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FB_EVENT
{
    /// <summary>The resolution of the passed in fb_info about to change</summary>
    MODE_CHANGE = 0x01,
    FB_REGISTERED = 0x05,
    FB_UNREGISTERED = 0x06,
    /// <summary>A display blank is requested</summary>
    BLANK = 0x09,
}
