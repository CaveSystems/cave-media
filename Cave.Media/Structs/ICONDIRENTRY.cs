using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// ICONDIRENTRY structure.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct ICONDIRENTRY
{
    /// <summary>
    /// Specifies image width in pixels. Can be any number between 0 and 255. Value 0 means image width is 256 pixels.
    /// </summary>
    public byte Width;

    /// <summary>
    /// Specifies image height in pixels. Can be any number between 0 and 255. Value 0 means image height is 256 pixels.
    /// </summary>
    public byte Height;

    /// <summary>
    /// Specifies number of colors in the color palette. Should be 0 if the image does not use a color palette.
    /// </summary>
    public byte Colors;

    /// <summary>
    /// Reserved. Should be 0.
    /// </summary>
    public byte Reserved;

    /// <summary>
    /// In ICO format: Specifies color planes. Should be 0 or 1<br />
    /// In CUR format: Specifies the horizontal coordinates of the hotspot in number of pixels from the left.
    /// </summary>
    public short ValueA;

    /// <summary>
    /// In ICO format: Specifies bits per pixel. <br />
    /// In CUR format: Specifies the vertical coordinates of the hotspot in number of pixels from the top.
    /// </summary>
    public short ValueB;

    /// <summary>
    /// Specifies the size of the image's data in bytes.
    /// </summary>
    public int Size;

    /// <summary>
    /// Specifies the offset of BMP or PNG data from the beginning of the ICO/CUR file.
    /// </summary>
    public int Offset;
}
