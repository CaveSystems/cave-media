using System;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// The extended header contains information that can provide further
    /// insight in the structure of the tag, but is not vital to the correct
    /// parsing of the tag information; hence the extended header is
    /// optional.
    /// </summary>
    public sealed class ID3v2ExtendedHeader : MP3MetaFrame
    {
        #region private fields and implementation
        readonly ID3v2Header m_Header;
        ID3v2ExtendedHeaderFlags m_Flags = new ID3v2ExtendedHeaderFlags();
        byte[] m_Data = null;

        #endregion

        #region Constructor and Parent field

        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        /// <param name="header"></param>
        public ID3v2ExtendedHeader(ID3v2Header header)
        {
            m_Header = header ?? throw new ArgumentNullException("Header");
        }
        #endregion

        #region parser functions

        bool ParseVersion2(DataFrameReader reader) => throw new NotImplementedException("TODO");

        bool ParseVersion3(DataFrameReader reader)
        {
            if (!reader.EnsureBuffer(4))
            {
                return false;
            }

            // calc size
            var sizeBytes = reader.Read(0, 4);
            var size = 0;
            for (var i = 0; i < 4; i++)
            {
                size = (size << 8) | sizeBytes[i];
            }

            size += 4;

            // get data
            if (!reader.EnsureBuffer(size))
            {
                return false;
            }

            m_Data = reader.GetBuffer(size);

            // get flags
            m_Flags = ID3v2ExtendedHeaderFlags.FromID3v23(m_Data);
            return true;
        }

        bool ParseVersion4(DataFrameReader reader)
        {
            if (!reader.EnsureBuffer(4))
            {
                return false;
            }

            // calc size
            var size = ID3v2DeUnsync.Int32(reader.Read(0, 4), 0);

            // get data
            if (!reader.EnsureBuffer(size))
            {
                return false;
            }

            m_Data = reader.GetBuffer(size);

            // get flags
            m_Flags = ID3v2ExtendedHeaderFlags.FromID3v24(m_Data);
            return true;
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        public override bool Parse(DataFrameReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Stream");
            }

            m_Flags = new ID3v2ExtendedHeaderFlags();

            switch (m_Header.Version)
            {
                case 2: return ParseVersion2(reader);
                case 3: return ParseVersion3(reader);
                case 4: return ParseVersion4(reader);
                default: return false;
            }
        }

        #endregion

        #region public properties

        /// <summary>
        /// Gets/sets the ID3v2 revision used.
        /// </summary>
        public ID3v2ExtendedHeaderFlags Flags
        {
            get { return m_Flags; }
            set
            {
                m_Flags = value;
                m_Data = null;
            }
        }

        /// <summary>
        /// Size of the extended header.
        /// </summary>
        public override int Length => m_Data.Length;

        /// <summary>
        /// Returns false (extended header may vary in size).
        /// </summary>
        public override bool IsFixedLength => false;

        #endregion

        /// <summary>
        /// TODO.
        /// </summary>
        /// <returns></returns>
        public override byte[] Data
        {
            get
            {
                if (m_Data == null)
                {
                    switch (m_Header.Version)
                    {
                        // TODO: implement data creation
                        default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", m_Header.Version));
                    }
                }
                return (byte[])m_Data.Clone();
            }
        }
    }
}
