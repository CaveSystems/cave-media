using System;
using Cave.IO;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Synchronized Lyrics Command.
    /// </summary>
    /// <seealso cref="SynchronizedLyricsCommand" />
    public class SlcScreenOffset : SynchronizedLyricsCommand
    {
        /// <summary>Gets the horizontal offset.</summary>
        /// <value>The horizontal offset.</value>
        public sbyte Horizontal { get; private set; }

        /// <summary>Gets the vertical offset.</summary>
        /// <value>The vertical offset.</value>
        public sbyte Vertical { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="SlcScreenOffset"/> class.</summary>
        /// <param name="reader">The reader.</param>
        public SlcScreenOffset(DataReader reader)
            : base(SynchronizedLyricsCommandType.ScreenOffset)
        {
            Horizontal = reader.ReadInt8();
            Vertical = reader.ReadInt8();
        }

        /// <summary>Initializes a new instance of the <see cref="SlcScreenOffset"/> class.</summary>
        /// <exception cref="NotSupportedException"></exception>
        public SlcScreenOffset(sbyte horizontal, sbyte vertical)
            : base(SynchronizedLyricsCommandType.ScreenOffset)
        {
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>Saves the content to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        protected override void SaveContentTo(DataWriter writer)
        {
            writer.Write(Horizontal);
            writer.Write(Vertical);
        }
    }
}