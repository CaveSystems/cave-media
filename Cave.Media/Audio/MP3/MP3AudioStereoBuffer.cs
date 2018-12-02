#region Notes
/*
    This is part of the MP3Decoder implementation.
    Based upon mpg123, jlayer 1.0.1 and some of the conversions done for mp3sharp by rob burke.
    I only converted the layer 3 decoder since we don't need anything else in our projects.
    Wherever possible everything was cleaned up and done the .net / CaveProjects way.
*/
#endregion

using System;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Audio data buffer for the MP3 decoder
    /// </summary>
    public sealed class MP3AudioStereoBuffer
    {
        readonly float[] m_Buffer = new float[1152 * 2];
        readonly int[] m_Index = new int[2];
        int m_SamplingRate;

        /// <summary>Gets the sample count.</summary>
        /// <value>The sample count.</value>
        public int SampleCount { get { return m_Index[0]; } }

        /// <summary>Initializes a new instance of the <see cref="MP3AudioStereoBuffer"/> class.</summary>
        public MP3AudioStereoBuffer(int samplingRate)
        {
            m_SamplingRate = samplingRate;
            Clear();
        }

        /// <summary>Clears this instance.</summary>
        public void Clear()
        {
            m_Index[0] = 0;
            m_Index[1] = 1;
        }

        /// <summary>Adds samples.</summary>
        /// <param name="channelNumber">The channel number.</param>
        /// <param name="samples">The samples.</param>
        public void AddSamples(int channelNumber, float[] samples)
        {
            int index = m_Index[channelNumber];
            for (int i = 0; i < samples.Length; i++)
            {
                float value = samples[i];
                m_Buffer[index] = value;
                index += 2;
            }
            m_Index[channelNumber] = index;
        }

        /// <summary>Retries the sample byte buffer as array.</summary>
        /// <returns></returns>
        public IAudioData GetAudioData()
        {
            return new AudioData(m_SamplingRate, AudioChannelSetup.Stereo, m_Buffer, SampleCount);
        }
    }
}