using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Comments frame:<br />
    /// This frame is intended for any kind of full text information that
    /// does not fit in any other frame.
    /// </summary>
    public sealed class ID3v2COMMFrame : ID3v2Frame
    {
        string language = null;
        string description = null;
        string[] lines;

        void Parse()
        {
            var encoding = (ID3v2EncodingType)Content[0];
            language = ID3v2Encoding.ISO88591.GetString(Content, 1, 3);
            var len = ID3v2Encoding.Parse(encoding, Content, 4, out description);
            ID3v2Encoding.Parse(encoding, Content, 4 + len, out var text);
            lines = text.SplitNewLine();
        }

        internal ID3v2COMMFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "COMM")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "COMM"));
            }
        }

        /// <summary>
        /// Gets the language (3 character code).
        /// </summary>
        public string Language
        {
            get
            {
                if (language == null)
                {
                    Parse();
                }

                return language;
            }
        }

        /// <summary>
        /// Gets the description of the text this frame contains.
        /// </summary>
        public string Description
        {
            get
            {
                if (description == null)
                {
                    Parse();
                }

                return description;
            }
        }

        /// <summary>
        /// Gets the text this frame contains.
        /// </summary>
        public string[] Lines
        {
            get
            {
                if (lines == null)
                {
                    Parse();
                }

                return lines;
            }
        }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Text".</returns>
        public override string ToString() => base.ToString() + " \"" + Description + '"';
    }
}
