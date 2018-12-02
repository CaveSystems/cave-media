namespace Cave.Media.Audio
{
    /// <summary>
    /// Implements an abstract base class for the <see cref="IAudioEncoderQuality"/> interface
    /// </summary>
    public abstract class AudioEncoderQuality : IAudioEncoderQuality
    {
        /// <summary>
        /// Obtains the name of the quality setting
        /// </summary>
        public abstract string Name { get; }

        /// <summary>
        /// Obtains the description for the quality setting
        /// </summary>
        public abstract string Description { get; }
    }
}
