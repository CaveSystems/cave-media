namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides available output modes for mp3 audio decoders
    /// </summary> 
    public enum MP3AudioOutputMode
    {
        /// <summary>both channels</summary>
        Both = 0,

        /// <summary>The left channel only</summary>
        Left = 1,

        /// <summary>The right channel only</summary>
        Right = 2,

        /// <summary>Down mix to mono</summary>
        DownMix = 3
	}
}