using System;

namespace Cave.Media
{
    /// <summary>
    /// Flags for resizing images.
    /// </summary>
    [Flags]
    public enum ResizeFlags
    {
        /// <summary>Simple resize and rouch from outside</summary>
        Default = 0,

        /// <summary>keep aspect</summary>
        KeepAspect = 1,

        /// <summary>touch from insize</summary>
        TouchFromInsize = 2,

        /// <summary>The keep aspect and touch from insize</summary>
        KeepAspectTouchFromInsize = KeepAspect | TouchFromInsize,
    }
}
