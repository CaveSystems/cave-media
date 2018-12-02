using System;

namespace Cave.Media.Structs
{
    /// <summary>
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags]
    public enum SMPTEFLAGS : int
    {
        /// <summary></summary>
        None = 0,

        /// <summary></summary>
        SMPTE_BINARY_GROUP = 0x07,

        /// <summary></summary>
        SMPTE_COLOR_FRAME = 0x08,
    }
}
