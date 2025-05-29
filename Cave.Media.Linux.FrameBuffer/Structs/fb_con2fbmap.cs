// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
struct fb_con2fbmap
{
    public uint console;
    public uint framebuffer;
};
