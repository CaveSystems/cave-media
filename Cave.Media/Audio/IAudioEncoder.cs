using System.IO;

namespace Cave.Media.Audio;

/// <summary>
/// Provides an interface for audio encoder implementations.
/// </summary>
public interface IAudioEncoder
{
    /// <summary>
    /// Gets the encoder name.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets the description of the encoder.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets the features list.
    /// </summary>
    string Features { get; }

    /// <summary>
    /// Gets the mime types the encoder is able to produce.
    /// </summary>
    string[] MimeTypes { get; }

    /// <summary>
    /// Gets the available <see cref="IAudioConfiguration"/>s.
    /// </summary>
    /// <returns></returns>
    IAudioConfiguration[] GetAvailableConfigurations();

    /// <summary>
    /// Gets the available <see cref="IAudioEncoderQuality"/>s.
    /// </summary>
    /// <returns></returns>
    IAudioEncoderQuality[] GetAvailableQualitySettings();

    /// <summary>
    /// Starts the decoding process with the specified configuration and quality.
    /// </summary>
    /// <param name="configuration">The audio configuration to use.</param>
    /// <param name="quality">The quality setting to use.</param>
    /// <param name="targetStream">The Stream to write the encoded data to.</param>
    void BeginEncode(IAudioConfiguration configuration, IAudioEncoderQuality quality, Stream targetStream);

    /// <summary>
    /// Encodes a byte[] buffer.
    /// </summary>
    /// <param name="data">The byte[] buffer to encode.</param>
    void Encode(byte[] data);

    /// <summary>
    /// Finishes encoding and writes the last encoded byte[] buffer.
    /// </summary>
    void EndEncode();
}
