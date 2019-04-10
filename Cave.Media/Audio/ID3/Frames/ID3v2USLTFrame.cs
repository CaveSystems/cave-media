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
        bool m_Parsed = false;
        string m_Language = null;
        string m_Descriptor = null;
        string[] m_Lines = null;

        void Parse()
        {
            var encoding = (ID3v2EncodingType)m_Content[0];
            m_Language = ID3v2Encoding.ISO88591.GetString(m_Content, 1, 3);
            int start = 4 + ID3v2Encoding.Parse(encoding, m_Content, 4, out m_Descriptor);
            string text;
            ID3v2Encoding.Parse(encoding, m_Content, start, out text);
            m_Lines = text.Split('\n');
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
        /// Provides the lyrics language.
        /// </summary>
        public string Language
        {
            get
            {
                if (!m_Parsed)
                {
                    Parse();
                }

                return m_Language;
            }
        }

        /// <summary>
        /// Provides the lyric descripto.
        /// </summary>
        public string Descriptor
        {
            get
            {
                if (!m_Parsed)
                {
                    Parse();
                }

                return m_Descriptor;
            }
        }

        /// <summary>
        /// Provides the full song text.
        /// </summary>
        public string[] Lines
        {
            get
            {
                if (!m_Parsed)
                {
                    Parse();
                }

                return (string[])m_Lines.Clone();
            }
        }

        /// <summary>
        /// Obtains a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] LNG:"Descriptor".</returns>
        public override string ToString()
        {
            return base.ToString() + " " + Language + ":\"" + Descriptor + '"';
        }
    }
}
