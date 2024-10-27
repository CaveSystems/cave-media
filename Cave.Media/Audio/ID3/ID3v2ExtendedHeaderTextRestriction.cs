namespace Cave.Media.Audio.ID3;

/// <summary>
/// Size and FrameCount restrictions.
/// </summary>
public enum ID3v2ExtendedHeaderTextRestriction
{
    /// <summary>
    /// Unlimited text length
    /// </summary>
    Unlimited = 0,

    /// <summary>
    /// up to 1 kB
    /// </summary>
    Big = 1,

    /// <summary>
    /// up to 128 bytes
    /// </summary>
    Small = 2,

    /// <summary>
    /// &lt;= 30 bytes
    /// </summary>
    Tiny = 3,
}
