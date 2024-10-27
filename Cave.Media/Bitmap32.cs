using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cave.Media;

/// <summary>Provides platform independent 32 bit argb bitmap functions.</summary>
public class Bitmap32 : IDisposable, IBitmap32
{
    #region Private Fields

    static IBitmap32Loader? loader;
    IBitmap32? parent;

    IBitmap32 Parent => parent ?? throw new InvalidOperationException("Parent is unset!");

    #endregion Private Fields

    #region Private Methods

    static IBitmap32Loader FindLoader()
    {
        var loaders = AppDom.GetInstances<IBitmap32Loader>(true);
        return loaders.FirstOrDefault() ?? throw new InvalidOperationException("No IBitmap32Loader found. Load any of the native implementations first!");
    }

    #endregion Private Methods

    #region Protected Constructors

    /// <summary>Creates an empty instance.</summary>
    protected Bitmap32() { }

    #endregion Protected Constructors

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
    /// <param name="bitmap"></param>
    public Bitmap32(IBitmap32 bitmap)
    {
        parent = bitmap;
    }

    /// <summary>Initializes a new instance of the <see cref="Bitmap32"/> class.</summary>
    /// <param name="width">Width in pixel.</param>
    /// <param name="height">Height in pixel.</param>
    public Bitmap32(int width, int height)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        parent = Loader.Create(width, height);
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Provides the default loader.</summary>
    public static IBitmap32Loader Loader { get => loader ??= FindLoader(); set => loader = value ?? throw new ArgumentNullException(nameof(value)); }

    /// <inheritdoc/>
    public virtual int Height => Parent.Height;

    /// <inheritdoc/>
    public virtual int Width => Parent.Width;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public static Bitmap32 Create(ARGBImageData data)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        return Loader.Create(data);
    }

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public static Bitmap32 Create(byte[] data)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        return Loader.Create(data);
    }

    /// <summary>Creates a bitmap instance from the specified data.</summary>
    public static Bitmap32 Create(string fontName, float fontSize, ARGB foreColor, ARGB backColor, string text)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        return Loader.Create(fontName, fontSize, foreColor, backColor, text);
    }

    /// <summary>Creates a bitmap instance from the specified file.</summary>
    public static Bitmap32 FromFile(string fileName)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        return Loader.FromFile(fileName);
    }

    /// <summary>Creates a bitmap instance from the specified stream.</summary>
    public static Bitmap32 FromStream(Stream stream)
    {
        if (Loader == null)
        {
            throw new Exception("No valid IBitmap32Loader found!");
        }

        return Loader.FromStream(stream);
    }

    /// <summary>Clear the image with the specified color.</summary>
    /// <param name="color">The color.</param>
    public virtual void Clear(ARGB color) => Parent.Clear(color);

    /// <inheritdoc/>
    public unsafe IList<ARGB> DetectColors(int max)
    {
        if ((Width + Height) / 2 > max)
        {
            using var bmp = Resize(max, max, ResizeMode.None);
            return bmp.DetectColors(max);
        }

        var colorCounters = new List<ColorCounter>();
        var data = GetImageData();
        var pixels = data.Pixels1;
        var pixelCount = data.PixelCount;
        for (var y = 0; y < Height; y++)
        {
            var colorDict = new Dictionary<ARGB, ColorCounter>();
            for (var i = 0; i < pixelCount; i++)
            {
                var color = pixels[i];
                if (!colorDict.ContainsKey(color))
                {
                    colorDict.Add(color, new ColorCounter(color, 1));
                }
                else
                {
                    colorDict[color].Count++;
                }
            }
            colorCounters.AddRange(colorDict.Values);
        }
        uint distance = 255;
        while (colorCounters.Count > max)
        {
            colorCounters.Sort();
            colorCounters = ColorCounter.Reduce(colorCounters, ref distance);
            distance += 255;
        }
        colorCounters.Sort();
        var colors = new List<ARGB>(colorCounters.Count);
        for (var i = 0; i < colorCounters.Count; i++)
        {
            colors.Add(colorCounters[i].Color);
        }

        return colors;
    }

    /// <inheritdoc/>
    public virtual void Dispose()
    {
        parent?.Dispose();
        parent = null;
        GC.SuppressFinalize(this);
    }

    /// <summary>Draws the specified image ontop of this one.</summary>
    /// <param name="other">The image to draw.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="translation">The translation.</param>
    public virtual void Draw(Bitmap32 other, int x, int y, Translation? translation = null) => Parent.Draw(other, x, y, other.Width, other.Height, translation);

    /// <summary>Draws the specified image ontop of this one.</summary>
    /// <param name="other">The image to draw.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="translation">The translation.</param>
    public virtual void Draw(Bitmap32 other, int x, int y, int width, int height, Translation? translation = null) => Parent.Draw(other, x, y, width, height, translation);

    /// <summary>Draws the specified image ontop of this one.</summary>
    /// <param name="other">The image to draw.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="translation">The translation.</param>
    public virtual void Draw(Bitmap32 other, float x, float y, float width, float height, Translation? translation = null) => Parent.Draw(other, x, y, width, height, translation);

    /// <summary>Draws the specified image ontop of this one.</summary>
    /// <param name="other">The image to draw.</param>
    /// <param name="x">The x position.</param>
    /// <param name="y">The y position.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="translation">The translation.</param>
    public virtual void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null) => Parent.Draw(other, x, y, width, height, translation);

    /// <inheritdoc/>
    public virtual ARGBImageData GetImageData() => Parent.GetImageData();

    /// <inheritdoc/>
    public virtual void MakeTransparent() => Parent.MakeTransparent();

    /// <inheritdoc/>
    public virtual void MakeTransparent(ARGB color) => Parent.MakeTransparent(color);

    /// <summary>Resizes the bitmap to the specified size.</summary>
    /// <remarks>This should be overloaded to speed up the resize.</remarks>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="mode">The resize mode.</param>
    /// <returns></returns>
    public virtual Bitmap32 Resize(int width, int height, ResizeMode mode = 0)
    {
        var result = new Bitmap32(width, height);
        float w = width;
        float h = height;
        if (mode != ResizeMode.None)
        {
            var fw = w / (float)Width;
            var fh = h / (float)Height;
            float f;
            if (mode.HasFlag(ResizeMode.TouchFromInside))
            {
                f = Math.Min(fw, fh);
            }
            else
            {
                f = Math.Max(fw, fh);
            }
            w = Width * f;
            h = Height * f;
        }
        var x = (width - w) / 2;
        var y = (height - h) / 2;
        result.Draw(this, x, y, w, h);
        return result;
    }

    /// <summary>Saves the image to the specified stream.</summary>
    /// <param name="stream">The stream.</param>
    /// <param name="type">The type.</param>
    /// <param name="quality">The quality.</param>
    /// <exception cref="NotImplementedException"></exception>
    public virtual void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100) => Parent.Save(stream, type, quality);

    /// <inheritdoc/>
    public virtual void Save(string fileName, int quality = 100) => Parent.Save(fileName, quality);

    #endregion Public Methods
}
