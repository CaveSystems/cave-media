using Cave.IO;

namespace Cave.Media.Lyrics;

/// <summary>
/// Provides a synchronized lyrics command interface.
/// </summary>
public interface ISynchronizedLyricsCommand
{
    /// <summary>Gets the command type.</summary>
    /// <value>The command type.</value>
    SynchronizedLyricsCommandType Type { get; }

    /// <summary>Saves the command to the specified writer.</summary>
    /// <param name="writer">The writer.</param>
    void SaveTo(DataWriter writer);
}