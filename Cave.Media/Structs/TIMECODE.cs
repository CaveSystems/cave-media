using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// Timecode structure.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct TIMECODE
{
    /// <summary>
    /// Framerate Frames/s.
    /// </summary>
    public short FrameRate;

    /// <summary>
    /// fractional frame. full scale is always 0x10000.
    /// </summary>
    public short FrameFract;

    /// <summary>
    /// For drop frame code, no lFrame values are skipped, so a drop frame timecode of 1:00:00;00 would be a lFrame value of 107892.
    /// </summary>
    public int Frames;
}
