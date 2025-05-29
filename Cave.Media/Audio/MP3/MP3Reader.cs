using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cave.IO;
using Cave.Media.Audio.ID3;

#nullable enable

namespace Cave.Media.Audio.MP3;

/// <summary>Provides a reader for mp3 audio files.</summary>
public sealed class MP3Reader : IFrameSource
{
    #region Private Classes

    /// <summary>Search class to detect valid headers at the mp3 file.</summary>
    class Search : IDataFrameSearch
    {
        #region Private Fields

        int m_CurrentValue;
        int m_Index;

        #endregion Private Fields

        #region Public Properties

        public int Index => m_Index - Length;
        public int Length { get; private set; }
        public MatchType Match { get; private set; }

        #endregion Public Properties

        #region Public Methods

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

        public override int GetHashCode() => Index;

        public override string ToString() => "Search[" + Match + ", " + Index + ", " + Length + "]";

        #endregion Public Methods
    }

    #endregion Private Classes

    #region Private Fields

    readonly DataFrameReader reader;
    AudioFrame? bufferedFrame;
    ID3v1? id3v1;
    MP3InvalidFrame? invalidFrame;

    #endregion Private Fields

    #region Private Enums

    /// <summary>used to communicate the found header.</summary>
    enum MatchType
    {
        Invalid = 0,
        MP3Frame,
        ID3Frame,
    }

    #endregion Private Enums

    #region Private Methods

    /// <summary>uses the <see cref="Search"/> class to find the next id3 / mp3 frame start at the buffer.</summary>
    /// <returns>Returns the search result.</returns>
    Search? FindFrame()
    {
        // initialize the search and run it at the buffer
        var search = new Search();
        while (true)
        {
            // fill the buffer
            if (!reader.EnsureBuffer(1024) && (reader.Available < 4))
            {
                if (reader.Available == 0)
                {
                    return null;
                }

                // end of stream
                var buffer = reader.GetBuffer(reader.Available);
                InvalidData(buffer);
                return null;
            }

            // run the search
            {
                if (reader.Contains(search))
                {
                    return search;
                }

                // nothing found, enqueue invalid data...
                var buffer = reader.GetBuffer(reader.Available - 2);
                InvalidData(buffer);

                // .. and start new search
                search = new Search();
            }
        }
    }

    /// <summary>Add invalid data (since we need to check garbage for a mp3 resync we may need to combine multiple invalid chunks).</summary>
    /// <param name="buffer">The buffer to add to invalid data frame.</param>
    void InvalidData(byte[] buffer)
    {
        if (buffer.Length == 0)
        {
            return;
        }

        if (invalidFrame == null)
        {
            invalidFrame = new MP3InvalidFrame();
        }
        invalidFrame.Add(buffer);
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Creates a new MP3Reader for the specified file.</summary>
    /// <param name="fileName">The file to load.</param>
    public MP3Reader(string fileName)
        : this(ResistantFileStream.OpenSequentialRead(fileName))
    {
        Name = fileName;
    }

    /// <summary>Creates a new MP3Reader for the specified stream.</summary>
    /// <param name="stream">The stream to load.</param>
    public MP3Reader(Stream stream)
    {
        if (stream is null) throw new ArgumentNullException(nameof(stream));
        Name = stream.ToString() ?? string.Empty;
        long endOfStream = 0;
        if (stream.CanSeek && (stream.Position == 0) && (stream.Length > 128))
        {
            // try loading ID3v1 first
            try
            {
                stream.Seek(-128, SeekOrigin.End);
                var buffer = new byte[128];
                var len = stream.Read(buffer, 0, 128);
                if ((len == 128) && (buffer[0] == (byte)'T') && (buffer[1] == (byte)'A') && (buffer[2] == (byte)'G'))
                {
                    id3v1 = new ID3v1(buffer);
                    endOfStream = stream.Length - 128;
                }
            }
            catch { }
            stream.Seek(0, SeekOrigin.Begin);
        }
        reader = new DataFrameReader(stream, endOfStream);
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the name of the log source.</summary>
    /// <value>The name of the log source.</value>
    public string LogSourceName => "MP3Reader";

    /// <summary>Gets or sets the name of the source.</summary>
    /// <value>The name.</value>
    public string Name { get; set; }

    #endregion Public Properties

    #region Public Methods

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

    /// <summary>Closes the reader and the underlying stream.</summary>
    public void Close() => reader.Close();

    /// <summary>Gets the next frame.</summary>
    /// <returns>Returns the next frame or null (at end of stream).</returns>
    public AudioFrame? GetNextFrame()
    {
        while (true)
        {
            #region return buffered frames first (if any)

            // got a decoded frame at the cache ?
            if (bufferedFrame != null)
            {
                AudioFrame result;
                if (invalidFrame != null)
                {
                    result = invalidFrame;
                    invalidFrame = null;
                }
                else
                {
                    result = bufferedFrame;
                    bufferedFrame = null;
                }
                return result;
            }

            #endregion return buffered frames first (if any)

            #region search next frame start

            // search the next interesting position
            var searchResult = FindFrame();
            if (searchResult == null)
            {
                #region end of stream cleanup

                // invalid data at end of stream ?
                if (reader.Available > 0)
                {
                    // yes buffer
                    InvalidData(reader.GetBuffer());
                }

                // got an invalid frame ?
                if (invalidFrame != null)
                {
                    // return invalid frame
                    AudioFrame result = invalidFrame;
                    invalidFrame = null;
                    return result;
                }

                // got an id3v1 ?
                if (id3v1 != null)
                {
                    // return id3v1
                    AudioFrame result = id3v1;
                    id3v1 = null;
                    return result;
                }

                // everything done, return null
                return null;

                #endregion end of stream cleanup
            }

            #endregion search next frame start

            #region check search result

            // got garbage at the beginning?
            if (searchResult.Index > 0)
            {
                // yes, invalid data
                InvalidData(reader.GetBuffer(searchResult.Index));
                continue;
            }

            #endregion check search result

            #region decode frame

            // try to decode frame
            try
            {
                var valid = false;
                AudioFrame? frame = null;
                switch (searchResult.Match)
                {
                    #region decode mp3 frame

                    case MatchType.MP3Frame:
                        var audioFrame = new MP3AudioFrame();
                        valid = audioFrame.Parse(reader);
                        frame = audioFrame;
                        break;

                    #endregion decode mp3 frame

                    #region decode id3 frame

                    case MatchType.ID3Frame:
                        frame = new ID3v2();
                        valid = frame.Parse(reader);
                        break;

                    #endregion decode id3 frame

                    default: throw new NotImplementedException(string.Format("Unknown frame type {0}", searchResult.Match));
                }

                // parsed successfully?
                if (valid)
                {
                    // yes, cache frame
                    bufferedFrame = frame;
                }
                else
                {
                    // no invalidate
                    InvalidData(reader.GetBuffer(1));
                }
            }
            catch (Exception ex)
            {
                Trace.TraceError(string.Format("Error while decoding {0} in stream {1}", searchResult.Match, reader));
                Trace.TraceError(ex.ToString());

                // invalid frame or decoder error, move ahead
                var count = (searchResult.Index < 0) ? 1 : searchResult.Index + 1;
                InvalidData(reader.GetBuffer(count));
            }

            #endregion decode frame
        }
    }

    #endregion Public Methods
}
