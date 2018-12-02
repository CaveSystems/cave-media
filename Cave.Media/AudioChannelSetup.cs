using System;

namespace Cave.Media
{
    /// <summary>
    /// Provides available audio channel configurations
    /// </summary>
    [Flags]
    public enum AudioChannelSetup : uint
    {
        /// <summary>
        /// No channels or unknown channel configuration (not combinable with other flags)
        /// </summary>
        None = 0,

        /// <summary>
        /// Mono single channel configuration (not combinable with other flags)
        /// </summary>
        Mono = 1,

        /// <summary>
        /// Stereo configuration (not combinable with other flags)
        /// </summary>
        Stereo = 2,

        /// <summary>Center plus stereo</summary>
        CenterPlusStereo = 3,

        /// <summary>Four corners speakers</summary>
        FourCorners = 4,

        /// <summary>Four corners plus center</summary>
        FivePointZero = 5,

        /// <summary>Four corners plus center plus subwoofer</summary>
        FivePointOne = 6,

        /// <summary>Stereo front, stereo mid, stereo back</summary>
        SevenPointZero = 7,

        /// <summary>Stereo front, stereo mid, stereo back plus subwoofer</summary>
        SevenPointOne = 8,

        /// <summary>Stereo front, stereo mid, stereo back plus two woofers</summary>
        SevenPointTwo = 9,
    }
}
