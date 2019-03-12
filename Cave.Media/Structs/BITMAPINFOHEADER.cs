using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// This structure contains information about the dimensions and color format of a device-independent bitmap (DIB).
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct BITMAPINFOHEADER
    {
        /// <summary>
        /// Specifies the size of the structure, in bytes.
        /// </summary>
        public uint Size;

        /// <summary>
        /// Specifies the width of the bitmap, in pixels.
        /// </summary>
        public int Width;

        /// <summary>
        /// Specifies the height of the bitmap, in pixels.
        /// </summary>
        public int Height;

        /// <summary>
        /// Specifies the number of planes for the target device.
        /// </summary>
        public short Planes;

        /// <summary>
        /// Specifies the number of bits per pixel.
        /// <list type="bullet">
        /// <item>1 = The bitmap is monochrome, and the bmiColors member contains two entries.</item>
        /// <item>2 = The bitmap has four possible color values.</item>
        /// <item>4 = The bitmap has a maximum of 16 colors, and the bmiColors member contains up to 16 entries.</item>
        /// <item>8 = The bitmap has a maximum of 256 colors, and the bmiColors member contains up to 256 entries. In this case, each byte in the array represents a single pixel.</item>
        /// <item>16 = The bitmap has a maximum of 2^16 colors.</item>
        /// <item>24 = The bitmap has a maximum of 2^24 colors, and the bmiColors member is NULL.</item>
        /// <item>32 = The bitmap has a maximum of 2^32 colors. If the biCompression member of the BITMAPINFOHEADER is BI_RGB, the bmiColors member is NULL. Each DWORD in the bitmap array represents the relative intensities of blue, green, and red, respectively, for a pixel. The high byte in each DWORD is not used. The bmiColors color table is used for optimizing colors used on palette-based devices, and must contain the number of entries specified by the biClrUsed member of the BITMAPINFOHEADER.</item>
        /// </list>
        /// </summary>
        public short BitCount;

        /// <summary>
        /// The <see cref="BITMAPCOMPRESSION"/> used.
        /// </summary>
        public BITMAPCOMPRESSION Compression;

        /// <summary>
        /// Specifies the size, in bytes, of the image.
        /// </summary>
        public uint SizeImage;

        /// <summary>
        /// Specifies the horizontal resolution, in pixels per meter, of the target device for the bitmap.
        /// </summary>
        public int XPelsPerMeter;

        /// <summary>
        /// Specifies the vertical resolution, in pixels per meter, of the target device for the bitmap.
        /// </summary>
        public int YPelsPerMeter;

        /// <summary>
        /// Specifies the number of color indexes in the color table that are actually used by the bitmap.
        /// </summary>
        public uint ClrUsed;

        /// <summary>
        /// Specifies the number of color indexes required for displaying the bitmap.
        /// </summary>
        public uint ClrImportant;
    }
}
