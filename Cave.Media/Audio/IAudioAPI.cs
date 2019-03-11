using System;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a generic interface for audio apis.
    /// </summary>
    public interface IAudioAPI : IComparable<IAudioAPI>
    {
        /// <summary>Gets the preference value.</summary>
        /// <remarks>Small values represent a higher priority.</remarks>
        /// <value>The preference.</value>
        int Preference { get; }

        /// <summary>
        /// Determines if the API is available.
        /// </summary>
        bool IsAvailable { get; }

        /// <summary>
        /// Obtains the default output device.
        /// </summary>
        /// <returns>Returns the default output device.</returns>
        IAudioDevice DefaultOutputDevice { get; }

        /// <summary>
        /// Obtains the default input device.
        /// </summary>
        /// <returns>Returns the default input device.</returns>
        IAudioDevice DefaultInputDevice { get; }

        /// <summary>
        /// Obtains all available output devices.
        /// </summary>
        /// <returns>Returns all output devices.</returns>
        IAudioDevice[] OutputDevices { get; }

        /// <summary>
        /// Obtains the available input devices.
        /// </summary>
        /// <returns>Returns all input devices.</returns>
        IAudioDevice[] InputDevices { get; }
    }
}
