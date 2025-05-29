using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;

namespace Cave.Media;

/// <summary>Provides generic 32 bit bitmap functions</summary>
public abstract class Bitmap32 : IDisposable, IBitmap32
{
    #region Private Fields

    static IBitmap32Loader? loader;

    #endregion Private Fields

    #region Private Methods

    static IBitmap32Loader LoaderFind()
    {
        var loaders = AppDom.GetInstances<IBitmap32Loader>(true);
        return loaders.FirstOrDefault() ?? throw new InvalidOperationException("No IBitmap32Loader found. Load any of the native implementations first!");
    }

    #endregion Private Methods

    #region Public Properties

    /// <summary>Provides the default loader.</summary>
    public static IBitmap32Loader Loader
    {
        get => loader ??= LoaderFind();
        set => loader = value ?? throw new ArgumentNullException(nameof(value));
    }

    /// <inheritdoc/>
    public abstract int Height { get; }

    /// <inheritdoc/>
    public abstract int Width { get; }

    #endregion Public Properties

    #region Public Methods

    /// <inheritdoc/>
    public abstract void Clear(ARGB color);

    /// <inheritdoc/>
    public virtual unsafe IList<ARGB> DetectColors(int max)
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
    public abstract void Dispose();

    /// <inheritdoc/>
    public abstract void Draw(IBitmap32 other, int x, int y, Translation? translation = null);

    /// <inheritdoc/>
    public abstract void Draw(IBitmap32 other, int x, int y, int width, int height, Translation? translation = null);

    /// <inheritdoc/>
    public abstract void Draw(IBitmap32 other, float x, float y, float width, float height, Translation? translation = null);

    /// <inheritdoc/>
    public abstract void Draw(ARGBImageData other, int x, int y, int width, int height, Translation? translation = null);

    /// <inheritdoc/>
    public abstract ARGBImageData GetImageData();

    /// <inheritdoc/>
    public abstract void MakeTransparent();

    /// <inheritdoc/>
    public abstract void MakeTransparent(ARGB color);

    /// <inheritdoc/>
    public virtual IBitmap32 Resize(int width, int height, ResizeMode mode = 0)
    {
        var result = Loader.Create(width, height);
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

    /// <inheritdoc/>
    public abstract void Save(Stream stream, ImageType type = ImageType.Png, int quality = 100);

    /// <inheritdoc/>
    public abstract void Save(string filename, int quality = 100);

    #endregion Public Methods
}
