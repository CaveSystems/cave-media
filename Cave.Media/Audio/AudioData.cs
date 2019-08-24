using System;
using System.Runtime.CompilerServices;
using Cave.Collections;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a data buffer with audio specific informations (<see cref="IAudioConfiguration"/>.
    /// </summary>
    public class AudioData : AudioConfiguration, IAudioData
    {
        byte[] data;
        float? peak;

        /// <summary>Initializes a new instance of the <see cref="AudioData" /> class.</summary>
        /// <param name="samplingRate">SamplingRate.</param>
        /// <param name="channelSetup">Audio channel setup.</param>
        /// <param name="samples">The samples.</param>
        /// <param name="sampleCount">The sample count.</param>
        /// <exception cref="ArgumentNullException">Buffer.</exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException">ChannelSetup.</exception>
        public AudioData(int samplingRate, AudioChannelSetup channelSetup, float[] samples, int sampleCount)
            : base(samplingRate, AudioSampleFormat.Float, channelSetup)
        {
            if (samples == null)
            {
                throw new ArgumentNullException("samples");
            }

            if ((sampleCount > samples.Length) || ((sampleCount % BytesPerTick) != 0))
            {
                throw new Exception(string.Format("Buffer length invalid!"));
            }

            if (channelSetup == AudioChannelSetup.None)
            {
                throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            }

            SampleCount = sampleCount;
            data = new byte[BytesPerSample * sampleCount];
            Buffer.BlockCopy(samples, 0, data, 0, sampleCount * 4);
        }

        /// <summary>Initializes a new instance of the <see cref="AudioData"/> class.</summary>
        /// <param name="samplingRate">SamplingRate.</param>
        /// <param name="format">AudioSampleFormat.</param>
        /// <param name="channelSetup">Audio channel setup.</param>
        /// <param name="startTime">start time of the audio data.</param>
        /// <param name="streamIndex">stream index.</param>
        /// <param name="channelNumber">channel.</param>
        /// <param name="buffer">buffer containing the sample data.</param>
        public AudioData(int samplingRate, AudioSampleFormat format, AudioChannelSetup channelSetup, TimeSpan startTime, int streamIndex, int channelNumber, byte[] buffer)
            : base(samplingRate, format, channelSetup)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer");
            }

            if ((BytesPerTick != 0) && ((buffer.Length % BytesPerTick) != 0))
            {
                throw new Exception(string.Format("Buffer length invalid!"));
            }

            if (channelSetup == AudioChannelSetup.None)
            {
                throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            }

            StreamIndex = streamIndex;
            data = buffer;
            StartTime = startTime;
            ChannelNumber = channelNumber;
            SampleCount = buffer.Length / BytesPerSample;
        }

        /// <summary>
        /// Creates a new AudioData instance.
        /// </summary>
        /// <param name="samplingRate">SamplingRate.</param>
        /// <param name="format">AudioSampleFormat.</param>
        /// <param name="channels">Audio channel count.</param>
        /// <param name="startTime">start time of the audio data.</param>
        /// <param name="streamIndex">stream index.</param>
        /// <param name="channelNumber">channel number.</param>
        /// <param name="buffer">buffer containing the sample data.</param>
        public AudioData(int samplingRate, AudioSampleFormat format, int channels, TimeSpan startTime, int streamIndex, int channelNumber, byte[] buffer)
            : base(samplingRate, format, channels)
        {
            if (buffer == null)
            {
                throw new ArgumentNullException("Buffer");
            }

            if ((BytesPerTick != 0) && ((buffer.Length % BytesPerTick) != 0))
            {
                throw new Exception(string.Format("Buffer length invalid!"));
            }

            if (channels <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            }

            StreamIndex = streamIndex;
            data = buffer;
            StartTime = startTime;
            ChannelNumber = channelNumber;
            SampleCount = buffer.Length / BytesPerSample;
        }

        /// <summary>Initializes a new instance of the <see cref="AudioData"/> class.</summary>
        /// <param name="config">The configuration.</param>
        /// <param name="data">The data.</param>
        public AudioData(IAudioConfiguration config, byte[] data)
            : base(config.SamplingRate, config.Format, config.Channels)
        {
            this.data = data;
            SampleCount = data.Length / BytesPerSample;
        }

        #region Peak calculation
        unsafe float CalculatePeakInt8()
        {
            int result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                byte* p = bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs((int)p[i]));
                }
            }
            return result / 128.0f;
        }

        unsafe float CalculatePeakInt16()
        {
            int result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                short* p = (short*)bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs((int)p[i]));
                }
            }
            return result / 32768.0f;
        }

        unsafe float CalculatePeakInt32()
        {
            uint result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                int* p = (int*)bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, (uint)Math.Abs(p[i]));
                }
            }
            return result / 2147483648f;
        }

        unsafe float CalculatePeakInt64()
        {
            ulong result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                long* p = (long*)bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, (ulong)Math.Abs(p[i]));
                }
            }
            return result / 9223372036854775808f;
        }

        unsafe float CalculatePeakFloat()
        {
            float result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                float* p = (float*)bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs(p[i]));
                }
            }
            return result;
        }

        unsafe float CalculatePeakDouble()
        {
            double result = 0;
            fixed (byte* bytePtr = &data[0])
            {
                double* p = (double*)bytePtr;
                for (int i = 0; i < SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs(p[i]));
                }
            }
            return (float)result;
        }
        #endregion

        #region Normalizer functions

        unsafe IAudioData NormalizeInt16(float factor)
        {
            byte[] result = (byte[])data.Clone();
#if DEBUG
            float overshoot = 0;
            float undershoot = 0;
#endif
            fixed (byte* bytePtr = &result[0])
            {
                short* p = (short*)bytePtr;
                int samples = data.Length / BytesPerSample;
                for (int i = 0; i < samples; i++)
                {
                    int v = (int)(p[i] * factor);
                    if (v > 32767)
                    {
#if DEBUG
                        overshoot = Math.Max(v, overshoot);
#endif
                        v = 32767;
                    }
                    else if (v < -32767)
                    {
#if DEBUG
                        undershoot = Math.Min(v, undershoot);
#endif
                        v = -32767;
                    }
                    p[i] = (short)v;
                }
            }
#if DEBUG
            if (overshoot != 0) Trace.WriteLine("AudioData: OverShoot = " + overshoot);
            if (undershoot != 0) Trace.WriteLine("AudioData: UnderShoot = " + undershoot);
#endif
            return new AudioData(SamplingRate, Format, ChannelSetup, StartTime, StreamIndex, ChannelNumber, result);
        }

        unsafe IAudioData NormalizeFloat(float factor)
        {
            byte[] result = (byte[])data.Clone();
#if DEBUG
            float overshoot = 0;
            float undershoot = 0;
#endif
            fixed (byte* bytePtr = &result[0])
            {
                int samples = data.Length / BytesPerSample;
                float* p = (float*)bytePtr;
                for (int i = 0; i < samples; i++)
                {
                    float v = p[i] * factor;
                    if (v > 1)
                    {
#if DEBUG
                        overshoot = Math.Max(v, overshoot);
#endif
                        v = 1;
                    }
                    else if (v < -1)
                    {
#if DEBUG
                        undershoot = Math.Min(v, undershoot);
#endif
                        v = -1;
                    }
                    p[i] = v;
                }
            }
#if DEBUG
            if (overshoot != 0) Trace.WriteLine("AudioData: OverShoot = " + overshoot);
            if (undershoot != 0) Trace.WriteLine("AudioData: UnderShoot = " + undershoot);
#endif
            return new AudioData(SamplingRate, Format, ChannelSetup, StartTime, StreamIndex, ChannelNumber, result);
        }

        #endregion

        #region IAudioData Member

        /// <summary>Gets the peak for this buffer.</summary>
        /// <value>The peak value (always positive, 0..1).</value>
        /// <exception cref="NotImplementedException">AudioSampleFormat.Int24.</exception>
        public float Peak
        {
            get
            {
                if (!peak.HasValue)
                {
                    switch (Format)
                    {
                        case AudioSampleFormat.Int8: peak = CalculatePeakInt8(); break;
                        case AudioSampleFormat.Int16: peak = CalculatePeakInt16(); break;
                        case AudioSampleFormat.Int32: peak = CalculatePeakInt32(); break;
                        case AudioSampleFormat.Int64: peak = CalculatePeakInt64(); break;
                        case AudioSampleFormat.Float: peak = CalculatePeakFloat(); break;
                        case AudioSampleFormat.Double: peak = CalculatePeakDouble(); break;
                        case AudioSampleFormat.Int24:
                        default: throw new NotImplementedException();
                    }
                }
                return peak.Value;
            }
        }

        /// <summary>Normalizes the buffer using the specified factor.</summary>
        /// <param name="factor">The normalization (scaling) factor.</param>
        /// <exception cref="NotImplementedException"></exception>
        public IAudioData Normalize(float factor)
        {
            switch (Format)
            {
                case AudioSampleFormat.Int16: return NormalizeInt16(factor);
                case AudioSampleFormat.Float: return NormalizeFloat(factor);
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the playtime of this buffer in seconds.</summary>
        /// <value>The playtime in seconds.</value>
        public float Seconds
        {
            get
            {
                return (float)data.Length / (BytesPerTick * SamplingRate);
            }
        }

        /// <summary>Gets the sample count (for all channels).</summary>
        /// <value>The sample count (for all channels).</value>
        public int SampleCount { get; private set; }

        /// <summary>Gets the length of the audio data in bytes.</summary>
        /// <value>The length in bytes.</value>
        public int Length { get { return data.Length; } }

        /// <summary>
        /// Gets the buffer containing the data (check <see cref="IAudioConfiguration.Format"/>,
        /// <see cref="IAudioConfiguration.SamplingRate"/> and <see cref="IAudioConfiguration.Channels"/>
        /// for more informations.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return (byte[])data.Clone();
            }
        }

        /// <summary>
        /// Stream index (&lt;0 = invalid or unknown index)<br/>
        /// If the audio source supports only one stream this is always set to 0.
        /// </summary>
        public int StreamIndex { get; private set; }

        /// <summary>
        /// Channel number (&lt;0 = invalid or unknown index).
        /// </summary>
        public int ChannelNumber {
/* Nicht gemergte Änderung aus Projekt "Cave.Media (net47)"
Vor:
            get { return m_ChannelNumber; }
Nach:
            get; private set; }
*/

/* Nicht gemergte Änderung aus Projekt "Cave.Media (net46)"
Vor:
            get { return m_ChannelNumber; }
Nach:
            get; private set; }
*/

/* Nicht gemergte Änderung aus Projekt "Cave.Media (net20)"
Vor:
            get { return m_ChannelNumber; }
Nach:
            get; private set; }
*/

/* Nicht gemergte Änderung aus Projekt "Cave.Media (net45)"
Vor:
            get { return m_ChannelNumber; }
Nach:
            get; private set; }
*/

/* Nicht gemergte Änderung aus Projekt "Cave.Media (net35)"
Vor:
            get { return m_ChannelNumber; }
Nach:
            get; private set; }
*/
 get; private set; }

        /// <summary>
        /// Gets the start time.
        /// </summary>
        public TimeSpan StartTime { get; private set; }

        /// <summary>
        /// Gets the duration.
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                return data.LongLength == 0
                    ? TimeSpan.Zero
                    : new TimeSpan(data.LongLength / BytesPerTick * TimeSpan.TicksPerSecond / SamplingRate);
            }
        }

        #endregion

        [MethodImpl(256)]
        short ToInt16(float f)
        {
            if (f > 32767)
            {
                return 32767;
            }

            return f < -32767 ? (short)-32767 : (short)f;
        }

        /// <summary>Converts the <see cref="AudioData"/> to <see cref="AudioSampleFormat.Int16"/>.</summary>
        /// <returns>Returns a new <see cref="AudioData"/> instance.</returns>
        /// <exception cref="NotImplementedException">"ToInt16() conversion is not implemented for AudioSampleFormat {0}.</exception>
        public unsafe IAudioData ConvertToInt16()
        {
            byte[] result = new byte[data.Length / BytesPerSample * 2];
            fixed (byte* ptrOut = &result[0]) fixed (byte* ptrIn = &data[0])
            {
                short* pOut = (short*)ptrOut;
                switch (Format)
                {
                    case AudioSampleFormat.Double:
                    {
                        double* pIn = (double*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = ToInt16((float)(pIn[i] * 32767.0));
                        }
                        break;
                    }
                    case AudioSampleFormat.Float:
                    {
                        float* pIn = (float*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = ToInt16(pIn[i] * 32767.0f);
                        }
                        break;
                    }
                    case AudioSampleFormat.Int64:
                    {
                        long* pIn = (long*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = ToInt16(pIn[i] / 9223372036854775808.0f);
                        }
                        break;
                    }
                    case AudioSampleFormat.Int32:
                    {
                        int* pIn = (int*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = ToInt16(pIn[i] / 2147483648.0f);
                        }
                        break;
                    }
                    case AudioSampleFormat.Int8:
                    {
                        short* pIn = (short*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = ToInt16(pIn[i] * 128.0f);
                        }
                        break;
                    }
                    default: throw new NotImplementedException(string.Format("Function is not implemented for AudioSampleFormat {0}", Format));
                }
            }
            return new AudioData(SamplingRate, AudioSampleFormat.Int16, ChannelSetup, StartTime, StreamIndex, ChannelNumber, result);
        }

        /// <summary>Converts the <see cref="AudioData"/> to <see cref="AudioSampleFormat.Float"/>.</summary>
        /// <returns>Returns a new <see cref="AudioData"/> instance.</returns>
        /// <exception cref="NotImplementedException">"ToInt16() conversion is not implemented for AudioSampleFormat {0}.</exception>
        public unsafe IAudioData ConvertToFloat()
        {
            byte[] result = new byte[data.Length / BytesPerSample * 4];
            fixed (byte* ptrOut = &result[0]) fixed (byte* ptrIn = &data[0])
            {
                float* pOut = (float*)ptrOut;
                switch (Format)
                {
                    case AudioSampleFormat.Double:
                    {
                        double* pIn = (double*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = (float)pIn[i];
                        }
                        break;
                    }
                    case AudioSampleFormat.Int64:
                    {
                        long* pIn = (long*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = pIn[i] / 9223372036854775808f;
                        }
                        break;
                    }
                    case AudioSampleFormat.Int32:
                    {
                        int* pIn = (int*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = pIn[i] / 2147483648f;
                        }
                        break;
                    }
                    case AudioSampleFormat.Int16:
                    {
                        int* pIn = (int*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = pIn[i] / 32768f;
                        }
                        break;
                    }
                    case AudioSampleFormat.Int8:
                    {
                        short* pIn = (short*)ptrIn;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            pOut[i] = pIn[i] / 256f;
                        }
                        break;
                    }
                    default: throw new NotImplementedException(string.Format("Function is not implemented for AudioSampleFormat {0}", Format));
                }
            }
            return new AudioData(SamplingRate, AudioSampleFormat.Int16, ChannelSetup, StartTime, StreamIndex, ChannelNumber, result);
        }

        /// <summary>Changes the volume.</summary>
        /// <param name="volume">The volume.</param>
        /// <returns></returns>
        public unsafe IAudioData ChangeVolume(float volume)
        {
            byte[] copy = (byte[])data.Clone();
            fixed (byte* ptr = &copy[0])
            {
                switch (Format)
                {
                    case AudioSampleFormat.Double:
                    {
                        double* p = (double*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            double d = *p * volume;
                            if (d > 1)
                            {
                                d = 1;
                            }
                            else if (d < -1)
                            {
                                d = -1;
                            }

                            *p = d;
                            p++;
                        }
                    }
                    break;
                    case AudioSampleFormat.Float:
                    {
                        float* p = (float*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            float f = *p * volume;
                            if (f > 1)
                            {
                                f = 1;
                            }
                            else if (f < -1)
                            {
                                f = -1;
                            }

                            *p = *p * volume;
                            p++;
                        }
                    }
                    break;
                    case AudioSampleFormat.Int64:
                    {
                        long* p = (long*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            float f = *p * volume;
                            if (f > long.MaxValue)
                            {
                                f = long.MaxValue;
                            }
                            else if (f < long.MinValue)
                            {
                                f = long.MinValue;
                            }

                            *p = (long)f;
                            p++;
                        }
                    }
                    break;
                    case AudioSampleFormat.Int32:
                    {
                        int* p = (int*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            float f = *p * volume;
                            if (f > int.MaxValue)
                            {
                                f = int.MaxValue;
                            }
                            else if (f < int.MinValue)
                            {
                                f = int.MinValue;
                            }

                            *p = (int)f;
                            p++;
                        }
                    }
                    break;
                    case AudioSampleFormat.Int16:
                    {
                        short* p = (short*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            float f = *p * volume;
                            if (f > short.MaxValue)
                            {
                                f = short.MaxValue;
                            }
                            else if (f < short.MinValue)
                            {
                                f = short.MinValue;
                            }

                            *p = (short)f;
                            p++;
                        }
                    }
                    break;
                    case AudioSampleFormat.Int8:
                    {
                        sbyte* p = (sbyte*)ptr;
                        for (int i = 0; i < SampleCount; i++)
                        {
                            float f = *p * volume;
                            if (f > sbyte.MaxValue)
                            {
                                f = sbyte.MaxValue;
                            }
                            else if (f < sbyte.MinValue)
                            {
                                f = sbyte.MinValue;
                            }

                            *p = (sbyte)f;
                            p++;
                        }
                    }
                    break;
                    default: throw new NotImplementedException(string.Format("Function is not implemented for AudioSampleFormat {0}", Format));
                }

            }
            return new AudioData(SamplingRate, Format, ChannelSetup, StartTime, StreamIndex, ChannelNumber, copy);
        }

        /// <summary>
        /// Determines whether the specified Object is equal to the current Object.
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public override bool Equals(object obj)
        {
            var other = obj as IAudioData;
            return other == null ? false : Equals(other) && DefaultComparer.Equals(other.Data, Data);
        }

        /// <summary>
        /// Gets the hash code of the buffer.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}
