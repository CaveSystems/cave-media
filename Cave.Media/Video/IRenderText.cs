using System;

namespace Cave.Media.Video
{
    /// <summary>
    /// Provides 2D text.
    /// </summary>
    public interface IRenderText : IDisposable
    {
        /// <summary>
        /// Gets the underlying sprite.
        /// </summary>
        IRenderSprite Sprite { get; }

        /// <summary>
        /// Gets or sets name of the font.
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Gets or sets font size in points.
        /// </summary>
        float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Gets or sets foreground color.
        /// </summary>
        ARGB ForeColor { get; set; }

        /// <summary>
        /// Gets or sets background color.
        /// </summary>
        ARGB BackColor { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        BoxAlignment Alignment { get; set; }

        /// <summary>
        /// Updates the texture and scale of the underlying Object3D.
        /// </summary>
        /// <param name="maxWidth">max width in pixels.</param>
        /// <param name="maxHeight">max height in pixels.</param>
        void Update(int maxWidth = 0, int maxHeight = 0);
    }
}
