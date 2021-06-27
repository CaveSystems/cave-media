using System;
namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Play counter frame:<br />
    /// This is simply a counter of the number of times a file has been played. The value is increased by one every time the file begins to play.
    /// Attention: Most users don't like it if someone touches the files just to collect data! Use only after displaying opt-in form!.
    /// </summary>
    public sealed class ID3v2PCNTFrame : ID3v2Frame
    {
        long counter = -1;

        void Parse()
        {
            counter = 0;
            for (var i = 10; i < RawData.Length; i++)
            {
                counter = (counter << 8) | RawData[i];
            }
        }

        internal ID3v2PCNTFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "PCNT")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "PCNT"));
            }
        }

        /// <summary>
        /// Gets the counter.
        /// </summary>
        public long Counter
        {
            get
            {
                if (counter == -1)
                {
                    Parse();
                }

                return counter;
            }
        }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] Counter.</returns>
        public override string ToString() => base.ToString() + " " + Counter.ToString();
    }
}
