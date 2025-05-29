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
    public static ARGBImageData ToARGBImageData(this SKBitmap bitmap) => new ARGBImageData(bitmap.Bytes, bitmap.Width, bitmap.Height, bitmap.RowBytes);

    /// <summary>Writes all data to a new <see cref="SKBitmap"/> instance</summary>
    /// <returns></returns>
    /// <exception cref="Exception">Invalid length!</exception>
    public static unsafe SKBitmap ToSKBitmap(this ARGBImageData imageData)
    {
        var imgInfo = new SKImageInfo(imageData.Width, imageData.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
        var image = SKImage.FromPixels(imgInfo, (IntPtr)imageData.Pixels1);
        return SKBitmap.FromImage(image);
    }

    #endregion Public Methods
}
