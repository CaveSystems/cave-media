using System;
using System.Diagnostics;
using System.Drawing;
using System.Runtime.InteropServices;
using System.Drawing.Imaging;

namespace Cave.Media;

/// <summary>Provides extensions to the <see cref="ARGBImageData"/> class.</summary>
public static class ARGBImageDataExtension
{
    #region Public Methods

    /// <summary>Copies the image to the specified bitmapdata instance.</summary>
    public static unsafe void CopyTo32BitBitmapData(this ARGBImageData source, BitmapData target)
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

        var targetPointer = (byte*)target.Scan0;
        var sourcePointer = (byte*)source.Pixels1;
        if (target.Stride == source.Stride)
        {
            Interop.SafeNativeMethods.memcpy(targetPointer, sourcePointer, source.DataLength);
            return;
        }

        var swapLines = (target.Stride > 0 && source.Stride < 0) || (target.Stride < 0 && source.Stride > 0);
        Trace.WriteLine(string.Format("Copy ARGB image data with stride {0} to GDI bitmap data with stride {1}!", source.Stride, target.Stride));
        if (swapLines)
        {
            sourcePointer += source.DataLength;
            for (var y = 0; y < source.Height; y++)
            {
                sourcePointer -= source.Stride;
                Interop.SafeNativeMethods.memcpy(targetPointer, sourcePointer, source.Stride);
                targetPointer += target.Stride;
            }
        }
        else
        {
            for (var y = 0; y < source.Height; y++)
            {
                Interop.SafeNativeMethods.memcpy(targetPointer, sourcePointer, source.Stride);
                sourcePointer += source.Stride;
                targetPointer += target.Stride;
            }
        }
    }

    /// <summary>Copies the image to the specified bitmap.</summary>
    public static void CopyToBitmap(this ARGBImageData source, Bitmap img)
    {
        var data = img.LockBits(new Rectangle(Point.Empty, img.Size), ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);
        source.CopyTo32BitBitmapData(data);
        img.UnlockBits(data);
    }

    /// <summary>Loads the specified bitmap.</summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <returns></returns>
    public static ARGBImageData ToARGBImageData(this Bitmap bitmap)
    {
        var data = bitmap.LockBits(new Rectangle(Point.Empty, bitmap.Size), ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);
        try
        {
            return ARGBImageData.Copy(data.Scan0, data.Stride, data.Width, data.Height);
        }
        finally
        {
            bitmap.UnlockBits(data);
        }
    }

    /// <summary>Creates a new bitmap from the image.</summary>
    public static Bitmap ToBitmap(this ARGBImageData source)
    {
        var result = new Bitmap(source.Width, source.Height, PixelFormat.Format32bppArgb);
        source.CopyToBitmap(result);
        return result;
    }

    #endregion Public Methods
}
