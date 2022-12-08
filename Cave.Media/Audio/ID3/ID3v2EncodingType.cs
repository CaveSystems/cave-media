#pragma warning disable CA1707 // Field names should not contain underscore

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// ID3 v2 text encodings.
    /// </summary>
    public enum ID3v2EncodingType : byte
    {
        /// <summary>The iso88591 encoding</summary>
        ISO88591 = 0,

        /// <summary>The unicode encoding</summary>
        Unicode = 1,

        /// <summary>The big endian unicode encoding</summary>
        BigEndianUnicode = 2,

        /// <summary>utf8 encoding (available since id3v2 2.4)</summary>
        UTF8 = 3,

        /// <summary>iso88591 encoding (id3v2 2.2)</summary>
        ISO88591_OLD = 32,
    }
}
