using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cave.Media;

/// <summary>Provides direct 32bit image data access.</summary>
public unsafe class ARGBImageData : IDisposable
{
    #region Private Fields

    readonly bool freePointerOnDispose;

    bool disposedValue;

    GCHandle? handle;

    #endregion Private Fields

    #region Private Destructors

    /// <summary>Finalizes this object if not already disposed</summary>
    ~ARGBImageData()
    {
        Dispose(disposing: false);
    }

    #endregion Private Destructors

    #region Protected Methods

    /// <summary>Disposes native memory and unpins net objects</summary>
    protected virtual void Dispose(bool disposing)
    {
        if (!disposedValue)
        {
            if (disposing)
            {
                handle?.Free();
            }
            if (freePointerOnDispose)
            {
                Marshal.FreeHGlobal((IntPtr)Pixels1);
            }
            disposedValue = true;
        }
    }

    #endregion Protected Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="ARGBImageData"/> class.</summary>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    public ARGBImageData(int width, int height)
    {
        freePointerOnDispose = true;
        PixelCount = width * height;
        DataLength = PixelCount * 4;
        Pixels1 = (int*)Marshal.AllocHGlobal(DataLength);
        Pixels2 = (ARGB*)Pixels1;
        Width = width;
        Height = height;
        PixelsPerLine = width;
        Stride = PixelsPerLine * 4;
    }

    /// <summary>Initializes a new instance of the <see cref="ARGBImageData"/> class.</summary>
    /// <param name="data">The data.</param>
    /// <param name="dataLength">Length of the image data memory buffer</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="stride">The stride.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ARGBImageData(IntPtr data, int dataLength, int width, int height, int stride) : this((int*)data.ToPointer(), dataLength, width, height, stride) { }

    /// <summary>Initializes a new instance of the <see cref="ARGBImageData"/> class.</summary>
    /// <param name="data">The data.</param>
    /// <param name="dataLength">Length of the image data memory buffer</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="stride">The stride.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ARGBImageData(int* data, int dataLength, int width, int height, int stride)
    {
        Pixels1 = data;
        Pixels2 = (ARGB*)Pixels1;
        Width = width;
        Height = height;
        Stride = stride;
        PixelsPerLine = Math.Abs(stride / 4);
        PixelCount = dataLength / 4;
        DataLength = dataLength;
        if (DataLength != Stride * Height) throw new ArgumentOutOfRangeException(nameof(stride), "Stride and Height do not match data.Length!");
        if (PixelCount != Width * Height) throw new ArgumentOutOfRangeException(nameof(stride), "Width and Height do not match PixelCount!");
    }

    /// <summary>Initializes a new instance of the <see cref="ARGBImageData"/> class.</summary>
    /// <param name="data">The data.</param>
    /// <param name="width">The width.</param>
    /// <param name="height">The height.</param>
    /// <param name="stride">The stride.</param>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public ARGBImageData(Array data, int width, int height, int stride)
    {
        handle = GCHandle.Alloc(data, GCHandleType.Pinned);
        Pixels1 = (int*)handle.Value.AddrOfPinnedObject();
        Pixels2 = (ARGB*)Pixels1;
        Width = width;
        Height = height;
        Stride = stride;
        DataLength = data.Length;
        PixelsPerLine = Math.Abs(stride / 4);
        PixelCount = DataLength / 4;
        if (DataLength != Stride * Height) throw new ArgumentOutOfRangeException(nameof(stride), "Stride and Height do not match data.Length!");
        if (PixelCount != Width * Height) throw new ArgumentOutOfRangeException(nameof(stride), "Width and Height do not match PixelCount!");
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Length in bytes of the image buffer</summary>
    public int DataLength { get; }

    /// <summary>Gets the height of the image.</summary>
    public int Height { get; }

    /// <summary>Number of pixels</summary>
    public int PixelCount { get; }

    /// <summary>Gets the data of the image.</summary>
    public int* Pixels1 { get; }

    /// <summary>Gets the data of the image.</summary>
    public ARGB* Pixels2 { get; }

    /// <summary>Number of pixels per line (this is &lt;= Abs(Stride) / 4)</summary>
    public int PixelsPerLine { get; }

    /// <summary>Gets the stride (bytes per line) of the image.</summary>
    public int Stride { get; }

    /// <summary>Gets the width of the image.</summary>
    public int Width { get; }

    #endregion Public Properties

    #region Public Indexers

    /// <summary>Retrieves a ARGB struct for the specified index.</summary>
    public ARGB this[int index]
    {
        get => ValidIndex(index) ? Pixels2[index] : throw new IndexOutOfRangeException(nameof(index));
        set => Pixels2[index] = ValidIndex(index) ? value : throw new IndexOutOfRangeException(nameof(index));
    }

    /// <summary>Retrieves a ARGB struct for the specified index.</summary>
    public ARGB this[int x, int y]
    {
        get => Pixels2[PositionToIndex(x, y)];
        set => Pixels2[PositionToIndex(x, y)] = value;
    }

    #endregion Public Indexers

    #region Public Methods

    /// <summary>Creates a copy of the specified bitmap.</summary>
    /// <param name="scan0">The start of the bitmap data.</param>
    /// <param name="height">The height in scanlines</param>
    /// <param name="stride">The number of bytes per scanline</param>
    /// <param name="width">The number of pixels per line</param>
    /// <returns></returns>
    public static ARGBImageData Copy(IntPtr scan0, int stride, int width, int height)
    {
        if (stride < width) throw new ArgumentOutOfRangeException(nameof(stride));
        var bytes = new byte[Math.Abs(stride * height)];
        Marshal.Copy(scan0, bytes, 0, bytes.Length);
        return new ARGBImageData(bytes, width, height, stride);
    }

    /// <summary>Clears the bitmap with a specific color.</summary>
    public void Clear(ARGB color) => Clear(color.AsInt32);

    /// <summary>Clears the bitmap with a specific value.</summary>
    public void Clear(uint value) => Clear((int)value);

    /// <summary>Clears the bitmap with a specific value.</summary>
    public void Clear(int value)
    {
        var source = (IntPtr)Pixels1;
        var target = (IntPtr)Pixels1 + 16;
        Pixels1[0] = value;
        Pixels1[1] = value;
        Pixels1[2] = value;
        Pixels1[3] = value;
        Interop.SafeNativeMethods.memcpy(target, source, DataLength - 16);
    }

    /// <inheritdoc/>
    public void Dispose()
    {
        Dispose(disposing: true);
        GC.SuppressFinalize(this);
    }

    /// <summary>Gets the raw data (stride * width).</summary>
    public byte[] GetBytes()
    {
        var data = new byte[DataLength];
        Marshal.Copy((IntPtr)Pixels1, data, 0, DataLength);
        return data;
    }

    /// <summary>Reduces the the colors of the image to the specified color count.</summary>
    /// <param name="colorCount">Number of colors to keep.</param>
    /// <returns>Returns the resulting color palette.</returns>
    public ARGB[] GetColors(uint colorCount)
    {
        var colorCounters = new List<ColorCounter>();
        {
            var colorDict = new Dictionary<int, ColorCounter>();
            for (var i = 0; i < PixelCount; i++)
            {
                var color = Pixels1[i];
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

        var distance = 255000 / colorCount;
        while (colorCounters.Count > colorCount)
        {
            colorCounters.Sort();
            colorCounters = ColorCounter.Reduce(colorCounters, ref distance);
            distance *= 2;
        }
        colorCounters.Sort();

        var colors = new ARGB[colorCounters.Count];
        for (var i = 0; i < colors.Length; i++)
        {
            colors[i] = colorCounters[i].Color;
        }

        return colors;
    }

    /// <summary>Calculates an index for the specified x- and y-position.</summary>
    public int PositionToIndex(int x, int y)
    {
        int result;
        if (Stride >= 0)
        {
            result = (y * Stride / 4) + x;
        }
        else
        {
            result = ((Height - y) * -Stride / 4) + x;
        }
        return ValidIndex(result) ? result : throw new IndexOutOfRangeException($"Position [x, y] = [{x}, {y}] is not valid!");
    }

    /// <summary>Strech the bitmap without interpolation.</summary>
    public ARGBImageData StretchSimple(int width, int height)
    {
        if (height < 0)
        {
            height = Height;
        }

        if (width < 0)
        {
            width = Width;
        }

        var result = new ARGBImageData(width, height);
        var targetIndex = 0;
        var moveX = (Width << 10) / width;
        var moveY = ((Height << 10) / height) - 1;

        var tY = 0;
        for (var y = 0; y < height; y++)
        {
            var tX = 0;
            for (var x = 0; x < width; x++)
            {
                var sourceIndex = PositionToIndex(tX >> 10, tY >> 10);
                result.Pixels1[targetIndex++] = Pixels1[sourceIndex];
                tX += moveX;
            }
            tY += moveY;
        }
        return result;
    }

    /// <summary>Tile the bitmap.</summary>
    public ARGBImageData TileSimple(int width, int height)
    {
        if (height < 0)
        {
            height = Height;
        }

        if (width < 0)
        {
            width = Width;
        }

        var result = new ARGBImageData(width, height);
        var w = Width;
        var h = Height;

        var targetIndex = 0;
        for (var y = 0; y < height; y++)
        {
            var tY = y % h;
            for (var x = 0; x < width; x++)
            {
                var tX = x % w;
                var sourceIndex = PositionToIndex(tX, tY);
                result.Pixels1[targetIndex++] = Pixels1[sourceIndex];
            }
        }
        return result;
    }

    /// <summary>Converts this instance to a bitmap32 instance.</summary>
    public Bitmap32 ToBitmap32() => Bitmap32.Loader.Create(this);

    /// <summary>Checks whether an index is valid or not</summary>
    /// <param name="index">Index to check</param>
    /// <returns>Returns <see langword="true"/> if the index is valid, false otherwise.</returns>
    public bool ValidIndex(int index) => index > 0 && index < PixelCount;

    #endregion Public Methods
}
