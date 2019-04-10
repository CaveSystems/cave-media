using System;
namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// With these frames dynamic data such as webpages with touring
    /// information, price information or plain ordinary news can be added to
    /// the tag.
    /// </summary>

    public sealed class ID3v2WXXXFrame : ID3v2Frame
    {
        string m_Address;
        string m_Description;

        void Parse()
        {
            var encoding = (ID3v2EncodingType)m_Content[0];
            int start = 1 + ID3v2Encoding.Parse(encoding, m_Content, 1, out m_Description);
            ID3v2Encoding.Parse(0, m_Content, start, out m_Address);
        }

        internal ID3v2WXXXFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "WXXX")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "WXXX"));
            }
        }

        /// <summary>
        /// Obtains the URL this frame contains.
        /// </summary>
        public string Address
        {
            get
            {
                if (m_Address == null)
                {
                    Parse();
                }

                return m_Address;
            }
        }

        /// <summary>
        /// Obtains the description for the URL this frame contains.
        /// </summary>
        public string Description
        {
            get
            {
                if (m_Description == null)
                {
                    Parse();
                }

                return m_Description;
            }
        }

        /// <summary>
        /// Obtains a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Description":"URL".</returns>
        public override string ToString()
        {
            return base.ToString() + " \"" + Description + "\":\"" + Address + '"';
        }
    }
}
