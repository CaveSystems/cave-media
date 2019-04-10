using System;
using System.IO;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides an ID3v2 header implementation.
    /// </summary>
    public class ID3v2Header : MP3MetaFrame
    {
        #region private fields and implementation
        byte[] m_Data;
        byte m_Revision;
        int m_BodySize;

        ID3v2HeaderFlags CheckFlags(byte b)
        {
            switch (Version)
            {
                case 0:
                case 1:
                    throw new InvalidDataException(string.Format("Invalid ID3v2.{0} tag!", Version));

                case 2:
                    if ((b & 0x3F) != 0)
                    {
                        throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
                    }

                    break;

                case 3:
                    if ((b & 0x1F) != 0)
                    {
                        throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
                    }

                    break;

                case 4:
                    if ((b & 0x0F) != 0)
                    {
                        throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
                    }

                    break;
            }
            return (ID3v2HeaderFlags)b;
        }
        #endregion

        #region parser functions

        /// <summary>
        /// Internally parses the current data and loads all fields.
        /// </summary>
        protected void ParseData()
        {
            if ((m_Data[0] != (byte)'I') || (m_Data[1] != (byte)'D') || (m_Data[2] != (byte)'3'))
            {
                throw new InvalidDataException(string.Format("Missing ID3 identifier!"));
            }

            Version = m_Data[3];
            m_Revision = m_Data[4];
            Flags = CheckFlags(m_Data[5]);
            m_BodySize = ID3v2DeUnsync.Int32(m_Data, 6);
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        public override bool Parse(DataFrameReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Reader");
            }

            if (!reader.EnsureBuffer(10))
            {
                return false;
            }

            m_Data = reader.Read(0, 10);
            ParseData();
            return true;
        }

        #endregion

        /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
        public ID3v2Header() { }

        /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
        /// <param name="data">The data.</param>
        public ID3v2Header(byte[] data)
        {
            m_Data = new byte[10];
            Array.Copy(data, m_Data, 10);
            ParseData();
        }

        /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
        /// <param name="version">The version.</param>
        /// <param name="revision">The revision.</param>
        /// <param name="flags">The flags.</param>
        /// <param name="size">The size.</param>
        /// <exception cref="NotSupportedException"></exception>
        public ID3v2Header(byte version, byte revision, ID3v2HeaderFlags flags, int size)
        {
            Version = version;
            m_Revision = revision;
            Flags = flags;
            m_BodySize = size;

            switch (Version)
            {
                case 2:
                    Flags = (ID3v2HeaderFlags)((int)Flags & 0xC0);
                    throw new NotImplementedException("Missing ID3v2.2 implementation.");
                case 3:
                    Flags = (ID3v2HeaderFlags)((int)Flags & 0xE0);
                    break;
                case 4:
                    Flags = (ID3v2HeaderFlags)((int)Flags & 0xF0);
                    break;
                default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", Version));
            }
            m_Data = new byte[10];
            m_Data[0] = (byte)'I';
            m_Data[1] = (byte)'D';
            m_Data[2] = (byte)'3';
            m_Data[3] = Version;
            m_Data[4] = 0;
            m_Data[5] = (byte)((int)Flags & 0xF0);
            ID3v2EnUnsync.Int32(m_BodySize, m_Data, 6);
        }

        #region public properties

        /// <summary>
        /// Gets/sets the ID3v2 (major) version used.
        /// </summary>
        public byte Version { get; private set; }

        /// <summary>
        /// Gets/sets the ID3v2 revision used.
        /// </summary>
        public byte Revision { get { return m_Revision; } }

        /// <summary>
        /// Gets/sets the ID3v2 revision used.
        /// </summary>
        public ID3v2HeaderFlags Flags { get; private set; }

        /// <summary>
        /// The ID3v2 tag size is the size of the complete tag after
        /// unsychronisation, including padding, excluding the header but not
        /// excluding the extended header (total tag size - 10). Only 28 bits
        /// (representing up to 256MB) are used in the size description to avoid
        /// the introducuction of 'false syncsignals'.
        /// </summary>
        public int BodySize { get { return m_BodySize; } }

        /// <summary>Gets the size of the header.</summary>
        /// <value>The size of the header.</value>
        public int HeaderSize { get { return 10; } }
        #endregion

        /// <summary>
        /// returns 10.
        /// </summary>
        public override int Length { get { return 10; } }

        /// <summary>
        /// Obtains an array with the data for this instance.
        /// </summary>
        /// <returns></returns>
        public override byte[] Data { get { return (byte[])m_Data.Clone(); } }

        /// <summary>
        /// Returns true (header length is fixed).
        /// </summary>
        public override bool IsFixedLength { get { return true; } }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"ID3v2 Header Version {Version} Revision {Revision} [{BodySize}] {Flags}";
        }
    }
}
