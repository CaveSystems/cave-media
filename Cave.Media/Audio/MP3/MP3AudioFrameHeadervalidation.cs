namespace Cave.Media.Audio.MP3;

/// <summary>
/// Provides available header validation codes.
/// </summary>
public enum MP3AudioFrameHeadervalidation
{
    /// <summary>
    /// Valid header
    /// </summary>
    Valid = 0,

    /// <summary>
    /// Invalid header marker
    /// </summary>
    InvalidHeader = 1,

    /// <summary>
    /// Invalid Mpeg version
    /// </summary>
    InvalidVersion = 2,

    /// <summary>
    /// Invalid BitRate
    /// </summary>
    InvalidBitRate = 3,

    /// <summary>
    /// Invalid Mpeg Layer
    /// </summary>
    InvalidLayer = 4,

    /// <summary>
    /// Invalid SampingRate
    /// </summary>
    InvalidSampleRate = 5,
}
