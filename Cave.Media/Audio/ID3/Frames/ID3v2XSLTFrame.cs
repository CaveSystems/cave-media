using Cave.IO;
using System;
using System.IO;
using System.Text;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// XSLT: extended synchronized lyrics tag.
    /// This frame contains a full sequence of lyrics with timestamps for the whole song.
    /// It is able to store CDG, CDG plus and some other animation formats for karaoke.
    /// </summary>
    public sealed class ID3v2XSLTFrame : ID3v2Frame
    {
        /// <summary>Creates a new header.</summary>
        /// <param name="header">The tag header.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException">Unsupported Header Version.</exception>
        public static ID3v2XSLTFrame Create(ID3v2Header header, ID3v2FrameFlags flags, byte[] data)
        {
            switch (header.Version)
            {
                case 3:
                case 4: break;
                default: throw new NotSupportedException("Unsupported Header Version");
            }
            ID3v2FrameHeader frameHeader = ID3v2FrameHeader.Create(header, "XSLT", flags, data.Length);
            using (MemoryStream ms = new MemoryStream())
            {
                DataWriter writer = new DataWriter(ms);
                writer.Write(frameHeader.Data);
                writer.Write(data);
                return new ID3v2XSLTFrame(new ID3v2Frame(header, ms.ToArray()));
            }
        }

        /// <summary>Initializes a new instance of the <see cref="ID3v2XSLTFrame"/> class.</summary>
        /// <param name="frame">The frame.</param>
        /// <exception cref="FormatException"></exception>
        internal ID3v2XSLTFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "XSLT")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "XSLT"));
            }
        }
    }
}
