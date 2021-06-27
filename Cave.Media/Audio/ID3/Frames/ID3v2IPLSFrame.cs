using System;
using System.Collections.Generic;
using System.Text;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Since there might be a lot of people contributing to an audio file in various ways, such as musicians and technicians,
    /// the 'Text information frames' are often insufficient to list everyone involved in a project.
    /// The 'Involved people list' is a frame containing the names of those involved, and how they were involved.
    /// </summary>
    public sealed class ID3v2IPLSFrame : ID3v2Frame
    {
        ID3v2Contributor[] list = null;

        void Parse()
        {
            var encoding = ID3v2Encoding.Get((ID3v2EncodingType)Content[0]);
            var strings = encoding.GetString(RawData, 1, Content.Length - 1).Split('\0');
            var i = 0;
            var list = new List<ID3v2Contributor>();
            while (i < strings.Length)
            {
                var item = default(ID3v2Contributor);
                item.Involvement = strings[i++];
                item.Involvee = strings[i++];
                list.Add(item);
            }
            this.list = list.ToArray();
        }

        internal ID3v2IPLSFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "IPLS")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "IPLS"));
            }
        }

        /// <summary>
        /// Gets the full list of Involvements and Involvees.
        /// </summary>
        public ID3v2Contributor[] List
        {
            get
            {
                if (list == null)
                {
                    Parse();
                }

                return (ID3v2Contributor[])list.Clone();
            }
        }
    }
}
