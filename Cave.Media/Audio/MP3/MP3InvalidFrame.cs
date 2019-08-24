using System;
using Cave.IO;

namespace Cave.Media.Audio.MP3
{
    /// <summary>
    /// Provides an implementation for invalid mp3 frames (garbage data).
    /// </summary>
    public class MP3InvalidFrame : MP3MetaFrame
    {
        FifoBuffer m_Buffer = new FifoBuffer();

        internal void Add(byte[] buffer)
        {
            m_Buffer.Enqueue(buffer);
        }

        /// <summary>
        /// Creates a new empty instance.
        /// </summary>
        public MP3InvalidFrame()
        {
        }

        /// <summary>
        /// Throws a NotSupportedException.
        /// </summary>
        /// <param name="reader">FrameReader to read from.</param>
        public override bool Parse(DataFrameReader reader)
        {
            throw new NotSupportedException();
        }

        /// <summary>
        /// Gets an array with the data for this instance.
        /// </summary>
        /// <returns></returns>
        public override byte[] Data
        {
            get
            {
                return m_Buffer.ToArray();
            }
        }

        /// <summary>
        /// Length of the frame in bytes.
        /// </summary>
        public override int Length
        {
            get
            {
                return m_Buffer.Length;
            }
        }

        /// <summary>
        /// Returns false (invalid data is as long as the garbage read).
        /// </summary>
        public override bool IsFixedLength { get { return false; } }

        /// <summary>
        /// Returns false.
        /// </summary>
        public override bool IsValid
        {
            get { return false; }
        }
    }
}
