using System;
namespace Cave.Media.Video
{
    /// <summary>
    /// Provides available aspect correction modes
    /// </summary>
    [Flags]
    public enum AspectCorrectionMode
    {
        /// <summary>
        /// no correction
        /// </summary>
        None = 0x00,

        /// <summary>
        /// touch window from inner side, scale down (screen is completely visible and borders are visible)
        /// </summary>
        TouchInner = 0x01,

        /// <summary>
        /// touch window from outer side, scale up (parts of the screen will be clipped)
        /// </summary>
        TouchOuter = 0x02,

    }
}
