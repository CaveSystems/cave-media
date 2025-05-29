using Cave.IO;

namespace Cave.Media.Lyrics;

internal class SlcReplacePaletteColors : SynchronizedLyricsCommand
{
    public ARGB[] PaletteUpdate { get; private set; }

    public byte ColorIndex { get; private set; }

    public SlcReplacePaletteColors(DataReader reader)
        : base(SynchronizedLyricsCommandType.ReplacePaletteColors)
    {
        ColorIndex = reader.ReadByte();
        int count = reader.ReadByte();
        var pal = new ARGB[count];
        for (var i = 0; i < count; i++)
        {
            pal[i] = reader.ReadUInt32();
        }
        PaletteUpdate = pal;
    }

    public SlcReplacePaletteColors(byte colorIndex, ARGB[] paletteUpdate)
        : base(SynchronizedLyricsCommandType.ReplacePaletteColors)
    {
        ColorIndex = colorIndex;
        PaletteUpdate = paletteUpdate;
    }

    protected override void SaveContentTo(DataWriter writer)
    {
        writer.Write(ColorIndex);
        writer.Write((byte)PaletteUpdate.Length);
        foreach (var value in PaletteUpdate)
        {
            writer.Write(value.AsUInt32);
        }
    }
}