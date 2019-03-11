namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides available mp3 audio frame channel configuration.
    /// </summary>
    public enum MP3AudioFrameChannels
    {
        /// <summary>
        /// stereo
        /// </summary>
        Stereo = 0,

        /// <summary>
        /// joint stereo (stereo surround information may be broken)
        /// </summary>
        JointStereo = 1,

        /// <summary>
        /// dual channel mono
        /// </summary>
        MonoDual = 2,

        /// <summary>
        /// mono
        /// </summary>
        Mono = 3,
    }
}
