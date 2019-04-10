using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// This frame's purpose is to be able to identify the audio file in a
    /// database, that may provide more information relevant to the content.
    /// Since standardisation of such a database is beyond this document, all
    /// UFID frames begin with an 'owner identifier' field. It is a null-
    /// terminated string with a URL [URL] containing an email address, or a
    /// link to a location where an email address can be found, that belongs
    /// to the organisation responsible for this specific database
    /// implementation. Questions regarding the database should be sent to
    /// the indicated email address. The URL should not be used for the
    /// actual database queries. The string
    /// "http://www.id3.org/dummy/ufid.html" should be used for tests. The
    /// 'Owner identifier' must be non-empty (more than just a termination).
    /// The 'Owner identifier' is then followed by the actual identifier,
    /// which may be up to 64 bytes. There may be more than one "UFID" frame
    /// in a tag, but only one with the same 'Owner identifier'.
    /// </summary>
    public sealed class ID3v2UFIDFrame : ID3v2Frame
    {
        string owner = null;
        byte[] ufid = null;

        void Split()
        {
            int index = Array.IndexOf<byte>(m_Content, 0);
            owner = ID3v2Encoding.ISO88591.GetString(m_Content, 0, index);
            ufid = new byte[m_Content.Length - index - 1];
            Array.Copy(m_Content, index + 1, ufid, 0, ufid.Length);
        }

        internal ID3v2UFIDFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID[0] != 'U')
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "U*"));
            }
        }

        /// <summary>
        /// Gets the owner of the ufid.
        /// </summary>
        public string Owner
        {
            get
            {
                if (owner == null)
                {
                    Split();
                }

                return owner;
            }
        }

        /// <summary>
        /// Gets the ufid data.
        /// </summary>
        public byte[] UFID
        {
            get
            {
                if (ufid == null)
                {
                    Split();
                }

                return (byte[])ufid.Clone();
            }
        }

        /// <summary>
        /// Gets the UFID as hex string.
        /// </summary>
        public string HexUFID => StringExtensions.ToHexString(UFID);

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Owner":HexUFID.</returns>
        public override string ToString()
        {
            return base.ToString() + " \"" + Owner + "\":" + HexUFID;
        }
    }
}
