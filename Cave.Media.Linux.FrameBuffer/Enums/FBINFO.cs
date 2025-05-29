// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

namespace Cave.Media.Linux.FrameBuffer.Enums;

public enum FBINFO
{
    /// <summary>use tile blitting</summary>
    MISC_TILEBLITTING = 0x20000,
    /// <summary>A driver may set this flag to indicate that it does want a set_par to be called every time when fbcon_switch is executed. The advantage is that with this flag set you can really be sure that set_par is always called before any of the functions dependent on the correct hardware state or altering that state, even if you are using some broken X releases. The disadvantage is that it introduces unwanted delays to every console switch if set_par is slow. It is a good idea to try this flag in the drivers initialization code whenever there is a bug report related to switching between X and the framebuffer console.</summary>
    MISC_ALWAYS_SETPAR = 0x40000,
    /// <summary>Host and GPU endianness differ.</summary>
    FOREIGN_ENDIAN = 0x100000,
    /// <summary>Big endian math. This is the same flags as above, but with different and host endianness. Drivers should not use this flag.</summary>
    BE_MATH = 0x100000,
    /// <summary>Hide smem_start in the FBIOGET_FSCREENINFO IOCTL. This is used by modern DRM drivers to stop userspace from trying to share buffers behind the kernel's back. Instead dma-buf based buffer sharing should be used.</summary>
    HIDE_SMEM_START = 0x200000,
}
