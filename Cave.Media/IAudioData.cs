using Cave.Media.Audio;
using System;

namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for audio data
    /// </summary>
    public interface IAudioData : IAudioConfiguration
    {
        /// <summary>
        /// Stream index (&lt;0 = invalid or unknown index)<br/>
        /// If the audio source supports only one stream this is always set to 0.
        /// </summary>
        int StreamIndex { get; }

        /// <summary>
        /// Channel number (&lt;0 = invalid or unknown index)
        /// </summary>
        int ChannelNumber { get; }

        /// <summary>
        /// Obtains the buffer containing the data (check <see cref="IAudioConfiguration.Format"/>,
        /// <see cref="IAudioConfiguration.SamplingRate"/> and <see cref="IAudioConfiguration.ChannelSetup"/>
        /// for more informations.
        /// </summary>
        byte[] Data { get; }

        /// <summary>Gets the length of the audio data in bytes, this always matches Data.Length!.</summary>
        /// <value>The length in bytes.</value>
        int Length { get; }

        /// <summary>
        /// Obtains the start time
        /// </summary>
        TimeSpan StartTime { get; }

        /// <summary>
        /// Obtains the duration
        /// </summary>
        TimeSpan Duration { get; }

        /// <summary>Gets the peak for this buffer.</summary>
        /// <value>The peak value (always positive).</value>
        float Peak { get; }

        /// <summary>Gets the playtime of this buffer in seconds.</summary>
        /// <value>The playtime in seconds.</value>
        float Seconds { get; }

        /// <summary>Gets the sample count.</summary>
        /// <value>The sample count.</value>
        int SampleCount { get; }

        /// <summary>Normalizes the buffer using the specified factor.</summary>
        /// <param name="factor">The normalization (scaling) factor.</param>
        IAudioData Normalize(float factor);

        /// <summary>Converts the <see cref="AudioData"/> to <see cref="AudioSampleFormat.Int16"/>.</summary>
        /// <remarks>This is currently only available for <see cref="AudioSampleFormat.Float"/> and <see cref="AudioSampleFormat.Double"/></remarks>
        /// <returns>Returns a new <see cref="AudioData"/> instance</returns>
        /// <exception cref="NotImplementedException">"ToInt16() conversion is not implemented for AudioSampleFormat {0}</exception>
        IAudioData ConvertToInt16();

        /// <summary>Changes the volume.</summary>
        /// <param name="volume">The volume.</param>
        /// <returns></returns>
        IAudioData ChangeVolume(float volume);
    }
}
