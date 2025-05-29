// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct dmt_videomode
{
    public uint dmt_id;
    public uint std_2byte_code;
    public uint cvt_3byte_code;
    /// <remarks>const struct fb_videomode* mode;</remarks>
    public UIntPtr mode;
};
