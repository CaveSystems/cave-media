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
        string address;
        string description;

        void Parse()
        {
            var encoding = (ID3v2EncodingType)Content[0];
            var start = 1 + ID3v2Encoding.Parse(encoding, Content, 1, out description);
            ID3v2Encoding.Parse(0, Content, start, out address);
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
        /// Gets the URL this frame contains.
        /// </summary>
        public string Address
        {
            get
            {
                if (address == null)
                {
                    Parse();
                }

                return address;
            }
        }

        /// <summary>
        /// Gets the description for the URL this frame contains.
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
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Description":"URL".</returns>
        public override string ToString() => base.ToString() + " \"" + Description + "\":\"" + Address + '"';
    }
}
