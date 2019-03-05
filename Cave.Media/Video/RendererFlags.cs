using System;

namespace Cave.Media.Video
{
    /// <summary>
    /// provides flags for renderer implementations.
    /// </summary>
    [Flags]
    public enum RendererFlags
    {
        /// <summary>no flags / default</summary>
        None = 0,

        /// <summary>wait for retrace</summary>
        WaitRetrace = 1,
    }
}
