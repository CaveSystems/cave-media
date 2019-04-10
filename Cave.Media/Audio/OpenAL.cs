#region License OpenAL Soft
/*
    Uses openal soft (http://kcat.strangesoft.net/openal.html)
    A non-GPL license for this library is not available.
*/
#endregion

using Cave.Media.Audio.OPENAL;
using System;
using System.Collections.Generic;
using System.Diagnostics;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides access to the open al api.
    /// </summary>
    /// <seealso cref="AudioAPI" />
    public sealed class OpenAL : AudioAPI
    {
        /// <summary>Gets the preference value.</summary>
        /// <value>Constant value = -10.</value>
        /// <remarks>Small values represent a higher priority.</remarks>
        public override int Preference { get { return -10; } }

        /// <summary>Determines if the API is available.</summary>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        public override bool IsAvailable
        {
            get
            {
                lock (OAL.SyncRoot)
                {
                    try
                    {
                        OAL.SafeNativeMethods.alGetString1(OAL.AL_VERSION);
                        return true;
                    }
                    catch (Exception ex)
                    {
                        Trace.TraceError("Could not load any OpenAL implementation!");
                        Trace.TraceError(ex.ToString());
                        return false;
                    }
                }
            }
        }

        /// <summary>Obtains the available input devices.</summary>
        /// <exception cref="NotImplementedException"></exception>
        public override IAudioDevice[] InputDevices
        {
            get
            {
                if (!IsAvailable)
                {
                    return new IAudioDevice[0];
                }

                lock (OAL.SyncRoot)
                {
                    string[] devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, OAL.ALC_ALL_DEVICES_SPECIFIER);
                    var result = new List<IAudioDevice>(devices.Length);
                    for (int i = 0; i < devices.Length; i++)
                    {
                        var device = new OALDevice(this, devices[i]);
                        if (device.SupportsRecording)
                        {
                            result.Add(device);
                        }
                    }
                    return result.ToArray();
                }
            }
        }

        /// <summary>Obtains all available output devices.</summary>
        /// <exception cref="NotSupportedException">Invalid bit size!.</exception>
        public override IAudioDevice[] OutputDevices
        {
            get
            {
                if (!IsAvailable)
                {
                    return new IAudioDevice[0];
                }

                lock (OAL.SyncRoot)
                {
                    string[] devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, OAL.ALC_ALL_DEVICES_SPECIFIER);
                    var result = new List<IAudioDevice>(devices.Length);
                    for (int i = 0; i < devices.Length; i++)
                    {
                        var l_Device = new OALDevice(this, devices[i]);
                        if (l_Device.SupportsPlayback)
                        {
                            result.Add(l_Device);
                        }
                    }
                    return result.ToArray();
                }
            }
        }
    }
}
