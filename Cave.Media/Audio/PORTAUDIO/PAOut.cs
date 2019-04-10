#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cave.IO;

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// port audio - audio out implementation.
    /// </summary>
    /// <seealso cref="AudioOut" />
    internal class PAOut : AudioOut
    {
        public readonly int BufferSize;

        public readonly int SamplesPerBuffer;

        #region private implementation

        readonly object m_SyncRoot = new object();
        Queue<IAudioData> m_Buffers = new Queue<IAudioData>();
        PA.StreamCallbackDelegate m_CallbackDelegate;
        IntPtr m_StreamHandle;
        FifoBuffer m_StreamData = new FifoBuffer();

        // Task m_Task;
        bool m_Exit = true;
        long m_BytesPassed;
        long m_BytesQueued;
        int m_InProgressBytes = 0;
        long m_BufferUnderflowCount;

        PAStreamCallbackResult Callback(IntPtr input, IntPtr output, uint frameCount, ref PAStreamCallbackTimeInfo timeInfo, PAStreamCallbackFlags statusFlags, IntPtr userData)
        {
            int byteCount = (int)frameCount * Configuration.BytesPerTick;
            lock (m_SyncRoot)
            {
                // fill output buffer
                while (m_StreamData.Length < byteCount)
                {
                    if (m_Buffers.Count > 0)
                    {
                        IAudioData audioData = m_Buffers.Dequeue();
                        if (Volume != 1)
                        {
                            audioData = audioData.ChangeVolume(Volume);
                        }

                        m_StreamData.Enqueue(audioData.Data);
                        continue;
                    }
                    int silenceBytes = byteCount - m_StreamData.Length;
                    m_BufferUnderflowCount++;
                    m_BytesQueued += silenceBytes;
                    m_StreamData.Enqueue(new byte[silenceBytes]);
                }
                m_StreamData.Dequeue(byteCount, output);
                m_BytesPassed += m_InProgressBytes;
                m_InProgressBytes = byteCount;
                return m_Exit ? PAStreamCallbackResult.Complete : PAStreamCallbackResult.Continue;
            }
        }
        #endregion

        #region constructor
        ~PAOut()
        {
            Dispose(false);
        }

        /// <summary>Initializes a new instance of the <see cref="PAOut"/> class.</summary>
        /// <param name="dev">The device to use.</param>
        /// <param name="configuration">The configuration to use.</param>
        /// <exception cref="NotSupportedException">
        /// </exception>
        /// <exception cref="Exception"></exception>
        internal PAOut(IAudioDevice dev, IAudioConfiguration configuration)
            : base(dev, configuration)
        {
            var l_OutputParameters = new PAStreamParameters();
            switch (configuration.ChannelSetup)
            {
                case AudioChannelSetup.Mono:
                case AudioChannelSetup.Stereo:
                    l_OutputParameters.ChannelCount = configuration.Channels; break;
                default: throw new NotSupportedException(string.Format("Audio channel setup {0} not supported!", configuration.ChannelSetup));
            }
            switch (configuration.Format)
            {
                case AudioSampleFormat.Float: l_OutputParameters.SampleFormat = PASampleFormat.Float32; break;
                case AudioSampleFormat.Int8: l_OutputParameters.SampleFormat = PASampleFormat.Int8; break;
                case AudioSampleFormat.Int16: l_OutputParameters.SampleFormat = PASampleFormat.Int16; break;
                case AudioSampleFormat.Int24: l_OutputParameters.SampleFormat = PASampleFormat.Int24; break;
                case AudioSampleFormat.Int32: l_OutputParameters.SampleFormat = PASampleFormat.Int32; break;
                default: throw new NotSupportedException(string.Format("Audio format {0} not supported!", configuration.Format));
            }
            l_OutputParameters.Device = ((PADevice)dev).DeviceIndex;

            SamplesPerBuffer = Math.Max(1, configuration.SamplingRate / PA.BuffersPerSecond);
            BufferSize = configuration.BytesPerTick * SamplesPerBuffer;
            m_CallbackDelegate = new PA.StreamCallbackDelegate(Callback);
            PAErrorCode l_ErrorCode = PA.SafeNativeMethods.Pa_OpenStream(out m_StreamHandle, IntPtr.Zero, ref l_OutputParameters, configuration.SamplingRate, (uint)SamplesPerBuffer, PAStreamFlags.ClipOff, m_CallbackDelegate, IntPtr.Zero);
            if (l_ErrorCode != PAErrorCode.NoError)
            {
                throw new Exception(PA.GetErrorText(l_ErrorCode));
            }
        }
        #endregion

        #region protected overrides

        /// <summary>Begins playing.</summary>
        /// <exception cref="Exception">
        /// Already started!
        /// or.
        /// </exception>
        protected override void StartPlayback()
        {
            if (!m_Exit)
            {
                throw new Exception("Already started!");
            }

            m_Exit = false;
            PAErrorCode errorCode = PA.SafeNativeMethods.Pa_StartStream(m_StreamHandle);
            if (errorCode != PAErrorCode.NoError)
            {
                throw new Exception(PA.GetErrorText(errorCode));
            }
        }

        /// <summary>Stops playing.</summary>
        /// <exception cref="Exception">
        /// Already stopped!
        /// or.
        /// </exception>
        protected override void StopPlayback()
        {
            if (m_Exit)
            {
                throw new Exception("Already stopped!");
            }

            m_Exit = true;
            PAErrorCode l_ErrorCode = PA.SafeNativeMethods.Pa_StopStream(m_StreamHandle);
            if (l_ErrorCode != PAErrorCode.NoError)
            {
                throw new Exception(PA.GetErrorText(l_ErrorCode));
            }
        }

        /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        protected override void Dispose(bool disposing)
        {
            m_Exit = true;
            if (m_StreamHandle != IntPtr.Zero)
            {
                PAErrorCode l_ErrorCode = PA.SafeNativeMethods.Pa_CloseStream(m_StreamHandle);
                if (l_ErrorCode != PAErrorCode.NoError)
                {
                    Trace.WriteLine("Error Pa_CloseStream " + PA.GetErrorText(l_ErrorCode));
                }
                m_StreamHandle = IntPtr.Zero;
            }
            m_CallbackDelegate = null;
            m_StreamData = null;
        }
        #endregion

        #region public overrides

        /// <summary>Gets the buffer underflow count.</summary>
        /// <value>The buffer underflow count.</value>
        public override long BufferUnderflowCount => m_BufferUnderflowCount;

        /// <summary>Writes a buffer to the device.</summary>
        /// <param name="audioData">The buffer.</param>
        public override void Write(IAudioData audioData)
        {
            lock (m_SyncRoot)
            {
                m_BytesQueued += audioData.Length;
                m_Buffers.Enqueue(audioData);
            }
        }

        /// <summary>Obtains the number of bytes passed since starting this queue.</summary>
        public override long BytesPassed
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_BytesPassed;
                }
            }
        }

        /// <summary>Obtains the bytes buffered (bytes to play until queue gets empty).</summary>
        public override long BytesBuffered
        {
            get
            {
                lock (m_SyncRoot)
                {
                    return m_BytesQueued - m_BytesPassed;
                }
            }
        }

        /// <summary>Obtains the latency of the queue.</summary>
        public override TimeSpan Latency
        {
            get
            {
                return new TimeSpan(SamplesPerBuffer * TimeSpan.TicksPerSecond / Configuration.SamplingRate);
            }
        }

        /// <summary>Obtains whether the IAudioQueue supports 3D positioning or not.</summary>
        public override bool Supports3D
        {
            get
            {
                return false;
            }
        }

        /// <summary>Gets or sets the volume.</summary>
        /// <value>The volume in range 0..1.</value>
        public override float Volume { get; set; }

#pragma warning disable 0809

        /// <summary>Gets or sets the pitch.</summary>
        /// <value>The pitch.</value>
        [Obsolete("NOT SUPPORTED")]
        public override float Pitch { get; set; }

        /// <summary>sets / gets the 3d position of the sound source.</summary>
        [Obsolete("NOT SUPPORTED")]
        public override Vector3 Position3D { get; set; }

#pragma warning restore 0809

        #endregion
    }
}
