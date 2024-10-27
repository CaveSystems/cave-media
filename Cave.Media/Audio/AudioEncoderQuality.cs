namespace Cave.Media.Audio;

/// <summary>
/// Implements an abstract base class for the <see cref="IAudioEncoderQuality"/> interface.
/// </summary>
public abstract class AudioEncoderQuality : IAudioEncoderQuality
{
    /// <summary>
    /// Gets the name of the quality setting.
    /// </summary>
    public abstract string Name { get; }

    /// <summary>
    /// Gets the description for the quality setting.
    /// </summary>
    public abstract string Description { get; }
}
