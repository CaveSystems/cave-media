using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Cave.Media;

/// <summary>
/// Provides extensions to the <see cref="ARGBImageData"/> class.
/// </summary>
public static class ARGBImageDataExtension
{
    /// <summary>
    /// Copies the image to the specified bitmapdata instance.
    /// </summary>
    public static void CopyTo32BitBitmapData(this ARGBImageData source, BitmapData target)
    {
        if (target.Width != source.Width)
        {
            throw new ArgumentException(string.Format("Width is not compatible!"));
        }

        if (target.Height != source.Height)
        {
            throw new ArgumentException(string.Format("Height is not compatible!"));
        }

        if (target.PixelFormat != PixelFormat.Format32bppArgb)
        {
            throw new ArgumentException(string.Format("PixelFormat is not compatible!"));
        }

        if (target.Stride != source.Stride)
        {
            Trace.WriteLine(string.Format("Copy ARGB image data with stride {0} to GDI bitmap data with stride {1}!", source.Stride, target.Stride));
            var start = target.Scan0;
            var index = 0;
            for (var y = 0; y < source.Height; y++)
            {
                Marshal.Copy(source.Data, index, start, source.Data.Length);
                index += source.Stride;
                start = new IntPtr(start.ToInt64() + target.Stride);
            }
        }
        else
        {
            Marshal.Copy(source.Data, 0, target.Scan0, source.Data.Length);
        }
    }

    /// <summary>
    /// Loads the specified bitmap.
    /// </summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <returns></returns>
    public static ARGBImageData ToARGBImageData(this Bitmap bitmap)
    {
        byte[] bytes;
        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        int stride;
        try
        {
            stride = data.Stride;
            bytes = new byte[Math.Abs(data.Stride * data.Height)];
            Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
        return new ARGBImageData(bytes, bitmap.Width, bitmap.Height, stride);
    }



    /// <summary>
    /// Copies the image to the specified bitmap.
    /// </summary>
    public static void CopyToBitmap(this ARGBImageData source, Bitmap img)
    {
        var data = img.LockBits(new Rectangle(Point.Empty, img.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        source.CopyTo32BitBitmapData(data);
        img.UnlockBits(data);
    }

    /// <summary>
    /// Creates a new bitmap from the image.
    /// </summary>
    public static Bitmap ToBitmap(this ARGBImageData source)
    {
        var result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
        source.CopyToBitmap(result);
        return result;
    }
}
