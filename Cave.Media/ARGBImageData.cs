using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;

namespace Cave.Media
{
    /// <summary>
    /// Provides direct 32bit image data access.
    /// </summary>
    public class ARGBImageData
    {
#if !NETSTANDARD20
        /// <summary>
        /// Loads the specified bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap.</param>
        /// <returns></returns>
        internal static ARGBImageData Load(System.Drawing.Bitmap bitmap)
        {
            byte[] bytes;
            var data = bitmap.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, bitmap.Size), System.Drawing.Imaging.ImageLockMode.ReadOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            int stride;
            try
            {
                stride = data.Stride;
                bytes = new byte[Math.Abs(data.Stride * data.Height)];
                Marshal.Copy(data.Scan0, bytes, 0, bytes.Length);
            }
            finally
            {
                bitmap.UnlockBits(data);
            }
            return new ARGBImageData(bytes, bitmap.Width, bitmap.Height, stride);
        }
#endif

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
        /// Obtains the width of the image.
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
        /// Obtains the raw data (stride * width).
        /// </summary>
        public byte[] Raw
        {
            get
            {
                byte[] data = new byte[Data.Length * 4];
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
                for (int i = 0; i < Data.Length; i++)
                {
                    int color = Data[i];
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

            uint distance = 255000 / colorCount;
            while (colorCounters.Count > colorCount)
            {
                colorCounters.Sort();
                colorCounters = ColorCounter.Reduce(colorCounters, ref distance);
                distance *= 2;
            }
            colorCounters.Sort();

            ARGB[] colors = new ARGB[colorCounters.Count];
            for (int i = 0; i < colors.Length; i++)
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
            int targetIndex = 0;
            int moveX = (Width << 10) / width;
            int moveY = ((Height << 10) / height) - 1;

            int tY = 0;
            for (int y = 0; y < height; y++)
            {
                int tX = 0;
                for (int x = 0; x < width; x++)
                {
                    int sourceIndex = PositionToIndex(tX >> 10, tY >> 10);
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
            int w = Width;
            int h = Height;

            int targetIndex = 0;
            for (int y = 0; y < height; y++)
            {
                int tY = y % h;
                for (int x = 0; x < width; x++)
                {
                    int tX = x % w;
                    int sourceIndex = PositionToIndex(tX, tY);
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

            bool l_TileY = height != Height;
            bool l_TileX = width != Width;

            // only tile one direction at the moment ! maybe later...
            if (l_TileX && l_TileY)
            {
                throw new NotSupportedException();
            }

            var result = new ARGBImageData(width, height);

            int heightCenterTop = (height - Height) / 2;
            if (l_TileY)
            {
                int heightCenterBottom = (height + Height) / 2;

                // fill top
                int sourceIndex = PositionToIndex(0, 0);
                for (int y = heightCenterTop; y >= 0; y--)
                {
                    int targetIndex = result.PositionToIndex(0, y);
                    Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
                }

                // fill bottom
                sourceIndex = PositionToIndex(0, Height - 1);
                for (int y = heightCenterBottom; y < height; y++)
                {
                    int targetIndex = result.PositionToIndex(0, y);
                    Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
                }
            }

            int widthCenterLeft = (width - Width) / 2;
            if (l_TileX)
            {
                int widthCenterRight = (width + Width) / 2;

                // fill left
                for (int y = 0; y < Height; y++)
                {
                    int sourceIndex = PositionToIndex(0, y);
                    for (int x = widthCenterLeft; x >= 0; x--)
                    {
                        int targetIndex = result.PositionToIndex(x, y);
                        result.Data[targetIndex] = Data[sourceIndex];
                    }

                    // fill right
                    sourceIndex = PositionToIndex(0, Height - 1);
                    for (int x = widthCenterRight; x < width; x++)
                    {
                        int targetIndex = result.PositionToIndex(x, y);
                        result.Data[targetIndex] = Data[sourceIndex];
                    }
                }
            }

            // copy center
            for (int y = 0; y < Height; y++)
            {
                int sourceIndex = PositionToIndex(0, y);
                int targetIndex = result.PositionToIndex(widthCenterLeft, y + heightCenterTop);
                Array.Copy(Data, sourceIndex, result.Data, targetIndex, Width);
            }

            return result;
        }

        /// <summary>
        /// Clears the bitmap with a specific color.
        /// </summary>
        public void Clear(ARGB color)
        {
            Clear(color.AsInt32);
        }

        /// <summary>
        /// Clears the bitmap with a specific value.
        /// </summary>
        public void Clear(uint value)
        {
            Clear((int)value);
        }

        /// <summary>
        /// Clears the bitmap with a specific value.
        /// </summary>
        public void Clear(int value)
        {
            int index = Data.Length;
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
        public ARGB this[int X, int Y]
        {
            get
            {
                return ARGB.FromValue(Data[PositionToIndex(X, Y)]);
            }
            set
            {
                Data[PositionToIndex(X, Y)] = value.AsInt32;
            }
        }

        /// <summary>
        /// Converts this instance to a bitmap32 instance.
        /// </summary>
        public Bitmap32 ToBitmap32()
        {
            return Bitmap32.Loader.Create(this);
        }

#if !NETSTANDARD20
        /// <summary>
        /// Copies the image to the specified bitmapdata instance
        /// </summary>
        public void CopyTo32BitBitmapData(System.Drawing.Imaging.BitmapData imgData)
        {
            if (imgData.Width != Width) throw new ArgumentException(string.Format("Width is not compatible!"));
            if (imgData.Height != Height) throw new ArgumentException(string.Format("Height is not compatible!"));
            if (imgData.PixelFormat != System.Drawing.Imaging.PixelFormat.Format32bppArgb) throw new ArgumentException(string.Format("PixelFormat is not compatible!"));
            if (imgData.Stride != Stride)
            {
                Trace.WriteLine(string.Format("Copy ARGB image data with stride {0} to GDI bitmap data with stride {1}!", Stride, imgData.Stride));
                IntPtr start = imgData.Scan0;
                int index = 0;
                for (int y = 0; y < Height; y++)
                {
                    Marshal.Copy(Data, index, start, Data.Length);
                    index += Stride;
                    start = new IntPtr(start.ToInt64() + imgData.Stride);
                }
            }
            else
            {
                Marshal.Copy(Data, 0, imgData.Scan0, Data.Length);
            }
        }

        /// <summary>
        /// Copies the image to the specified bitmap
        /// </summary>
        public void CopyToBitmap(System.Drawing.Bitmap img)
        {
            var data = img.LockBits(new System.Drawing.Rectangle(System.Drawing.Point.Empty, img.Size), System.Drawing.Imaging.ImageLockMode.WriteOnly, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            CopyTo32BitBitmapData(data);
            img.UnlockBits(data);
        }

        /// <summary>
        /// Creates a new bitmap from the image
        /// </summary>
        public System.Drawing.Bitmap ToGdiBitmap()
        {
            var result = new System.Drawing.Bitmap(Width, Height, System.Drawing.Imaging.PixelFormat.Format32bppArgb);
            CopyToBitmap(result);
            return result;
        }
#endif
    }
}

