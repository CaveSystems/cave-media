using System;
namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Flags for the <see cref="ID3v2ExtendedHeader"/>.
    /// </summary>
    [Flags]
    public enum ID3v2d4ExtendedHeaderFlags : byte
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,

        /// <summary>
        /// For some applications it might be desired to restrict a tag in more
        /// ways than imposed by the ID3v2 specification. Note that the
        /// presence of these restrictions does not affect how the tag is
        /// decoded, merely how it was restricted before encoding.
        /// </summary>
        Restrictions = 0x10,

        /// <summary>
        /// If this flag is set, a CRC-32 [ISO-3309] data is included in the
        /// extended header. The CRC is calculated on all the data between the
        /// header and footer as indicated by the header's tag length field,
        /// minus the extended header. Note that this includes the padding (if
        /// there is any), but excludes the footer.
        /// </summary>
        CRC32 = 0x20,

        /// <summary>
        /// If this flag is set, the present tag is an update of a tag found
        /// earlier in the present file or stream. If frames defined as unique
        /// are found in the present tag, they are to override any
        /// corresponding ones found in the earlier tag.
        /// </summary>
        Update = 0x40,
    }
}
