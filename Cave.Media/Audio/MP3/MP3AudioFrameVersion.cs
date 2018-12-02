namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides the available mp3 audio frame versions
    /// </summary>
    public enum MP3AudioFrameVersion
    {
        /// <summary>
        /// frame is version 2.5
        /// </summary>
        Version25 = 0,

        /// <summary>
        /// Reserved - do not use
        /// </summary>
        Reserved = 1,

        /// <summary>
        /// frame is version 2
        /// </summary>
        Version2 = 2,

        /// <summary>
        /// frame is version 1
        /// </summary>
        Version1 = 3,
    }
}
