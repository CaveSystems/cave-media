namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a generic interface for audio device capabilities.
    /// </summary>
    public interface IAudioDeviceCapabilities
    {
        /// <summary>
        /// Obtains the device type.
        /// </summary>
        AudioDeviceType Type { get; }

        /// <summary>
        /// Determines if the device is an input device.
        /// </summary>
        bool IsInput { get; }

        /// <summary>
        /// Determines if the device is an output device.
        /// </summary>
        bool IsOutput { get; }

        /// <summary>
        /// Obtains the supported output configurations.
        /// </summary>
        IAudioConfiguration[] OutputConfigurations { get; }

        /// <summary>
        /// Obtains the supported output configurations.
        /// </summary>
        IAudioConfiguration[] InputConfigurations { get; }
    }
}
