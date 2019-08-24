using System;
namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// This frame contains the lyrics of the song or a text transcription of
    /// other vocal activities. The head includes an encoding descriptor and
    /// a content descriptor.
    /// </summary>
    public sealed class ID3v2USLTFrame : ID3v2Frame
    {
        bool parsed = false;
        string language = null;
        string descriptor = null;
        string[] lines = null;

        void Parse()
        {
            var encoding = (ID3v2EncodingType)Content[0];
            language = ID3v2Encoding.ISO88591.GetString(Content, 1, 3);
            int start = 4 + ID3v2Encoding.Parse(encoding, Content, 4, out descriptor);
            string text;
            ID3v2Encoding.Parse(encoding, Content, start, out text);
            lines = text.Split('\n');
        }

        internal ID3v2USLTFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "USLT")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "USLT"));
            }
        }

        /// <summary>
        /// Gets the lyrics language.
        /// </summary>
        public string Language
        {
            get
            {
                if (!parsed)
                {
                    Parse();
                }

                return language;
            }
        }

        /// <summary>
        /// Gets the lyric descripto.
        /// </summary>
        public string Descriptor
        {
            get
            {
                if (!parsed)
                {
                    Parse();
                }

                return descriptor;
            }
        }

        /// <summary>
        /// Gets the full song text.
        /// </summary>
        public string[] Lines
        {
            get
            {
                if (!parsed)
                {
                    Parse();
                }

                return (string[])lines.Clone();
            }
        }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] LNG:"Descriptor".</returns>
        public override string ToString()
        {
            return base.ToString() + " " + Language + ":\"" + Descriptor + '"';
        }
    }
}
