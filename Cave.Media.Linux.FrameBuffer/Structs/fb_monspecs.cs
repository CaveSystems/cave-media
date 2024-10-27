// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_monspecs
{
    public fb_chroma chroma;

    /// <summary>mode database</summary>
    public nuint modedb;

    /// <summary>Manufacturer</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public byte[] manufacturer;

    /// <summary>Monitor String</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] monitor;

    /// <summary>Serial Number</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] serial_no;

    /// <summary>?</summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 14)]
    public byte[] ascii;

    /// <summary>mode database length</summary>
    public uint modedb_len;

    /// <summary>Monitor Model</summary>
    public uint model;

    /// <summary>Serial Number - Integer</summary>
    public uint serial;

    /// <summary>Year manufactured</summary>
    public uint year;

    /// <summary>Week Manufactured</summary>
    public uint week;

    /// <summary>hfreq lower limit (Hz)</summary>
    public uint hfmin;

    /// <summary>hfreq upper limit (Hz)</summary>
    public uint hfmax;

    /// <summary>pixelclock lower limit (Hz)</summary>
    public uint dclkmin;

    /// <summary>pixelclock upper limit (Hz)</summary>
    public uint dclkmax;

    /// <summary>display type - see FB_DISP_*</summary>
    public ushort input;

    /// <summary>DPMS support - see FB_DPMS_</summary>
    public ushort dpms;

    /// <summary>Signal Type - see FB_SIGNAL_*</summary>
    public ushort signal;

    /// <summary>vfreq lower limit (Hz)</summary>
    public ushort vfmin;

    /// <summary>vfreq upper limit (Hz)</summary>
    public ushort vfmax;

    /// <summary>Gamma - in fractions of 100</summary>
    public ushort gamma;

    /// <summary>supports GTF</summary>
    public ushort gtf;

    /// <summary>Misc flags - see FB_MISC_*</summary>
    public ushort misc;

    /// <summary>EDID version...</summary>
    public byte version;

    /// <summary>...and revision</summary>
    public byte revision;

    /// <summary>Maximum horizontal size (cm)</summary>
    public byte max_x;

    /// <summary>Maximum vertical size (cm)</summary>
    public byte max_y;
};
