
namespace Cave.Media.Lyrics
{
    /// <summary>
    /// CDG instructions.
    /// </summary>
    public enum CdgInstruction
    {
        /// <summary>unknown</summary>
        Unknown = 0,

        /// <summary>The memory preset (clear screen)</summary>
        MemoryPreset = 1,

        /// <summary>The border preset (set border color)</summary>
        BorderPreset = 2,

        /// <summary>The tile block (sprite)</summary>
        TileBlock = 6,

        /// <summary>The scroll preset</summary>
        ScrollPreset = 20,

        /// <summary>The scroll copy</summary>
        ScrollCopy = 24,

        /// <summary>The define transparent color</summary>
        DefineTransparentColor = 28,

        /// <summary>The load lower color table</summary>
        LoadLowerColorTable = 30,

        /// <summary>The load higher color table</summary>
        LoadHigherColorTable = 31,

        /// <summary>The tile block xor (sprite)</summary>
        TileBlockXor = 38,
    }
}
