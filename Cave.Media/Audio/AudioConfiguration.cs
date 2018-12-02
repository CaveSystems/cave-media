using System;
namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a simple implementation of the <see cref="IAudioConfiguration"/> interface
    /// </summary>
    public class AudioConfiguration : IAudioConfiguration
    {
        int m_SamplingRate;
        AudioSampleFormat m_Format;
        AudioChannelSetup m_ChannelSetup;
        int m_BytesPerSample;
        int m_BytesPerTick;

        /// <summary>
        /// Creates a new AudioConfiguration instance
        /// </summary>
        /// <param name="samplingRate">The samplingrate to use</param>
        /// <param name="format">The format to use</param>
        /// <param name="channels">Number of channels</param>
        public AudioConfiguration(int samplingRate, AudioSampleFormat format, int channels)
            : this(samplingRate, format, (AudioChannelSetup)channels)
        {
        }

        /// <summary>
        /// Creates a new AudioConfiguration instance
        /// </summary>
        /// <param name="samplingRate">The samplingrate to use</param>
        /// <param name="format">The format to use</param>
        /// <param name="channelSetup">The channel configuration to use</param>
        public AudioConfiguration(int samplingRate, AudioSampleFormat format, AudioChannelSetup channelSetup)
        {
            m_SamplingRate = samplingRate;
            m_Format = format;
            m_ChannelSetup = channelSetup;

            switch (m_Format)
            {
                case AudioSampleFormat.Int8: m_BytesPerSample = 1; break;
                case AudioSampleFormat.Int16: m_BytesPerSample = 2; break;
                case AudioSampleFormat.Int24: m_BytesPerSample = 3; break;
                case AudioSampleFormat.Int32:
                case AudioSampleFormat.Float: m_BytesPerSample = 4; break;
                case AudioSampleFormat.Double: m_BytesPerSample = 8; break;
                case AudioSampleFormat.Unknown: m_BytesPerSample = 0; break;
                default: throw new NotImplementedException();
            }

            m_BytesPerTick = m_BytesPerSample;
            m_ChannelSetup = channelSetup;
            m_BytesPerTick *= (int)m_ChannelSetup;
        }

        #region IAudioConfiguration Member

        /// <summary>
        /// Obtains the sampling rate
        /// </summary>
        public int SamplingRate
        {
            get { return m_SamplingRate; }
        }

        /// <summary>
        /// Obtains the sample format
        /// </summary>
        public AudioSampleFormat Format
        {
            get { return m_Format; }
        }

        /// <summary>
        /// Obtains the channel configuration
        /// </summary>
        public AudioChannelSetup ChannelSetup
        {
            get { return m_ChannelSetup; }
        }

        /// <summary>
        /// Obtains the number of channels
        /// </summary>
        public int Channels
        {
            get { return (int)m_ChannelSetup; }
        }

        /// <summary>
        /// Obtains the bytes per sample (one channel)
        /// </summary>
        public int BytesPerSample
        {
            get { return m_BytesPerSample; }
        }

        /// <summary>
        /// Obtains the bytes per tick (one sample on all channels)
        /// </summary>
        public int BytesPerTick
        {
            get { return m_BytesPerTick; }
        }
        #endregion

        /// <summary>
        /// Provides a string describing the audio configuration
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return SamplingRate + " Hz, " + Format + ", " + ChannelSetup;
        }

        /// <summary>Gets the byte count for a specific duration.</summary>
        /// <param name="duration">The duration.</param>
        /// <returns></returns>
        public int GetByteCount(TimeSpan duration)
        {
            return (int)(duration.Ticks * SamplingRate / TimeSpan.TicksPerSecond * BytesPerTick);
        }

        /// <summary>
        /// Obtains the hash code for the audio configuration
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return (m_BytesPerTick ^ m_SamplingRate);
        }

        /// <summary>
        /// Checks for equality with another IAudioConfiguration
        /// </summary>
        /// <param name="other"></param>
        /// <returns></returns>
        public bool Equals(IAudioConfiguration other)
        {
            if (other == null) return false;
            return
                (other.SamplingRate == SamplingRate) &&
                (other.Format == Format) &&
                (other.Channels == Channels);
        }

        /// <summary>Determines whether the specified <see cref="object" />, is equal to this instance.</summary>
        /// <param name="obj">The <see cref="object" /> to compare with this instance.</param>
        /// <returns><c>true</c> if the specified <see cref="object" /> is equal to this instance; otherwise, <c>false</c>.</returns>
        public override bool Equals(object obj)
        {
            return Equals(obj as IAudioConfiguration);
        }
    }
}
