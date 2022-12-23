using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using Cave.IO;
using Cave.Media.Audio.ID3.Frames;
using Cave.Media.Audio.MP3;
using Cave.Media.Lyrics;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides an ID3v2 tag implementation.
    /// </summary>
    public sealed class ID3v2 : MP3MetaFrame
    {
        ID3v2ExtendedHeader extendedHeader = null;
        List<ID3v2Frame> frames = new List<ID3v2Frame>(100);
        ID3v2Footer footer = null;
        byte[] data;

        /// <summary>
        /// Gets the text of the first T* (not TXXX) frame with the specified ID.
        /// </summary>
        /// <param name="frameID">ID of the frame.</param>
        /// <returns>Returns the text of the frame or an empty string.</returns>
        string GetTextFrameText(string frameID)
        {
            foreach (var frame in frames)
            {
                if (frame.ID != frameID)
                {
                    continue;
                }

                if (frame is ID3v2TextFrame textFrame)
                {
                    return textFrame.Text;
                }
            }
            return string.Empty;
        }

        #region parser functions
        bool ParseFramesV2(ID3v2Reader reader)
        {
            while (reader.State == ID3v2ReaderState.ReadFrames)
            {
                if (!reader.ReadFrame(out var frame))
                {
                    return false;
                }

                if (frame == null)
                {
                    continue;
                }

                switch (frame.ID)
                {
                    case "COM": /* TODO */ break;
                    case "TT2": /* TODO */ break;
                    case "TP1": /* TODO */ break;
                    case "TRK": /* TODO */ break;
                    case "TEN": /* TODO */ break;
                    case "TCO": /* TODO */ break;
                    default: // TODO IMPLEMENT ME
                        break;
                }
                frames.Add(frame);
            }
            return true;
        }

        bool ParseFrames(ID3v2Reader reader)
        {
            while (reader.State == ID3v2ReaderState.ReadFrames)
            {
                if (!reader.ReadFrame(out var frame))
                {
                    if (frames.Count > 0)
                    {
                        if (reader.Available > 0)
                        {
                            Trace.WriteLine(string.Format("Invalid frame in id3v2 tag, ignoring remaining data of {0} bytes.", reader.Available));
                            ParserError = true;
                        }
                        return true;
                    }
                    return false;
                }
                if (frame == null)
                {
                    continue;
                }

                switch (frame.ID)
                {
                    case "APIC": frame = new ID3v2APICFrame(frame); break;
                    case "COMM": frame = new ID3v2COMMFrame(frame); break;
                    case "ETCO": frame = new ID3v2ETCOFrame(frame); break;
                    case "GEOB": /*TODO*/ break;
                    case "IPLS": frame = new ID3v2IPLSFrame(frame); break;
                    case "MCDI": frame = new ID3v2MCDIFrame(frame); break;
                    case "MLLT": frame = new ID3v2MLLTFrame(frame); break;
                    case "NCON": /*TODO*/ break;
                    case "PCNT": frame = new ID3v2PCNTFrame(frame); break;
                    case "PRIV": frame = new ID3v2PRIVFrame(frame); break;
                    case "SYLT": frame = new ID3v2SYLTFrame(frame); break;
                    case "SYTC": frame = new ID3v2SYTCFrame(frame); break;
                    case "TXXX": frame = new ID3v2TXXXFrame(frame); break;
                    case "UFID": frame = new ID3v2UFIDFrame(frame); break;
                    case "USLT": frame = new ID3v2USLTFrame(frame); break;
                    case "WXXX": frame = new ID3v2WXXXFrame(frame); break;
                    case "XSLT": frame = new ID3v2XSLTFrame(frame); break;
                    default: // TODO IMPLEMENT ME
                        switch (frame.ID[0])
                        {
                            case 'W': frame = new ID3v2WebFrame(frame); break;
                            case 'T': frame = new ID3v2TextFrame(frame); break;
                            default: Trace.WriteLine(string.Format("Unknown ID3v2 frame type {0}", frame.ID)); break;
                        }
                        break;
                }
                frames.Add(frame);
            }
            return true;
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame
        /// This function will throw exceptions on parser errors.
        /// </summary>
        /// <param name="frameReader">FrameReader to read from.</param>
        public override bool Parse(DataFrameReader frameReader)
        {
            var reader = new ID3v2Reader(frameReader);

            // read header
            Header = reader.ReadHeader(out data);
            if (Header == null)
            {
                return false;
            }

            // read extended header (may be null)
            if (!reader.ReadExtendedHeader(out extendedHeader))
            {
                return false;
            }

            // read frames
            switch (Header.Version)
            {
                case 2: if (!ParseFramesV2(reader)) { return false; } break;
                case 3:
                case 4: if (!ParseFrames(reader)) { return false; } break;
                default: return false;
            }

            // read footer
            if (reader.State == ID3v2ReaderState.ReadFooter)
            {
                if (!reader.ReadFooter(out footer))
                {
                    return false;
                }
            }
            return true;
        }
        #endregion

        #region frame getters

        /// <summary>Tries to get a matching frame.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <param name="frame">The frame.</param>
        /// <returns></returns>
        public bool TryGetFrame<T>(string id, out T frame)
            where T : ID3v2Frame
        {
            foreach (var f in frames)
            {
                if (id != null && !string.Equals(f.ID, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (f is T)
                {
                    frame = (T)f;
                    return true;
                }
            }
            frame = default;
            return false;
        }

        /// <summary>Tries to get the TXXX frame with the given name.</summary>
        /// <param name="name">The name.</param>
        /// <param name="frame">The frame.</param>
        /// <returns></returns>
        public bool TryGetTXXXFrame(string name, out ID3v2TXXXFrame frame)
        {
            foreach (var f in frames)
            {
                frame = f as ID3v2TXXXFrame;
                if (frame == null)
                {
                    continue;
                }

                if (!string.Equals(name, frame.Name, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                return true;
            }
            frame = default;
            return false;
        }

        /// <summary>Gets frames matching the identifier.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T[] GetFrames<T>(string id = null)
            where T : ID3v2Frame
        {
            var result = new List<T>();
            foreach (var f in frames)
            {
                if (id != null && !string.Equals(f.ID, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (f is T)
                {
                    result.Add((T)f);
                }
            }
            return result.ToArray();
        }

        /// <summary>Gets the picture frame with the specified type.</summary>
        /// <param name="type">The type.</param>
        /// <returns></returns>
        public ID3v2APICFrame GetPictureFrame(ID3v2PictureType type)
        {
            foreach (var frame in GetFrames<ID3v2APICFrame>("APIC"))
            {
                if (frame.PictureType == type)
                {
                    return frame;
                }
            }
            return null;
        }
        #endregion

        #region public properties

        /// <summary>Gets a value indicating whether a parser error occured.</summary>
        /// <value><c>true</c> if a parser error within the tag; otherwise, <c>false</c>.</value>
        public bool ParserError { get; private set; }

        /// <summary>
        /// Gets the mood.
        /// </summary>
        /// <value>
        /// The mood.
        /// </value>
        public string[] Moods => TryGetTXXXFrame("albummood", out var mood)
                    ? mood.Value.Split(new char[] { ';', ',', '/', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    : (new string[0]);

        /// <summary>
        /// AcoustID.org Id.
        /// </summary>
        public BinaryGuid AcousticGuid
        {
            get
            {
                if (TryGetTXXXFrame("Acoustid Id", out var acoustid))
                {
                    if (BinaryGuid.TryParse(acoustid.Value, out var g))
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        /// <summary>Gets the music brainz artist identifier.</summary>
        /// <value>The music brainz artist identifier.</value>
        public BinaryGuid MusicBrainzArtistId
        {
            get
            {
                if (TryGetTXXXFrame("MusicBrainz Artist Id", out var frame))
                {
                    var val = frame.Value.Split(';', ',', '/', ' ')[0];
                    if (BinaryGuid.TryParse(val, out var g))
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        /// <summary>Gets the music brainz artist identifier.</summary>
        /// <value>The music brainz artist identifier.</value>
        public BinaryGuid MusicBrainzAlbumArtistId
        {
            get
            {
                if (TryGetTXXXFrame("MusicBrainz Album Artist Id", out var frame))
                {
                    var val = frame.Value.Split(';', ',', '/', ' ')[0];
                    if (BinaryGuid.TryParse(val, out var g))
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        /// <summary>Gets the music brainz album identifier.</summary>
        /// <value>The music brainz album identifier.</value>
        public BinaryGuid MusicBrainzAlbumId
        {
            get
            {
                if (TryGetTXXXFrame("MusicBrainz Album Id", out var frame))
                {
                    if (BinaryGuid.TryParse(frame.Value, out var g))
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        /// <summary>Gets the music brainz release group identifier.</summary>
        /// <value>The music brainz release group identifier.</value>
        public BinaryGuid MusicBrainzReleaseGroupId
        {
            get
            {
                if (TryGetTXXXFrame("MusicBrainz Release Group Id", out var frame))
                {
                    if (BinaryGuid.TryParse(frame.Value, out var g))
                    {
                        return g;
                    }
                }
                return null;
            }
        }

        /// <summary>Gets or sets the cover front bitmap.</summary>
        /// <value>The cover front bitmap.</value>
        public ID3v2APICFrame CoverFront => GetPictureFrame(ID3v2PictureType.CoverFront);

        /// <summary>
        /// Gets the ID3v2.x version with 2 &lt;= x &lt;= 4.
        /// </summary>
        public byte Version => Header.Version;

        /// <summary>
        /// Gets whether a date frame has been set or not.
        /// </summary>
        public bool HasDate
        {
            get
            {
                var year = 0;
                {
                    var yyyy = GetTextFrameText("TYER");
                    if (yyyy != null)
                    {
                        int.TryParse(yyyy, out year);
                    }
                }
                return year != 0;
            }
        }

        /// <summary>
        /// Gets the date (if present, check <see cref="HasDate"/>).
        /// </summary>
        public DateTime Date
        {
            get
            {
                var year = 0;
                {
                    var yyyy = GetTextFrameText("TYER");
                    if ((yyyy != null) && (yyyy.Length == 4))
                    {
                        int.TryParse(yyyy, out year);
                    }
                }
                if (year == 0)
                {
                    return default;
                }

                var hour = 0;
                var min = 0;
                {
                    var hHmm = GetTextFrameText("TIME");
                    if ((hHmm != null) && (hHmm.Length == 4))
                    {
                        int.TryParse(hHmm.Substring(0, 2), out hour);
                        int.TryParse(hHmm.Substring(2), out min);
                    }
                }

                var day = 1;
                var month = 1;
                {
                    var ddMM = GetTextFrameText("TDAT");
                    if ((ddMM != null) && (ddMM.Length == 4))
                    {
                        int.TryParse(ddMM.Substring(0, 2), out day);
                        int.TryParse(ddMM.Substring(2), out month);
                    }
                    if (day < 1)
                    {
                        day = 1;
                    }

                    if (month < 1)
                    {
                        month = 1;
                    }
                }

                return new DateTime(year, month, day, hour, min, 0);
            }
        }

        /// <summary>
        /// The 'Content group description' frame is used if the sound belongs to a larger category of sounds/music. For example, classical music is often sorted in different musical sections (e.g. "Piano Concerto", "Weather - Hurricane").
        /// or the series name at audio books.
        /// </summary>
        public string Group => GetTextFrameText("TIT1");

        /// <summary>
        /// The 'Title/Songname/Content description' frame is the actual name of the piece (e.g. "Adagio", "Hurricane Donna").
        /// </summary>
        public string Title => GetTextFrameText("TIT2");

        /// <summary>
        /// The 'Subtitle/Description refinement' frame is used for information directly related to the contents title (e.g. "Op. 16" or "Performed live at Wembley").
        /// </summary>
        public string SubTitle => GetTextFrameText("TIT3");

        /// <summary>
        /// The 'Album/Movie/Show title' frame is intended for the title of the recording(/source of sound) which the audio in the file is taken from.
        /// </summary>
        public string Album => GetTextFrameText("TALB");

        /// <summary>
        /// The 'Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group' is used for the main artist(s). They are seperated with the "/" character.
        /// </summary>
        public string SongArtist => GetTextFrameText("TPE1");

        /// <summary>
        /// The 'Band/orchestra/accompaniment' is used for the album/recording artist(s). They are seperated with the "/" character.
        /// </summary>
        public string AlbumArtist => GetTextFrameText("TPE2");

        /// <summary>
        /// The 'Interpreted, remixed, or otherwise modified by' frame contains more information about the people behind a remix and similar interpretations of another existing piece.
        /// </summary>
        public string Performer => GetTextFrameText("TPE4");

        /// <summary>
        /// Gets the content types (genres).
        /// </summary>
        public string[] ContentTypes
        {
            get
            {
                var str = GetTextFrameText("TCON");
                if (str == null)
                {
                    return new string[0];
                }

                var parts = str.Split('(', '/');
                var result = new List<string>();
                for (var i = 0; i < parts.Length; i++)
                {
                    var n = i + 1;
                    if ((n < parts.Length) && (parts[i] == string.Empty))
                    {
                        parts[n] = "(" + parts[n];
                        continue;
                    }

                    if (parts[i].StartsWith("(") && parts[i].EndsWith(")"))
                    {
                        switch (parts[i])
                        {
                            case "RX": result.Add("Remix"); continue;
                            case "CR": result.Add("Cover"); continue;
                        }
                        if (uint.TryParse(parts[i].Substring(1, parts[i].Length - 2), out var genreCode))
                        {
                            if (genreCode < ID3v1.Genres.Length)
                            {
                                result.Add(ID3v1.Genres[genreCode]);
                                continue;
                            }
                        }
                    }
                    result.AddRange(parts[i].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
                return result.ToArray();
            }
        }

        /// <summary>
        /// The 'Track number/Position in set' frame is a numeric string containing the order number of the audio-file on its original recording.
        /// This may be extended with a "/" character and a numeric string containing the total numer of tracks/elements on the original recording. E.g. "4/9".
        /// </summary>
        public string Track => GetTextFrameText("TRCK");

        /// <summary>Gets or sets the track number.</summary>
        /// <value>The track number.</value>
        public int TrackNumber
        {
            get
            {
                var result = 0;
                var track = Track;
                if (track != null)
                {
                    var index = track.IndexOf('/');
                    if (index > 0)
                    {
                        int.TryParse(track.Substring(0, index), out result);
                    }
                    else
                    {
                        int.TryParse(track, out result);
                    }
                }
                return result;
            }
        }

        /// <summary>Gets or sets the track count.</summary>
        /// <value>The track count.</value>
        public int TrackCount
        {
            get
            {
                var result = 0;
                var track = Track;
                if (track != null)
                {
                    var index = track.IndexOf('/');
                    if (index > 0)
                    {
                        int.TryParse(track.Substring(index + 1), out result);
                    }
                }
                return result;
            }
        }

        /// <summary>Gets the header.</summary>
        /// <value>The header.</value>
        public ID3v2Header Header { get; private set; }

        /// <summary>
        /// Gets all frames currently present at the tag.
        /// </summary>
        public ID3v2Frame[] Frames => frames.ToArray();

        /// <summary>Gets or sets the lyrics.</summary>
        /// <value>The lyrics.</value>
        public SynchronizedLyrics Lyrics => TryGetFrame("XSLT", out ID3v2XSLTFrame frame) ? SynchronizedLyrics.FromData(frame.Content) : null;

        /// <summary>
        /// Gets an array with the data for this instance.
        /// </summary>
        /// <returns></returns>
        public override byte[] Data => data;

        /// <summary>
        /// Length of the frame in bytes.
        /// </summary>
        public override int Length => Header.BodySize + Header.Length;

        /// <summary>
        /// Returns false.
        /// </summary>
        public override bool IsFixedLength => false;
        #endregion

        /// <summary>
        /// Returns the full tag as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(base.ToString());
            foreach (var frame in Frames)
            {
                result.AppendLine();
                result.Append(frame.ToString());
            }
            return result.ToString();
        }
    }
}
