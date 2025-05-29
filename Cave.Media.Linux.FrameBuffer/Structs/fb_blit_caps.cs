// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_blit_caps
{
    /// <remarks>DECLARE_BITMAP(x, FB_MAX_BLIT_WIDTH);</remarks>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 8)]
    public byte[] x;

    /// <remarks>DECLARE_BITMAP(y, FB_MAX_BLIT_HEIGHT);</remarks>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 16)]
    public byte[] y;

    public uint len;
    public uint flags;
};
