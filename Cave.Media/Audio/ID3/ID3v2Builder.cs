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
        ID3v2FrameFlags m_Flags = new ID3v2FrameFlags() { Unsynchronisation = true };
        ID3v2Header m_Header = new ID3v2Header(3, 0, ID3v2HeaderFlags.None, 0);
        List<ID3v2Frame> m_Frames = new List<ID3v2Frame>();

        /// <summary>Retrieves the tag as byte array.</summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            FifoBuffer buffer = new FifoBuffer();
            ID3v2HeaderFlags flags = ID3v2HeaderFlags.None;
            /* we do not like extended headers so we won't write them
            if (m_ExtendedHeader != null)
            {
                buffer.Enqueue(m_ExtendedHeader.Data);
                flags |= ID3v2HeaderFlags.ExtendedHeader;
            }
            */
            foreach (ID3v2Frame frame in m_Frames)
            {
                buffer.Enqueue(frame.RawData);
            }
            int bodySize = buffer.Length;

            // no one likes footers so we won't write them
            ID3v2Header header = new ID3v2Header(Header.Version, Header.Revision, flags, bodySize);
            buffer.Prepend(header.Data);
            return buffer.ToArray();
        }

        /// <summary>Gets the header.</summary>
        /// <value>The header.</value>
        public ID3v2Header Header => m_Header;

        /// <summary>
        /// Obtains the text of the first T* (not TXXX) frame with the specified ID.
        /// </summary>
        /// <param name="frameID">ID of the frame.</param>
        /// <returns></returns>
        string GetTextFrameText(string frameID)
        {
            foreach (ID3v2Frame l_Frame in m_Frames)
            {
                if (l_Frame.ID != frameID)
                {
                    continue;
                }

                ID3v2TextFrame textFrame = l_Frame as ID3v2TextFrame;
                if (textFrame != null)
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
        public bool TryGetFrame<T>(string id, out T frame) where T : ID3v2Frame
        {
            foreach (ID3v2Frame f in m_Frames)
            {
                if (id != null && !string.Equals(f.ID, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    continue;
                }

                if (f is T) { frame = (T)f;
                    return true; }
            }
            frame = default(T);
            return false;
        }

        /// <summary>Gets the frames with the specified properties.</summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="id">The identifier.</param>
        /// <returns></returns>
        public T[] GetFrames<T>(string id = null) where T : ID3v2Frame
        {
            List<T> result = new List<T>();
            foreach (ID3v2Frame f in m_Frames)
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
            foreach (ID3v2Frame f in m_Frames)
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
            frame = default(ID3v2TXXXFrame);
            return false;
        }

        /// <summary>Adds a new frame.</summary>
        /// <param name="frame">The frame.</param>
        public void AddFrame(ID3v2Frame frame)
        {
            m_Frames.Add(frame);
        }

        /// <summary>Replaces the first frame with the same ID or adds a new one.</summary>
        /// <param name="frame">The frame.</param>
        public void ReplaceFrame(ID3v2Frame frame)
        {
            for (int i = 0; i < m_Frames.Count; i++)
            {
                if (string.Equals(m_Frames[i].ID, frame.ID, StringComparison.InvariantCultureIgnoreCase))
                {
                    m_Frames[i] = frame;
                    return;
                }
            }
            AddFrame(frame);
        }

        /// <summary>Removes all frames with the given ID.</summary>
        /// <param name="id">The identifier.</param>
        public void RemoveFrames(string id)
        {
            foreach (ID3v2Frame frame in m_Frames.ToArray())
            {
                if (string.Equals(frame.ID, id, StringComparison.InvariantCultureIgnoreCase))
                {
                    m_Frames.Remove(frame);
                }
            }
        }

        /// <summary>Removes the TXXX frames with the given name.</summary>
        /// <param name="name">The name.</param>
        public void RemoveTXXXFrames(string name)
        {
            foreach (ID3v2Frame frame in m_Frames.ToArray())
            {
                ID3v2TXXXFrame txxxframe = frame as ID3v2TXXXFrame;
                if (txxxframe == null)
                {
                    continue;
                }

                if (string.Equals(txxxframe.Name, name, StringComparison.InvariantCultureIgnoreCase))
                {
                    m_Frames.Remove(frame);
                }
            }
        }

        /// <summary>Replaces all text frame with the given ID with a single new one with the given value.</summary>
        /// <param name="id">The identifier.</param>
        /// <param name="value">The value.</param>
        /// <exception cref="System.ArgumentException">Invalid id!.</exception>
        public void ReplaceTextFrame(string id, string value)
        {
            if (!id.StartsWith("T") || id == "TXXX")
            {
                throw new ArgumentException("Invalid id!", nameof(id));
            }

            RemoveFrames(id);
            ID3v2TextFrame frame = ID3v2TextFrame.Create(m_Header, m_Flags, id, value);
            AddFrame(frame);
        }

        /// <summary>Removes the frame.</summary>
        /// <param name="frame">The frame.</param>
        public void RemoveFrame(ID3v2Frame frame)
        {
            m_Frames.Remove(frame);
        }

        /// <summary>Removes the frame at the given index.</summary>
        /// <param name="index">The index.</param>
        public void RemoveFrameAt(int index)
        {
            m_Frames.RemoveAt(index);
        }

        /// <summary>Gets or sets the <see cref="ID3v2Frame"/> at the specified index.</summary>
        /// <value>The <see cref="ID3v2Frame"/>.</value>
        /// <param name="index">The index.</param>
        /// <returns></returns>
        public ID3v2Frame this[int index]
        {
            get => m_Frames[index];
            set => m_Frames[index] = value;
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
                return TryGetTXXXFrame("albummood", out ID3v2TXXXFrame mood)
                    ? mood.Value.Split(new char[] { ';', ',', '/', '\n', '\r' }, StringSplitOptions.RemoveEmptyEntries)
                    : (new string[0]);
            }
            set
            {
                RemoveTXXXFrames("albummood");
                Set<string> items = new Set<string>();
                foreach (string item in value)
                {
                    items.Include(item.ToLower().Trim());
                }
                ID3v2TXXXFrame mood = ID3v2TXXXFrame.Create(m_Header, m_Flags, "albummood", string.Join(";", items.ToArray()));
                AddFrame(mood);
            }
        }

        /// <summary>
        /// Obtains the content types (genres).
        /// </summary>
        public string[] ContentTypes
        {
            get
            {
                string str = GetTextFrameText("TCON");
                if (str == null)
                {
                    return new string[0];
                }

                string[] parts = str.Split('(', '/');
                List<string> result = new List<string>();
                for (int i = 0; i < parts.Length; i++)
                {
                    int n = i + 1;
                    if ((n < parts.Length) && (parts[i] == ""))
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
                        if (uint.TryParse(parts[i].Substring(1, parts[i].Length - 2), out uint l_GenreCode))
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
                return TryGetTXXXFrame("Acoustid Id", out ID3v2TXXXFrame acoustid) ? (BinaryGuid)new Guid(acoustid.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("Acoustid Id");
                ID3v2TXXXFrame frame = ID3v2TXXXFrame.Create(m_Header, m_Flags, "Acoustid Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz artist identifier.</summary>
        /// <value>The music brainz artist identifier.</value>
        public BinaryGuid MusicBrainzArtistId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Artist Id", out ID3v2TXXXFrame frame) ? (BinaryGuid)new Guid(frame.Value.Split(';', ',', '/', ' ')[0]) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Artist Id");
                ID3v2TXXXFrame frame = ID3v2TXXXFrame.Create(m_Header, m_Flags, "MusicBrainz Artist Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz album identifier.</summary>
        /// <value>The music brainz album identifier.</value>
        public BinaryGuid MusicBrainzAlbumId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Album Id", out ID3v2TXXXFrame frame) ? (BinaryGuid)new Guid(frame.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Album Id");
                ID3v2TXXXFrame frame = ID3v2TXXXFrame.Create(m_Header, m_Flags, "MusicBrainz Album Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets the music brainz release group identifier.</summary>
        /// <value>The music brainz release group identifier.</value>
        public BinaryGuid MusicBrainzReleaseGroupId
        {
            get
            {
                return TryGetTXXXFrame("MusicBrainz Release Group Id", out ID3v2TXXXFrame frame) ? (BinaryGuid)new Guid(frame.Value) : null;
            }
            set
            {
                RemoveTXXXFrames("MusicBrainz Release Group Id");
                ID3v2TXXXFrame frame = ID3v2TXXXFrame.Create(m_Header, m_Flags, "MusicBrainz Release Group Id", value.ToString());
                AddFrame(frame);
            }
        }

        /// <summary>Gets or sets the cover front bitmap.</summary>
        /// <value>The cover front bitmap.</value>
        public ID3v2APICFrame CoverFront
        {
            get
            {
                foreach (ID3v2APICFrame frame in GetFrames<ID3v2APICFrame>("APIC"))
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
            foreach (ID3v2APICFrame frame in GetFrames<ID3v2APICFrame>("APIC"))
            {
                if (frame.PictureType == ID3v2PictureType.CoverFront)
                {
                    m_Frames.Remove(frame);
                }
            }
            {
                ID3v2APICFrame frame = ID3v2APICFrame.Create(m_Header, m_Flags, "", ID3v2PictureType.CoverFront, mimeType, imageData);
                AddFrame(frame);
            }
        }

        /// <summary>
        /// Obtains the date.
        /// </summary>
        public DateTime Date
        {
            get
            {
                int year = 0;
                {
                    string yyyy = GetTextFrameText("TYER");
                    if ((yyyy != null) && (yyyy.Length == 4))
                    {
                        int.TryParse(yyyy, out year);
                    }
                }
                if (year == 0)
                {
                    return default(DateTime);
                }

                int hour = 0;
                int min = 0;
                {
                    string hHmm = GetTextFrameText("TIME");
                    if ((hHmm != null) && (hHmm.Length == 4))
                    {
                        int.TryParse(hHmm.Substring(0, 2), out hour);
                        int.TryParse(hHmm.Substring(2), out min);
                    }
                }

                int day = 1;
                int month = 1;
                {
                    string ddMM = GetTextFrameText("TDAT");
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
                int result = 0;
                string track = Track;
                if (track != null)
                {
                    int index = track.IndexOf('/');
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
                string track = Track;
                if (string.IsNullOrEmpty(track))
                {
                    track = value.ToString();
                }
                else
                {
                    string[] parts = track.Split('/');
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
                int result = 0;
                string track = Track;
                if (track != null)
                {
                    int index = track.IndexOf('/');
                    if (index > 0)
                    {
                        int.TryParse(track.Substring(index + 1), out result);
                    }
                }
                return result;
            }
            set
            {
                string track = Track;
                if (string.IsNullOrEmpty(track))
                {
                    track = "0/" + value;
                }
                else
                {
                    string[] parts = track.Split('/');
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
                ID3v2XSLTFrame frame = ID3v2XSLTFrame.Create(Header, new ID3v2FrameFlags(), value.ToArray());
                ReplaceFrame(frame);
            }
        }

        /// <summary>Loads the specified tag.</summary>
        /// <param name="tag">The tag.</param>
        /// <exception cref="System.NotSupportedException"></exception>
        public void Load(ID3v2 tag)
        {
            m_Frames.Clear();
            switch (tag.Version)
            {
                case 3:
                case 4: break;
                default: throw new NotSupportedException(string.Format("Editing ID3v2.{0} is not supported!", tag.Version));
            }
            m_Header = tag.Header;
            m_Frames.Clear();
            foreach (ID3v2Frame frame in tag.Frames)
            {
                m_Frames.Add(frame);
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
            if ((null != tag.AcousticGuid) && (null == AcousticGuid))
            {
                AcousticGuid = tag.AcousticGuid;
            }

            if ((null != tag.CoverFront) && (null == CoverFront))
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

            if ((default(DateTime) != tag.Date) && (default(DateTime) == Date))
            {
                Date = tag.Date;
            }

            if ((default(Guid) != tag.MusicBrainzAlbumId) && (default(Guid) == MusicBrainzAlbumId))
            {
                MusicBrainzAlbumId = tag.MusicBrainzAlbumId;
            }

            if ((default(Guid) != tag.MusicBrainzArtistId) && (default(Guid) == MusicBrainzArtistId))
            {
                MusicBrainzArtistId = tag.MusicBrainzArtistId;
            }

            if ((default(Guid) != tag.MusicBrainzReleaseGroupId) && (default(Guid) == MusicBrainzReleaseGroupId))
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
            StringBuilder result = new StringBuilder();
            result.Append(base.ToString());
            foreach (ID3v2Frame frame in m_Frames)
            {
                result.AppendLine();
                result.Append(frame.ToString());
            }
            return result.ToString();
        }
    }
}
