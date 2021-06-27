using System;
using Cave.IO;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Synchronized Lyrics Command with color index.
    /// </summary>
    /// <seealso cref="SynchronizedLyricsCommand" />
    public class SlcWithColorIndex : SynchronizedLyricsCommand
    {
        /// <summary>Gets the index of the color.</summary>
        /// <value>The index of the color.</value>
        public byte ColorIndex { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="SlcWithColorIndex"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="reader">The reader.</param>
        public SlcWithColorIndex(SynchronizedLyricsCommandType type, DataReader reader)
            : base(type)
        {
            ColorIndex = reader.ReadByte();
        }

        /// <summary>Initializes a new instance of the <see cref="SlcWithColorIndex"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="colorIndex">Index of the color.</param>
        /// <exception cref="NotSupportedException"></exception>
        public SlcWithColorIndex(SynchronizedLyricsCommandType type, byte colorIndex)
            : base(type)
        {
            switch (type)
            {
                case SynchronizedLyricsCommandType.SetTransparentColor:
                case SynchronizedLyricsCommandType.ClearScreen: break;
                default: throw new NotSupportedException();
            }
            ColorIndex = colorIndex;
        }

        /// <summary>Saves the content to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        protected override void SaveContentTo(DataWriter writer) => writer.Write(ColorIndex);
    }
}