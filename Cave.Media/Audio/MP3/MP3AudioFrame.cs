using Cave.Collections.Generic;
using Cave.Text;
using System;
using System.IO;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides a mp3 audio frame (containing encoded mp3 audio data)
    /// </summary>
    public sealed class MP3AudioFrame : AudioFrame
    {
        MP3AudioFrameHeader m_Header;
        byte[] m_Data;
        bool m_InvalidPaddingCorrected;
        MP3BitReserve m_Bits;

        /// <summary>Retrieves the bits of the frame exclusing header.</summary>
        /// <value>The bits.</value>
        public MP3BitReserve Bits
        {
            get
            {
                if (m_Bits == null)
                {
                    if (m_Header.Protection)
                    {
                        m_Bits = new MP3BitReserve(m_Data, 6);
                    }
                    else
                    {
                        m_Bits = new MP3BitReserve(m_Data, 4);
                    }
                }
                return m_Bits;
            }
        }

        /// <summary>
        /// Creates a new empty frame
        /// </summary>
        public MP3AudioFrame()
        {
        }

        /// <summary>
        /// Creates a new empty frame
        /// </summary>
        public MP3AudioFrame(byte[] data)
        {
            m_Data = data;
            m_Header = new MP3AudioFrameHeader(data);
            if (m_Header.Validation != MP3AudioFrameHeadervalidation.Valid) throw new InvalidDataException();
            int dataLength = m_Header.Length;
            if (dataLength == 0 || data.Length != dataLength) throw new InvalidDataException();
        }

        /// <summary>
        /// Parses the specified stream to load all fields for this instance
        /// </summary>
        /// <param name="reader">FrameReader to read from</param>
        public override bool Parse(DataFrameReader reader)
        {
            if (reader == null) throw new ArgumentNullException("Reader");
            if (!reader.EnsureBuffer(4)) return false;
            byte[] headerData = reader.Read(0, 4);
            m_Header = new MP3AudioFrameHeader(headerData);
            if (m_Header.Validation != MP3AudioFrameHeadervalidation.Valid) return false;

            int dataLength = m_Header.Length;
            if (dataLength == 0) return false;

            if (!reader.EnsureBuffer(dataLength)) return false;
            m_Data = reader.Read(0, dataLength);

            //check next header
            if (reader.EnsureBuffer(dataLength + 4))
            {
                byte[] nextHeaderBuffer = reader.Read(dataLength, 4);
                MP3AudioFrameHeader next = new MP3AudioFrameHeader(nextHeaderBuffer);
                if (next.Validation != MP3AudioFrameHeadervalidation.Valid)
                {
                    if ((nextHeaderBuffer[0] == 'I') && (nextHeaderBuffer[1] == 'D') && (nextHeaderBuffer[2] == '3'))
                    {
                        //ID3 v2 tag incoming
                    }
                    else if ((nextHeaderBuffer[0] == 'T') && (nextHeaderBuffer[1] == 'A') && (nextHeaderBuffer[2] == 'G'))
                    {
                        //ID3 v1 tag incoming
                    }
                    else
                    {
                        //next header is invalid, check if the padding bit is set incorrectly
                        //there is a high pobability that the padding bit is invalid if
                        //the framestart is not directly after our buffer but one byte late
                        int newStart = dataLength + (m_Header.Padding ? -1 : 1);
                        nextHeaderBuffer = reader.Read(newStart, 4);
                        next = new MP3AudioFrameHeader(nextHeaderBuffer);
                        if (next.Validation == MP3AudioFrameHeadervalidation.Valid)
                        {
                            if (!m_Header.Padding)
                            {
                                //frame has a padding byte but the header padding bit is not set
                                m_Data = reader.Read(0, newStart);
                            }
                            else
                            {
                                //frame has no padding byte but the header padding bit is set
                                m_Data = reader.Read(0, newStart);
                            }
                            m_InvalidPaddingCorrected = true;
                        }
                    }
                }
            }            

            reader.Remove(m_Data.Length);
            return true;
        }

        TimeSpan m_Duration = TimeSpan.Zero;

        /// <summary>
        /// Obtains the duration of the audio frame
        /// </summary>
        public override TimeSpan Duration
        {
            get
            {
                if (m_Duration == TimeSpan.Zero)
                {
                    m_Duration = new TimeSpan(m_Header.SampleCount * TimeSpan.TicksPerSecond / m_Header.SamplingRate);
                }
                return m_Duration;
            }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public override bool IsAudio
        {
            get { return true; }
        }

        /// <summary>
        /// Returns true
        /// </summary>
        public override bool IsValid
        {
            get { return true; }
        }

        /// <summary>
        /// Obtains whether the padding bit at the header was corrected during Parse()
        /// </summary>
        public bool InvalidPaddingCorrected { get { return m_InvalidPaddingCorrected; } }

        /// <summary>
        /// Obtains the <see cref="MP3AudioFrameHeader"/>
        /// </summary>
        public MP3AudioFrameHeader Header { get { return m_Header; } }

        /// <summary>
        /// Obtains an array with the data for this instance
        /// </summary>
        /// <returns></returns>
        public override byte[] Data
        {
            get
            {
                return (byte[])m_Data.Clone();
            }
        }

        /// <summary>
        /// Length of the frame in bytes
        /// </summary>
        public override int Length { get { return m_Data.Length; } }

        /// <summary>
        /// Returns false (mp3 audio frames may differ in size depending on layer, bitrate and samplerate)
        /// </summary>
        public override bool IsFixedLength { get { return false; } }

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return $"MP3AudioFrame {Header}";
        }
    }
}
