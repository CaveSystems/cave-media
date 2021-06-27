using System;
using System.IO;
using System.Text;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides support for reading and writing ID3v1.0 and ID3v1.1 format.
    /// </summary>
    public sealed class ID3v1 : MP3MetaFrame
    {
        /// <summary>
        /// Reads a <see cref="ID3v1"/> tag directly from a file.
        /// </summary>
        /// <param name="fileName">The file to read from.</param>
        /// <returns></returns>
        public static ID3v1 FromFile(string fileName)
        {
            Stream stream = File.OpenRead(fileName);
            stream.Seek(-128, SeekOrigin.End);
            var buffer = new byte[128];
            stream.Read(buffer, 0, 128);
            var result = new ID3v1(buffer);
            stream.Close();
            return result;
        }

        #region Genre List

        /// <summary>
        /// Provides the full Genre List supported by ID3v1.
        /// </summary>
        public static string[] Genres => new string[]
                {
                    "Blues", "ClassicRock", "Country", "Dance", "Disco", "Funk", "Grunge", "Hip-Hop",
                    "Jazz", "Metal", "NewAge", "Oldies", "Other", "Pop", "R&B", "Rap", "Reggae",
                    "Rock", "Techno", "Industrial", "Alternative", "Ska", "DeathMetal", "Pranks", "Soundtrack",
                    "Euro-Techno", "Ambient", "Trip-Hop", "Vocal", "Jazz+Funk", "Fusion", "Trance", "Classical",
                    "Instrumental", "Acid", "House", "Game", "SoundClip", "Gospel", "Noise", "AlternRock",
                    "Bass", "Soul", "Punk", "Space", "Meditative", "InstrumentalPop", "InstrumentalRock", "Ethnic",
                    "Gothic", "Darkwave", "Techno-Industrial", "Electronic", "Pop-Folk", "Eurodance", "Dream", "SouthernRock",
                    "Comedy", "Cult", "Gangsta", "Top", "ChristianRap", "Pop/Funk", "Jungle", "NativeAmerican",
                    "Cabaret", "NewWave", "Psychadelic", "Rave", "Showtunes", "Trailer", "Lo-Fi", "Tribal",
                    "AcidPunk", "AcidJazz", "Polka", "Retro", "Musical", "Rock&Roll", "HardRock", "Folk",
                    "Folk-Rock", "NationalFolk", "Swing", "FastFusion", "Bebob", "Latin", "Revival", "Celtic",
                    "Bluegrass", "Avantgarde", "GothicRock", "ProgressiveRock", "PsychedelicRock", "SymphonicRock", "SlowRock",
                    "BigBand", "Chorus", "EasyListening", "Acoustic", "Humour", "Speech", "Chanson", "Opera",
                    "ChamberMusic", "Sonata", "Symphony", "BootyBass", "Primus", "PornGroove", "Satire", "SlowJam",
                    "Club", "Tango", "Samba", "Folklore", "Ballad", "PowerBallad", "RhythmicSoul", "Freestyle",
                    "Duet", "PunkRock", "DrumSolo", "Acapella", "Euro-House", "DanceHall",
                };
        #endregion

        #region private fields and implementation
        Encoding encoding = Encoding.GetEncoding("ISO-8859-1");
        string title;
        string artist;
        string album;
        string year;
        string comment;
        string genre;
        byte trackNumber;
        byte[] data = null;
        #endregion

        /// <summary>
        /// Creates a new empty ID3v1 tag.
        /// </summary>
        public ID3v1()
        {
        }

        /// <summary>
        /// Creates a ID3v1 tag instance containing the specified data.
        /// </summary>
        /// <param name="data"></param>
        public ID3v1(byte[] data)
        {
            if (data == null)
            {
                throw new ArgumentNullException("Data");
            }

            if (data.Length != 128)
            {
                throw new InvalidDataException();
            }

            this.data = data;
            ParseData();
        }

        #region parser functions

        /// <summary>
        /// Internally parses the current data and loads all fields.
        /// </summary>
        void ParseData()
        {
            var str = encoding.GetString(data);
            if (str.Substring(0, 3) != "TAG")
            {
                throw new InvalidDataException(string.Format("TAG header not found!"));
            }

            title = ASCII.Clean(str, 3, 30).TrimEnd();
            artist = ASCII.Clean(str, 33, 30).TrimEnd();
            album = ASCII.Clean(str, 63, 30).TrimEnd();
            year = ASCII.Clean(str, 93, 4).TrimEnd();
            if ((data[126] != 0) && (data[125] == 0))
            {
                comment = ASCII.Clean(str, 97, 28).TrimEnd();
                trackNumber = data[126];
            }
            else
            {
                comment = ASCII.Clean(str, 97, 30).TrimEnd();
            }
            var l_Genre = data[127];
            if (l_Genre < Genres.Length)
            {
                genre = Genres[l_Genre];
            }
            else
            {
                genre = null;
            }
        }

        /// <summary>
        /// Parses the specified buffer starting at index to load all data for this frame
        /// This function will throw exceptions on parser errors.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        public override bool Parse(DataFrameReader reader)
        {
            if (reader == null)
            {
                throw new ArgumentNullException("Reader");
            }

            if (!reader.EnsureBuffer(128))
            {
                return false;
            }

            data = reader.Read(0, 128);
            ParseData();
            reader.Remove(128);
            return true;
        }
        #endregion

        #region public properties

        /// <summary>
        /// Title of the track.
        /// </summary>
        public string Title
        {
            get => title;
            set
            {
                data = null;
                title = value;
            }
        }

        /// <summary>
        /// Semicolon separated list of performers.
        /// </summary>
        public string Artist
        {
            get => artist;
            set
            {
                data = null;
                artist = value;
            }
        }

        /// <summary>
        /// Album name the track belongs to.
        /// </summary>
        public string Album
        {
            get => album;
            set
            {
                data = null;
                album = value;
            }
        }

        /// <summary>
        /// 4 digit year.
        /// </summary>
        public string Year
        {
            get => year;
            set
            {
                data = null;
                year = value;
            }
        }

        /// <summary>
        /// User comment.
        /// </summary>
        public string Comment
        {
            get => comment;
            set
            {
                data = null;
                comment = value;
            }
        }

        /// <summary>
        /// Genre this track belongs to.
        /// </summary>
        public string Genre
        {
            get => genre;
            set
            {
                data = null;
                genre = value;
            }
        }

        /// <summary>
        /// Number of the track. Valid values range from 1..254.
        /// </summary>
        public byte TrackNumber
        {
            get => trackNumber;
            set
            {
                data = null;
                trackNumber = value;
            }
        }

        /// <summary>
        /// Length of the frame in bytes.
        /// </summary>
        public override int Length => 128;

        /// <summary>
        /// Returns true (tag has always 128 byte).
        /// </summary>
        public override bool IsFixedLength => true;

        /// <summary>
        /// Gets an array with the data for this instance.
        /// </summary>
        /// <returns></returns>
        public override byte[] Data
        {
            get
            {
                if (data == null)
                {
                    var stringBuilder = new StringBuilder();
                    stringBuilder.Append("TAG");
                    stringBuilder.Append(title.ForceLength(30, string.Empty, "\0"));
                    stringBuilder.Append(artist.ForceLength(30, string.Empty, "\0"));
                    stringBuilder.Append(album.ForceLength(30, string.Empty, "\0"));
                    stringBuilder.Append(year.ForceLength(4, string.Empty, "\0"));
                    if ((trackNumber > 0) && (trackNumber < 255))
                    {
                        stringBuilder.Append(comment.ForceLength(28, string.Empty, "\0"));
                        stringBuilder.Append("\0\0");
                    }
                    else
                    {
                        stringBuilder.Append(comment.ForceLength(30, string.Empty, "\0"));
                    }
                    stringBuilder.Append("\0");
                    var data = encoding.GetBytes(stringBuilder.ToString());
                    if (data.Length != 128)
                    {
                        throw new Exception(string.Format("Encoding for the TAG is invalid and does not return one byte for one character!"));
                    }

                    data[127] = (byte)Array.IndexOf(Genres, genre);
                    this.data = data;
                }
                return (byte[])data.Clone();
            }
        }

        /// <summary>
        /// Gets the full tag as string.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.AppendLine(base.ToString());
            result.Append("Album: ");
            result.AppendLine(Album);
            result.Append("Artist: ");
            result.AppendLine(Artist);
            result.Append("Comment: ");
            result.AppendLine(Comment);
            result.Append("Genre: ");
            result.AppendLine(Genre);
            result.Append("Title: ");
            result.AppendLine(Title);
            result.Append("TrackNumber: ");
            result.AppendLine(TrackNumber.ToString());
            result.Append("Year: ");
            result.AppendLine(Year);
            return result.ToString();
        }
        #endregion

        /// <summary>
        /// Directly writes a <see cref="ID3v1"/> tag to the specified file replacing the existing one (if any).
        /// </summary>
        /// <param name="fileName">The file.</param>
        public void Write(string fileName)
        {
            Stream stream = File.Open(fileName, FileMode.OpenOrCreate, FileAccess.ReadWrite, FileShare.None);
            stream.Seek(-128, SeekOrigin.End);

            // save position for replace
            var l_Position = stream.Position;
            try
            {
                // try to load existing tag
                var buffer = new byte[128];
                stream.Read(buffer, 0, 128);
                var result = new ID3v1(buffer);

                // success -> replace
                stream.Position = l_Position;
            }
            catch
            {
                // error while reading existing tag -> append new one
                stream.Seek(0, SeekOrigin.End);
            }
            stream.Write(Data, 0, Length);
            stream.Close();
        }
    }
}
