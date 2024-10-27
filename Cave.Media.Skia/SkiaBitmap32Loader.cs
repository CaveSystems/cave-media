using System.IO;
using System.Linq;
using System.Text;
using SkiaSharp;

namespace Cave.Media;

/// <summary>Provides platform independent 32 bit argb bitmap functions</summary>
/// <seealso cref="System.IDisposable"/>
/// <seealso cref="Cave.Media.IBitmap32"/>
public class SkiaBitmap32Loader : IBitmap32Loader
{
    #region Public Properties

    /// <inheritdoc/>
    public string[] FontNames => SKFontManager.Default.FontFamilies.ToArray();

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    /// <param name="data">The data.</param>
    /// <returns></returns>
    public Bitmap32 Create(byte[] data)
    {
        return new SkiaBitmap32(SKBitmap.Decode(data));
    }

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public Bitmap32 Create(ARGBImageData data)
    {
        return new SkiaBitmap32(data);
    }

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    /// <returns></returns>
    public Bitmap32 Create(int width, int height)
    {
        return new SkiaBitmap32(width, height);
    }

    /// <summary>Creates a new bitmap instance</summary>
    /// <param name="fontName">Name of the font</param>
    /// <param name="fontSize">Size in points</param>
    /// <param name="foreColor">ForeColor</param>
    /// <param name="backColor">BackColor</param>
    /// <param name="text">text to draw</param>
    public Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text)
    {
        var paint = new SKPaint();
        var emSize = fontSize / 4f * 3f;
        paint.TextSize = emSize;
        paint.TextEncoding = SKTextEncoding.Utf8;
        paint.Color = foreColor.AsUInt32;
        if (fontName != null) paint.Typeface = SKTypeface.FromFamilyName(fontName);
        paint.IsAntialias = true;
        var height = paint.GetFontMetrics(out var metrics);
        var width = paint.MeasureText(text);
        var bitmap = new SKBitmap(1 + (int)width, 1 + (int)height, SKImageInfo.PlatformColorType, SKAlphaType.Unpremul);
        using (var canvas = new SKCanvas(bitmap))
        {
            canvas.Clear(new SKColor(backColor.AsUInt32));
            canvas.DrawText(text, 0, -metrics.Ascent, paint);
        }
        return new SkiaBitmap32(bitmap);
    }

    /// <summary>Creates a bitmap instance from the specified file.</summary>
    /// <param name="fileName">Name of the file.</param>
    /// <returns></returns>
    public Bitmap32 FromFile(string fileName)
    {
        return Create(File.ReadAllBytes(fileName));
    }

    /// <summary>Creates a bitmap instance from the specified stream.</summary>
    /// <param name="stream">The stream.</param>
    /// <returns></returns>
    public Bitmap32 FromStream(Stream stream)
    {
        return new SkiaBitmap32(SKBitmap.Decode(stream));
    }

    #endregion Public Methods
}
