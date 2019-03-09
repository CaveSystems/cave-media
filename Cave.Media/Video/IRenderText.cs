using System;

namespace Cave.Media.Video
{
    /// <summary>
    /// Provides 2D text.
    /// </summary>
    public interface IRenderText : IDisposable
    {
        /// <summary>
        /// The underlying sprite.
        /// </summary>
        IRenderSprite Sprite { get; }

        /// <summary>
        /// Name of the font.
        /// </summary>
        string FontName { get; set; }

        /// <summary>
        /// Font size in points.
        /// </summary>
        float FontSize { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        string Text { get; set; }

        /// <summary>
        /// Foreground color to use.
        /// </summary>
        ARGB ForeColor { get; set; }

        /// <summary>
        /// Background color to use.
        /// </summary>
        ARGB BackColor { get; set; }

        /// <summary>
        /// Gets/sets the text alignment.
        /// </summary>
        BoxAlignment Alignment { get; set; }

        /// <summary>
        /// Updates the texture and scale of the underlying Object3D.
        /// </summary>
        void Update();
    }
}
