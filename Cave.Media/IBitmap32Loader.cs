using System.IO;

namespace Cave.Media;

/// <summary>Provides platform independant 32 bit argb bitmap image manipulation.</summary>
public interface IBitmap32Loader
{
    #region Public Properties

    /// <summary>Gets available font names</summary>
    string[] FontNames { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    Bitmap32 Create(byte[] data);

    /// <summary>Creates a new bitmap instance.</summary>
    Bitmap32 Create(int width, int height);

    /// <summary>Creates a new bitmap instance.</summary>
    Bitmap32 Create(ARGBImageData data);

    /// <summary>Creates a new bitmap instance.</summary>
    /// <param name="fontName">Name of the font.</param>
    /// <param name="fontSize">Size in points.</param>
    /// <param name="foreColor">ForeColor.</param>
    /// <param name="backColor">BackColor.</param>
    /// <param name="text">text to draw.</param>
    Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text);

    /// <summary>Creates a bitmap instance from the specified file.</summary>
    Bitmap32 FromFile(string fileName);

    /// <summary>Creates a bitmap instance from the specified stream.</summary>
    Bitmap32 FromStream(Stream stream);

    #endregion Public Methods
}
