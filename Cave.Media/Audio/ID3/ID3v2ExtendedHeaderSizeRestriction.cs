namespace Cave.Media.Audio.ID3;

/// <summary>
/// Size and FrameCount restrictions.
/// </summary>
public enum ID3v2ExtendedHeaderSizeRestriction
{
    /// <summary>
    /// maximum of 1 MB size and 128 frames
    /// </summary>
    Mega = 0,

    /// <summary>
    /// maximum of 128 kB size and 64 frames
    /// </summary>
    Big = 1,

    /// <summary>
    /// maximum of 40 kB size and 32 frames
    /// </summary>
    Small = 2,

    /// <summary>
    /// maximum of 4 kB size and 32 frames
    /// </summary>
    Tiny = 3,
}
