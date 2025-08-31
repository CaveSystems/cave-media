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
        if (bitmap.ColorType != SkiaBitmap32Loader.ColorType || bitmap.AlphaType != SKAlphaType.Unpremul)
        {
            var result = new SKBitmap(bitmap.Width, bitmap.Height, SkiaBitmap32Loader.ColorType, SKAlphaType.Unpremul);
            bitmap.CopyTo(result, SkiaBitmap32Loader.ColorType);
            bitmap = result;
        }
        return new ARGBImageData(bitmap.GetPixels(), bitmap.RowBytes * bitmap.Height, bitmap.Width, bitmap.Height, bitmap.RowBytes);
    }

    /// <summary>Writes all data to a new <see cref="SKBitmap"/> instance</summary>
    /// <returns></returns>
    /// <exception cref="Exception">Invalid length!</exception>
    public static unsafe SKBitmap ToSKBitmap(this ARGBImageData imageData)
    {
        var bitmap = new SKBitmap();
        var imgInfo = new SKImageInfo(imageData.Width, imageData.Height, SkiaBitmap32Loader.ColorType, SKAlphaType.Unpremul);
        bitmap.InstallPixels(imgInfo, imageData.Pointer, imageData.Stride);
        return bitmap;
    }

    #endregion Public Methods
}
