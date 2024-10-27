using System;
using System.Collections.Generic;
using System.Linq;
using Cave.IO;

namespace Cave.Media.Lyrics;

/// <summary>
/// Provides a synchronized lyrics item with timecode containing multiple commands.
/// </summary>
public class SynchronizedLyricsItem
{
    ISynchronizedLyricsCommand[] m_Commands;

    /// <summary>Initializes a new instance of the <see cref="SynchronizedLyricsItem"/> class.</summary>
    /// <param name="timeCode">The time code.</param>
    /// <param name="items">The items.</param>
    public SynchronizedLyricsItem(TimeSpan timeCode, IEnumerable<ISynchronizedLyricsCommand> items)
    {
        TimeCode = timeCode;
        m_Commands = items.ToArray();
    }

    /// <summary>Initializes a new instance of the <see cref="SynchronizedLyricsItem"/> class.</summary>
    /// <param name="timeCode">The time code.</param>
    /// <param name="items">The items.</param>
    public SynchronizedLyricsItem(TimeSpan timeCode, params ISynchronizedLyricsCommand[] items)
    {
        TimeCode = timeCode;
        m_Commands = (ISynchronizedLyricsCommand[])items.Clone();
    }

    /// <summary>Gets the commands.</summary>
    /// <value>The commands.</value>
    public ISynchronizedLyricsCommand[] Commands => (ISynchronizedLyricsCommand[])m_Commands.Clone();

    /// <summary>The time code.</summary>
    public readonly TimeSpan TimeCode;

    /// <summary>Saves the specified previous time.</summary>
    /// <param name="previousTime">The previous time.</param>
    /// <param name="writer">The writer.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public void Save(TimeSpan previousTime, DataWriter writer)
    {
        // save time in milliseconds since last slc
        var lastMilliSecond = previousTime.Ticks / TimeSpan.TicksPerMillisecond;
        var myMilliSecond = TimeCode.Ticks / TimeSpan.TicksPerMillisecond;
        var diff = myMilliSecond - lastMilliSecond;
        if (diff <= 0 && myMilliSecond > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(previousTime));
        }

        writer.Write7BitEncoded64(diff);

        // save all commands
        foreach (SynchronizedLyricsCommand command in Commands)
        {
            command.SaveTo(writer);
        }

        // zero terminate
        writer.Write((byte)0);
    }

    /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
    /// <returns>A <see cref="string" /> that represents this instance.</returns>
    public override string ToString() => StringExtensions.FormatTime(TimeCode) + " " + StringExtensions.Join(Commands, ", ");
}