using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cave.IO;
using Cave.Media.Audio.ID3;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides a reader for mp3 audio files.
    /// </summary>
    public sealed class MP3Reader : IFrameSource
    {
        /// <summary>Reads all frames.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static List<AudioFrame> ReadAllFrames(string fileName)
        {
            using var stream = File.OpenRead(fileName);
            return ReadAllFrames(stream);
        }

        /// <summary>Reads all frames.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static List<AudioFrame> ReadAllFrames(Stream stream)
        {
            var reader = new MP3Reader(stream);
            var frames = new List<AudioFrame>();
            while (true)
            {
                var frame = reader.GetNextFrame();
                if (frame == null)
                {
                    break;
                }

                frames.Add(frame);
            }
            reader.Close();
            return frames;
        }

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName => "MP3Reader";

        #region Buffer search and frame header detection

        /// <summary>
        /// used to communicate the found header.
        /// </summary>
        enum MatchType
        {
            Invalid = 0,
            MP3Frame,
            ID3Frame,
        }

        /// <summary>
        /// Search class to detect valid headers at the mp3 file.
        /// </summary>
        class Search : IDataFrameSearch
        {
            int m_CurrentValue;
            int m_Index;

            public MatchType Match { get; private set; }

            public int Index => m_Index - Length;

            public int Length { get; private set; }

            #region IBufferSearch Member

            public bool Check(byte value)
            {
                m_Index++;
                m_CurrentValue = (m_CurrentValue << 8) | value;
                if ((m_CurrentValue & 0xFFFF) == 0xFFFF)
                {
                    Match = MatchType.Invalid;
                    Length = 1;
                    return true;
                }

                // mp3 data start
                if ((m_CurrentValue & 0xFFE0) == 0xFFE0)
                {
                    Match = MatchType.MP3Frame;
                    Length = 2;
                    return true;
                }

                // id3v2 start
                if ((m_CurrentValue & 0xFFFFFF) == 0x494433)
                {
                    Match = MatchType.ID3Frame;
                    Length = 3;
                    return true;
                }
                return false;
            }
            #endregion

            public override string ToString() => "Search[" + Match + ", " + Index + ", " + Length + "]";

            public override int GetHashCode() => Index;
        }
        #endregion

        DataFrameReader m_Reader;
        ID3v1 m_ID3v1 = null;

        AudioFrame m_BufferedFrame = null;
        MP3InvalidFrame m_InvalidFrame = null;

        /// <summary>Gets or sets the name of the source.</summary>
        /// <value>The name.</value>
        public string Name { get; set; }

        /// <summary>
        /// Creates a new MP3Reader for the specified file.
        /// </summary>
        /// <param name="fileName">The file to load.</param>
        public MP3Reader(string fileName)
            : this(ResistantFileStream.OpenSequentialRead(fileName))
        {
            Name = fileName;
        }

        /// <summary>
        /// Creates a new MP3Reader for the specified stream.
        /// </summary>
        /// <param name="stream">The stream to load.</param>
        public MP3Reader(Stream stream)
        {
            Name = stream.ToString();
            long endOfStream = 0;
            if (stream.CanSeek && (stream.Position == 0) && (stream.Length > 128))
            {
                // try loading ID3v1 first
                try
                {
                    stream.Seek(-128, SeekOrigin.End);
                    var buffer = new byte[128];
                    stream.Read(buffer, 0, 128);
                    if ((buffer[0] == (byte)'T') && (buffer[1] == (byte)'A') && (buffer[2] == (byte)'G'))
                    {
                        m_ID3v1 = new ID3v1(buffer);
                        endOfStream = stream.Length - 128;
                    }
                }
                catch { }
                stream.Seek(0, SeekOrigin.Begin);
            }
            m_Reader = new DataFrameReader(stream, endOfStream);
        }

        /// <summary>
        /// Add invalid data (since we need to check garbage for a mp3 resync we may need to combine multiple invalid chunks).
        /// </summary>
        /// <param name="buffer">The buffer to add to invalid data frame.</param>
        void InvalidData(byte[] buffer)
        {
            if (buffer.Length == 0)
            {
                return;
            }

            if (m_InvalidFrame == null)
            {
                m_InvalidFrame = new MP3InvalidFrame();
            }
            m_InvalidFrame.Add(buffer);
        }

        /// <summary>
        /// uses the <see cref="Search"/> class to find the next id3 / mp3 frame start at the buffer.
        /// </summary>
        /// <returns>Returns the search result.</returns>
        Search FindFrame()
        {
            // initialize the search and run it at the buffer
            var search = new Search();
            while (true)
            {
                // fill the buffer
                if (!m_Reader.EnsureBuffer(1024) && (m_Reader.Available < 4))
                {
                    if (m_Reader.Available == 0)
                    {
                        return null;
                    }

                    // end of stream
                    var buffer = m_Reader.GetBuffer(m_Reader.Available);
                    InvalidData(buffer);
                    return null;
                }

                // run the search
                {
                    if (m_Reader.Contains(search))
                    {
                        return search;
                    }

                    // nothing found, enqueue invalid data...
                    var buffer = m_Reader.GetBuffer(m_Reader.Available - 2);
                    InvalidData(buffer);

                    // .. and start new search
                    search = new Search();
                }
            }
        }

        /// <summary>
        /// Gets the next frame.
        /// </summary>
        /// <returns>Returns the next frame or null (at end of stream).</returns>
        public AudioFrame GetNextFrame()
        {
            while (true)
            {
                #region return buffered frames first (if any)
                // got a decoded frame at the cache ?
                if (m_BufferedFrame != null)
                {
                    AudioFrame result;
                    if (m_InvalidFrame != null)
                    {
                        result = m_InvalidFrame;
                        m_InvalidFrame = null;
                    }
                    else
                    {
                        result = m_BufferedFrame;
                        m_BufferedFrame = null;
                    }
                    return result;
                }
                #endregion

                #region search next frame start

                // search the next interesting position
                var searchResult = FindFrame();
                if (searchResult == null)
                {
                    #region end of stream cleanup
                    // invalid data at end of stream ?
                    if (m_Reader.Available > 0)
                    {
                        // yes buffer
                        InvalidData(m_Reader.GetBuffer());
                    }

                    // got an invalid frame ?
                    if (m_InvalidFrame != null)
                    {
                        // return invalid frame
                        AudioFrame result = m_InvalidFrame;
                        m_InvalidFrame = null;
                        return result;
                    }

                    // got an id3v1 ?
                    if (m_ID3v1 != null)
                    {
                        // return id3v1
                        AudioFrame result = m_ID3v1;
                        m_ID3v1 = null;
                        return result;
                    }

                    // everything done, return null
                    return null;
                    #endregion
                }
                #endregion

                #region check search result

                // got garbage at the beginning?
                if (searchResult.Index > 0)
                {
                    // yes, invalid data
                    InvalidData(m_Reader.GetBuffer(searchResult.Index));
                    continue;
                }
                #endregion

                #region decode frame

                // try to decode frame
                try
                {
                    var valid = false;
                    AudioFrame frame = null;
                    switch (searchResult.Match)
                    {
                        #region decode mp3 frame
                        case MatchType.MP3Frame:
                            var audioFrame = new MP3AudioFrame();
                            valid = audioFrame.Parse(m_Reader);
                            frame = audioFrame;
                            break;
                        #endregion

                        #region decode id3 frame
                        case MatchType.ID3Frame:
                            frame = new ID3v2();
                            valid = frame.Parse(m_Reader);
                            break;
                        #endregion

                        default: throw new NotImplementedException(string.Format("Unknown frame type {0}", searchResult.Match));
                    }

                    // parsed successfully?
                    if (valid)
                    {
                        // yes, cache frame
                        m_BufferedFrame = frame;
                    }
                    else
                    {
                        // no invalidate
                        InvalidData(m_Reader.GetBuffer(1));
                    }
                }
                catch (Exception ex)
                {
                    Trace.TraceError(string.Format("Error while decoding {0} in stream {1}", searchResult.Match, m_Reader));
                    Trace.TraceError(ex.ToString());

                    // invalid frame or decoder error, move ahead
                    var count = (searchResult.Index < 0) ? 1 : searchResult.Index + 1;
                    InvalidData(m_Reader.GetBuffer(count));
                }
                #endregion
            }
        }

        /// <summary>
        /// Closes the reader and the underlying stream.
        /// </summary>
        public void Close() => m_Reader.Close();
    }
}
