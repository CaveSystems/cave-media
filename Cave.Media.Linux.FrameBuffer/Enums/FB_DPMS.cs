// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux.FrameBuffer.Enums;

[Flags]
public enum FB_DPMS { ACTIVE_OFF = 1, SUSPEND = 2, STANDBY = 4, }
