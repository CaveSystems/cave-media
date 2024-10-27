// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux.FrameBuffer.Enums;

[Flags]
public enum FB_SIGNAL { NONE = 0, BLANK_BLANK = 1, SEPARATE = 2, COMPOSITE = 4, SYNC_ON_GREEN = 8, SERRATION_ON = 16 }
