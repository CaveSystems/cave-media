using System;

namespace Cave.Media
{
	/// <summary>
	/// Modes for resizing images
	/// </summary>
	public enum ResizeMode
	{
		/// <summary>Simple resize, exact fit in bounds</summary>
		None = 0,

		/// <summary>keep aspect, touch from insize</summary>
		TouchFromInside = 1,

        /// <summary>keep aspect, touch from outsize</summary>
        TouchFromOutside = 2,
    }
}
