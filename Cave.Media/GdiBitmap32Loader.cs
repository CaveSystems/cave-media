#if NET20 || NET35 || NET40 || NET45 || NET46 || NET471

using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Text;
using Cave.Media.Video;

namespace Cave.Media
{
    /// <summary>
    /// Provides gdi 32 bit argb bitmap functions.
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Cave.Media.IBitmap32" />
    public class GdiBitmap32Loader : IBitmap32Loader
    {
        /// <summary>Creates a bitmap instance from the specified data.</summary>
        /// <param name="data">bitmap as byte array.</param>
        /// <returns>new bitmap32.</returns>
        public Bitmap32 Create(byte[] data)
        {
            using (var ms = new MemoryStream(data))
            {
                return FromStream(ms);
            }
        }

        /// <summary>Creates a new bitmap instance.</summary>
        /// <param name="width">the height in pixels.</param>
        /// <param name="height">the width in pixels.</param>
        /// <returns>a new bitmap32 with the given size.</returns>
        public Bitmap32 Create(int width, int height)
        {
            return new GdiBitmap32(width, height);
        }

        /// <summary>Creates a bitmap instance from the specified data.</summary>
        /// <param name="data">the pixel data.</param>
        /// <returns>a new bitmap32.</returns>
        public Bitmap32 Create(ARGBImageData data)
        {
            return new GdiBitmap32(data);
        }

        /// <summary>
        /// Creates a new bitmap instance.
        /// </summary>
        /// <param name="fontName">Name of the font.</param>
        /// <param name="fontSize">Size in points.</param>
        /// <param name="foreColor">ForeColor.</param>
        /// <param name="backColor">BackColor.</param>
        /// <param name="text">text to draw.</param>
        /// <returns>a new bitmap32 with the text on it.</returns>
        public Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text)
        {
            SizeF size;
            using (var b = new Bitmap(1, 1))
            {
                float emSize = fontSize / 4f * 3f;
                var font = fontName == null ? new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Point) : new Font(fontName, fontSize, GraphicsUnit.Point);
                using (font)
                {
                    using (var g = Graphics.FromImage(b))
                    {
                        size = g.MeasureString(text, font);
                    }
                    Bitmap result = new Bitmap((int)size.Width + 1, (int)size.Height + 1, PixelFormat.Format32bppArgb);
                    using (Graphics g = Graphics.FromImage(result))
                    {
                        g.TextRenderingHint = TextRenderingHint.AntiAlias;
                        g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                        g.CompositingMode = CompositingMode.SourceOver;
                        g.CompositingQuality = CompositingQuality.HighQuality;
                        g.SmoothingMode = SmoothingMode.HighQuality;
                        g.Clear(backColor);
                        g.DrawString(text, font, new SolidBrush(foreColor), 0, 0);
                    }
                    return new GdiBitmap32(result);
                }
            }
        }

        /// <summary>Creates a bitmap instance from the specified file.</summary>
        /// <param name="fileName">name of the file to load the bitmap from.</param>
        /// <returns>the loaded bitmap.</returns>
        public Bitmap32 FromFile(string fileName)
        {
            return new GdiBitmap32(Image.FromFile(fileName));
        }

        /// <summary>Creates a bitmap instance from the specified stream.</summary>
        /// <param name="stream">stream to load the bitmap from.</param>
        /// <returns>the loaded bitmap.</returns>
        public Bitmap32 FromStream(Stream stream)
        {
            return new GdiBitmap32(Image.FromStream(stream));
        }
    }
}

#endif
