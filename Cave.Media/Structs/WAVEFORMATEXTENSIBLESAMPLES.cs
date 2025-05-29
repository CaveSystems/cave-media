using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// Provides access to the union at <see cref="WAVEFORMATEXTENSIBLE"/> Format.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 2)]
public struct WAVEFORMATEXTENSIBLESAMPLES
{
    /// <summary>
    /// bits of precision (BitsPerSample != 0).
    /// </summary>
    [FieldOffset(0)]
    public ushort ValidBitsPerSample;

    /// <summary>
    /// (BitsPerSample == 0).
    /// </summary>
    [FieldOffset(0)]
    public ushort SamplesPerBlock;

    /// <summary>
    /// If neither applies, set to zero.
    /// </summary>
    [FieldOffset(0)]
    public ushort Reserved;
}
