using System;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides an interface to audio devices
    /// </summary>
    
    public interface IAudioDevice : IDisposable
    {
        /// <summary>Gets the used audio API.</summary>
        /// <value>The audio API.</value>
        IAudioAPI API { get; }

        /// <summary>
        /// Obtains the devices capabilities
        /// </summary>
        IAudioDeviceCapabilities Capabilities { get; }

        /// <summary>
        /// Retrieves the device name
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Obtains whether the device supports playback or not
        /// </summary>
        bool SupportsPlayback { get; }

        /// <summary>
        /// Obtains whether the device supports recording or not
        /// </summary>
        bool SupportsRecording { get; }

        /// <summary>
        /// Obtains a new audio out stream
        /// </summary>
        AudioOut CreateAudioOut(IAudioConfiguration configuration);
    }
}
