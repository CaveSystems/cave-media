using System;
using System.Diagnostics;
using System.IO;
using Cave.IO;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// TXXX : extended name value text frames.
    /// </summary>
    public sealed class ID3v2TXXXFrame : ID3v2Frame
    {
        /// <summary>Creates a new header.</summary>
        /// <param name="header">The tag header.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="name">The name.</param>
        /// <param name="value">The value.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static ID3v2TXXXFrame Create(ID3v2Header header, ID3v2FrameFlags flags, string name, string value)
        {
            ID3v2EncodingType encoding = ID3v2Encoding.Select(header, name + value);

            // header, encoding[1], name+0, value+0
            byte[] nameBytes = ID3v2Encoding.GetBytes(encoding, name, true);
            byte[] valueBytes = ID3v2Encoding.GetBytes(encoding, value, true);
            int contentSize = nameBytes.Length + valueBytes.Length + 1;
            var frameHeader = ID3v2FrameHeader.Create(header, "TXXX", flags, contentSize);
            using (var ms = new MemoryStream())
            {
                var writer = new DataWriter(ms);
                writer.Write(frameHeader.Data);
                writer.Write((byte)encoding);
                writer.Write(nameBytes);
                writer.Write(valueBytes);
                return new ID3v2TXXXFrame(new ID3v2Frame(header, ms.ToArray()));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ID3v2TXXXFrame"/> class.</summary>
        /// <param name="frame">The frame.</param>
        /// <exception cref="FormatException"></exception>
        internal ID3v2TXXXFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "TXXX")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "TXXX"));
            }

            ParseData();
        }

        void ParseData()
        {
            int start = 0;
            EncodingType = (ID3v2EncodingType)m_Content[start++];
            start += ID3v2Encoding.Parse(EncodingType, m_Content, start, out string name);
            start += ID3v2Encoding.Parse(EncodingType, m_Content, start, out string value);
            if (start != m_Content.Length)
            {
                Trace.WriteLine(string.Format("{0} bytes garbage at end of frame!", m_Content.Length - start));
            }
            Name = name;
            Value = value;
        }

        /// <summary>The encoding of this frame.</summary>
        public ID3v2EncodingType EncodingType { get; private set; }

        /// <summary>
        /// Gets the name of this frame.
        /// </summary>
        public string Name { get; private set; }

        /// <summary>
        /// Gets the value of this frame.
        /// </summary>
        public string Value { get; private set; }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "<see cref="Name"/>":"<see cref="Value"/>".</returns>
        public override string ToString()
        {
            return base.ToString() + " \"" + Name + "\":\"" + Value + '"';
        }
    }
}
