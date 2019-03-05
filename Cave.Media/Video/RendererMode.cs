using System;

namespace Cave.Media.Video
{
    /// <summary>
    /// provides modes for renderer implementations.
    /// </summary>
    [Flags]
    public enum RendererMode
    {
        /// <summary>
        /// Full screen
        /// </summary>
        FullScreen = 0,

        /// <summary>
        /// Full screen within a window without window decoration
        /// </summary>
        WindowedFullScreen = 1,

        /// <summary>
        /// Normal OS Window
        /// </summary>
        Window = 2,
    }
}
