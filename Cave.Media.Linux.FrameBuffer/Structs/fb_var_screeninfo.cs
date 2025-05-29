// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;
using Cave.Media.Linux.FrameBuffer.Enums;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_var_screeninfo
{
    /// <summary>visible resolution</summary>
    public uint xres;

    /// <summary>visible resolution</summary>
    public uint yres;

    /// <summary>virtual resolution</summary>
    public uint xres_virtual;

    /// <summary>virtual resolution</summary>
    public uint yres_virtual;

    /// <summary>offset from virtual to visible</summary>
    public uint xoffset;

    /// <summary>resolution</summary>
    public uint yoffset;

    /// <summary>guess what</summary>
    public uint bits_per_pixel;

    /// <summary>0 = color, 1 = grayscale, &gt;1 = FOURCC</summary>
    public uint grayscale;

    /// <summary>bitfield in fb mem if true color, else only length is significant</summary>
    public fb_bitfield red;

    /// <summary>bitfield in fb mem if true color, else only length is significant</summary>
    public fb_bitfield green;

    /// <summary>bitfield in fb mem if true color, else only length is significant</summary>
    public fb_bitfield blue;

    /// <summary>transparency</summary>
    public fb_bitfield transp;

    /// <summary>!= 0 Non standard pixel format</summary>
    public uint nonstd;

    /// <summary>see FB_ACTIVATE_*</summary>
    [MarshalAs(UnmanagedType.U4)]
    public FB_ACTIVATE activate;

    /// <summary>height of picture in mm</summary>
    public uint height;

    /// <summary>width of picture in mm</summary>
    public uint width;

    /// <summary>(OBSOLETE) see fb_info.flags Timing: All values in pixclocks, except pixclock (of course)</summary>
    public uint accel_flags;

    /// <summary>pixel clock in ps (pico seconds)</summary>
    public uint pixclock;

    /// <summary>time from sync to picture</summary>
    public uint left_margin;

    /// <summary>time from picture to sync</summary>
    public uint right_margin;

    /// <summary>time from sync to picture</summary>
    public uint upper_margin;

    public uint lower_margin;

    /// <summary>length of horizontal sync</summary>
    public uint hsync_len;

    /// <summary>length of vertical sync</summary>
    public uint vsync_len;

    /// <summary>see FB_SYNC_*</summary>
    [MarshalAs(UnmanagedType.U4)]
    public FB_SYNC sync;

    /// <summary>see FB_VMODE_*</summary>
    [MarshalAs(UnmanagedType.U4)]
    public FB_VMODE vmode;

    /// <summary>angle we rotate counter clockwise</summary>
    public uint rotate;

    /// <summary>colorspace for FOURCC-based modes</summary>
    public uint colorspace;

    /// <summary>Reserved for future compatibility</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public uint[] reserved;
};
