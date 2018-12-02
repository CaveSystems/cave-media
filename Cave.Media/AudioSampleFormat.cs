namespace Cave.Media
{
    /// <summary>
    /// Provides available sample formats
    /// </summary>
    
    public enum AudioSampleFormat
    {
        /// <summary>
        /// Unknown format
        /// </summary>
        Unknown = 0x00,

        /// <summary>
        /// signed 8 bit samples
        /// </summary>
        Int8 = 0x01,

        /// <summary>
        /// signed 16 bit samples
        /// </summary>
        Int16 = 0x02,

        /// <summary>
        /// signed 24 bit samples
        /// </summary>
        Int24 = 0x03,

        /// <summary>
        /// signed 32 bit samples
        /// </summary>
        Int32 = 0x04,

        /// <summary>
        /// signed 32 bit samples
        /// </summary>
        Int64 = 0x05,

        /// <summary>
        /// floating point samples (range -1..0..+1)
        /// </summary>
        Float = 0x101,

        /// <summary>
        /// double precision floating point samples (range -1..0..+1)
        /// </summary>
        Double = 0x102,
    }
}
