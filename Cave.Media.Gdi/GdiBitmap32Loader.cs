using System;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.Drawing.Text;
using System.IO;
using System.Linq;

#nullable enable

namespace Cave.Media;

/// <summary>Provides gdi 32 bit argb bitmap functions.</summary>
/// <seealso cref="IDisposable"/>
/// <seealso cref="IBitmap32"/>
public class GdiBitmap32Loader : IBitmap32Loader
{
    #region Public Properties

    /// <inheritdoc/>
    public string[] FontNames => new InstalledFontCollection().Families.Select(f => f.Name).ToArray();

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public Bitmap32 Create(byte[] data)
    {
        using var ms = new MemoryStream(data);
        return FromStream(ms);
    }

    /// <summary>Creates a new bitmap instance.</summary>
    public Bitmap32 Create(int width, int height) => new GdiBitmap32(width, height);

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public Bitmap32 Create(ARGBImageData data) => new GdiBitmap32(data);

    /// <summary>Creates a new bitmap instance.</summary>
    /// <param name="fontName">Name of the font.</param>
    /// <param name="fontSize">Size in points.</param>
    /// <param name="foreColor">ForeColor.</param>
    /// <param name="backColor">BackColor.</param>
    /// <param name="text">text to draw.</param>
    public Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text)
    {
        SizeF size;
        using var b = new Bitmap(1, 1);
        var emSize = fontSize / 4f * 3f;
        var font = fontName == null ? new Font(FontFamily.GenericSansSerif, fontSize, GraphicsUnit.Point) : new Font(fontName, fontSize, GraphicsUnit.Point);
        using (font)
        {
            using (var g = Graphics.FromImage(b))
            {
                size = g.MeasureString(text, font);
            }
            var result = new Bitmap((int)size.Width + 1, (int)size.Height + 1, PixelFormat.Format32bppArgb);
            using (var g = Graphics.FromImage(result))
            {
                g.TextRenderingHint = TextRenderingHint.AntiAlias;
                g.InterpolationMode = InterpolationMode.HighQualityBicubic;
                g.CompositingMode = CompositingMode.SourceOver;
                g.CompositingQuality = CompositingQuality.HighQuality;
                g.SmoothingMode = SmoothingMode.HighQuality;
                g.Clear(backColor);
                g.DrawString(text, font, new SolidBrush(foreColor), 0, 0);
            }
            return new GdiBitmap32(result);
        }
    }

    /// <summary>Creates a bitmap instance from the specified file.</summary>
    public Bitmap32 FromFile(string fileName) => new GdiBitmap32(Image.FromFile(fileName));

    /// <summary>Creates a bitmap instance from the specified stream.</summary>
    public Bitmap32 FromStream(Stream stream) => new GdiBitmap32(Image.FromStream(stream));

    #endregion Public Methods
}
