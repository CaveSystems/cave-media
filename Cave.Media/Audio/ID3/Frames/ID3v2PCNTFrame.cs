using System;
namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// Play counter frame:<br />
    /// This is simply a counter of the number of times a file has been played. The value is increased by one every time the file begins to play.
    /// Attention: Most users don't like it if someone touches the files just to collect data! Use only after displaying opt-in form!
    /// </summary>
    
    public sealed class ID3v2PCNTFrame : ID3v2Frame
    {
        long m_Counter = -1;

        void Parse()
        {
            m_Counter = 0;
            for (int i = 10; i < m_Data.Length; i++)
            {
                m_Counter = (m_Counter << 8) | m_Data[i];
            }
        }

        internal ID3v2PCNTFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "PCNT") throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "PCNT"));
        }

        /// <summary>
        /// Obtains the counter
        /// </summary>
        public long Counter
        {
            get
            {
                if (m_Counter == -1) Parse();
                return m_Counter;
            }
        }

        /// <summary>
        /// Obtains a string describing this frame
        /// </summary>
        /// <returns>ID[Length] Counter</returns>
        public override string ToString()
        {
            return base.ToString() + " " + Counter.ToString();
        }
    }
}
