﻿using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Cave.Media;

/// <summary>Gdi 32 bit argb bitmap functions.</summary>
public class GdiBitmap32 : Bitmap32
{
    #region Private Fields

    bool disposed;
    Graphics graphics;

    #endregion Private Fields

    #region Private Methods

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

    #endregion Private Methods

    #region Protected Internal Methods

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

    #endregion Protected Internal Methods

    #region Internal Properties

    internal Bitmap Bitmap { get; }

    #endregion Internal Properties

    #region Public Constructors

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

    #endregion Public Constructors

    #region Public Properties

    /// <inheritdoc/>
    public override int Height => Bitmap.Height;

    /// <inheritdoc/>
    public override int Width => Bitmap.Width;

    #endregion Public Properties

    #region Public Methods

    /// <inheritdoc/>
    public override void Clear(ARGB color) => graphics.Clear(color);

    /// <inheritdoc/>
    public override void Dispose()
    {
        if (!disposed)
        {
            disposed = true;
            graphics?.Dispose();
            Bitmap?.Dispose();
            GC.SuppressFinalize(this);
        }
    }

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, int x, int y, Translation? translation = null) => Draw(other, x, y, other.Width, other.Height, translation);

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, int x, int y, int width, int height, Translation? translation = null) => Draw(GdiBitmap32Extensions.ToGdiBitmap(other), x, y, width, height, translation);

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, float x, float y, float width, float height, Translation? translation = null) => Draw(GdiBitmap32Extensions.ToGdiBitmap(other), x, y, width, height, translation);

    /// <inheritdoc/>
    public override void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null) => Draw(other.ToBitmap32(), x, y, width, height, translation);

    /// <inheritdoc/>
    public virtual void Draw(Bitmap other, float x, float y, float width, float height, Translation? translation = null)
    {
        if (disposed) throw new ObjectDisposedException(nameof(GdiBitmap32));
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

    /// <inheritdoc/>
    public override ARGBImageData GetImageData() => Bitmap.ToARGBImageData();

    /// <inheritdoc/>
    public override void MakeTransparent() => Bitmap.MakeTransparent();

    /// <inheritdoc/>
    public override void MakeTransparent(ARGB color) => Bitmap.MakeTransparent(color);

    /// <inheritdoc/>
    public override void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100)
    {
        if (disposed) throw new ObjectDisposedException(nameof(GdiBitmap32));
        switch (type)
        {
            case ImageType.Png: Save(stream, ImageFormat.Png, quality); break;
            case ImageType.Jpeg: Save(stream, ImageFormat.Jpeg, quality); break;
            default: throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
    public override void Save(string fileName, int quality = 100)
    {
        if (disposed) throw new ObjectDisposedException(nameof(GdiBitmap32));
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

    #endregion Public Methods
}
