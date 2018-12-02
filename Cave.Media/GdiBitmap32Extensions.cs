#if NET20 || NET35 || NET40 || NET45 || NET46 || NET471

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
			Bitmap bitmap = image as Bitmap;
			if (bitmap?.PixelFormat == PixelFormat.Format32bppArgb)
			{
				return bitmap;
			}

			Bitmap b = new Bitmap(image.Width, image.Height);
			using (Graphics g = Graphics.FromImage(b))
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
			using (MemoryStream ms = new MemoryStream())
			{
				other.Save(ms);
				ms.Position = 0;
				return ToGdiBitmap(Image.FromStream(ms));
			}
		}
	}
}

#endif