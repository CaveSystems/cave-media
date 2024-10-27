using System;

namespace Cave.Media;

/// <summary>
/// Provides an interface for audio configurations.
/// </summary>
public interface IAudioConfiguration : IEquatable<IAudioConfiguration>
{
    /// <summary>
    /// Gets the sampling rate.
    /// </summary>
    int SamplingRate { get; }

    /// <summary>
    /// Gets the sample format.
    /// </summary>
    AudioSampleFormat Format { get; }

    /// <summary>
    /// Gets the channel configuration.
    /// </summary>
    AudioChannelSetup ChannelSetup { get; }

    /// <summary>
    /// Gets the number of channels.
    /// </summary>
    int Channels { get; }

    /// <summary>
    /// Gets the bytes per sample (one channel).
    /// </summary>
    int BytesPerSample { get; }

    /// <summary>
    /// Gets the bytes per tick (one sample on all channels).
    /// </summary>
    int BytesPerTick { get; }

    /// <summary>Gets the byte count for a specific duration.</summary>
    /// <param name="duration">The duration.</param>
    /// <returns></returns>
    int GetByteCount(TimeSpan duration);
}
