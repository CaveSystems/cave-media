#region License OpenAL Soft
/*
    Uses openal soft (http://kcat.strangesoft.net/openal.html)
    A non-GPL license for this library is not available.
*/
#endregion

using System;
using System.Collections.Generic;
using System.Runtime.ConstrainedExecution;

namespace Cave.Media.Audio.OPENAL
{
    /// <summary>
    /// Implements <see cref="IAudioDevice"/> for open al devices.
    /// </summary>
    /// <seealso cref="IAudioDevice" />
    /// <seealso cref="IDisposable" />
    public sealed class OALDevice : CriticalFinalizerObject, IAudioDevice, IDisposable
    {
        /// <summary>Gets the used audio API.</summary>
        /// <value>The audio API.</value>
        public IAudioAPI API { get; }

        /// <summary>Gets the device handle.</summary>
        public IntPtr Handle { get; private set; }

        /// <summary>Gets the open al context this device uses.</summary>
        public IntPtr Context { get; private set; }

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
                OAL.SafeNativeMethods.alListener3f(OAL.AL_POSITION, 0, 0, 0);
                OAL.SafeNativeMethods.CheckError();
                OAL.SafeNativeMethods.alListener3f(OAL.AL_VELOCITY, 0, 0, 0);
                OAL.SafeNativeMethods.CheckError();
                OAL.SafeNativeMethods.alListenerfv(OAL.AL_ORIENTATION, new float[] { 0f, 0f, -1.0f, 0f, 1.0f, 0f });
                OAL.SafeNativeMethods.CheckError();
            }
        }

        /// <summary>Finalizes an instance of the <see cref="OALDevice"/> class.</summary>
        ~OALDevice()
        {
            if (Handle != IntPtr.Zero)
            {
                FreeHandles();
            }
        }

        /// <summary>Initializes a new instance of the <see cref="OALDevice" /> class.</summary>
        /// <param name="api">The API.</param>
        /// <param name="name">Name of the device.</param>
        /// <exception cref="ArgumentNullException">
        /// API
        /// or
        /// Name.
        /// </exception>
        /// <exception cref="ArgumentException">DeviceName.</exception>
        internal OALDevice(IAudioAPI api, string name)
        {
            API = api ?? throw new ArgumentNullException("API");
            Name = name ?? throw new ArgumentNullException("Name");
            string[] l_Devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, OAL.ALC_ALL_DEVICES_SPECIFIER);
            if (Array.IndexOf(l_Devices, name) < 0)
            {
                throw new ArgumentException(string.Format("Device Name {0} not found!", name), "DeviceName");
            }
        }

        /// <summary>Obtains the devices capabilities.</summary>
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
                foreach (int sampleRate in new int[] { 8000, 11025, 16000, 22050, 32000, 44100, 48000, 64000, 96000 })
                {
                    foreach (AudioChannelSetup setup in new AudioChannelSetup[] { AudioChannelSetup.Mono, AudioChannelSetup.Stereo })
                    {
                        foreach (AudioSampleFormat format in new AudioSampleFormat[] { AudioSampleFormat.Float, AudioSampleFormat.Int16, })
                        {
                            configs.Add(new AudioConfiguration(sampleRate, format, setup));
                        }
                    }
                }
                return new AudioDeviceCapabilities(AudioDeviceType.Output, configs.ToArray());
            }
        }

        /// <summary>Retrieves the device name.</summary>
        public string Name { get; private set; }

        /// <summary>Obtains whether the device supports playback or not.</summary>
        public bool SupportsPlayback
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    IntPtr l_Device = OAL.SafeNativeMethods.alcOpenDevice(Name);
                    if (l_Device != IntPtr.Zero)
                    {
                        OAL.SafeNativeMethods.alcCloseDevice(l_Device);
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>Obtains whether the device supports recording or not.</summary>
        public bool SupportsRecording
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    IntPtr l_Device = OAL.SafeNativeMethods.alcCaptureOpenDevice(Name, 44100, OAL.AL_FORMAT_MONO16, 44100 * 4 / OAL.BuffersPerSecond);
                    if (l_Device != IntPtr.Zero)
                    {
                        OAL.SafeNativeMethods.alcCaptureCloseDevice(l_Device);
                        return true;
                    }
                    return false;
                }
            }
        }

        /// <summary>Obtains a new audio out stream.</summary>
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

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
            if (Handle != IntPtr.Zero)
            {
                FreeHandles();
            }
            GC.SuppressFinalize(this);
        }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>Returns the name of the device.</returns>
        public override string ToString()
        {
            return Name;
        }
    }
}