using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cave.Media.Audio.OPENAL;

namespace Cave.Media.Audio;

/// <summary>Provides access to the open al api.</summary>
/// <seealso cref="AudioAPI"/>
public sealed class OpenAL : AudioAPI
{
    #region Public Properties

    /// <summary>Gets the available input devices.</summary>
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
                var devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, ALCenum.ALC_ALL_DEVICES_SPECIFIER);
                var result = new List<IAudioDevice>(devices.Length);
                for (var i = 0; i < devices.Length; i++)
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
                    OAL.SafeNativeMethods.alGetString1(ALenum.AL_VERSION);
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

    /// <summary>Gets all available output devices.</summary>
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
                var devices = OAL.SafeNativeMethods.alcGetStringv(IntPtr.Zero, ALCenum.ALC_ALL_DEVICES_SPECIFIER);
                var result = new List<IAudioDevice>(devices.Length);
                for (var i = 0; i < devices.Length; i++)
                {
                    var device = new OALDevice(this, devices[i]);
                    if (device.SupportsPlayback)
                    {
                        result.Add(device);
                    }
                }
                return result.ToArray();
            }
        }
    }

    /// <summary>Gets the preference value.</summary>
    /// <value>Constant value = -10.</value>
    /// <remarks>Small values represent a higher priority.</remarks>
    public override int Preference => -10;

    #endregion Public Properties
}
