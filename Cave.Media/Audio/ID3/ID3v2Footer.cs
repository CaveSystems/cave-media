using System.IO;
using Cave.Media.Audio.MP3;
using System;
using Cave.Collections.Generic;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides an ID3v2 footer implementation. <br />
    /// To speed up the process of locating an ID3v2 tag when searching from
    /// the end of a file, a footer can be added to the tag. It is REQUIRED
    /// to add a footer to an appended tag, i.e. a tag located after all
    /// audio data. The footer is a copy of the header, but with a different
    /// identifier.
    /// </summary>
    
    public class ID3v2Footer : MP3MetaFrame
    {
        #region private fields and implementation
        byte m_Version;
        byte m_Revision;
        ID3v2HeaderFlags m_Flags;
        int m_BodySize;
        byte[] m_Data = null;

        ID3v2HeaderFlags CheckFlags(byte b)
        {
            switch (m_Version)
            {
                case 0:
                case 1:
                    throw new InvalidDataException(string.Format("Invalid ID3v2.{0} tag!", m_Version));

                case 2:
                    if ((b & 0x3F) != 0) throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", m_Version));
                    break;

                case 3:
                    if ((b & 0x1F) != 0) throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", m_Version));
                    break;

                case 4:
                    if ((b & 0x0F) != 0) throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", m_Version));
                    break;
            }
            return (ID3v2HeaderFlags)b;
        }
        #endregion

        #region parser functions
        /// <summary>
        /// Internally parses the current data and loads all fields
        /// </summary>
        protected void ParseData()
        {
            if ((m_Data[0] != (byte)'3') || (m_Data[1] != (byte)'D') || (m_Data[2] != (byte)'I')) throw new InvalidDataException(string.Format("Missing ID3 identifier!"));
            m_Version = m_Data[3];
            m_Revision = m_Data[4];
            m_Flags = CheckFlags(m_Data[5]);
            m_BodySize = ID3v2DeUnsync.Int32(m_Data, 6);
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame
        /// This function will throw exceptions on parser errors.
        /// </summary>
        /// <param name="reader">FrameReader to read from</param>
        public override bool Parse(DataFrameReader reader)
        {
            if (reader == null) throw new ArgumentNullException("Reader");
            if (!reader.EnsureBuffer(10)) return false;
            m_Data = reader.Read(0, 10);
            ParseData();
            reader.Remove(10);
            return false;
        }
        #endregion

        #region public properties
        /// <summary>
        /// Gets/sets the ID3v2 (major) version used
        /// </summary>
        public byte Version
        {
            get { return m_Version; }
            set { m_Version = value; m_Data = null; }
        }

        /// <summary>
        /// Gets/sets the ID3v2 revision used
        /// </summary>
        public byte Revision
        {
            get { return m_Revision; }
            set { m_Revision = value; m_Data = null; }
        }

        /// <summary>
        /// Gets/sets the ID3v2 revision used
        /// </summary>
        public ID3v2HeaderFlags Flags
        {
            get { return m_Flags; }
            set { m_Flags = value; m_Data = null; }
        }

        /// <summary>
        /// The ID3v2 tag size is the size of the complete tag after
        /// unsychronisation, including padding, excluding the header but not
        /// excluding the extended header (total tag size - 10). Only 28 bits
        /// (representing up to 256MB) are used in the size description to avoid
        /// the introducuction of 'false syncsignals'.
        /// </summary>
        public int BodySize
        {
            get { return m_BodySize; }
            set { m_BodySize = value; m_Data = null; }
        }
        #endregion

        /// <summary>
        /// returns 10
        /// </summary>
        public override int Length { get { return 10; } }

        /// <summary>
        /// Obtains an array with the data for this instance
        /// </summary>
        /// <returns></returns>
        public override byte[] Data
        {
            get
            {
                if (m_Data == null)
                {
                    switch (m_Version)
                    {
                        case 2:
                            m_Flags = (ID3v2HeaderFlags)((int)m_Flags & 0xC0);
                            break;
                        case 3:
                            m_Flags = (ID3v2HeaderFlags)((int)m_Flags & 0xE0);
                            break;
                        case 4:
                            m_Flags = (ID3v2HeaderFlags)((int)m_Flags & 0xF0);
                            break;
                        default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", m_Version));
                    }
                    m_Data = new byte[10];
                    m_Data[0] = (byte)'3';
                    m_Data[1] = (byte)'D';
                    m_Data[2] = (byte)'I';
                    m_Data[3] = m_Version;
                    m_Data[4] = 0;
                    m_Data[5] = (byte)((int)m_Flags & 0xF0);
                    ID3v2EnUnsync.Int32(m_BodySize, m_Data, 6);
                    ParseData();
                }
                return (byte[])m_Data.Clone();
            }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public override bool IsFixedLength { get { return true; } }
    }
}
