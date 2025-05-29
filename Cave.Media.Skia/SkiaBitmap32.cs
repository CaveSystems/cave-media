using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using SkiaSharp;

namespace Cave.Media;

/// <summary>Provides platform independent 32 bit argb bitmap functions</summary>
/// <seealso cref="System.IDisposable"/>
/// <seealso cref="Cave.Media.IBitmap32"/>
public class SkiaBitmap32 : Bitmap32
{
    #region Private Fields

    SKBitmap? skBitmap;

    #endregion Private Fields

    #region Private Methods

    SKCanvas GetCanvas() => new SKCanvas(SKBitmap);

    #endregion Private Methods

    #region Protected Properties

    protected SKBitmap SKBitmap => skBitmap ?? throw new ObjectDisposedException(nameof(SkiaBitmap32));

    #endregion Protected Properties

    #region Public Constructors

    /// <summary>Creates a new SkiaBitmap</summary>
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
        skBitmap = bitmap ?? throw new ArgumentNullException(nameof(bitmap));
    }

    /// <summary>Initializes a new instance of the <see cref="SkiaBitmap32"/> class.</summary>
    /// <param name="data">The data.</param>
    public SkiaBitmap32(ARGBImageData data)
    {
        skBitmap = data.ToSKBitmap();
    }

    /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public SkiaBitmap32(int width, int height)
    {
        skBitmap = new SKBitmap(width, height);
        skBitmap.Erase(new SKColor(0));
    }

    #endregion Public Constructors

    #region Public Properties

    /// <inheritdoc/>
    public override int Height => SKBitmap.Height;

    /// <inheritdoc/>
    public override int Width => SKBitmap.Width;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Converts a bitmap</summary>
    /// <param name="bitmap"></param>
    /// <returns></returns>
    public static SKBitmap Convert(IBitmap32 bitmap)
    {
        if (bitmap is SkiaBitmap32 skiaBitmap32) return skiaBitmap32.SKBitmap;
        using var ms = new MemoryStream();
        bitmap.Save(ms);
        ms.Position = 0;
        return SKBitmap.Decode(ms);
    }

    /// <inheritdoc/>
    public override void Clear(ARGB color)
    {
        using var canvas = GetCanvas();
        canvas.Clear(new SKColor(color.AsUInt32));
    }

    /// <inheritdoc/>
    public override void Dispose()
    {
        skBitmap?.Dispose();
        skBitmap = null;
    }

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, int x, int y, Translation? translation = null) => Draw(other, x, y, other.Width, other.Height, translation);

    /// <inheritdoc/>
    public override void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null) => Draw(other.ToSKBitmap(), x, y, width, height, translation);

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, int x, int y, int width, int height, Translation? translation = null) => Draw(Convert(other), x, y, width, height, translation);

    /// <inheritdoc/>
    public override void Draw(IBitmap32 other, float x, float y, float width, float height, Translation? translation = null) => Draw(Convert(other), x, y, width, height, translation);

    /// <summary>Draws the specified image ontop of this one.</summary>
    /// <param name="other">The image to draw.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="translation">The translation.</param>
    public void Draw(SKBitmap other, float x, float y, float width, float height, Translation? translation = null)
    {
        using var canvas = GetCanvas();
        using var paint = new SKPaint() { BlendMode = SKBlendMode.SrcOver, FilterQuality = SKFilterQuality.High, };
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
        if (translation.HasValue) canvas.ResetMatrix();
    }

    /// <inheritdoc/>
    public override ARGBImageData GetImageData() => SKBitmap.ToARGBImageData();

    /// <inheritdoc/>
    public override void MakeTransparent()
    {
        if (skBitmap is null) throw new ObjectDisposedException(nameof(SkiaBitmap32));
        var result = new SKBitmap(skBitmap.Width, skBitmap.Height);
        using (var canvas = new SKCanvas(result))
        using (var paint = new SKPaint())
        using (var colorFilter = SKColorFilter.CreateBlendMode(skBitmap.GetPixel(0, 0), SKBlendMode.DstIn))
        {
            paint.ColorFilter = colorFilter;
            canvas.DrawBitmap(skBitmap, 0, 0, paint);
        }
        skBitmap.Dispose();
        skBitmap = result;
    }

    /// <inheritdoc/>
    public override void MakeTransparent(ARGB color)
    {
        if (skBitmap is null) throw new ObjectDisposedException(nameof(SkiaBitmap32));
        var result = new SKBitmap(skBitmap.Width, skBitmap.Height);
        using (var canvas = new SKCanvas(result))
        using (var paint = new SKPaint())
        using (var colorFilter = SKColorFilter.CreateBlendMode(new SKColor(color.AsUInt32), SKBlendMode.DstIn))
        {
            paint.ColorFilter = colorFilter;
            canvas.DrawBitmap(skBitmap, 0, 0, paint);
        }
        skBitmap.Dispose();
        skBitmap = result;
    }

    /// <inheritdoc/>
    public override void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100)
    {
        switch (type)
        {
            case ImageType.Jpeg: SKBitmap.Save(stream, SKEncodedImageFormat.Jpeg, quality); break;
            case ImageType.Png: SKBitmap.Save(stream, SKEncodedImageFormat.Png, quality); break;
            default: throw new NotImplementedException();
        }
    }

    /// <inheritdoc/>
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
        using var file = File.Create(fileName); Save(file, type, quality);
    }

    #endregion Public Methods
}
