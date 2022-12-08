using System;
using System.Runtime.InteropServices;
using SkiaSharp;

namespace Cave.Media
{
    /// <summary>
    /// Provides extensions for <see cref="ARGBImageData"/>
    /// </summary>
    public static class ARGBImageDataExtension
    {
        /// <summary>
        /// Loads the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        public static ARGBImageData ToARGBImageData(this SKBitmap bitmap)
        {
            var pix = bitmap.Bytes;
            var data = new int[bitmap.Width * bitmap.Height];
            Buffer.BlockCopy(pix, 0, data, 0, 4 * data.Length);
            return new ARGBImageData(data, bitmap.Width, bitmap.Height, pix.Length / bitmap.Height);
        }

        /// <summary>
        /// Writes all data to a new <see cref="SKBitmap"/> instance
        /// </summary>
        /// <returns></returns>
        /// <exception cref="Exception">Invalid length!</exception>
        public static SKBitmap ToSKBitmap(this ARGBImageData imageData)
        {
            var imgInfo = new SKImageInfo(imageData.Width, imageData.Height, SKColorType.Rgba8888, SKAlphaType.Unpremul);
            var bitmap = SKBitmap.FromImage(SKImage.Create(imgInfo));
            IntPtr len;
            var ptr = bitmap.GetPixels(out len);
            var byteLen = imageData.Data.Length * 4;
            if (len.ToInt32() != byteLen) throw new Exception("Invalid length!");
            Marshal.Copy(imageData.Data, 0, ptr, imageData.Data.Length);
            return bitmap;
        }
    }
}
