
using System;

namespace Cave.Media
{
    /// <summary>
    /// Provides an interface for video frame informations.
    /// </summary>
    public interface IVideoFrame
    {
        /// <summary>
        /// Stream index (&lt;0 = invalid or unknown index)<br/>
        /// If the frame source supports only one stream this is always set to 0.
        /// </summary>
        int StreamIndex { get; }

        /// <summary>
        /// The argb data of the frame.
        /// </summary>
        ARGBImageData Image { get; }

        /// <summary>
        /// Obtains the display time.
        /// </summary>
        TimeSpan DisplayTime { get; }
    }
}
