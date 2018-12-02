using Cave.IO;
using System;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides a single synchronized lyrics command. This is used to set colors, scroll, write text, draw sprites, ...
    /// Multiple commands can be run at the same timecode. See <see cref="SynchronizedLyricsItem"/> for more information.
    /// </summary>
    /// <seealso cref="ISynchronizedLyricsCommand" />
    public abstract class SynchronizedLyricsCommand : ISynchronizedLyricsCommand
    {
        /// <summary>Parses a <see cref="SynchronizedLyricsCommand"/> from the specified reader.</summary>
        /// <param name="reader">The reader.</param>
        /// <returns></returns>
        /// <exception cref="NotImplementedException"></exception>
        public static ISynchronizedLyricsCommand Parse(DataReader reader)
        {
            SynchronizedLyricsCommandType type = (SynchronizedLyricsCommandType)reader.Read7BitEncodedInt64();
            switch (type)
            {
                case SynchronizedLyricsCommandType.None: return null;
                case SynchronizedLyricsCommandType.ClearScreen: return new SlcWithColorIndex(type, reader); 
                case SynchronizedLyricsCommandType.SetSprite2Colors: return new SlcSetSprite2Colors(type, reader); 
                case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: return new SlcSetSprite2Colors(type, reader);
                case SynchronizedLyricsCommandType.SetTransparentColor: return new SlcWithColorIndex(type, reader);
                case SynchronizedLyricsCommandType.ReplacePaletteColor: return new SlcReplacePaletteColor(reader);
                case SynchronizedLyricsCommandType.ReplacePaletteColors: return new SlcReplacePaletteColors(reader);
                case SynchronizedLyricsCommandType.ScreenOffset: return new SlcScreenOffset(reader);
                case SynchronizedLyricsCommandType.ScreenRoll: return new SlcScreenRoll(reader);
                case SynchronizedLyricsCommandType.ScreenScroll: return new SlcScreenScroll(reader);
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Gets the command type.</summary>
        /// <value>The command type.</value>
        public SynchronizedLyricsCommandType Type { get; private set; }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedLyricsCommand"/> class.</summary>
        /// <param name="type">The type.</param>
        protected SynchronizedLyricsCommand(SynchronizedLyricsCommandType type) { Type = type; }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Type.ToString();
        }

        /// <summary>Saves the command to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        public void SaveTo(DataWriter writer)
        {
            writer.Write7BitEncoded64((long)Type);
            SaveContentTo(writer);
        }

        /// <summary>Saves the content to the specified writer.</summary>
        /// <param name="writer">The writer.</param>
        protected abstract void SaveContentTo(DataWriter writer);
    }
}