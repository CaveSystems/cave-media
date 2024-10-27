using System;
using Cave.IO;

namespace Cave.Media.Lyrics;

/// <summary>
/// Synchronized Lyrics Command.
/// </summary>
/// <seealso cref="SynchronizedLyricsCommand" />
public class SlcScreenRoll : SynchronizedLyricsCommand
{
    /// <summary>Gets the horizontal offset.</summary>
    /// <value>The horizontal offset.</value>
    public sbyte Horizontal { get; private set; }

    /// <summary>Gets the vertical offset.</summary>
    /// <value>The vertical offset.</value>
    public sbyte Vertical { get; private set; }

    /// <summary>Initializes a new instance of the <see cref="SlcScreenRoll"/> class.</summary>
    /// <param name="reader">The reader.</param>
    public SlcScreenRoll(DataReader reader)
        : base(SynchronizedLyricsCommandType.ScreenRoll)
    {
        Horizontal = reader.ReadInt8();
        Vertical = reader.ReadInt8();
    }

    /// <summary>Initializes a new instance of the <see cref="SlcScreenRoll"/> class.</summary>
    /// <exception cref="NotSupportedException"></exception>
    public SlcScreenRoll(sbyte horizontal, sbyte vertical)
        : base(SynchronizedLyricsCommandType.ScreenRoll)
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