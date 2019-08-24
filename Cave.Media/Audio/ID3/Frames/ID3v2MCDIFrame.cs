using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Since there might be a lot of people contributing to an audio file in various ways, such as musicians and technicians,
    /// the 'Text information frames' are often insufficient to list everyone involved in a project.
    /// The 'Involved people list' is a frame containing the names of those involved, and how they were involved.
    /// </summary>
    public sealed class ID3v2MCDIFrame : ID3v2Frame
    {
        internal ID3v2MCDIFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "MCDI")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "MCDI"));
            }
        }

        /// <summary>
        /// Gets the TOC.
        /// </summary>
        public byte[] TOC => (byte[])m_Content.Clone();

        /// <summary>
        /// Returns the TOC as hexadecimal string.
        /// </summary>
        public string HexTOC => StringExtensions.ToHexString(m_Content);

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] HexTOC.</returns>
        public override string ToString()
        {
            return base.ToString() + " " + HexTOC;
        }
    }
}
