#if NETSTANDARD20
#elif NET20 || NET35 || NET40 || NET45 || NET46 || NET47

using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;

namespace Cave.Media
{
    /// <summary>
    /// Provides extensions for GdiBitmap32 instances
    /// </summary>
    public static class GdiBitmap32Extensions
    {
        /// <summary>Creates a bitmap instance from the specified image.</summary>
        public static Bitmap ToGdiBitmap(this Image image)
        {
            var bitmap = image as Bitmap;
            if (bitmap?.PixelFormat == PixelFormat.Format32bppArgb)
            {
                return bitmap;
            }

            var b = new Bitmap(image.Width, image.Height);
            using (var g = Graphics.FromImage(b))
            {
                g.DrawImage(image, 0, 0, b.Width, b.Height);
            }
            return b;
        }

        /// <summary>Creates a bitmap instance from the specified Bitmap32.</summary>
        public static Bitmap ToGdiBitmap(this IBitmap32 other)
        {
            if (other is GdiBitmap32)
            {
                return ((GdiBitmap32)other).Bitmap;
            }
            using (var ms = new MemoryStream())
            {
                other.Save(ms);
                ms.Position = 0;
                return ToGdiBitmap(Image.FromStream(ms));
            }
        }
    }
}

#else

#error No code defined for the current framework or NETXX version define missing!

#endif
