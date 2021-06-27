using System;
using System.Collections.Generic;
using System.Text;
using Cave.Collections.Generic;
using Cave.IO;
using Cave.Media.Audio.ID3.Frames;
using Cave.Media.Lyrics;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides a builder for basic id3v2 tags.
    /// </summary>
    public class ID3v2Builder
    {
        ID3v2FrameFlags flags = new ID3v2FrameFlags() { Unsynchronisation = true };
        List<ID3v2Frame> frames = new List<ID3v2Frame>();

        /// <summary>Retrieves the tag as byte array.</summary>
        /// <returns>Returns a new byte array.</returns>
        public byte[] ToArray()
        {
            var buffer = new FifoBuffer();
            var flags = ID3v2HeaderFlags.None;
            /* we do not like extended headers so we won't write them
            if (m_ExtendedHeader != null)
            {
                buffer.Enqueue(m_ExtendedHeader.Data);
                flags |= ID3v2HeaderFlags.ExtendedHeader;
            }
            */
            foreach (var frame in frames)
            {
                buffer.Enqueue(frame.RawData, true);
            }
            var bodySize = buffer.Length;

            // no one likes footers so we won't write them
            var header = new ID3v2Header(Header.Version, Header.Revision, flags, bodySize);
            buffer.Prepend(header.Data, true);
            return buffer.ToArray();
        }

        /// <summary>Gets the header.</summary>
        /// <value>The header.</value>
        public ID3v2Header Header { get; private set; } = new ID3v2Header(3, 0, ID3v2HeaderFlags.None, 0);

        /// <summary>
        /// Gets the text of the first T* (not TXXX) frame with the specified ID.
        /// </summary>
        /// <param name="frameID">ID of the frame.</param>
        /// <returns></returns>
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
            return null;
        }

        /// <summary>Tries to the get frame with the specified properties.</summary>
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

        /// <summary>Gets the frames with the specified properties.</summary>
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

        /// <summary>Tries to the get frame with the specified properties.</summary>
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

        /// <summary>Adds a new frame.</summary>
        /// <param name="frame">The frame.</param>
        public void AddFrame(ID3v2Frame frame) => frames.Add(frame);

        /// <summary>Replaces the first frame with the same ID or adds a new one.</summary>
        /// <param name="frame">The frame.</param>
        public void ReplaceFrame(ID3v2Frame frame)
        {
            for (var i = 0; i < frames.Count; i++)
            {
                if (string.Equals(frames[i].ID, frame.ID, StringComparison.InvariantCultureIgnoreCase))
                {
                    frames[i] = frame;
                    return;
                }
            }
            AddFrame(frame);
        }

        /// <summary>Removes all frames with the given ID.</summary>
        /// <param name="id">The identifier.</param>
        public void RemoveFrames(string id)
        {
            foreach (var frame in frames.ToArray())
            {
                if (string.Equals(frame.ID, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    frames.Remove(frame);
                }
            }
        }

        /// <summary>Removes the TXXX frames with the given name.</summary>
        /// <param name="name">The name.</param>
        public void RemoveTXXXFrames(string name)
        {
            foreach (var frame in frames.ToArray())
            {
                if (frame is not ID3v2TXXXFrame txxxframe)
                {
                    continue;
                }

                if (string.Equals(txxxframe.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    frames.Remove(frame);
                }
            }
        }

        /// <summary>Replaces all text frame with the given ID with a single new one with the given value.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="ArgumentException">Invalid id!.</exception>
        public void ReplaceTextFrame(string id, string value)
        {
            if (!id.StartsWith("T") || id == "TXXX")
            {
                throw new ArgumentException("Invalid id!", nameof(id));
            }

            RemoveFrames(id);
            var frame = ID3v2TextFrame.Create(Header, flags, id, value);
            AddFrame(frame);
        }

        /// <summary>Removes the frame.</summary>
        /// <param name="frame">The frame.</param>
        public void RemoveFrame(ID3v2Frame frame) => frames.Remove(frame);

        /// <summary>Removes the frame at the given index.</summary>
        /// <param name="index">The index.</param>
        public void RemoveFrameAt(int index) => frames.RemoveAt(index);

        /// <summary>Gets or sets the <see cref="ID3v2Frame"/> at the specified index.</summary>
        /// <value>The <see cref="ID3v2Frame"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ID3v2Frame this[int index]
        {
            get => frames[index];
            set => frames[index] = value;
        }

        /// <summary>
        /// Gets the mood.
        /// </summary>
        /// <value>
        /// The mood.
        /// </value>
        public string[] Moods
        {
            get
            {
                return TryGetTXXXFrame("albummood", out var mood)
                    ? mood.Value.Split(new char[] { ';', ',', '/', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    : (new string[0]);
            }
            set
            {
                RemoveTXXXFrames("albummood");
                var items = new Set<string>();
                foreach (var item in value)
                {
                    items.Include(item.ToLower().Trim());
                }
                var mood = ID3v2TXXXFrame.Create(Header, flags, "albummood", string.Join(";", items.ToArray()));
                AddFrame(mood);
            }
        }

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
                        if (uint.TryParse(parts[i].Substring(1, parts[i].Length - 2), out var l_GenreCode))
                        {
                            if (l_GenreCode < ID3v1.Genres.Length)
                            {
                                result.Add(ID3v1.Genres[l_GenreCode]);
                                continue;
                            }
                        }
                    }
                    result.AddRange(parts[i].Split(new char[] { ',', ';' }, StringSplitOptions.RemoveEmptyEntries));
                }
                return result.ToArray();
            }
            set => ReplaceTextFrame("TCON", string.Join(";", value));
        }

        /// <summary>
        /// AcoustID.org Id.
        /// </summary>
        public BinaryGuid AcousticGuid
        {
            get
            {
                return TryGetTXXXFrame("Acoustid Id", out var acoustid) ? (BinaryGuid)new Guid(acoustid.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("Acoustid Id");
                var frame = ID3v2TXXXFrame.Create(Header, flags, "Acoustid Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz artist identifier.</summary>
        /// <value>The music brainz artist identifier.</value>
        public BinaryGuid MusicBrainzArtistId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Artist Id", out var frame) ? (BinaryGuid)new Guid(frame.Value.Split(';', ',', '/', ' ')[0]) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Artist Id");
                var frame = ID3v2TXXXFrame.Create(Header, flags, "MusicBrainz Artist Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz album identifier.</summary>
        /// <value>The music brainz album identifier.</value>
        public BinaryGuid MusicBrainzAlbumId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Album Id", out var frame) ? (BinaryGuid)new Guid(frame.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Album Id");
                var frame = ID3v2TXXXFrame.Create(Header, flags, "MusicBrainz Album Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz release group identifier.</summary>
        /// <value>The music brainz release group identifier.</value>
        public BinaryGuid MusicBrainzReleaseGroupId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Release Group Id", out var frame) ? (BinaryGuid)new Guid(frame.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Release Group Id");
                var frame = ID3v2TXXXFrame.Create(Header, flags, "MusicBrainz Release Group Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets or sets the cover front bitmap.</summary>
        /// <value>The cover front bitmap.</value>
        public ID3v2APICFrame CoverFront
        {
            get
            {
                foreach (var frame in GetFrames<ID3v2APICFrame>("APIC"))
                {
                    if (frame.PictureType == ID3v2PictureType.CoverFront)
                    {
                        return frame;
                    }
                }
                return null;
            }
        }

        /// <summary>Sets the cover front image.</summary>
        /// <param name="mimeType">Mime type of the image data.</param>
        /// <param name="imageData">The image data.</param>
        public void SetCoverFront(string mimeType, byte[] imageData)
        {
            foreach (var frame in GetFrames<ID3v2APICFrame>("APIC"))
            {
                if (frame.PictureType == ID3v2PictureType.CoverFront)
                {
                    frames.Remove(frame);
                }
            }
            {
                var frame = ID3v2APICFrame.Create(Header, flags, string.Empty, ID3v2PictureType.CoverFront, mimeType, imageData);
                AddFrame(frame);
            }
        }

        /// <summary>
        /// Gets the date.
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
            set
            {
                ReplaceTextFrame("TYER", value.ToString("yyyy"));
                ReplaceTextFrame("TIME", value.ToString("HHmm"));
                ReplaceTextFrame("TDAT", value.ToString("ddMM"));
            }
        }

        /// <summary>
        /// The 'Content group description' frame is used if the sound belongs to a larger category of sounds/music. For example, classical music is often sorted in different musical sections (e.g. "Piano Concerto", "Weather - Hurricane").
        /// or the series name at audio books.
        /// </summary>
        public string Group
        {
            get => GetTextFrameText("TIT1");
            set => ReplaceTextFrame("TIT1", value);
        }

        /// <summary>
        /// The 'Title/Songname/Content description' frame is the actual name of the piece (e.g. "Adagio", "Hurricane Donna").
        /// </summary>
        public string Title
        {
            get => GetTextFrameText("TIT2");
            set => ReplaceTextFrame("TIT2", value);
        }

        /// <summary>
        /// The 'Subtitle/Description refinement' frame is used for information directly related to the contents title (e.g. "Op. 16" or "Performed live at Wembley").
        /// </summary>
        public string SubTitle
        {
            get => GetTextFrameText("TIT3");
            set => ReplaceTextFrame("TIT3", value);
        }

        /// <summary>
        /// The 'Album/Movie/Show title' frame is intended for the title of the recording(/source of sound) which the audio in the file is taken from.
        /// </summary>
        public string Album
        {
            get => GetTextFrameText("TALB");
            set => ReplaceTextFrame("TALB", value);
        }

        /// <summary>
        /// The 'Lead artist(s)/Lead performer(s)/Soloist(s)/Performing group' is used for the main artist(s). They are seperated with the "/" character.
        /// </summary>
        public string SongArtist
        {
            get => GetTextFrameText("TPE1");
            set => ReplaceTextFrame("TPE1", value);
        }

        /// <summary>
        /// The 'Band/orchestra/accompaniment' is used for the album/recording artist(s). They are seperated with the "/" character.
        /// </summary>
        public string AlbumArtist
        {
            get => GetTextFrameText("TPE2");
            set => ReplaceTextFrame("TPE2", value);
        }

        /// <summary>
        /// The 'Interpreted, remixed, or otherwise modified by' frame contains more information about the people behind a remix and similar interpretations of another existing piece.
        /// </summary>
        public string Performer
        {
            get => GetTextFrameText("TPE4");
            set => ReplaceTextFrame("TPE4", value);
        }

        /// <summary>
        /// The 'Track number/Position in set' frame is a numeric string containing the order number of the audio-file on its original recording.
        /// This may be extended with a "/" character and a numeric string containing the total numer of tracks/elements on the original recording. E.g. "4/9".
        /// </summary>
        public string Track
        {
            get => GetTextFrameText("TRCK");
            set => ReplaceTextFrame("TRCK", value);
        }

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
            set
            {
                var track = Track;
                if (string.IsNullOrEmpty(track))
                {
                    track = value.ToString();
                }
                else
                {
                    var parts = track.Split('/');
                    track = value.ToString();
                    if (parts.Length > 1)
                    {
                        track += "/" + parts[1];
                    }
                }
                Track = track;
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
            set
            {
                var track = Track;
                if (string.IsNullOrEmpty(track))
                {
                    track = "0/" + value;
                }
                else
                {
                    var parts = track.Split('/');
                    track = parts[0] + "/" + value;
                }
                Track = track;
            }
        }

        /// <summary>Gets or sets the lyrics.</summary>
        /// <value>The lyrics.</value>
        public SynchronizedLyrics Lyrics
        {
            get
            {
                return TryGetFrame("XSLT", out ID3v2XSLTFrame frame) ? SynchronizedLyrics.FromData(frame.Content) : null;
            }
            set
            {
                var frame = ID3v2XSLTFrame.Create(Header, new ID3v2FrameFlags(), value.ToArray());
                ReplaceFrame(frame);
            }
        }

        /// <summary>Loads the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <exception cref="NotSupportedException"></exception>
        public void Load(ID3v2 tag)
        {
            frames.Clear();
            switch (tag.Version)
            {
                case 3:
                case 4: break;
                default: throw new NotSupportedException(string.Format("Editing ID3v2.{0} is not supported!", tag.Version));
            }
            Header = tag.Header;
            frames.Clear();
            foreach (var frame in tag.Frames)
            {
                frames.Add(frame);
            }
        }

        /// <summary>Updates the builder with the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        public void UpdateWith(ID3v1 tag)
        {
            if (!string.IsNullOrEmpty(tag.Title) && string.IsNullOrEmpty(Title))
            {
                Title = tag.Title;
            }

            if (!string.IsNullOrEmpty(tag.Album) && string.IsNullOrEmpty(Album))
            {
                Album = tag.Album;
            }

            if (!string.IsNullOrEmpty(tag.Genre) && string.IsNullOrEmpty(Group))
            {
                Group = tag.Genre;
            }

            if (!string.IsNullOrEmpty(tag.Artist) && string.IsNullOrEmpty(SongArtist))
            {
                SongArtist = tag.Artist;
            }

            if (TrackNumber == 0)
            {
                TrackNumber = tag.TrackNumber;
            }
        }

        /// <summary>Updates the builder with the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        public void UpdateWith(ID3v2 tag)
        {
            if ((tag.AcousticGuid != null) && (AcousticGuid == null))
            {
                AcousticGuid = tag.AcousticGuid;
            }

            if ((tag.CoverFront != null) && (CoverFront == null))
            {
                SetCoverFront(tag.CoverFront.MimeType, tag.CoverFront.ImageData);
            }

            if (!string.IsNullOrEmpty(tag.Album) && string.IsNullOrEmpty(Album))
            {
                Album = tag.Album;
            }

            if (!string.IsNullOrEmpty(tag.SongArtist) && string.IsNullOrEmpty(SongArtist))
            {
                SongArtist = tag.SongArtist;
            }

            if (!string.IsNullOrEmpty(tag.AlbumArtist) && string.IsNullOrEmpty(AlbumArtist))
            {
                AlbumArtist = tag.AlbumArtist;
            }

            if (!string.IsNullOrEmpty(tag.Group) && string.IsNullOrEmpty(Group))
            {
                Group = tag.Group;
            }

            // if ((tag.ContentTypes.Length > 0) && (ContentTypes.Length == 0)) ContentTypes = tag.ContentTypes;
            if ((tag.Moods.Length > 0) && (Moods.Length == 0))
            {
                Moods = tag.Moods;
            }

            if ((tag.Date != default) && (Date == default))
            {
                Date = tag.Date;
            }

            if ((tag.MusicBrainzAlbumId != null) && (MusicBrainzAlbumId == null))
            {
                MusicBrainzAlbumId = tag.MusicBrainzAlbumId;
            }

            if ((tag.MusicBrainzArtistId != null) && (MusicBrainzArtistId == null))
            {
                MusicBrainzArtistId = tag.MusicBrainzArtistId;
            }

            if ((tag.MusicBrainzReleaseGroupId != null) && (MusicBrainzReleaseGroupId == null))
            {
                MusicBrainzArtistId = tag.MusicBrainzReleaseGroupId;
            }

            if (!string.IsNullOrEmpty(tag.SubTitle) && string.IsNullOrEmpty(SubTitle))
            {
                SubTitle = tag.SubTitle;
            }

            if (!string.IsNullOrEmpty(tag.Title) && string.IsNullOrEmpty(Title))
            {
                Title = tag.Title;
            }

            if (!string.IsNullOrEmpty(tag.Track) && string.IsNullOrEmpty(Track))
            {
                Track = tag.Track;
            }
        }

        /// <summary>
        /// Returns the full tag as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append(base.ToString());
            foreach (var frame in frames)
            {
                result.AppendLine();
                result.Append(frame.ToString());
            }
            return result.ToString();
        }
    }
}
