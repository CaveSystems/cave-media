using System;

namespace Cave.Media.Audio.MP3;

/// <summary>
/// Provides an abstract base class for (valid) mp3 meta frames.
/// </summary>
public abstract class MP3MetaFrame : AudioFrame
{
    #region public properties

    /// <summary>
    /// This tag is not an audio tag.
    /// </summary>
    public override bool IsAudio => false;

    /// <summary>
    /// Tag is valid.
    /// </summary>
    public override bool IsValid => true;

    /// <summary>
    /// Duration of this tag is <see cref="TimeSpan.Zero"/>.
    /// </summary>
    public override TimeSpan Duration => TimeSpan.Zero;

    #endregion
}
