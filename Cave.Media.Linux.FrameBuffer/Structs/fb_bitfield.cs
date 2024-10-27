// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System.Runtime.InteropServices;

namespace Cave.Media.Linux.FrameBuffer.Structs;

/// <summary>
/// Interpretation of offset for color fields: All offsets are from the right, inside a "pixel" value, which is exactly 'bits_per_pixel' wide (means: you can
/// use the offset as right argument to &lt;&lt;). A pixel afterwards is a bit stream and is written to video memory as that unmodified. For pseudocolor: offset
/// and length should be the same for all color components. Offset specifies the position of the least significant bit of the palette index in a pixel value.
/// Length indicates the number of available palette entries (i.e. # of entries = 1 &lt;&lt; length).
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1, CharSet = CharSet.Ansi)]
public struct fb_bitfield
{
    /// <summary>beginning of bitfield</summary>
    public uint offset;

    /// <summary>length of bitfield</summary>
    public uint length;

    /// <summary>!= 0 : Most significant bit is right</summary>
    public uint msb_right;
};
