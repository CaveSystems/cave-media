using System;
namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides ID3 header flags
    /// </summary>
    [Flags]
    public enum ID3v2HeaderFlags : byte
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,

        /// <summary>
        /// Footer present. A set bit indicates the presence of a footer.
        /// </summary>
        Footer = 0x10,

        /// <summary>
        /// Experimental tag. This flag SHALL always be set when the tag is in an experimental stage.
        /// </summary>
        Experimental = 0x20,

        /// <summary>
        /// Indicates whether or not the header is followed by an extended header. A set bit indicates the presence of an extended header.
        /// </summary>
        ExtendedHeader = 0x40,

        /// <summary>
        /// Indicates whether or not unsynchronisation is used. A set bit indicates usage.
        /// </summary>
        Unsynchronisation = 0x80,
    }
}
