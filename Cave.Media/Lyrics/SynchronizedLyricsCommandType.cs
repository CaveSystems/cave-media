namespace Cave.Media.Lyrics;

/// <summary>
/// Provides command types for synchronized lyrics.
/// </summary>
public enum SynchronizedLyricsCommandType
{
    /// <summary>No command</summary>
    None = 0,

    /// <summary>The clear screen command</summary>
    ClearScreen = 1,

    /// <summary>The replace palette color command</summary>
    ReplacePaletteColor = 2,

    /// <summary>The set sprite2 colors command</summary>
    SetSprite2Colors = 3,

    /// <summary>The set sprite2 colors xor command</summary>
    SetSprite2ColorsXOR = 4,

    /// <summary>The screen offset command</summary>
    ScreenOffset = 5,

    /// <summary>The screen scroll command</summary>
    ScreenScroll = 6,

    /// <summary>The set transparent color command</summary>
    SetTransparentColor = 7,

    /// <summary>The replace palette colors command</summary>
    ReplacePaletteColors = 8,

    /// <summary>The screen roll command</summary>
    ScreenRoll = 9,
}