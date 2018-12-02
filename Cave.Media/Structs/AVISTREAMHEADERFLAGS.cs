using System;

namespace Cave.Media.Structs
{
    /// <summary>
    /// AVISTREAMINFOFLAGS flags
    /// </summary>
    [Flags]
    public enum AVISTREAMHEADERFLAGS : int
    {
        /// <summary>
        /// Indicates this stream should be rendered when explicitly enabled by the user.
        /// </summary>
        DISABLED = 0x00000001,

        /// <summary>
        /// Indicates this video stream contains palette changes. This flag warns the playback software that it will need to animate the palette.
        /// </summary>
        FORMATCHANGES = 0x00010000,
    }
}
