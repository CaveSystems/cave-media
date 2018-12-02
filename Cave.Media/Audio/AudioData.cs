using Cave.Collections;
using System;
using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a data buffer with audio specific informations (<see cref="IAudioConfiguration"/>
    /// </summary>

    public class AudioData : AudioConfiguration, IAudioData
    {
        byte[] m_Data;
        TimeSpan m_StartTime;
        int m_StreamIndex;
        int m_ChannelNumber;
        int m_SampleCount;
        float? m_Peak;

        /// <summary>Initializes a new instance of the <see cref="AudioData" /> class.</summary>
        /// <param name="samplingRate">SamplingRate</param>
        /// <param name="channelSetup">Audio channel setup</param>
        /// <param name="samples">The samples.</param>
        /// <param name="sampleCount">The sample count.</param>
        /// <exception cref="ArgumentNullException">Buffer</exception>
        /// <exception cref="Exception"></exception>
        /// <exception cref="ArgumentOutOfRangeException">ChannelSetup</exception>
        public AudioData(int samplingRate, AudioChannelSetup channelSetup, float[] samples, int sampleCount)
            : base(samplingRate, AudioSampleFormat.Float, channelSetup)
        {
            if (samples == null) throw new ArgumentNullException("samples");
            if ((sampleCount > samples.Length) || ((sampleCount % BytesPerTick) != 0)) throw new Exception(string.Format("Buffer length invalid!"));
            if (channelSetup == AudioChannelSetup.None) throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            m_SampleCount = sampleCount;
            m_Data = new byte[BytesPerSample * sampleCount];
            Buffer.BlockCopy(samples, 0, m_Data, 0, sampleCount * 4);
        }

        /// <summary>Initializes a new instance of the <see cref="AudioData"/> class.</summary>
        /// <param name="samplingRate">SamplingRate</param>
        /// <param name="format">AudioSampleFormat</param>
        /// <param name="channelSetup">Audio channel setup</param>
        /// <param name="startTime">start time of the audio data</param>
        /// <param name="streamIndex">stream index</param>
        /// <param name="channelNumber">channel</param>
        /// <param name="buffer">buffer containing the sample data</param>
        public AudioData(int samplingRate, AudioSampleFormat format, AudioChannelSetup channelSetup, TimeSpan startTime, int streamIndex, int channelNumber, byte[] buffer)
            : base(samplingRate, format, channelSetup)
        {
            if (buffer == null) throw new ArgumentNullException("Buffer");
            if ((BytesPerTick != 0) && ((buffer.Length % BytesPerTick) != 0)) throw new Exception(string.Format("Buffer length invalid!"));
            if (channelSetup == AudioChannelSetup.None) throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            m_StreamIndex = streamIndex;
            m_Data = buffer;
            m_StartTime = startTime;
            m_ChannelNumber = channelNumber;
            m_SampleCount = buffer.Length / BytesPerSample;
        }

        /// <summary>
        /// Creates a new AudioData instance
        /// </summary>
        /// <param name="samplingRate">SamplingRate</param>
        /// <param name="format">AudioSampleFormat</param>
        /// <param name="channels">Audio channel count</param>
        /// <param name="startTime">start time of the audio data</param>
        /// <param name="streamIndex">stream index</param>
        /// <param name="channelNumber">channel number</param>
        /// <param name="buffer">buffer containing the sample data</param>
        public AudioData(int samplingRate, AudioSampleFormat format, int channels, TimeSpan startTime, int streamIndex, int channelNumber, byte[] buffer)
            : base(samplingRate, format, channels)
        {
            if (buffer == null) throw new ArgumentNullException("Buffer");
            if ((BytesPerTick != 0) && ((buffer.Length % BytesPerTick) != 0)) throw new Exception(string.Format("Buffer length invalid!"));
            if (channels <= 0) throw new ArgumentOutOfRangeException(nameof(ChannelSetup));
            m_StreamIndex = streamIndex;
            m_Data = buffer;
            m_StartTime = startTime;
            m_ChannelNumber = channelNumber;
            m_SampleCount = buffer.Length / BytesPerSample;
        }

        /// <summary>Initializes a new instance of the <see cref="AudioData"/> class.</summary>
        /// <param name="config">The configuration.</param>
        /// <param name="data">The data.</param>
        public AudioData(IAudioConfiguration config, byte[] data)
            : base(config.SamplingRate, config.Format, config.Channels)
        {
            m_Data = data;
            m_SampleCount = data.Length / BytesPerSample;
        }

        #region Peak calculation
        unsafe float CalculatePeakInt8()
        {
            int result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                byte* p = bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs((int)p[i]));
                }
            }
            return result / 128.0f;
        }

        unsafe float CalculatePeakInt16()
        {
            int result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                short* p = (short*)bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs((int)p[i]));
                }
            }
            return result / 32768.0f;
        }

        unsafe float CalculatePeakInt32()
        {
            uint result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                int* p = (int*)bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
                {
                    result = Math.Max(result, (uint)Math.Abs(p[i]));
                }
            }
            return result / 2147483648f;
        }

        unsafe float CalculatePeakInt64()
        {
            ulong result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                long* p = (long*)bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
                {
                    result = Math.Max(result, (ulong)Math.Abs(p[i]));
                }
            }
            return result / 9223372036854775808f;
        }

        unsafe float CalculatePeakFloat()
        {
            float result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                float* p = (float*)bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
                {
                    result = Math.Max(result, Math.Abs(p[i]));
                }
            }
            return result;
        }

        unsafe float CalculatePeakDouble()
        {
            double result = 0;
            fixed (byte* bytePtr = &m_Data[0])
            {
                double* p = (double*)bytePtr;
                for (int i = 0; i < m_SampleCount; i++)
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
            byte[] result = (byte[])m_Data.Clone();
#if DEBUG
            float overshoot = 0;
            float undershoot = 0;
#endif
            fixed (byte* bytePtr = &result[0])
            {
                short* p = (short*)bytePtr;
                int samples = m_Data.Length / BytesPerSample;
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
            byte[] result = (byte[])m_Data.Clone();
#if DEBUG
            float overshoot = 0;
            float undershoot = 0;
#endif
            fixed (byte* bytePtr = &result[0])
            {
                int samples = m_Data.Length / BytesPerSample;
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
        /// <exception cref="NotImplementedException">AudioSampleFormat.Int24</exception>
        public float Peak
        {
            get
            {
                if (!m_Peak.HasValue)
                {
                    switch (Format)
                    {
                        case AudioSampleFormat.Int8: m_Peak = CalculatePeakInt8(); break;
                        case AudioSampleFormat.Int16: m_Peak = CalculatePeakInt16(); break;
                        case AudioSampleFormat.Int32: m_Peak = CalculatePeakInt32(); break;
                        case AudioSampleFormat.Int64: m_Peak = CalculatePeakInt64(); break;
                        case AudioSampleFormat.Float: m_Peak = CalculatePeakFloat(); break;
                        case AudioSampleFormat.Double: m_Peak = CalculatePeakDouble(); break;
                        case AudioSampleFormat.Int24:
                        default: throw new NotImplementedException();
                    }
                }
                return m_Peak.Value;
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
                return (float)m_Data.Length / (BytesPerTick * SamplingRate);
            }
        }

        /// <summary>Gets the sample count (for all channels).</summary>
        /// <value>The sample count (for all channels).</value>
        public int SampleCount { get { return m_SampleCount; } }

        /// <summary>Gets the length of the audio data in bytes.</summary>
        /// <value>The length in bytes.</value>
        public int Length { get { return m_Data.Length; } }

        /// <summary>
        /// Obtains the buffer containing the data (check <see cref="IAudioConfiguration.Format"/>,
        /// <see cref="IAudioConfiguration.SamplingRate"/> and <see cref="IAudioConfiguration.Channels"/>
        /// for more informations.
        /// </summary>
        public byte[] Data
        {
            get
            {
                return (byte[])m_Data.Clone();
            }
        }

        /// <summary>
        /// Stream index (&lt;0 = invalid or unknown index)<br/>
        /// If the audio source supports only one stream this is always set to 0.
        /// </summary>
        public int StreamIndex
        {
            get { return m_StreamIndex; }
        }

        /// <summary>
        /// Channel number (&lt;0 = invalid or unknown index)
        /// </summary>
        public int ChannelNumber
        {
            get { return m_ChannelNumber; }
        }

        /// <summary>
        /// Obtains the start time
        /// </summary>
        public TimeSpan StartTime
        {
            get
            {
                return m_StartTime;
            }
        }

        /// <summary>
        /// Obtains the duration
        /// </summary>
        public TimeSpan Duration
        {
            get
            {
                if (m_Data.LongLength == 0) return TimeSpan.Zero;
                return new TimeSpan(((m_Data.LongLength / BytesPerTick) * TimeSpan.TicksPerSecond) / SamplingRate);
            }
        }

        #endregion

        [MethodImpl(256)]
        short ToInt16(float f)
        {
            if (f > 32767) return 32767;
            if (f < -32767) return -32767;
            return (short)f;
        }

        /// <summary>Converts the <see cref="AudioData"/> to <see cref="AudioSampleFormat.Int16"/>.</summary>
        /// <returns>Returns a new <see cref="AudioData"/> instance</returns>
        /// <exception cref="NotImplementedException">"ToInt16() conversion is not implemented for AudioSampleFormat {0}</exception>
        public unsafe IAudioData ConvertToInt16()
        {
            byte[] result = new byte[m_Data.Length / BytesPerSample * 2];
            fixed (byte* ptrOut = &result[0]) fixed (byte* ptrIn = &m_Data[0])
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
        /// <returns>Returns a new <see cref="AudioData"/> instance</returns>
        /// <exception cref="NotImplementedException">"ToInt16() conversion is not implemented for AudioSampleFormat {0}</exception>
        public unsafe IAudioData ConvertToFloat()
        {
            byte[] result = new byte[m_Data.Length / BytesPerSample * 4];
            fixed (byte* ptrOut = &result[0]) fixed (byte* ptrIn = &m_Data[0])
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
            byte[] copy = (byte[])m_Data.Clone();
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
                                if (d > 1) d = 1; else if (d < -1) d = -1;
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
                                if (f > 1) f = 1; else if (f < -1) f = -1;
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
                                float f = (*p * volume);
                                if (f > Int64.MaxValue) f = Int64.MaxValue; else if (f < Int64.MinValue) f = Int64.MinValue;
                                * p = (long)f;
                                p++;
                            }
                        }
                        break;
                    case AudioSampleFormat.Int32:
                        {
                            int* p = (int*)ptr;
                            for (int i = 0; i < SampleCount; i++)
                            {
                                float f = (*p * volume);
                                if (f > Int32.MaxValue) f = Int32.MaxValue; else if (f < Int32.MinValue) f = Int32.MinValue;
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
                                float f = (*p * volume);
                                if (f > Int16.MaxValue) f = Int16.MaxValue; else if (f < Int16.MinValue) f = Int16.MinValue;
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
                                float f = (*p * volume);
                                if (f > SByte.MaxValue) f = SByte.MaxValue; else if (f < SByte.MinValue) f = SByte.MinValue;
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
            IAudioData other = obj as IAudioData;
            if (other == null) return false;
            return Equals(other) && DefaultComparer.Equals(other.Data, Data);
        }

        /// <summary>
        /// Obtains the hash code of the buffer
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return Data.GetHashCode();
        }
    }
}
