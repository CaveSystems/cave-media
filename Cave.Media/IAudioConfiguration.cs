using System;

namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for audio configurations
    /// </summary>
    
    public interface IAudioConfiguration : IEquatable<IAudioConfiguration>
    {
        /// <summary>
        /// Obtains the sampling rate
        /// </summary>
        int SamplingRate { get; }

        /// <summary>
        /// Obtains the sample format
        /// </summary>
        AudioSampleFormat Format { get; }

        /// <summary>
        /// Obtains the channel configuration
        /// </summary>
        AudioChannelSetup ChannelSetup { get; }

        /// <summary>
        /// Obtains the number of channels
        /// </summary>
        int Channels { get; }

        /// <summary>
        /// Obtains the bytes per sample (one channel)
        /// </summary>
        int BytesPerSample { get; }

        /// <summary>
        /// Obtains the bytes per tick (one sample on all channels)
        /// </summary>
        int BytesPerTick { get; }

        /// <summary>Gets the byte count for a specific duration.</summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        int GetByteCount(TimeSpan duration);
    }
}
