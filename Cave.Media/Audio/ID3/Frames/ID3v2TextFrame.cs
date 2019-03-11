using System;
using System.IO;
using Cave.IO;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Text information frame:<br />
    /// The text information frames are the most important frames, containing information like artist, album and more.
    /// There may only be one text information frame of its kind in an tag.
    /// </summary>

    public class ID3v2TextFrame : ID3v2Frame
    {
        /// <summary>Creates a new ID3v2TextFrame.</summary>
        /// <param name="header">The header.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="id">The identifier.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <exception cref="System.NotSupportedException"></exception>
        public static ID3v2TextFrame Create(ID3v2Header header, ID3v2FrameFlags flags, string id, string text)
        {
            ID3v2EncodingType encoding = ID3v2Encoding.Select(header, text);

            // header, encoding[1], name+0
            byte[] textBytes = ID3v2Encoding.GetBytes(encoding, text, true);
            int contentSize = 1 + textBytes.Length;
            ID3v2FrameHeader frameHeader = ID3v2FrameHeader.Create(header, id, flags, contentSize);
            using (MemoryStream ms = new MemoryStream())
            {
                DataWriter writer = new DataWriter(ms);
                writer.Write(frameHeader.Data);
                writer.Write((byte)encoding);
                writer.Write(textBytes);
                return new ID3v2TextFrame(new ID3v2Frame(header, ms.ToArray()));
            }
        }

        void ParseData()
        {
            EncodingType = (ID3v2EncodingType)m_Content[0];
            ID3v2Encoding.Parse(EncodingType, m_Content, 1, out string text);
            Text = text;
        }

        internal ID3v2TextFrame(ID3v2Frame frame)
            : base(frame)
        {
            if ((frame.ID[0] != 'T') || (frame.ID == "TXXX"))
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "T*"));
            }

            ParseData();
        }

        /// <summary>The encoding of this frame.</summary>
        public ID3v2EncodingType EncodingType { get; private set; }

        /// <summary>
        /// Obtains the text this frame contains.
        /// </summary>
        public string Text { get; private set; }

        /// <summary>
        /// Obtains a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Text".</returns>
        public override string ToString()
        {
            return base.ToString() + " \"" + Text + '"';
        }
    }
}
