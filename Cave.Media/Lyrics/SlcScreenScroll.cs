using System;
using Cave.IO;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Synchronized Lyrics Command. 
    /// </summary>
    /// <seealso cref="SynchronizedLyricsCommand" />
    public class SlcScreenScroll : SynchronizedLyricsCommand
    {
        /// <summary>Gets the index of the color.</summary>
        /// <value>The index of the color.</value>
        public byte ColorIndex { get; private set; }

        /// <summary>Gets the horizontal offset.</summary>
        /// <value>The horizontal offset.</value>
        public sbyte Horizontal { get; private set; }

        /// <summary>Gets the vertical offset.</summary>
        /// <value>The vertical offset.</value>
        public sbyte Vertical { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="SlcScreenScroll"/> class.</summary>
        /// <param name="reader">The reader.</param>
        public SlcScreenScroll(DataReader reader)
            : base(SynchronizedLyricsCommandType.ScreenScroll)
        {
            ColorIndex = reader.ReadByte();
            Horizontal = reader.ReadInt8();
            Vertical = reader.ReadInt8();
        }

        /// <summary>Initializes a new instance of the <see cref="SlcScreenScroll"/> class.</summary>
        /// <exception cref="NotSupportedException"></exception>
        public SlcScreenScroll(byte colorIndex, sbyte horizontal, sbyte vertical)
            : base(SynchronizedLyricsCommandType.ScreenScroll)
        {
            ColorIndex = colorIndex;
            Horizontal = horizontal;
            Vertical = vertical;
        }

        /// <summary>Saves the content to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        protected override void SaveContentTo(DataWriter writer)
        {
            writer.Write(ColorIndex);
            writer.Write(Horizontal);
            writer.Write(Vertical);
        }
    }
}