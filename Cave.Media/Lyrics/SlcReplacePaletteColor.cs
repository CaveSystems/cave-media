using Cave.IO;

namespace Cave.Media.Lyrics
{
	internal class SlcReplacePaletteColor : SynchronizedLyricsCommand
    {
        public byte ColorIndex { get; private set; }
        public ARGB ColorValue { get; private set; }

        public SlcReplacePaletteColor(DataReader reader)
            : base(SynchronizedLyricsCommandType.ReplacePaletteColor)
        {
            ColorIndex = reader.ReadByte();
            ColorValue = reader.ReadInt32();
        }

        public SlcReplacePaletteColor(byte colorIndex, ARGB colorValue)
            : base(SynchronizedLyricsCommandType.ReplacePaletteColor)
        {
            ColorIndex = colorIndex;
            ColorValue = colorValue;
        }

        protected override void SaveContentTo(DataWriter writer)
        {
            writer.Write(ColorIndex);
            writer.Write(ColorValue.AsUInt32);
        }
    }
}