using System;
using System.Collections.Generic;
using System.IO;

namespace Cave.Media
{
    /// <summary>
    /// Provides platform independant 32 bit argb bitmap image manipulation.
    /// </summary>
    public interface IBitmap32 : IDisposable
    {
        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        int Height { get; }

        /// <summary>Gets the width.</summary>
        /// <value>The width.</value>
        int Width { get; }

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        ARGBImageData Data { get; }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="translation">The translation.</param>
        void Draw(Bitmap32 other, int x, int y, Translation? translation = null);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        void Draw(Bitmap32 other, int x, int y, int width, int height, Translation? translation = null);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        void Draw(Bitmap32 other, float x, float y, float width, float height, Translation? translation = null);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null);

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <param name="quality">The quality.</param>
        void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100);

        /// <summary>Saves the image to the specified file.</summary>
        /// <param name="filename">The filename.</param>
        /// <param name="quality">The quality.</param>
        void Save(string filename, int quality = 100);

        /// <summary>
        /// Resizes the image.
        /// </summary>
        /// <param name="width">new width.</param>
        /// <param name="height">new height.</param>
        /// <param name="mode">the resize mode.</param>
        Bitmap32 Resize(int width, int height, ResizeMode mode = 0);

        /// <summary>Detects the most common colors.</summary>
        /// <param name="max">The maximum number of colors to retrieve.</param>
        /// <returns>Returns an array of <see cref="ARGB"/> values.</returns>
        IList<ARGB> DetectColors(int max);

        /// <summary>
        /// Clear the image with the specified color.
        /// </summary>
        /// <param name="color"></param>
        void Clear(ARGB color);

        /// <summary>Makes the bitmap transparent by blending the color of the top left pixel</summary>
        void MakeTransparent();

        /// <summary>Sets the specified color to transparent.</summary>
        /// <param name="color">Color to set transparent.</param>
        void MakeTransparent(ARGB color);
    }
}
