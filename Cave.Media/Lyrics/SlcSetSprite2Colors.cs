using Cave.IO;
using System;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Synchronized lyrics command for 2 color (indexed) sprites
    /// </summary>
    /// <seealso cref="SynchronizedLyricsCommand" />
    public class SlcSetSprite2Colors : SynchronizedLyricsCommand
    {
        /// <summary>Gets the width.</summary>
        /// <value>The width.</value>
        public int Width { get; private set; }

        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        public int Height { get; private set; }

        /// <summary>Gets the x position.</summary>
        /// <value>The x position.</value>
        public int X { get; private set; }

        /// <summary>Gets the y position.</summary>
        /// <value>The y position.</value>
        public int Y { get; private set; }

        /// <summary>Gets the first color (0 = background).</summary>
        /// <value>The first color.</value>
        public byte Color0 { get; private set; }

        /// <summary>Gets the second color (1 = foreground).</summary>
        /// <value>The second color.</value>
        public byte Color1 { get; private set; }

        /// <summary>Gets the bit array.</summary>
        /// <value>The bit array.</value>
        public byte[] BitArray { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="SlcSetSprite2Colors"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="w">The width.</param>
        /// <param name="h">The height.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="color0">The color0.</param>
        /// <param name="color1">The color1.</param>
        /// <param name="data">The data.</param>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="Exception">Data length invalid!</exception>
        public SlcSetSprite2Colors(SynchronizedLyricsCommandType type, int w, int h, int x, int y, byte color0, byte color1, byte[] data)
            : base(type)
        {
            switch(type)
            {
                case SynchronizedLyricsCommandType.SetSprite2Colors:
                case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: break;
                default: throw new NotSupportedException();
            }
            if (data.Length != (w + 7) / 8 * h) throw new Exception("Data length invalid!");
            Width = w;
            Height = h;
            X = x;
            Y = y;
            Color0 = color0;
            Color1 = color1;
            BitArray = data;
        }

        /// <summary>Initializes a new instance of the <see cref="SlcSetSprite2Colors"/> class.</summary>
        /// <param name="type">The type.</param>
        /// <param name="reader">The reader.</param>
        /// <exception cref="NotSupportedException"></exception>
        public SlcSetSprite2Colors(SynchronizedLyricsCommandType type, DataReader reader)
            : base(type)
        {
            switch (type)
            {
                case SynchronizedLyricsCommandType.SetSprite2Colors:
                case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: break;
                default: throw new NotSupportedException();
            }
            Width = reader.Read7BitEncodedInt32();
            Height = reader.Read7BitEncodedInt32();
            X = reader.Read7BitEncodedInt32();
            Y = reader.Read7BitEncodedInt32();
            Color0 = reader.ReadByte();
            Color1 = reader.ReadByte();
            BitArray = reader.ReadBytes((Width + 7) / 8 * Height);
        }

        /// <summary>Saves the content to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        protected override void SaveContentTo(DataWriter writer)
        {
            writer.Write7BitEncoded32(Width);
            writer.Write7BitEncoded32(Height);
            writer.Write7BitEncoded32(X);
            writer.Write7BitEncoded32(Y);
            writer.Write(Color0);
            writer.Write(Color1);
            writer.Write(BitArray);
        }
    }
}