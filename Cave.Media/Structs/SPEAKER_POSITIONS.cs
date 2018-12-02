using System;

namespace Cave.Media.Structs
{
    /// <summary>
    /// Provides common speaker positions
    /// </summary>
    [Flags]
    public enum SPEAKER_POSITIONS : uint
    {
        /// <summary>
        /// front left speaker
        /// </summary>
        FRONT_LEFT = 0x1,
        /// <summary>
        /// front right speaker
        /// </summary>
        FRONT_RIGHT = 0x2,
        /// <summary>
        /// front center speaker
        /// </summary>
        FRONT_CENTER = 0x4,
        /// <summary>
        /// low frequency surround speaker (no position)
        /// </summary>
        LOW_FREQUENCY = 0x8,
        /// <summary>
        /// back left speaker
        /// </summary>
        BACK_LEFT = 0x10,
        /// <summary>
        /// back right speaker
        /// </summary>
        BACK_RIGHT = 0x20,
        /// <summary>
        /// front left of center speaker
        /// </summary>
        FRONT_LEFT_OF_CENTER = 0x40,
        /// <summary>
        /// front right of center speaker
        /// </summary>
        FRONT_RIGHT_OF_CENTER = 0x80,
        /// <summary>
        /// back center speaker
        /// </summary>
        BACK_CENTER = 0x100,
        /// <summary>
        /// side left speaker
        /// </summary>
        SIDE_LEFT = 0x200,
        /// <summary>
        /// side right speaker
        /// </summary>
        SIDE_RIGHT = 0x400,
        /// <summary>
        /// top center speaker
        /// </summary>
        TOP_CENTER = 0x800,
        /// <summary>
        /// top front left speaker
        /// </summary>
        TOP_FRONT_LEFT = 0x1000,
        /// <summary>
        /// top front center speaker
        /// </summary>
        TOP_FRONT_CENTER = 0x2000,
        /// <summary>
        /// top front right speaker
        /// </summary>
        TOP_FRONT_RIGHT = 0x4000,
        /// <summary>
        /// top back left speaker
        /// </summary>
        TOP_BACK_LEFT = 0x8000,
        /// <summary>
        /// top back center speaker
        /// </summary>
        TOP_BACK_CENTER = 0x10000,
        /// <summary>
        /// top back right speaker
        /// </summary>
        TOP_BACK_RIGHT = 0x20000,

        /// <summary>
        /// bottom center speaker
        /// </summary>
        BOTTOM_CENTER = 0x40000,
        /// <summary>
        /// bottom front left speaker
        /// </summary>
        BOTTOM_FRONT_LEFT = 0x80000,
        /// <summary>
        /// bottom front center speaker
        /// </summary>
        BOTTOM_FRONT_CENTER = 0x100000,
        /// <summary>
        /// bottom front right speaker
        /// </summary>
        BOTTOM_FRONT_RIGHT = 0x200000,
        /// <summary>
        /// bottom back left speaker
        /// </summary>
        BOTTOM_BACK_LEFT = 0x400000,
        /// <summary>
        /// bottom back center speaker
        /// </summary>
        BOTTOM_BACK_CENTER = 0x800000,
        /// <summary>
        /// bottom back right speaker
        /// </summary>
        BOTTOM_BACK_RIGHT = 0x10000000,

        /// <summary>
        /// Used to specify that any possible permutation of speaker configurations
        /// </summary>
        ALL = 0x80000000,
    }
}
