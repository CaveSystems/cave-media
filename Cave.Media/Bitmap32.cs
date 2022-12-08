using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cave.Media
{
    /// <summary>
    /// Provides platform independent 32 bit argb bitmap functions.
    /// </summary>
    public class Bitmap32 : IDisposable, IBitmap32
    {
        static IBitmap32Loader loader;

        /// <summary>
        /// Provides the default loader.
        /// </summary>
        public static IBitmap32Loader Loader { get => loader ??= FindLoader(); set => loader = value ?? throw new ArgumentNullException(nameof(value)); }

        static IBitmap32Loader FindLoader()
        {
            var loaders = AppDom.GetInstances<IBitmap32Loader>(true);
            return loaders.FirstOrDefault();
        }

        IBitmap32 bitmap;

        /// <summary>Creates a bitmap instance from the specified stream.</summary>
        public static Bitmap32 FromStream(Stream stream)
        {
            if (Loader == null)
            {
                throw new Exception("No valid IBitmap32Loader found!");
            }

            return Loader.FromStream(stream);
        }

        /// <summary>Creates a bitmap instance from the specified file.</summary>
        public static Bitmap32 FromFile(string fileName)
        {
            if (Loader == null)
            {
                throw new Exception("No valid IBitmap32Loader found!");
            }

            return Loader.FromFile(fileName);
        }

        /// <summary>Creates a bitmap instance from the specified data.</summary>
        public static Bitmap32 Create(byte[] data)
        {
            if (Loader == null)
            {
                throw new Exception("No valid IBitmap32Loader found!");
            }

            return Loader.Create(data);
        }

        /// <summary>Creates a bitmap instance from the specified data.</summary>
        public static Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text)
        {
            if (Loader == null)
            {
                throw new Exception("No valid IBitmap32Loader found!");
            }

            return Loader.Create(fontName, fontSize, foreColor, backColor, text);
        }

        /// <summary>
        /// Creates an empty instance.
        /// </summary>
        protected Bitmap32() { }

        /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
        /// <param name="bitmap"></param>
        public Bitmap32(IBitmap32 bitmap)
        {
            this.bitmap = bitmap;
        }

        /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
        /// <param name="width">Width in pixel.</param>
        /// <param name="height">Height in pixel.</param>
        public Bitmap32(int width, int height)
        {
            if (Loader == null)
            {
                throw new Exception("No valid IBitmap32Loader found!");
            }

            bitmap = Loader.Create(width, height);
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="translation">The translation.</param>
        public virtual void Draw(Bitmap32 other, int x, int y, Translation? translation = null) => bitmap.Draw(other, x, y, other.Width, other.Height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public virtual void Draw(Bitmap32 other, int x, int y, int width, int height, Translation? translation = null) => bitmap.Draw(other, x, y, width, height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public virtual void Draw(Bitmap32 other, float x, float y, float width, float height, Translation? translation = null) => bitmap.Draw(other, x, y, width, height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public virtual void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null) => bitmap.Draw(other, x, y, width, height, translation);

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <param name="quality">The quality.</param>
        /// <exception cref="NotImplementedException"></exception>
        public virtual void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100) => bitmap.Save(stream, type, quality);

        /// <summary>
        /// Clear the image with the specified color.
        /// </summary>
        /// <param name="color">The color.</param>
        public virtual void Clear(ARGB color) => bitmap.Clear(color);

        /// <summary>Resizes the bitmap to the specified size.</summary>
        /// <remarks>This should be overloaded to speed up the resize.</remarks>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="mode">The resize mode.</param>
        /// <returns></returns>
        public virtual Bitmap32 Resize(int width, int height, ResizeMode mode = 0)
        {
            var result = new Bitmap32(width, height);
            float w = width;
            float h = height;
            if (mode != ResizeMode.None)
            {
                var fw = w / (float)Width;
                var fh = h / (float)Height;
                float f;
                if (mode.HasFlag(ResizeMode.TouchFromInside))
                {
                    f = Math.Min(fw, fh);
                }
                else
                {
                    f = Math.Max(fw, fh);
                }
                w = Width * f;
                h = Height * f;
            }
            var x = (width - w) / 2;
            var y = (height - h) / 2;
            result.Draw(this, x, y, w, h);
            return result;
        }

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="quality">The quality.</param>
        /// <exception cref="Exception">Invalid extension {extension} use Save(Stream, ImageType, Quality) instead!.</exception>
        public virtual void Save(string fileName, int quality = 100) => bitmap.Save(fileName, quality);

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        public virtual ARGBImageData Data => bitmap.Data;

        /// <summary>Gets the width.</summary>
        /// <value>The width.</value>
        public virtual int Width => bitmap.Width;

        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        public virtual int Height => bitmap.Height;

        /// <summary>
        /// Disposes the image.
        /// </summary>
        public virtual void Dispose()
        {
            bitmap?.Dispose();
            bitmap = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>Detects the most common colors.</summary>
        /// <param name="max">The maximum number of colors to retrieve.</param>
        /// <returns>Returns an array of <see cref="T:Cave.Media.ARGB" /> values.</returns>
        public IList<ARGB> DetectColors(int max)
        {
            if ((Width + Height) / 2 > max)
            {
                using var bmp = Resize(max, max, ResizeMode.None);
                return bmp.DetectColors(max);
            }

            var colorCounters = new List<ColorCounter>();
            for (var y = 0; y < Height; y++)
            {
                var data = Data.Data;
                var colorDict = new Dictionary<ARGB, ColorCounter>();
                unsafe
                {
                    fixed (int* p = &data[0])
                    {
                        for (var i = 0; i < data.Length; i++)
                        {
                            ARGB color = p[i];
                            if (!colorDict.ContainsKey(color))
                            {
                                colorDict.Add(color, new ColorCounter(color, 1));
                            }
                            else
                            {
                                colorDict[color].Count++;
                            }
                        }
                    }
                }
                colorCounters.AddRange(colorDict.Values);
            }
            uint distance = 255;
            while (colorCounters.Count > max)
            {
                colorCounters.Sort();
                colorCounters = ColorCounter.Reduce(colorCounters, ref distance);
                distance += 255;
            }
            colorCounters.Sort();
            var colors = new List<ARGB>(colorCounters.Count);
            for (var i = 0; i < colorCounters.Count; i++)
            {
                colors.Add(colorCounters[i].Color);
            }

            return colors;
        }
    }
}
