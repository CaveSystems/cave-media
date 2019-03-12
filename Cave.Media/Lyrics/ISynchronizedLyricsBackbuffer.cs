using System.Drawing;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides a backbuffer for synchronized lyrics display.
    /// </summary>
    public interface ISynchronizedLyricsBackbuffer
    {
        /// <summary>Gets or sets the global alpha value (0 = transparent, 255 = opaque).</summary>
        /// <value>The alpha.</value>
        byte GlobalAlpha { get; set; }

#if NET20 || NET35 || NET40 || !SKIA
#elif SKIA && (NETSTANDARD20 || NET45 || NET46 || NET471)
        /// <summary>Gets the current backbuffer.</summary>
        /// <value>The backbuffer.</value>
        /// <remarks>
        /// Do not dispose this bitmap. The bitmap will be automatically replaced, disposed, updated, whenever <see cref="Play(ISynchronizedLyricsCommand)" /> is called.
        /// Do not save references to this across play calls!
        /// </remarks>
        SkiaSharp.SKBitmap ToSKBitmap();
#else
#error No code defined for the current framework or NETXX version define missing!
#endif

#if NETSTANDARD20
#elif NET20 || NET35 || NET40 || NET45 || NET46 || NET47
        /// <summary>Gets the current backbuffer.</summary>
        /// <value>The backbuffer.</value>
        /// <remarks>
        /// Do not dispose this bitmap. The bitmap will be automatically replaced, disposed, updated, whenever <see cref="Play(ISynchronizedLyricsCommand)" /> is called.
        /// Do not save references to this across play calls!
        /// </remarks>
        System.Drawing.Bitmap ToBitmap();
#else
#error No code defined for the current framework or NETXX version define missing!
#endif

        /// <summary>Gets the size of the screen.</summary>
        /// <value>The size of the screen.</value>
        Size ScreenSize { get; }

        /// <summary>Gets or sets a value indicating whether [to use transparent color override].</summary>
        /// <value>
        /// <c>true</c> if [transparent color override]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Transparent color override replaces all background colors with the value specified by <see cref="TransparentColorValue" />.
        /// </remarks>
        bool TransparentColorOverride { get; set; }

        /// <summary>Gets or sets the transparent color used in override mode.</summary>
        /// <value>The transparent color value in override mode.</value>
        ARGB TransparentColorValue { get; set; }

        /// <summary>Gets a value indicating whether this <see cref="ISynchronizedLyricsBackbuffer"/> was updated by a play command.</summary>
        /// <value><c>true</c> if updated; otherwise, <c>false</c>.</value>
        bool Updated { get; }

        /// <summary>Plays the specified <see cref="SynchronizedLyricsItem"/>.</summary>
        /// <remarks>This function calls <see cref="Play(ISynchronizedLyricsCommand)"/> for all commands present at the specified item.</remarks>
        /// <param name="item">The item.</param>
        void Play(SynchronizedLyricsItem item);

        /// <summary>Plays the specified command.</summary>
        /// <param name="command">The command.</param>
        void Play(ISynchronizedLyricsCommand command);
    }
}