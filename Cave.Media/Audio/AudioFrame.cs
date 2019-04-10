using System;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides a basic audio frame (may be encoded, packed or encrypted and is not directly playable.
    /// For playable audio data see <see cref="IAudioData"/>.
    /// </summary>
    public abstract class AudioFrame : DataFrame
    {
        /// <summary>
        /// If <see cref="IsAudio"/> == true this returns the duration of the frame otherwise it always returns <see cref="TimeSpan.Zero"/>.
        /// </summary>
        public abstract TimeSpan Duration { get; }

        /// <summary>
        /// Obtains whether the frame contains audio or not.
        /// </summary>
        public abstract bool IsAudio { get; }
    }
}
