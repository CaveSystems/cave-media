using System;
using System.Collections.Generic;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides a synchronized lyrics item builder with timecode containing multiple commands
    /// </summary>
    public class SynchronizedLyricsItemBuilder
    {
        /// <summary>The time code</summary>
        public TimeSpan TimeCode { get; set; }

        /// <summary>The commands</summary>
        public readonly List<ISynchronizedLyricsCommand> Commands = new List<ISynchronizedLyricsCommand>();

        /// <summary>Converts to the synchronized lyrics item.</summary>
        /// <returns></returns>
        public SynchronizedLyricsItem ToSynchronizedLyricsItem()
        {
            return new SynchronizedLyricsItem(TimeCode, Commands);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return StringExtensions.FormatTime(TimeCode) + " " + StringExtensions.Join(Commands, ", ");
        }
    }
}
