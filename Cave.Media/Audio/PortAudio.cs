using System;
using System.Collections.Generic;
using Cave.Media.Audio.PORTAUDIO;

namespace Cave.Media.Audio;

/// <summary>Provides access to the port audio api.</summary>
/// <seealso cref="AudioAPI"/>
public class PortAudio : AudioAPI
{
    #region Private Fields

    bool m_Initialized = false;

    #endregion Private Fields

    #region Private Methods

    void m_CheckErrorCode(PAErrorCode errorCode)
    {
        if (errorCode != PAErrorCode.NoError)
        {
            throw new Exception("PortAudio: error " + errorCode.ToString());
        }
    }

    #endregion Private Methods

    #region Protected Methods

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    /// <param name="disposing"><c>true</c> to release both managed and unmanaged resources; <c>false</c> to release only unmanaged resources.</param>
    protected void Dispose(bool disposing)
    {
        if (disposing)
        {
            var error = PA.SafeNativeMethods.Pa_Terminate();
            m_CheckErrorCode(error);
            m_Initialized = false;
        }
    }

    #endregion Protected Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="PortAudio"/> class.</summary>
    public PortAudio()
    {
        try
        {
            var error = PA.SafeNativeMethods.Pa_Initialize();
            m_Initialized = error == PAErrorCode.NoError;
            m_CheckErrorCode(error);
        }
        catch { }
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the available input devices.</summary>
    public override IAudioDevice[] InputDevices
    {
        get
        {
            var deviceCount = PA.SafeNativeMethods.Pa_GetDeviceCount();
            var devices = new List<IAudioDevice>();
            for (var i = 0; i < deviceCount; i++)
            {
                var deviceInfo = PA.GetDeviceInfo(i);
                if (deviceInfo.MaxInputChannels > 0)
                {
                    devices.Add(new PADevice(this, i));
                }
            }
            return devices.ToArray();
        }
    }

    /// <summary>Determines if the API is available.</summary>
    public override bool IsAvailable => m_Initialized;

    /// <summary>Gets all available output devices.</summary>
    public override IAudioDevice[] OutputDevices
    {
        get
        {
            var deviceCount = PA.SafeNativeMethods.Pa_GetDeviceCount();
            var devices = new List<IAudioDevice>();
            for (var i = 0; i < deviceCount; i++)
            {
                var deviceInfo = PA.GetDeviceInfo(i);
                if (deviceInfo.MaxOutputChannels > 0)
                {
                    devices.Add(new PADevice(this, i));
                }
            }
            return devices.ToArray();
        }
    }

    /// <summary>Gets the preference value.</summary>
    /// <value>Constant value = +10.</value>
    /// <remarks>Small values represent a higher priority.</remarks>
    public override int Preference => +10;

    /// <summary>Gets the version text.</summary>
    /// <returns></returns>
    /// <exception cref="Exception">PortAudio not initialized!.</exception>
    public string VersionText
    {
        get
        {
            if (!m_Initialized)
            {
                throw new Exception("PortAudio not initialized!");
            }

            return PA.VersionText ?? string.Empty;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Releases unmanaged and - optionally - managed resources.</summary>
    public void Dispose()
    {
        Dispose(true);
        GC.SuppressFinalize(this);
    }

    #endregion Public Methods
}
