using System;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides audio frame event arguments.
    /// </summary>
    /// <seealso cref="System.EventArgs" />
    public class AudioFrameEventArgs : EventArgs
    {
        /// <summary>The frame.</summary>
        public readonly AudioFrame Frame;

        /// <summary>Initializes a new instance of the <see cref="AudioFrameEventArgs"/> class.</summary>
        /// <param name="frame">The frame.</param>
        public AudioFrameEventArgs(AudioFrame frame)
        {
            Frame = frame;
        }
    }
}