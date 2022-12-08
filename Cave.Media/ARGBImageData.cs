using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;

namespace Cave.Media
{
    /// <summary>
    /// Provides direct 32bit image data access.
    /// </summary>
    public class ARGBImageData
    {
        /// <summary>
        /// Loads the specified bitmap.
        /// </summary>
        /// <param name="scan0">The start of the bitmap data.</param>
        /// <param name="height">The height in scanlines</param>
        /// <param name="stride">The number of bytes per scanline</param>
        /// <param name="width">The number of pixels per line</param>
        /// <returns></returns>
        public static ARGBImageData Load(IntPtr scan0, int stride, int width, int height)
        {
            if (stride < width) throw new ArgumentOutOfRangeException(nameof(stride));
            var bytes = new byte[Math.Abs(stride * height)];
            Marshal.Copy(scan0, bytes, 0, bytes.Length);
            return new ARGBImageData(bytes, width, height, stride);
        }

        /// <summary>
        /// Gets the data of the image.
        /// </summary>
        public int[] Data { get; }

        /// <summary>
        /// Gets the stride (bytes per line) of the image.
        /// </summary>
        public int Stride { get; }

        /// <summary>
        /// Gets the height of the image.
        /// </summary>
        public int Height { get; }

        /// <summary>
        /// Gets the width of the image.
        /// </summary>
        public int Width { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ARGBImageData"/> class.
        /// </summary>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        public ARGBImageData(int width, int height)
        {
            Data = new int[width * height];
            Width = width;
            Height = height;
            Stride = width * 4;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ARGBImageData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stride">The stride.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ARGBImageData(int[] data, int width, int height, int stride)
        {
            if (height != data.Length * 4 / stride)
            {
                throw new ArgumentOutOfRangeException();
            }

            Data = data;
            Width = width;
            Height = height;
            Stride = stride;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ARGBImageData"/> class.
        /// </summary>
        /// <param name="data">The data.</param>
        /// <param name="width">The width.</param>
        /// <param name="height">The height.</param>
        /// <param name="stride">The stride.</param>
        /// <exception cref="ArgumentOutOfRangeException"></exception>
        public ARGBImageData(byte[] data, int width, int height, int stride)
        {
            if (height != data.Length / stride)
            {
                throw new ArgumentOutOfRangeException();
            }

            Data = new int[data.Length / 4];
            Buffer.BlockCopy(data, 0, Data, 0, data.Length);
            Width = width;
            Height = height;
            Stride = stride;
        }

        /// <summary>
        /// Gets the raw data (stride * width).
        /// </summary>
        public byte[] Raw
        {
            get
            {
                var data = new byte[Data.Length * 4];
                Buffer.BlockCopy(Data, 0, data, 0, data.Length);
                return data;
            }
        }

        /// <summary>
        /// Calculates an index for the specified x- and y-position.
        /// </summary>
        public int PositionToIndex(int x, int y)
        {
            if (Stride >= 0)
            {
                return (y * Stride / 4) + x;
            }
            else
            {
                return ((Height - y) * -Stride / 4) + x;
            }
        }

        /// <summary>
        /// Reduces the the colors of the image to the specified color count.
        /// </summary>
        /// <param name="colorCount">Number of colors to keep.</param>
        /// <returns>Returns the resulting color palette.</returns>
        public ARGB[] GetColors(uint colorCount)
        {
            var colorCounters = new List<ColorCounter>();
            {
                var colorDict = new Dictionary<int, ColorCounter>();
                for (var i = 0; i < Data.Length; i++)
                {
                    var color = Data[i];
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

        /// <summary>
        /// Strech the bitmap without interpolation.
        /// </summary>
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
                    result.Data[targetIndex++] = Data[sourceIndex];
                    tX += moveX;
                }
                tY += moveY;
            }
            return result;
        }

        /// <summary>
        /// Tile the bitmap.
        /// </summary>
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
                    result.Data[targetIndex++] = Data[sourceIndex];
                }
            }
            return result;
        }

        /// <summary>
        /// Tiles the outer border of the image and centers it.
        /// </summary>
        public ARGBImageData CenterTile(int width, int height)
        {
            if (height < 0)
            {
                height = Height;
            }

            if (width < 0)
            {
                width = Width;
            }

            var l_TileY = height != Height;
            var l_TileX = width != Width;

            // only tile one direction at the moment ! maybe later...
            if (l_TileX && l_TileY)
            {
                throw new NotSupportedException();
            }

            var result = new ARGBImageData(width, height);

            var heightCenterTop = (height - Height) / 2;
            if (l_TileY)
            {
                var heightCenterBottom = (height + Height) / 2;

                // fill top
                var sourceIndex = PositionToIndex(0, 0);
                for (var y = heightCenterTop; y >= 0; y--)
                {
                    var targetIndex = result.PositionToIndex(0, y);
                    Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
                }

                // fill bottom
                sourceIndex = PositionToIndex(0, Height - 1);
                for (var y = heightCenterBottom; y < height; y++)
                {
                    var targetIndex = result.PositionToIndex(0, y);
                    Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
                }
            }

            var widthCenterLeft = (width - Width) / 2;
            if (l_TileX)
            {
                var widthCenterRight = (width + Width) / 2;

                // fill left
                for (var y = 0; y < Height; y++)
                {
                    var sourceIndex = PositionToIndex(0, y);
                    for (var x = widthCenterLeft; x >= 0; x--)
                    {
                        var targetIndex = result.PositionToIndex(x, y);
                        result.Data[targetIndex] = Data[sourceIndex];
                    }

                    // fill right
                    sourceIndex = PositionToIndex(0, Height - 1);
                    for (var x = widthCenterRight; x < width; x++)
                    {
                        var targetIndex = result.PositionToIndex(x, y);
                        result.Data[targetIndex] = Data[sourceIndex];
                    }
                }
            }

            // copy center
            for (var y = 0; y < Height; y++)
            {
                var sourceIndex = PositionToIndex(0, y);
                var targetIndex = result.PositionToIndex(widthCenterLeft, y + heightCenterTop);
                Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
            }

            return result;
        }

        /// <summary>
        /// Clears the bitmap with a specific color.
        /// </summary>
        public void Clear(ARGB color) => Clear(color.AsInt32);

        /// <summary>
        /// Clears the bitmap with a specific value.
        /// </summary>
        public void Clear(uint value) => Clear((int)value);

        /// <summary>
        /// Clears the bitmap with a specific value.
        /// </summary>
        public void Clear(int value)
        {
            var index = Data.Length;
            while (index-- > 0)
            {
                Data[index] = value;
            }
        }

        /// <summary>
        /// Retrieves a ARGB struct for the specified index.
        /// </summary>
        public ARGB this[int index]
        {
            get
            {
                return ARGB.FromValue(Data[index]);
            }
            set
            {
                Data[index] = value.AsInt32;
            }
        }

        /// <summary>
        /// Retrieves a ARGB struct for the specified index.
        /// </summary>
        public ARGB this[int x, int y]
        {
            get
            {
                return ARGB.FromValue(Data[PositionToIndex(x, y)]);
            }
            set
            {
                Data[PositionToIndex(x, y)] = value.AsInt32;
            }
        }

        /// <summary>
        /// Converts this instance to a bitmap32 instance.
        /// </summary>
        public Bitmap32 ToBitmap32() => Bitmap32.Loader.Create(this);
    }
}

