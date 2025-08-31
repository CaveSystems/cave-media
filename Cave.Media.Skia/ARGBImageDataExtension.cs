using System;
using SkiaSharp;

namespace Cave.Media;

/// <summary>Provides extensions for <see cref="ARGBImageData"/></summary>
public static class ARGBImageDataExtension
{
    #region Public Methods

    /// <summary>Loads the specified bitmap.</summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <returns></returns>
    public static ARGBImageData ToARGBImageData(this SKBitmap bitmap)
    {
        if (bitmap.ColorType != SKColorType.Rgba8888 || bitmap.AlphaType != SKAlphaType.Unpremul)
        {
            bitmap = new SKBitmap(bitmap.Width, bitmap.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
        }
        return new ARGBImageData(bitmap.GetPixels(), bitmap.RowBytes * bitmap.Height, bitmap.Width, bitmap.Height, bitmap.RowBytes);
    }

    /// <summary>Writes all data to a new <see cref="SKBitmap"/> instance</summary>
    /// <returns></returns>
    /// <exception cref="Exception">Invalid length!</exception>
    public static unsafe SKBitmap ToSKBitmap(this ARGBImageData imageData)
    {
        var bitmap = new SKBitmap();
        var imgInfo = new SKImageInfo(imageData.Width, imageData.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
        bitmap.InstallPixels(imgInfo, imageData.Pointer, imageData.Stride);
        return bitmap;
    }

    #endregion Public Methods
}
