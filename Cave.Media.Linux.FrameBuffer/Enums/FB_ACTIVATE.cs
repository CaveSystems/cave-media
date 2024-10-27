// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux.FrameBuffer.Enums;

[Flags]
public enum FB_ACTIVATE
{
    /// <summary>set values immediately (or vbl)</summary>
    NOW = 0,
    /// <summary>activate on next open</summary>
    NXTOPEN = 1,
    /// <summary>don't set, round up impossible</summary>
    TEST = 2,
    MASK = 15,
    /// <summary>activate values on next vbl</summary>
    VBL = 16,
    /// <summary>change colormap on vbl</summary>
    CHANGE_CMAP_VBL = 32,
    /// <summary>change all VCs on this fb</summary>
    ALL = 64,
    /// <summary>force apply even when no change</summary>
    FORCE = 128,
    /// <summary>invalidate videomode</summary>
    INV_MODE = 256,
    /// <summary>for KDSET vt ioctl</summary>
    KD_TEXT = 512,
}
