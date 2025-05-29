using System.Collections.Generic;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>port audio - audio device implementation.</summary>
/// <seealso cref="AudioDevice"/>
internal class PADevice : AudioDevice
{
    #region Private Methods

    static IAudioDeviceCapabilities GetCapabilities(int devNumber)
    {
        var configs = new List<AudioConfiguration>();
        foreach (var sampleRate in new int[] { 11025, 16000, 22050, 32000, 44100, 48000, 64000, 96000 })
        {
            foreach (var setup in new AudioChannelSetup[] { AudioChannelSetup.Mono, AudioChannelSetup.Stereo })
            {
                foreach (var format in new AudioSampleFormat[] { AudioSampleFormat.Float, AudioSampleFormat.Int8, AudioSampleFormat.Int16, AudioSampleFormat.Int32 })
                {
                    configs.Add(new AudioConfiguration(sampleRate, format, setup));
                }
            }
        }
        return new AudioDeviceCapabilities(AudioDeviceType.Output, configs.ToArray());
    }

    static string GetName(int devIndex)
    {
        var deviceInfo = PA.GetDeviceInfo(devIndex);
        var hostApiInfo = PA.GetHostApiInfo(deviceInfo.HostApi);

        // by default use utf-8, but mme uses ansi
        switch (hostApiInfo.Type)
        {
            case PAHostApiTypeId.MME: return $"PortAudio {hostApiInfo.NameUtf8}: {deviceInfo.NameAnsi}";
        }
        return $"PortAudio {hostApiInfo.NameUtf8}: {deviceInfo.NameUtf8}";
    }

    #endregion Private Methods

    #region Internal Constructors

    /// <summary>Initializes a new instance of the <see cref="PADevice"/> class.</summary>
    /// <param name="api">The API.</param>
    /// <param name="devIndex">Index of the device.</param>
    internal PADevice(IAudioAPI api, int devIndex)
        : base(api, GetName(devIndex), GetCapabilities(devIndex))
    {
        DeviceIndex = devIndex;
    }

    #endregion Internal Constructors

    #region Public Fields

    /// <summary>The device index.</summary>
    public readonly int DeviceIndex;

    #endregion Public Fields

    #region Public Properties

    /// <summary>Gets whether the device supports playback or not.</summary>
    public override bool SupportsPlayback
    {
        get
        {
            var deviceInfo = PA.GetDeviceInfo(DeviceIndex);
            return deviceInfo.MaxOutputChannels > 0;
        }
    }

    /// <summary>Gets whether the device supports recording or not.</summary>
    public override bool SupportsRecording
    {
        get
        {
            var deviceInfo = PA.GetDeviceInfo(DeviceIndex);
            return deviceInfo.MaxInputChannels > 0;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a new audio queue (sound target/source).</summary>
    /// <param name="configuration">The desired AudioConfiguration.</param>
    /// <returns>Returns an IAudioQueue or IAudioQueue3D.</returns>
    public override AudioOut CreateAudioOut(IAudioConfiguration configuration) => new PAOut(this, configuration);

    /// <summary>Disposes all unmanged resources.</summary>
    public override void Dispose() { }

    #endregion Public Methods
}
