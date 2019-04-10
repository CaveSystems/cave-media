using System;
using System.Runtime.Serialization;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Exception: Device is not a valid audio device !.
    /// </summary>
    [Serializable]
    public class InvalidAudioDeviceException : Exception
    {
        /// <summary>Initializes a new instance of the <see cref="InvalidAudioDeviceException"/> class.</summary>
        public InvalidAudioDeviceException()
            : base("Invalid audio device!") { }

        /// <summary>Initializes a new instance of the <see cref="InvalidAudioDeviceException"/> class.</summary>
        /// <param name="dev">The device.</param>
        public InvalidAudioDeviceException(string dev)
            : base(string.Format("Device '{0}' is not a valid audio device!", dev)) { }

        /// <summary>Initializes a new instance of the <see cref="InvalidAudioDeviceException"/> class.</summary>
        /// <param name="dev">The device.</param>
        /// <param name="innerException">Inner exception.</param>
        public InvalidAudioDeviceException(string dev, Exception innerException)
            : base(string.Format("Device '{0}' is not a valid audio device!", dev), innerException) { }

        /// <summary>Initializes a new instance of the <see cref="InvalidAudioDeviceException"/> class.</summary>
        /// <param name="info">The serialization information.</param>
        /// <param name="context">The streaming context.</param>
        protected InvalidAudioDeviceException(SerializationInfo info, StreamingContext context)
            : base(info, context) { }
    }
}