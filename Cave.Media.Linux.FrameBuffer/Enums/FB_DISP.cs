// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux.FrameBuffer.Enums;

[Flags]
public enum FB_DISP { DDI = 1, ANA_700_300 = 2, ANA_714_286 = 4, ANA_1000_400 = 8, ANA_700_000 = 16, MONO = 32, RGB = 64, MULTI = 128, UNKNOWN = 256 }
