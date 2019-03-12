namespace Cave.Media.Codecs
{
    /// <summary>
    /// Provides decoder states.
    /// </summary>
    public enum CCITT4DecoderState
    {
        /// <summary>
        /// white pixels expected
        /// </summary>
        White = 0,

        /// <summary>
        /// white termination pixels expected
        /// </summary>
        WhiteTerminationRequired = 1,

        /// <summary>
        /// black pixels expected
        /// </summary>
        Black = 2,

        /// <summary>
        /// black termination pixels expected
        /// </summary>
        BlackTerminationRequired = 3,
    }
}
