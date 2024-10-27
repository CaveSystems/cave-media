using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Cave.Media.Audio.OPENAL;

/// <summary>Implements <see cref="IAudioDevice"/> for open al devices.</summary>
/// <seealso cref="IAudioDevice"/>
/// <seealso cref="IDisposable"/>
public sealed class OALDevice : CriticalFinalizerObject, IAudioDevice, IDisposable
{
    #region Private Destructors

    /// <summary>Finalizes an instance of the <see cref="OALDevice"/> class.</summary>
    ~OALDevice()
    {
        if (Handle != IntPtr.Zero)
        {
            FreeHandles();
        }
    }

    #endregion Private Destructors

    #region Private Methods

    void FreeHandles()
    {
        OAL.SafeNativeMethods.alcDestroyContext(Context);
        OAL.SafeNativeMethods.alcCloseDevice(Handle);
        Context = IntPtr.Zero;
        Handle = IntPtr.Zero;
    }

    void OpenHandles()
    {
        lock (OAL.SyncRoot)
        {
            if (Handle != IntPtr.Zero)
            {
                throw new InvalidOperationException();
            }

            Handle = OAL.SafeNativeMethods.alcOpenDevice(Name);
            if (Handle == IntPtr.Zero)
            {
                throw new Exception("Device " + Name + " not found!");
            }

            Context = OAL.SafeNativeMethods.alcCreateContext(Handle, IntPtr.Zero);
            if (Context == IntPtr.Zero)
            {
                throw new Exception("Device context of device " + Name + " could not be created!");
            }

            OAL.SafeNativeMethods.alcMakeContextCurrent(Context);
            OAL.SafeNativeMethods.alListener3f(ALenum.AL_POSITION, 0, 0, 0);
            OAL.CheckError();
            OAL.SafeNativeMethods.alListener3f(ALenum.AL_VELOCITY, 0, 0, 0);
            OAL.CheckError();
            OAL.SafeNativeMethods.alListenerfv(ALenum.AL_ORIENTATION, [0f, 0f, -1.0f, 0f, 1.0f, 0f]);
            OAL.CheckError();
        }
    }

    #endregion Private Methods

    #region Internal Constructors

    /// <summary>Initializes a new instance of the <see cref="OALDevice"/> class.</summary>
    /// <param name="api">The API.</param>
    /// <param name="name">Name of the device.</param>
    /// <exception cref="ArgumentNullException">API or Name.</exception>
    /// <exception cref="ArgumentException">DeviceName.</exception>
    internal OALDevice(IAudioAPI api, UTF8 name)
    {
        API = api ?? throw new ArgumentNullException(nameof(api));
        Name = name?.ToString() ?? throw new ArgumentNullException(nameof(name));
        var devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, ALCenum.ALC_ALL_DEVICES_SPECIFIER);
        if (Array.IndexOf(devices, name) < 0)
        {
            throw new ArgumentException(string.Format("Device Name {0} not found!", name), "DeviceName");
        }
    }

    #endregion Internal Constructors

    #region Public Properties

    /// <summary>Gets the used audio API.</summary>
    /// <value>The audio API.</value>
    public IAudioAPI API { get; }

    /// <summary>Gets the devices capabilities.</summary>
    /// <exception cref="ObjectDisposedException">OpenALOutputDevice.</exception>
    public IAudioDeviceCapabilities Capabilities
    {
        get
        {
            if (Handle == IntPtr.Zero)
            {
                throw new ObjectDisposedException("OpenALOutputDevice");
            }

            var configs = new List<AudioConfiguration>();
            foreach (var sampleRate in new int[] { 8000, 11025, 16000, 22050, 32000, 44100, 48000, 64000, 96000 })
            {
                foreach (var setup in new AudioChannelSetup[] { AudioChannelSetup.Mono, AudioChannelSetup.Stereo })
                {
                    foreach (var format in new AudioSampleFormat[] { AudioSampleFormat.Float, AudioSampleFormat.Int16, })
                    {
                        configs.Add(new AudioConfiguration(sampleRate, format, setup));
                    }
                }
            }
            return new AudioDeviceCapabilities(AudioDeviceType.Output, configs.ToArray());
        }
    }

    /// <summary>Gets the open al context this device uses.</summary>
    public IntPtr Context { get; private set; }

    /// <summary>Gets the device handle.</summary>
    public IntPtr Handle { get; private set; }

    /// <summary>Retrieves the device name.</summary>
    public string Name { get; private set; }

    /// <summary>Gets whether the device supports playback or not.</summary>
    public bool SupportsPlayback
    {
        get
        {
            lock (OAL.SyncRoot)
            {
                var device = OAL.SafeNativeMethods.alcOpenDevice(Name);
                if (device != IntPtr.Zero)
                {
                    OAL.SafeNativeMethods.alcCloseDevice(device);
                    return true;
                }
                return false;
            }
        }
    }

    /// <summary>Gets whether the device supports recording or not.</summary>
    public bool SupportsRecording
    {
        get
        {
            lock (OAL.SyncRoot)
            {
                var device = OAL.SafeNativeMethods.alcCaptureOpenDevice(Name, 44100, ALenum.AL_FORMAT_MONO16, 44100 * 4 / OAL.BuffersPerSecond);
                if (device != IntPtr.Zero)
                {
                    OAL.SafeNativeMethods.alcCaptureCloseDevice(device);
                    return true;
                }
                return false;
            }
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a new audio out stream.</summary>
    /// <param name="configuration">Audio configuration to use.</param>
    /// <returns></returns>
    /// <exception cref="ObjectDisposedException">OpenALOutputDevice.</exception>
    public AudioOut CreateAudioOut(IAudioConfiguration configuration)
    {
        if (Handle == IntPtr.Zero)
        {
            OpenHandles();
        }

        return new OALOut(this, configuration);
    }

    /// <summary>Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.</summary>
    public void Dispose()
    {
        if (Handle != IntPtr.Zero)
        {
            FreeHandles();
        }
        GC.SuppressFinalize(this);
    }

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>Returns the name of the device.</returns>
    public override string ToString() => Name;

    #endregion Public Methods
}
