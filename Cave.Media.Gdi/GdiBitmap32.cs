using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Cave.Media
{
    /// <summary>
    /// Gdi 32 bit argb bitmap functions.
    /// </summary>
    public class GdiBitmap32 : Bitmap32
    {
        Graphics graphics;

        internal Bitmap Bitmap { get; private set; }

        ImageCodecInfo GetEncoder(ImageFormat format)
        {
            foreach (var codec in ImageCodecInfo.GetImageEncoders())
            {
                if (codec.FormatID == format.Guid)
                {
                    return codec;
                }
            }
            throw new ArgumentException(string.Format("Could not find an image encoder for format {0}", format));
        }

        /// <summary>Initializes a new instance of the <see cref="GdiBitmap32"/> class.</summary>
        /// <param name="data"></param>
        public GdiBitmap32(ARGBImageData data)
            : this(data.ToBitmap())
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GdiBitmap32"/> class.</summary>
        /// <param name="bitmap"></param>
        public GdiBitmap32(Bitmap bitmap)
        {
            if (bitmap.PixelFormat != PixelFormat.Format32bppArgb)
            {
                throw new ArgumentException("Invalid image format!");
            }

            Bitmap = bitmap;
            graphics = Graphics.FromImage(bitmap);
            graphics.TextRenderingHint = TextRenderingHint.AntiAlias;
            graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
            graphics.CompositingMode = CompositingMode.SourceOver;
            graphics.CompositingQuality = CompositingQuality.HighQuality;
            graphics.SmoothingMode = SmoothingMode.HighQuality;
        }

        /// <summary>Initializes a new instance of the <see cref="GdiBitmap32"/> class.</summary>
        /// <param name="bitmap"></param>
        public GdiBitmap32(IBitmap32 bitmap)
            : this(GdiBitmap32Extensions.ToGdiBitmap(bitmap))
        {
        }

        /// <summary>Initializes a new instance of the <see cref="GdiBitmap32"/> class.</summary>
        /// <param name="width">Width in pixel.</param>
        /// <param name="height">Height in pixel.</param>
        public GdiBitmap32(int width, int height)
            : this(new Bitmap(width, height, PixelFormat.Format32bppArgb)) => graphics.Clear(Color.Transparent);

        /// <summary>Initializes a new instance of the <see cref="GdiBitmap32"/> class.</summary>
        /// <param name="image"></param>
        public GdiBitmap32(Image image)
            : this(GdiBitmap32Extensions.ToGdiBitmap(image))
        {
        }

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, int x, int y, Translation? translation = null) => Draw(other, x, y, other.Width, other.Height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, int x, int y, int width, int height, Translation? translation = null) => Draw(GdiBitmap32Extensions.ToGdiBitmap(other), x, y, width, height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(Bitmap32 other, float x, float y, float width, float height, Translation? translation = null) => Draw(GdiBitmap32Extensions.ToGdiBitmap(other), x, y, width, height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public override void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null) => Draw(other.ToBitmap32(), x, y, width, height, translation);

        /// <summary>Draws the specified image ontop of this one.</summary>
        /// <param name="other">The image to draw.</param>
        /// <param name="x">The x position.</param>
        /// <param name="y">The y position.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="translation">The translation.</param>
        public virtual void Draw(Bitmap other, float x, float y, float width, float height, Translation? translation = null)
        {
            if (translation.HasValue)
            {
                var mx = Width / 2f;
                var my = Height / 2f;
                graphics.TranslateTransform(mx, my);
                if (translation.Value.Rotation != 0)
                {
                    graphics.RotateTransform(translation.Value.Rotation / (float)Math.PI * 180f);
                }
                if (translation.Value.FlipVertically || translation.Value.FlipHorizontally)
                {
                    graphics.ScaleTransform(translation.Value.FlipHorizontally ? -1 : 1, translation.Value.FlipVertically ? -1 : 1);
                }
                graphics.TranslateTransform(-mx, -my);
            }
            graphics.DrawImage(other, x, y, width, height);
            if (translation.HasValue)
            {
                graphics.ResetTransform();
            }
        }

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="type">The type.</param>
        /// <param name="quality">The quality.</param>
        public override void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100)
        {
            switch (type)
            {
                case ImageType.Png: Save(stream, ImageFormat.Png, quality); break;
                case ImageType.Jpeg: Save(stream, ImageFormat.Jpeg, quality); break;
                default: throw new NotImplementedException();
            }
        }

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        /// <param name="format">The format.</param>
        /// <param name="quality">The quality.</param>
        protected internal void Save(Stream stream, ImageFormat format, int quality)
        {
            var encoder = GetEncoder(format);
            var encoderParams = new EncoderParameters(1);
            encoderParams.Param[0] = new EncoderParameter(Encoder.Quality, quality);
            Bitmap.Save(stream, encoder, encoderParams);
        }

        /// <summary>
        /// Disposes the image.
        /// </summary>
        public override void Dispose()
        {
            base.Dispose();
            graphics?.Dispose();
            graphics = null;
            Bitmap?.Dispose();
            Bitmap = null;
            GC.SuppressFinalize(this);
        }

        /// <summary>Gets the data.</summary>
        /// <value>The data.</value>
        public override ARGBImageData Data => Bitmap.ToARGBImageData();

        /// <summary>Gets the width.</summary>
        /// <value>The width.</value>
        public override int Width => Bitmap.Width;

        /// <summary>Gets the height.</summary>
        /// <value>The height.</value>
        public override int Height => Bitmap.Height;

        /// <summary>Saves the image to the specified stream.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <param name="quality">The quality.</param>
        /// <exception cref="Exception">Invalid extension {extension} use Save(Stream, ImageType, Quality) instead!.</exception>
        public override void Save(string fileName, int quality = 100)
        {
            var extension = Path.GetExtension(fileName).ToLower();
            var type = extension switch
            {
                ".png" => ImageType.Png,
                ".jpg" => ImageType.Jpeg,
                _ => throw new Exception($"Invalid extension {extension} use Save(Stream, ImageType, Quality) instead!"),
            };
            using var file = File.Create(fileName);
            Save(file, type, quality);
        }

        /// <summary>
        /// Clear the image with the specified color.
        /// </summary>
        /// <param name="color"></param>
        public override void Clear(ARGB color) => graphics.Clear(color);
    }
}
