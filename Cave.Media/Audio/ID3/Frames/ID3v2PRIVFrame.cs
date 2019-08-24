using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Private frame:<br />
    /// This frame is used to contain information from a software producer that its program uses and does not fit into the other frames.
    /// </summary>
    public sealed class ID3v2PRIVFrame : ID3v2Frame
    {
        string m_Owner;
        int m_StartIndex;

        void Parse()
        {
            m_StartIndex = 1 + ID3v2Encoding.Parse(0, m_Content, 0, out m_Owner);
        }

        internal ID3v2PRIVFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "PRIV")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "PRIV"));
            }
        }

        /// <summary>
        /// Gets the Owner identifier for this frame.
        /// </summary>
        public string Owner
        {
            get
            {
                if (m_Owner == null)
                {
                    Parse();
                }

                return m_Owner;
            }
        }

        /// <summary>
        /// Gets the private bytes this frame contains.
        /// </summary>
        public byte[] PrivateBytes
        {
            get
            {
                if (m_StartIndex <= 0)
                {
                    Parse();
                }

                return new DataFrameReader(m_Data).Read(m_StartIndex, m_Data.Length - m_StartIndex);
            }
        }

        /// <summary>
        /// Gets the private bytes this frame contains as hexadecimal string.
        /// </summary>
        public string HexPrivateBytes => StringExtensions.ToHexString(PrivateBytes);

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Owner".</returns>
        public override string ToString()
        {
            return base.ToString() + " \"" + Owner + '"';
        }
    }
}
