#region License OpenAL Soft
/*
    Uses openal soft (http://kcat.strangesoft.net/openal.html)
    A non-GPL license for this library is not available.
*/
#endregion
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Media.Audio.OPENAL
{
    /// <summary>
    /// Provides an audio out stream implementation for the open al api.
    /// </summary>
    /// <seealso cref="AudioOut" />
    public sealed class OALOut : AudioOut
    {
        // int m_BufferSize;
        long m_BytesQueued;
        long m_BytesPassed;
        int m_Source;

        /// <summary>The buffers (id, length).</summary>
        Dictionary<int, IAudioData> m_Buffers = new Dictionary<int, IAudioData>();
        OALDevice m_Device;
        bool m_Playing;
        long m_BufferUnderflowCount;

        /// <summary>Unqueues and disposes the played buffers.</summary>
        void UnqueuePlayedBuffers()
        {
            if (m_Source == 0)
            {
                return;
            }

            OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);

            while (true)
            {
                int count;
                int[] bufferIDs;
                OAL.SafeNativeMethods.alGetSourcei(m_Source, OAL.AL_BUFFERS_PROCESSED, out count);
                OAL.SafeNativeMethods.CheckError();
                if (count <= 0)
                {
                    break;
                }

                bufferIDs = new int[count];
                OAL.SafeNativeMethods.alSourceUnqueueBuffers(m_Source, count, bufferIDs);
                OAL.SafeNativeMethods.CheckError();
                OAL.SafeNativeMethods.alDeleteBuffers(bufferIDs.Length, bufferIDs);
                OAL.SafeNativeMethods.CheckError();
                foreach (int bufferID in bufferIDs)
                {
                    m_BytesPassed += m_Buffers[bufferID].Length;
                    if (!m_Buffers.Remove(bufferID))
                    {
                        throw new KeyNotFoundException();
                    }
                }

                if (m_Playing && m_Buffers.Count == 0)
                {
                    Trace.WriteLine("OpenAL AudioOut stopped.");
                }
            }
        }

        private void StartChecked(bool isStartup)
        {
            int state = 0;
            for (int tryNumber = 0; tryNumber < 10 && m_Buffers.Count > 0; tryNumber++)
            {
                OAL.SafeNativeMethods.alGetSourcei(m_Source, OAL.AL_SOURCE_STATE, out state);
                OAL.SafeNativeMethods.CheckError();
                if (state != OAL.AL_PLAYING)
                {
                    OAL.SafeNativeMethods.alSourcePlay(m_Source);
                    OAL.SafeNativeMethods.CheckError();
                    if (!isStartup)
                    {
                        m_BufferUnderflowCount++;
                    }
                }
                else
                {
                    break;
                }
            }
            if (state != OAL.AL_PLAYING)
            {
                throw new Exception("Could not start open al playback.");
            }

            m_Playing = true;
        }

        private void StopChecked()
        {
            int state = 0;
            for (int tryNumber = 0; tryNumber < 10 && m_Playing; tryNumber++)
            {
                OAL.SafeNativeMethods.alGetSourcei(m_Source, OAL.AL_SOURCE_STATE, out state);
                OAL.SafeNativeMethods.CheckError();

                // if not playing restart
                if (state != OAL.AL_STOPPED)
                {
                    OAL.SafeNativeMethods.alSourceStop(m_Source);
                    OAL.SafeNativeMethods.CheckError();
                }
                else
                {
                    break;
                }
            }
            if (state != OAL.AL_STOPPED)
            {
                throw new Exception("Could not stop open al playback.");
            }

            m_Playing = false;
        }

        #region constructor
        internal OALOut(OALDevice dev, IAudioConfiguration configuration)
            : base(dev, configuration)
        {
            // m_BufferSize = configuration.BytesPerTick * Math.Max(1, configuration.SamplingRate / OAL.BuffersPerSecond);
            m_Device = dev;
            lock (OAL.SyncRoot)
            {
                OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                OAL.SafeNativeMethods.alGenSources(1, out m_Source);
                OAL.SafeNativeMethods.alSourcei(m_Source, OAL.AL_LOOPING, OAL.AL_FALSE);
                OAL.SafeNativeMethods.CheckError();
                Position3D = Vector3.Create(0, 0, 0);
            }
        }
        #endregion

        #region protected overrides

        /// <summary>Begins playing.</summary>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        protected override void StartPlayback()
        {
            lock (OAL.SyncRoot)
            {
                if (m_Playing)
                {
                    throw new InvalidOperationException("Already started!");
                }

                OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                m_Playing = true;
                m_BufferUnderflowCount = 0;
                StartChecked(true);
            }
        }

        /// <summary>Stops playing.</summary>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        protected override void StopPlayback()
        {
            lock (OAL.SyncRoot)
            {
                if (!m_Playing)
                {
                    throw new InvalidOperationException("Already stopped!");
                }

                OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                StopChecked();
            }
        }

        /// <summary>Releases unmanaged resources.</summary>
        /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        protected override void Dispose(bool disposing)
        {
            lock (OAL.SyncRoot)
            {
                if (m_Buffers.Count > 0)
                {
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    StopPlayback();
                    foreach (int bufferID in m_Buffers.Keys)
                    {
                        int b = bufferID;
                        OAL.SafeNativeMethods.alSourceUnqueueBuffers(m_Source, 1, ref b);
                        OAL.SafeNativeMethods.alDeleteBuffers(1, ref b);
                        if (OAL.SafeNativeMethods.alGetError() != OAL.AL_NO_ERROR)
                        {
                            Console.WriteLine("OpenAL.AudioOut: High probability for memory leak on delete buffer!");
                        }
                    }
                    m_Buffers.Clear();
                }
                if (m_Source != 0)
                {
                    OAL.SafeNativeMethods.alDeleteSources(1, ref m_Source);
                    if (OAL.SafeNativeMethods.alGetError() != OAL.AL_NO_ERROR)
                    {
                        Console.WriteLine("OpenAL.AudioOut: High probability for memory leak on delete source!");
                    }
                    m_Source = 0;
                }
            }
        }
        #endregion

        #region public overrides

        /// <summary>Gets or sets the pitch.</summary>
        /// <value>The pitch.</value>
        public override float Pitch
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    float pitch;
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alGetSourcef(m_Source, OAL.AL_PITCH, out pitch);
                    OAL.SafeNativeMethods.CheckError();
                    return pitch;
                }
            }
            set
            {
                lock (OAL.SyncRoot)
                {
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alSourcef(m_Source, OAL.AL_PITCH, value);
                    OAL.SafeNativeMethods.CheckError();
                }
            }
        }

        /// <summary>Sets the 3d position of the sound source.</summary>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        public override Vector3 Position3D
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    float x, y, z;
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alGetSource3f(m_Source, OAL.AL_POSITION, out x, out y, out z);
                    OAL.SafeNativeMethods.CheckError();
                    return Vector3.Create(x, y, z);
                }
            }
            set
            {
                lock (OAL.SyncRoot)
                {
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alSource3f(m_Source, OAL.AL_POSITION, value.X, value.Y, value.Z);
                    OAL.SafeNativeMethods.CheckError();
                }
            }
        }

        /// <summary>Sets the volume.</summary>
        /// <param name="value">The volume in range [0..x].</param>
        /// <exception cref="ArgumentOutOfRangeException">volume.</exception>
        public override float Volume
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    float volume;
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alGetSourcef(m_Source, OAL.AL_GAIN, out volume);
                    OAL.SafeNativeMethods.CheckError();
                    return volume;
                }
            }
            set
            {
                if (value < 0)
                {
                    throw new ArgumentOutOfRangeException();
                }

                lock (OAL.SyncRoot)
                {
                    OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                    OAL.SafeNativeMethods.alSourcef(m_Source, OAL.AL_GAIN, value);
                    OAL.SafeNativeMethods.CheckError();
                }
            }
        }

        /// <summary>Gets the number of bytes passed since starting this queue.</summary>
        public override long BytesPassed
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    UnqueuePlayedBuffers();
                    int position = 0;
                    OAL.SafeNativeMethods.alGetSourcei(m_Source, OAL.AL_BYTE_OFFSET, out position);
                    return m_BytesPassed + position;
                }
            }
        }

        /// <summary>Gets the bytes buffered (bytes to play until queue gets empty).</summary>
        public override long BytesBuffered
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    UnqueuePlayedBuffers();
                    int position = 0;
                    OAL.SafeNativeMethods.alGetSourcei(m_Source, OAL.AL_BYTE_OFFSET, out position);
                    return m_BytesQueued - m_BytesPassed - position;
                }
            }
        }

        /// <summary>Gets the buffer underflow count.</summary>
        /// <value>The buffer underflow count.</value>
        public override long BufferUnderflowCount => m_BufferUnderflowCount;

        /// <summary>Gets the latency of the queue.</summary>
        public override TimeSpan Latency
        {
            get
            {
                return new TimeSpan(TimeSpan.TicksPerSecond / Configuration.SamplingRate);
            }
        }

        /// <summary>Gets whether the IAudioQueue supports 3D positioning or not (only supported on mono streams).</summary>
        public override bool Supports3D
        {
            get
            {
                return Configuration.Channels == 1;
            }
        }

        /// <summary>Writes a buffer to the device.</summary>
        /// <param name="audioData">The buffer.</param>
        /// <exception cref="ArgumentException">AudioConfiguration does not match!.</exception>
        /// <exception cref="ObjectDisposedException">OpenAL AudioOut.</exception>
        public override void Write(IAudioData audioData)
        {
            if (!Configuration.Equals(audioData))
            {
                throw new ArgumentException("AudioConfiguration does not match!");
            }

            lock (OAL.SyncRoot)
            {
                if (m_Source == 0)
                {
                    throw new ObjectDisposedException("OpenAL AudioOut");
                }

                UnqueuePlayedBuffers();

                OAL.SafeNativeMethods.alcMakeContextCurrent(m_Device.Context);
                int bufferID;
                OAL.SafeNativeMethods.alGenBuffers(1, out bufferID);
                OAL.SafeNativeMethods.CheckError();
                OAL.SafeNativeMethods.alBufferData(bufferID, OAL.AL_FORMAT(Configuration), audioData.Data, audioData.Length, Configuration.SamplingRate);
                OAL.SafeNativeMethods.CheckError();
                OAL.SafeNativeMethods.alSourceQueueBuffers(m_Source, 1, ref bufferID);
                OAL.SafeNativeMethods.CheckError();
                m_Buffers.Add(bufferID, audioData);
                m_BytesQueued += audioData.Length;

                if (m_Playing)
                {
                    StartChecked(false);
                }
            }
        }
        #endregion
    }
}
