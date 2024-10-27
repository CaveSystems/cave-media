using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Media.Audio.OPENAL;

/// <summary>Provides an audio out stream implementation for the open al api.</summary>
/// <seealso cref="AudioOut"/>
public sealed class OALOut : AudioOut
{
    #region Private Fields

    /// <summary>The buffers (id, length).</summary>
    readonly Dictionary<ALbuffer, IAudioData> buffers = new Dictionary<ALbuffer, IAudioData>();

    readonly OALDevice device;
    long bufferUnderflowCount;
    long bytesPassed;
    long bytesQueued;
    bool playing;
    ALsource source;

    #endregion Private Fields

    #region Private Methods

    private void StartChecked(bool isStartup)
    {
        ALenum state = default;
        for (var tryNumber = 0; tryNumber < 10 && buffers.Count > 0; tryNumber++)
        {
            state = source.GetState();
            if (state != ALenum.AL_PLAYING)
            {
                source.Play();
                if (!isStartup)
                {
                    bufferUnderflowCount++;
                }
            }
            else
            {
                break;
            }
        }
        if (state != ALenum.AL_PLAYING)
        {
            throw new InvalidOperationException("Could not start open al playback.");
        }

        playing = true;
    }

    private void StopChecked()
    {
        ALenum state = default;
        for (var tryNumber = 0; tryNumber < 10 && playing; tryNumber++)
        {
            state = source.GetState();

            // if not playing restart
            if (state != ALenum.AL_STOPPED)
            {
                source.Stop();
            }
            else
            {
                break;
            }
        }
        if (state != ALenum.AL_STOPPED)
        {
            throw new InvalidOperationException("Could not stop open al playback.");
        }

        playing = false;
    }

    /// <summary>Unqueues and disposes the played buffers.</summary>
    void UnqueuePlayedBuffers()
    {
        if (source == 0)
        {
            return;
        }

        OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
        while (true)
        {
            int count;
            ALbuffer[] bufferIDs;
            OAL.SafeNativeMethods.alGetSourcei(source, ALenum.AL_BUFFERS_PROCESSED, out count);
            OAL.CheckError();
            if (count <= 0)
            {
                break;
            }

            bufferIDs = new ALbuffer[count];
            OAL.SafeNativeMethods.alSourceUnqueueBuffers(source, count, bufferIDs);
            OAL.CheckError();
            OAL.SafeNativeMethods.alDeleteBuffers(bufferIDs.Length, bufferIDs);
            OAL.CheckError();
            foreach (var bufferID in bufferIDs)
            {
                bytesPassed += buffers[bufferID].Length;
                if (!buffers.Remove(bufferID))
                {
                    throw new KeyNotFoundException();
                }
            }

            if (playing && buffers.Count == 0)
            {
                Trace.WriteLine("OpenAL AudioOut stopped.");
            }
        }
    }

    #endregion Private Methods

    #region Protected Methods

    /// <summary>Releases unmanaged resources.</summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
    protected override void Dispose(bool disposing)
    {
        lock (OAL.SyncRoot)
        {
            if (buffers.Count > 0)
            {
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                StopPlayback();
                foreach (var bufferID in buffers.Keys)
                {
                    var b = bufferID;
                    OAL.SafeNativeMethods.alSourceUnqueueBuffers(source, 1, ref b);
                    OAL.SafeNativeMethods.alDeleteBuffers(1, ref b);
                    if (OAL.SafeNativeMethods.alGetError() != ALenum.AL_NO_ERROR)
                    {
                        Console.WriteLine("OpenAL.AudioOut: High probability for memory leak on delete buffer!");
                    }
                }
                buffers.Clear();
            }
            if (source != 0)
            {
                OAL.SafeNativeMethods.alDeleteSources(1, ref source);
                if (OAL.SafeNativeMethods.alGetError() != ALenum.AL_NO_ERROR)
                {
                    Console.WriteLine("OpenAL.AudioOut: High probability for memory leak on delete source!");
                }
                source = 0;
            }
        }
    }

    /// <summary>Begins playing.</summary>
    /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
    protected override void StartPlayback()
    {
        lock (OAL.SyncRoot)
        {
            if (playing)
            {
                throw new InvalidOperationException("Already started!");
            }

            OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
            playing = true;
            bufferUnderflowCount = 0;
            StartChecked(true);
        }
    }

    /// <summary>Stops playing.</summary>
    /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
    protected override void StopPlayback()
    {
        lock (OAL.SyncRoot)
        {
            if (!playing)
            {
                throw new InvalidOperationException("Already stopped!");
            }

            OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
            StopChecked();
        }
    }

    #endregion Protected Methods

    #region Internal Constructors

    internal OALOut(OALDevice dev, IAudioConfiguration configuration)
                    : base(dev, configuration)
    {
        // m_BufferSize = configuration.BytesPerTick * Math.Max(1, configuration.SamplingRate / OAL.BuffersPerSecond);
        device = dev;
        lock (OAL.SyncRoot)
        {
            OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
            OAL.SafeNativeMethods.alGenSources(1, out source);
            OAL.SafeNativeMethods.alSourcei(source, ALenum.AL_LOOPING, ALenum.AL_FALSE);
            OAL.CheckError();
            Position3D = Vector3.Create(0, 0, 0);
        }
    }

    #endregion Internal Constructors

    #region Public Properties

    /// <summary>Gets the buffer underflow count.</summary>
    /// <value>The buffer underflow count.</value>
    public override long BufferUnderflowCount => bufferUnderflowCount;

    /// <summary>Gets the bytes buffered (bytes to play until queue gets empty).</summary>
    public override long BytesBuffered
    {
        get
        {
            lock (OAL.SyncRoot)
            {
                UnqueuePlayedBuffers();
                var position = 0;
                OAL.SafeNativeMethods.alGetSourcei(source, ALenum.AL_BYTE_OFFSET, out position);
                return bytesQueued - bytesPassed - position;
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
                var position = 0;
                OAL.SafeNativeMethods.alGetSourcei(source, ALenum.AL_BYTE_OFFSET, out position);
                return bytesPassed + position;
            }
        }
    }

    /// <summary>Gets the latency of the queue.</summary>
    public override TimeSpan Latency => new TimeSpan(TimeSpan.TicksPerSecond / Configuration.SamplingRate);

    /// <summary>Gets or sets the pitch.</summary>
    /// <value>The pitch.</value>
    public override float Pitch
    {
        get
        {
            lock (OAL.SyncRoot)
            {
                float pitch;
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alGetSourcef(source, ALenum.AL_PITCH, out pitch);
                OAL.CheckError();
                return pitch;
            }
        }
        set
        {
            lock (OAL.SyncRoot)
            {
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alSourcef(source, ALenum.AL_PITCH, value);
                OAL.CheckError();
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
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alGetSource3f(source, ALenum.AL_POSITION, out x, out y, out z);
                OAL.CheckError();
                return Vector3.Create(x, y, z);
            }
        }
        set
        {
            lock (OAL.SyncRoot)
            {
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alSource3f(source, ALenum.AL_POSITION, value.X, value.Y, value.Z);
                OAL.CheckError();
            }
        }
    }

    /// <summary>Gets whether the IAudioQueue supports 3D positioning or not (only supported on mono streams).</summary>
    public override bool Supports3D => Configuration.Channels == 1;

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
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alGetSourcef(source, ALenum.AL_GAIN, out volume);
                OAL.CheckError();
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
                OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
                OAL.SafeNativeMethods.alSourcef(source, ALenum.AL_GAIN, value);
                OAL.CheckError();
            }
        }
    }

    #endregion Public Properties

    #region Public Methods

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
            if (source == 0)
            {
                throw new ObjectDisposedException("OpenAL AudioOut");
            }

            UnqueuePlayedBuffers();

            OAL.SafeNativeMethods.alcMakeContextCurrent(device.Context);
            OAL.SafeNativeMethods.alGenBuffers(1, out var bufferID);
            OAL.CheckError();
            OAL.SafeNativeMethods.alBufferData(bufferID, OAL.AL_FORMAT(Configuration), audioData.Data, audioData.Length, Configuration.SamplingRate);
            OAL.CheckError();
            OAL.SafeNativeMethods.alSourceQueueBuffers(source, 1, ref bufferID);
            OAL.CheckError();
            buffers.Add(bufferID, audioData);
            bytesQueued += audioData.Length;

            if (playing)
            {
                StartChecked(false);
            }
        }
    }

    #endregion Public Methods
}
