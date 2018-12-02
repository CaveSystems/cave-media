
namespace Cave.Media.Structs
{
    /// <summary>
    /// Compression formats for DIBs
    /// </summary>
    public enum BITMAPCOMPRESSION : int
    {
        /// <summary>
        /// No compression
        /// </summary>
        BI_RGB = 0,

        /// <summary>
        /// Can be used only with 8-bit/pixel bitmaps
        /// </summary>
        BI_RLE8 = 1,

        /// <summary>
        /// Can be used only with 4-bit/pixel bitmaps
        /// </summary>
        BI_RLE4 = 2,

        /// <summary>
        /// Can be used only with 16 and 32-bit/pixel bitmaps
        /// </summary>
        BI_BITFIELDS = 3,

        /// <summary>
        /// The bitmap contains a JPEG image
        /// </summary>
        BI_JPEG = 4,

        /// <summary>
        /// The bitmap contains a PNG image
        /// </summary>
        BI_PNG = 5,
    }
}
