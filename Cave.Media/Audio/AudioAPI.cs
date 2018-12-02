using Cave.Text;
using System;
using System.Collections.Generic;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides an abstract base class to implement audio apis
    /// </summary>
    public abstract class AudioAPI : IAudioAPI
    {
        /// <summary>
        /// Obtains all available <see cref="AudioAPI"/> instances.
        /// </summary>
        /// <returns></returns>
        public static IAudioAPI[] GetAvailableAudioAPIs()
        {
            var apis = AppDom.GetTypes<IAudioAPI>();
            apis.Sort();
            return apis.ToArray();
        }

        /// <summary>
        /// Obtains all available <see cref="AudioAPI"/> instances.
        /// </summary>
        /// <returns></returns>
        public static IAudioEncoder[] GetAvailableAudioEncoders()
        {
            var encoder = AppDom.GetTypes<IAudioEncoder>();
            encoder.Sort();
            return encoder.ToArray();
        }

        /// <summary>
        /// Obtains all available <see cref="AudioAPI"/> instances.
        /// </summary>
        /// <returns></returns>
        public static IAudioDecoder[] GetAvailableAudioDecoders()
        {
            var decoder = AppDom.GetTypes<IAudioDecoder>();
            decoder.Sort();
            return decoder.ToArray();
        }

        /// <summary>Initializes a new instance of the <see cref="AudioAPI"/> class.</summary>
        public AudioAPI() { }

        #region IAudioAPI Member

        /// <summary>Gets the preference value.</summary>
        /// <value>The preference.</value>
        /// <remarks>Small values represent a higher priority</remarks>
        public abstract int Preference { get; }

        /// <summary>
        /// Determines if the API is available
        /// </summary>
        public abstract bool IsAvailable { get; }

        /// <summary>
        /// Obtains the default output device
        /// </summary>
        /// <returns>Returns the default output device</returns>
        public virtual IAudioDevice DefaultOutputDevice
        {
            get
            {
                if (OutputDevices.Length == 0) return null;
                return OutputDevices[0];
            }
        }

        /// <summary>
        /// Obtains the default input device
        /// </summary>
        /// <returns>Returns the default input device</returns>
        public virtual IAudioDevice DefaultInputDevice
        {
            get
            {
                if (InputDevices.Length == 0) return null;
                return InputDevices[0];
            }
        }

        /// <summary>
        /// Obtains all available output devices
        /// </summary>
        /// <returns>Returns all output devices</returns>
        public abstract IAudioDevice[] OutputDevices { get; }

        /// <summary>
        /// Obtains the available input devices
        /// </summary>
        /// <returns>Returns all input devices</returns>
        public abstract IAudioDevice[] InputDevices { get; }

        #endregion

        /// <summary>Vergleicht das aktuelle Objekt mit einem anderen Objekt desselben Typs.</summary>
        /// <param name="other">Ein Objekt, das mit diesem Objekt verglichen werden soll.</param>
        /// <returns>
        /// Ein Wert, der die relative Reihenfolge der verglichenen Objekte angibt.Der Rückgabewert hat folgende Bedeutung:Wert Bedeutung Kleiner als 0 (null) Dieses Objekt ist kleiner als der <paramref name="other" />-Parameter.Zero Dieses Objekt ist gleich <paramref name="other" />. Größer als 0 (null) Dieses Objekt ist größer als <paramref name="other" />.
        /// </returns>
        public int CompareTo(IAudioAPI other)
        {
            return Preference.CompareTo(other.Preference);
        }
    }
}
