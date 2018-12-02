using System;

namespace Cave.Media.Structs
{
    /// <summary>
    /// Flags for <see cref="AVIOLDINDEXENTRY"/>s
    /// </summary>
    [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2217:DoNotMarkEnumsWithFlags")]
    [Flags]
    public enum AVIOLDINDEXENTRYFLAGS : int
    {
        /// <summary>
        /// Entry is a list
        /// </summary>
        LIST = 0x00000001,

        /// <summary>
        /// Entry is a keyframe
        /// </summary>
        KEYFRAME = 0x00000010,

        /// <summary>
        /// Entry has no time
        /// </summary>
        NO_TIME = 0x00000100,

        /// <summary>
        /// Unused ? Informations welcome mailto:info@caveprojects.org
        /// </summary>
        COMPRESSOR = 0x0FFF0000,
    }
}
