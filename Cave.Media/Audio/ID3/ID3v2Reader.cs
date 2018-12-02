using System;
using System.Diagnostics;
using System.IO;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides an ID3v2 tag reader for mp3 files
    /// </summary>
    
    public class ID3v2Reader
    {
        ID3v2ReaderState m_State = ID3v2ReaderState.ReadHeader;
        DataFrameReader m_Reader;
        ID3v2Header m_Header;
        long m_BodyBytes;

        internal ID3v2Reader(Stream stream)
        {
            m_Reader = new DataFrameReader(stream, 0);
        }

        internal ID3v2Reader(DataFrameReader reader)
        {
            m_Reader = reader;
        }

        /// <summary>Obtains the number of bytes currently available for reading</summary>
        public int Available { get { return m_Reader.Available; } }

        /// <summary>
        /// Obtains the current <see cref="ID3v2ReaderState"/>
        /// </summary>
        public ID3v2ReaderState State { get { return m_State; } }

        /// <summary>
        /// Reads the header (check <see cref="State"/> before usage)
        /// </summary>
        /// <returns></returns>
        public ID3v2Header ReadHeader(out byte[] tagData)
        {
            if (m_State != ID3v2ReaderState.ReadHeader) throw new InvalidOperationException(string.Format("Cannot read header at state {0}", m_State));
            ID3v2Header header = new ID3v2Header();
            if (!header.Parse(m_Reader))
            {
                tagData = null;
                return null;
            }
            m_BodyBytes = header.BodySize;
            if ((header.Flags & ID3v2HeaderFlags.Footer) != 0)
            {
                m_BodyBytes -= 10;
            }
            m_State++;
            if (header.Version < 2)
            {
                tagData = null;
                return null;
            }

            tagData = m_Reader.GetBuffer(header.HeaderSize + header.BodySize);
            byte[] bodyData = tagData.GetRange(header.HeaderSize);
            //need to unsync whole tag?
            if ((header.Flags & ID3v2HeaderFlags.Unsynchronisation) != 0)
            {
                bodyData = ID3v2DeUnsync.Buffer(bodyData);
                m_BodyBytes = bodyData.Length;
            }
            //update reader (use cached data)
            m_Header = header;
            m_Reader = new DataFrameReader(bodyData);
            return header;
        }

        /// <summary>
        /// Reads the extended header if any exist (check <see cref="State"/> before usage).
        /// </summary>
        /// <returns>Returns the extended header is present or null otherwise</returns>
        public bool ReadExtendedHeader(out ID3v2ExtendedHeader extendedHeader)
        {
            if (m_State != ID3v2ReaderState.ReadExtendedHeader) throw new InvalidOperationException(string.Format("Cannot read extended header at state {0}", m_State));
            m_State++;
            extendedHeader = null;
            if ((m_Header.Flags & ID3v2HeaderFlags.ExtendedHeader) == 0)
            {
                //no extended header present
                return true;
            }
            extendedHeader = new ID3v2ExtendedHeader(m_Header);
            return extendedHeader.Parse(m_Reader);
        }

        /// <summary>
        /// Reads a frame (check <see cref="State"/> before usage)
        /// </summary>
        /// <returns>Returns a frame if one left or null otherwise</returns>
        public bool ReadFrame(out ID3v2Frame frame)
        {
            if (m_State != ID3v2ReaderState.ReadFrames) throw new InvalidOperationException(string.Format("Cannot read frame at state {0}", m_State));
            frame = null;
            if (m_Reader.BufferStartPosition >= m_BodyBytes)
            {
                if ((m_Header.Flags & ID3v2HeaderFlags.Footer) == 0)
                {
                    //no footer, end of tag
                    m_State = ID3v2ReaderState.ReadEnd;
                    return true;
                }
                m_State = ID3v2ReaderState.ReadFooter;
                return true;
            }

            if (m_Reader.ReadByte(0) == 0)
            {
                //check padding with zero bytes
                byte[] data;
                switch (m_Header.Version)
                {
                    case 2:
                        {
                            long read = m_BodyBytes - m_Reader.BufferStartPosition;
                            data = m_Reader.GetBuffer((int)read);
                            m_State = ID3v2ReaderState.ReadEnd;
                            frame = null;
                            break;
                        }
                    case 3:
                    case 4:
                        {
                            //null frame, used by some encoders to implement padding, this is not allowed if footer present
                            if ((m_Header.Flags & ID3v2HeaderFlags.Footer) != 0) throw new InvalidDataException(string.Format("Invalid padding frames inside of tag with footer!"));
                            //load padding bytes
                            long read = m_BodyBytes - m_Reader.BufferStartPosition;
                            data = m_Reader.GetBuffer((int)read);
                            m_State = ID3v2ReaderState.ReadEnd;
                            frame = null;
                        }
                        break;
                    default: return false;
                }
                for (int i = 0; i < data.Length; i++)
                {
                    if (data[i] != 0)
                    {
                        Trace.TraceError("Additional garbage in padding of ID3v2 tag!");
                        break;
                    }
                }
                return false;
            }

            try
            {
                frame = new ID3v2Frame(m_Header, m_Reader);
            }
            catch (Exception ex)
            {
                Trace.TraceError("Error parsing id3 frame.");
                return false;
            }
            return true;
        }

        /// <summary>
        /// Reads the footer (check <see cref="State"/> before usage)
        /// </summary>
        /// <returns></returns>
        public bool ReadFooter(out ID3v2Footer footer)
        {
            if (m_State != ID3v2ReaderState.ReadFooter) throw new InvalidOperationException(string.Format("Cannot read footer at state {0}", m_State));
            footer = new ID3v2Footer();
            if (!footer.Parse(m_Reader))
            {
                return false;
            }
            return true;
        }

        /// <summary>
        /// Returns "ID3v2Reader Name"
        /// </summary>
        /// <returns>Returns "ID3v2Reader Name"</returns>
        public override string ToString()
        {
            return "ID3v2Reader " + m_Reader.Source;
        }
    }
}
