using System.IO;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// TODO: Clean up comment :D
    /// This header is a result of a collective brainfuck. The only useful setting is the crc, that is ignored by most decoders.
    /// Attention: Some players may skip the whole tag if this header is used.
    /// </summary>
    
    public class ID3v2ExtendedHeaderRestrictions
    {
        /// <summary>
        /// Load ID3v2ExtendedHeaderRestrictions for ID3v2.4 byte flag
        /// </summary>
        /// <param name="value"></param>
        public static ID3v2ExtendedHeaderRestrictions FromID3v24(byte value)
        {
            ID3v2ImageSize l_ImageSize;
            ID3v2ExtendedHeaderSizeRestriction size = (ID3v2ExtendedHeaderSizeRestriction)(value >> 6);
            bool textEncoding = (value & 0x20) != 0;
            ID3v2ExtendedHeaderTextRestriction textLength = (ID3v2ExtendedHeaderTextRestriction)((value >> 3) & 0x03);
            bool l_ImageEncoding = (value & 0x4) != 0;
            switch (value & 0x03)
            {
                case 0: l_ImageSize = ID3v2ImageSize.None; break;
                case 1: l_ImageSize = ID3v2ImageSize.SizeVar256; break;
                case 2: l_ImageSize = ID3v2ImageSize.SizeVar64; break;
                case 3: l_ImageSize = ID3v2ImageSize.SizeFixed64; break;
                default: throw new InvalidDataException();
            }
            return new ID3v2ExtendedHeaderRestrictions(size, textEncoding, textLength, l_ImageEncoding, l_ImageSize);
        }

        /// <summary>
        /// Creates a new ID3v2ExtendedHeaderRestrictions instance
        /// </summary>
        /// <param name="size">The ID3v2ExtendedHeaderSizeRestriction</param>
        /// <param name="textEncoding">Tag encoding restriction</param>
        /// <param name="textLength">Tag text length restriction</param>
        /// <param name="imageEncoding">Tag image encoding restriction</param>
        /// <param name="imageSize">Tag image size restriction</param>
        public ID3v2ExtendedHeaderRestrictions(ID3v2ExtendedHeaderSizeRestriction size, bool textEncoding, ID3v2ExtendedHeaderTextRestriction textLength, bool imageEncoding, ID3v2ImageSize imageSize)
        {
            switch (size)
            {
                case ID3v2ExtendedHeaderSizeRestriction.Mega: Frames = 128; Size = 1024 * 1024; break;
                case ID3v2ExtendedHeaderSizeRestriction.Big: Frames = 64; Size = 128 * 1024; break;
                case  ID3v2ExtendedHeaderSizeRestriction.Small: Frames = 32; Size = 40 * 1024; break;
                case  ID3v2ExtendedHeaderSizeRestriction.Tiny: Frames = 32; Size = 4 * 1024; break;
                default: throw new InvalidDataException();
            }
            switch (textLength)
            {
                case ID3v2ExtendedHeaderTextRestriction.Unlimited: TextLength = 0; break;
                case  ID3v2ExtendedHeaderTextRestriction.Big: TextLength = 1024; break;
                case  ID3v2ExtendedHeaderTextRestriction.Small: TextLength = 128; break;
                case  ID3v2ExtendedHeaderTextRestriction.Tiny: TextLength = 30; break;
                default: throw new InvalidDataException();
            }
            TextEncoding = textEncoding;
            ImageEncoding = imageEncoding;
            ImageSize = imageSize;
        }

        /// <summary>
        /// Maximum size restriction.
        /// <para>
        /// &lt;= 0: no restriction<br />
        /// &gt; 0: Restricted to the specified number of bytes
        /// </para>
        /// </summary>
        public readonly int Size;

        /// <summary>
        /// Maximum frames restriction.
        /// <para>
        /// &lt;= 0: no restriction<br />
        /// &gt; 0: Restricted to the specified number of frames
        /// </para>
        /// </summary>
        public readonly int Frames;

        /// <summary>
        /// <para>
        /// 0: No restrictions
        /// 1: Strings are only encoded with ISO-8859-1 [ISO-8859-1] or UTF-8 [UTF-8].
        /// </para>
        /// </summary>
        public readonly bool TextEncoding;

        /// <summary>
        /// Maximum TextLength
        /// <para>
        /// &lt;= 0: no restriction<br />
        /// &gt; 0: Restricted to the specified number of chars
        /// </para>
        /// Note that nothing is said about how many bytes is used to
        /// represent those characters, since it is encoding dependent. If a
        /// text frame consists of more than one string, the sum of the
        /// strings is restricted as stated.
        /// </summary>
        public readonly int TextLength;

        /// <summary>
        /// Image encoding restrictions
        /// <para>
        /// 0: No restrictions<br />
        /// 1: Images are encoded only with PNG [PNG] or JPEG [JFIF].
        /// </para>
        /// </summary>
        public readonly bool ImageEncoding;

        /// <summary>
        /// Image size. If set (Size != 0,0) all Images have the specified size.
        /// </summary>
        public readonly ID3v2ImageSize ImageSize;
    }
}
