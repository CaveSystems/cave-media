using System;
using System.IO;
using System.Threading;
using SkiaSharp;

namespace Cave.Media
{
    /// <summary>
    /// Provides platform independent 32 bit argb bitmap functions
    /// </summary>
    /// <seealso cref="System.IDisposable" />
    /// <seealso cref="Cave.Media.IBitmap32" />
    public class SkiaBitmap32 : Bitmap32
    {
        SKBitmap bitmap;
        SKCanvas canvas;
        Thread canvasThread;

        /// <summary>
        /// Converts a bitmap
        /// </summary>
        /// <param name="bitmap"></param>
        /// <returns></returns>
        public static SKBitmap Convert(IBitmap32 bitmap)
        {
            if (bitmap is SkiaBitmap32) return ((SkiaBitmap32)bitmap).bitmap;
            using (var ms = new MemoryStream())
            {
                bitmap.Save(ms);
                ms.Position = 0;
                return SKBitmap.Decode(ms);
            }
        }

        /// <summary>
        /// Creates a new SkiaBitmap
        /// </summary>
        /// <param name="bitmap"></param>
        public SkiaBitmap32(IBitmap32 bitmap)
            : this(Convert(bitmap))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
        /// <param name="bitmap">The bitmap.</param>
        public SkiaBitmap32(SKBitmap bitmap)
            : base()
        {
            if (bitmap.ColorType != SKImageInfo.PlatformColorType)
            {
                //change colortype
                var bmp = new SKBitmap(bitmap.Width, bitmap.Height, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul);
                bitmap.CopyTo(bmp, SKImageInfo.PlatformColorType);
                bitmap = bmp;
            }
            this.bitmap = bitmap ?? throw new ArgumentNullException("bitmap");
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="SkiaBitmap32"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        public SkiaBitmap32(ARGBImageData data)
        {
            bitmap = data.ToSKBitmap();
        }

        /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public SkiaBitmap32(int width, int height)
        {
            bitmap = new SKBitmap(width, height);
            bitmap.Erase(new SKColor(0));
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, int x, int y, Translation? translation = null)
        {
            Draw(other, x, y, other.Width, other.Height, translation);
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null)
        {
            Draw(other.ToSKBitmap(), x, y, width, height, translation);
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, int x, int y, int width, int height, Translation? translation = null)
        {
            Draw(Convert(other), x, y, width, height, translation);
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, float x, float y, float width, float height, Translation? translation = null)
        {
            Draw(Convert(other), x, y, width, height, translation);
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public void Draw(SKBitmap other, float x, float y, float width, float height, Translation? translation = null)
        {
            if (canvas == null)
            {
                canvas = new SKCanvas(bitmap);
                canvasThread = Thread.CurrentThread;
            }
            else
            {
                if (Thread.CurrentThread != canvasThread) throw new InvalidOperationException("Canvas Thread Boundary Incursion!");
            }
            using (var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver, FilterQuality = SKFilterQuality.High, })
            {
                if (translation.HasValue)
                {
                    var mx = Width / 2f;
                    var my = Height / 2f;
                    if (translation.Value.Rotation != 0)
                    {
                        canvas.RotateRadians(translation.Value.Rotation, other.Width / 2, other.Height / 2);
                    }
                    if (translation.Value.FlipHorizontally)
                    {
                        canvas.Scale(-1, 1);
                        canvas.Translate(-Width, 0);
                    }
                    if (translation.Value.FlipVertically)
                    {
                        canvas.Scale(1, -1);
                        canvas.Translate(1, -Height);
                    }
                }
                canvas.DrawBitmap(other, SKRect.Create(x, y, width, height), paint);
            }
            if (translation.HasValue) canvas.ResetMatrix();
        }

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <param name="quality">The quality.</param>
        /// <exception cref="NotImplementedException"></exception>
        public override void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100)
        {
            switch (type)
            {
                case ImageType.Jpeg: bitmap.Save(stream, SKEncodedImageFormat.Jpeg, quality); break;
                case ImageType.Png: bitmap.Save(stream, SKEncodedImageFormat.Png, quality); break;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public override void Dispose()
        {
            canvas?.Dispose(); canvas = null;
            bitmap?.Dispose(); bitmap = null;
            base.Dispose();
        }

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        public override ARGBImageData Data => bitmap.ToARGBImageData();

        /// <summary>Gets the width.</summary>
        /// <value>The width.</value>
        public override int Width => bitmap.Width;

        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        public override int Height => bitmap.Height;

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="quality">The quality.</param>
        /// <exception cref="Exception">Invalid extension {extension} use Save(Stream, ImageType, Quality) instead!</exception>
        public override void Save(string fileName, int quality = 100)
        {
            ImageType type;
            var extension = Path.GetExtension(fileName).ToLower();
            switch (extension)
            {
                case ".png": type = ImageType.Png; break;
                case ".jpg": type = ImageType.Jpeg; break;
                default: throw new Exception($"Invalid extension {extension} use Save(Stream, ImageType, Quality) instead!");
            }
            using (var file = File.Create(fileName)) Save(file, type, quality);
        }

        /// <summary>
        /// Clear the image with the specified color
        /// </summary>
        /// <param name="color"></param>
        public override void Clear(ARGB color)
        {
            canvas.Clear(new SKColor(color.AsUInt32));
        }

        /// <inheritdoc />
        public override void MakeTransparent(ARGB color)
        {
            var result = new SKBitmap(bitmap.Width, bitmap.Height);
            using (var canvas = new SKCanvas(result))
            using (var paint = new SKPaint())
            using (var colorFilter = SKColorFilter.CreateBlendMode(new SKColor(color.AsUInt32), SkiaSharp.SKBlendMode.DstIn))
            {
                paint.ColorFilter = colorFilter;
                canvas.DrawBitmap(bitmap, 0, 0, paint);
            }
            bitmap.Dispose();
            bitmap = result;
        }
    }
}
