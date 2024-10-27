// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct fb_pixmap
{
    /// <summary>pointer to memory</summary>
    /// <remarks>u8* addr;</remarks>
    public nuint addr;

    /// <summary>size of buffer in bytes</summary>
    public uint size;

    /// <summary>current offset to buffer</summary>
    public uint offset;

    /// <summary>byte alignment of each bitmap</summary>
    public uint buf_align;

    /// <summary>alignment per scanline</summary>
    public uint scan_align;

    /// <summary>alignment per read/write (bits)</summary>
    public uint access_align;

    /// <summary>
    /// see FB_PIXMAP_* supported bit block dimensions Format: test_bit(width - 1, blit_x) test_bit(height - 1, blit_y) if zero, will be set to full (all)
    /// </summary>
    public uint flags;

    /// <remarks>DECLARE_BITMAP(blit_x, FB_MAX_BLIT_WIDTH);</remarks>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 64 / 8)]
    public byte[] blit_x;

    /// <summary>access methods</summary>
    /// <remarks>DECLARE_BITMAP(blit_y, FB_MAX_BLIT_HEIGHT);</remarks>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 128 / 8)]
    public byte[] blit_y;

    /// <remarks>void (*writeio)(struct fb_info* info, void __iomem* dst, void* src, unsigned int size);</remarks>
    public fb_pixmap_writeio writeio;

    /// <remarks>void (*readio) (struct fb_info* info, void* dst, void __iomem* src, unsigned int size);</remarks>
    public fb_pixmap_readio readio;
};

public delegate void fb_pixmap_writeio(nuint fb_info, nuint iomem_dst, nuint src, nuint size);

public delegate void fb_pixmap_readio(nuint fb_info, nuint dst, nuint iomem_src, nuint size);
