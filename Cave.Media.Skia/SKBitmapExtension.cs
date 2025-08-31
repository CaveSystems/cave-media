using SkiaSharp;
using System;
using System.IO;

namespace Cave.Media;

/// <summary>Provides extensions for <see cref="SKBitmap"/> and <see cref="SKImage"/> instances.</summary>
public static class SKBitmapExtension
{
    #region Public Methods

    /// <summary>Saves the specified file name.</summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="format">The format.</param>
    /// <param name="quality">The quality.</param>
    public static void Save(this SKBitmap bitmap, string fileName, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        using var img = SKImage.FromBitmap(bitmap);
        Save(img, fileName, format, quality);
    }

    /// <summary>Saves the specified stream.</summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="format">The format.</param>
    /// <param name="quality">The quality.</param>
    public static void Save(this SKBitmap bitmap, Stream stream, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        using var img = SKImage.FromBitmap(bitmap);
        Save(img, stream, format, quality);
    }

    /// <summary>Saves the specified file name.</summary>
    /// <param name="image">The image.</param>
    /// <param name="fileName">Name of the file.</param>
    /// <param name="format">The format.</param>
    /// <param name="quality">The quality.</param>
    public static void Save(this SKImage image, string fileName, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        using var file = File.Create(fileName);
        Save(image, file, format, quality);
    }

    /// <summary>Saves the specified stream.</summary>
    /// <param name="image">The image.</param>
    /// <param name="stream">The stream.</param>
    /// <param name="format">The format.</param>
    /// <param name="quality">The quality.</param>
    /// <exception cref="ArgumentOutOfRangeException">quality</exception>
    public static void Save(this SKImage image, Stream stream, SKEncodedImageFormat format = SKEncodedImageFormat.Png, int quality = 100)
    {
        if (quality < 1 || quality > 100) throw new ArgumentOutOfRangeException(nameof(quality));
        using var data = image.Encode(format, quality);
        data.SaveTo(stream);
    }

    #endregion Public Methods
}
