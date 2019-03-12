using System;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides an ID3v2 frame implementation.
    /// </summary>
    public class ID3v2Frame
    {
        /// <summary>The full data (including header, may be compressed, encrypted, unsynced, ...)</summary>
        protected byte[] m_Data;

        /// <summary>The (decrypted, decoded, deunsynced, uncompressed, ...) content.</summary>
        protected byte[] m_Content;

        /// <summary>Gets the frame header.</summary>
        /// <value>The frame header.</value>
        protected ID3v2FrameHeader m_Header;

        /// <summary>Initializes a new instance of the <see cref="ID3v2Frame"/> class.</summary>
        /// <param name="frame">The frame.</param>
        /// <exception cref="ArgumentNullException">Frame.</exception>
        public ID3v2Frame(ID3v2Frame frame)
        {
            if (frame == null)
            {
                throw new ArgumentNullException("Frame");
            }

            m_Data = frame.m_Data;
            m_Content = frame.m_Content;
            m_Header = frame.m_Header;
        }

        /// <summary>Initializes a new instance of the <see cref="ID3v2Frame" /> class.</summary>
        /// <param name="header">The header.</param>
        /// <param name="reader">The reader.</param>
        /// <exception cref="ArgumentNullException">Header.</exception>
        /// <exception cref="NotSupportedException"></exception>
        public ID3v2Frame(ID3v2Header header, DataFrameReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Reader");
            }

            m_Header = new ID3v2FrameHeader(header, reader);

            // prepare content (has to be decoded, decrypted, decompressed, ...
            m_Content = reader.Read(m_Header.HeaderSize, m_Header.ContentSize);

            switch (header.Version)
            {
                case 2: /*nothing to do, raw plain content data*/ break;
                case 3: ParseVersion3(reader); break;
                case 4: ParseVersion4(reader); break;
                default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", header.Version));
            }

            // copy raw data and remove from reader
            m_Data = reader.GetBuffer(m_Header.HeaderSize + m_Header.ContentSize);
        }

        /// <summary>Initializes a new instance of the <see cref="ID3v2Frame" /> class.</summary>
        /// <param name="header">The header.</param>
        /// <param name="data">The data.</param>
        public ID3v2Frame(ID3v2Header header, byte[] data)
        {
            m_Header = new ID3v2FrameHeader(header, data);
            if (m_Header.ContentSize + m_Header.HeaderSize != data.Length)
            {
                throw new ArgumentOutOfRangeException("data", $"Invalid size of data! Expected {m_Header.ContentSize + m_Header.HeaderSize} bytes, got {data.Length}!");
            }

            m_Data = data;
            m_Content = new byte[m_Header.ContentSize];
            Array.Copy(m_Data, m_Header.HeaderSize, m_Content, 0, m_Header.ContentSize);
        }

        #region parser functions

        /// <summary>
        /// Provides decompression.
        /// </summary>
        /// <param name="data">The data to be decompressed.</param>
        /// <returns>Retruns decompressed data.</returns>
        protected byte[] Decompress(byte[] data)
        {
            throw new NotSupportedException("ID3v2 Compressed Data is not jet supported!");
        }

        /// <summary>
        /// Provides decryption.
        /// </summary>
        /// <param name="data">The data to be decompressed.</param>
        /// <returns>Retruns decrypted data.</returns>
        protected byte[] Decrypt(byte[] data)
        {
            throw new NotSupportedException("ID3v2 Encrypted Data is not jet supported!");
        }

        void ParseVersion3(DataFrameReader reader)
        {
            if (m_Header.Flags.Compression)
            {
                m_Content = Decompress(m_Content);
            }

            if (m_Header.Flags.Encryption)
            {
                m_Content = Decrypt(m_Content);
            }
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame
        /// This function will throw exceptions on parser errors.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        void ParseVersion4(DataFrameReader reader)
        {
            if ((m_Header.TagHeader.Flags & ID3v2HeaderFlags.Unsynchronisation) == 0)
            {
                // no full unsync done, check if we have to unsync now
                if (m_Header.Flags.Unsynchronisation)
                {
                    m_Content = ID3v2DeUnsync.Buffer(m_Content);
                }
            }
            if (m_Header.Flags.Compression)
            {
                m_Content = Decompress(m_Content);
            }

            if (m_Header.Flags.Encryption)
            {
                m_Content = Decrypt(m_Content);
            }
        }

        #endregion

        #region public properties

        /// <summary>Gets the identifier.</summary>
        /// <value>The identifier.</value>
        public string ID => m_Header.ID;

        /// <summary>Gets the flags.</summary>
        /// <value>The flags.</value>
        public ID3v2FrameFlags Flags => m_Header.Flags;

        /// <summary>Gets the length of the raw data including header and body encoded, encrypted, compressed, ...</summary>
        /// <value>The length of the raw data.</value>
        public int Length => m_Header.HeaderSize + m_Header.ContentSize;

        /// <summary>Gets the length of the content.</summary>
        /// <value>The length of the content.</value>
        public int ContentLength => m_Header.ContentSize;

        /// <summary>Gets the raw data.</summary>
        /// <value>The raw data.</value>
        public byte[] RawData => (byte[])m_Data.Clone();

        /// <summary>Gets the content.</summary>
        /// <value>The content.</value>
        public byte[] Content => (byte[])m_Content.Clone();

        #endregion

        /// <summary>
        /// Obtains a string describing this frame.
        /// </summary>
        /// <returns>ID[Length].</returns>
        public override string ToString()
        {
            return m_Header.ToString();
        }

        /// <summary>
        /// Obtains the hashcode for this instance.
        /// </summary>
        /// <returns></returns>
        public override int GetHashCode()
        {
            return m_Data.GetHashCode();
        }
    }
}
