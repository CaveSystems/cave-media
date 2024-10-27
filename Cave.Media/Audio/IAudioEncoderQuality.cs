namespace Cave.Media.Audio;

/// <summary>
/// Provides an interface for quality settings for <see cref="IAudioEncoder"/> implementations.
/// </summary>
public interface IAudioEncoderQuality
{
    /// <summary>
    /// Gets the name of the quality setting.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description for the quality setting.
    /// </summary>
    string Description { get; }
}
