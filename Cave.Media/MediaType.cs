using System;

namespace Cave.Media
{
    /// <summary>Provides default media types</summary>
    [Flags]
    public enum MediaType : int
    {
        /// <summary>
        /// Unknown
        /// </summary>
        Unknown = -1,

        /// <summary>
        /// None
        /// </summary>
        None = 0,

        /// <summary>
        /// Picture (Stills)
        /// </summary>
        Picture = 0x1,

        /// <summary>
        /// Audio
        /// </summary>
        Audio = 0x2,

        /// <summary>
        /// Video
        /// </summary>
        Video = 0x4,

        /// <summary>
        /// Subtitle
        /// </summary>
        Subtitle = 0x8,

        /// <summary>
        /// Start of custom media types
        /// </summary>
        Custom = 0x1000,
    }
}
