using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Private frame:<br />
    /// This frame is used to contain information from a software producer that its program uses and does not fit into the other frames.
    /// </summary>
    public sealed class ID3v2PRIVFrame : ID3v2Frame
    {
        string owner;
        int startIndex;

        void Parse() => startIndex = 1 + ID3v2Encoding.Parse(0, Content, 0, out owner);

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
                if (owner == null)
                {
                    Parse();
                }

                return owner;
            }
        }

        /// <summary>
        /// Gets the private bytes this frame contains.
        /// </summary>
        public byte[] PrivateBytes
        {
            get
            {
                if (startIndex <= 0)
                {
                    Parse();
                }

                return new DataFrameReader(RawData).Read(startIndex, RawData.Length - startIndex);
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
        public override string ToString() => base.ToString() + " \"" + Owner + '"';
    }
}
