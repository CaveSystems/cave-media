namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides the available ID3v2Reader states.
    /// </summary>
    public enum ID3v2ReaderState
    {
        /// <summary>
        /// An error occured
        /// </summary>
        Error = -1,

        /// <summary>
        /// currently reading the header
        /// </summary>
        ReadHeader = 0,

        /// <summary>
        /// currently reading the extended header
        /// </summary>
        ReadExtendedHeader,

        /// <summary>
        /// currently reading frames
        /// </summary>
        ReadFrames,

        /// <summary>
        /// currently reading the footer
        /// </summary>
        ReadFooter,

        /// <summary>
        /// reading ended
        /// </summary>
        ReadEnd,
    }
}
