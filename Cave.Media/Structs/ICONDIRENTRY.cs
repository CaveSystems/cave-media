using System;
using System.IO;
using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// ICONDIRENTRY structure.
/// </summary>
[StructLayout(LayoutKind.Sequential, Size = 16)]
public struct ICONDIRENTRY
{
    /// <summary>
    /// Gets the size of the structure.
    /// </summary>
    public const int StructureSize = 16;

    /// <summary>
    /// Reads the ICONDIRENTRY from the specified stream.
    /// </summary>
    /// <param name="stream"></param>
    /// <returns></returns>
    public static ICONDIRENTRY FromStream(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        var buffer = new byte[16];
        if (stream.Read(buffer, 0, 16) != 16)
        {
            throw new EndOfStreamException();
        }
        var bufferPtr = Marshal.AllocHGlobal(16);
        Marshal.Copy(buffer, 0, bufferPtr, 16);
        var result = (ICONDIRENTRY)Marshal.PtrToStructure(bufferPtr, typeof(ICONDIRENTRY));
        Marshal.FreeHGlobal(bufferPtr);
        return result;
    }

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

    /// <summary>
    /// "ICON[&lt;Bytes&gt;] (&lt;Width&gt;x&lt;Height&gt;).
    /// </summary>
    /// <returns></returns>
    public override string ToString() => string.Format("ICON[{0}] ({1}x{2})", Size, Width == 0 ? 256 : Width, Height == 0 ? 256 : Height);

    /// <summary>
    /// Gets the structure as byte array.
    /// </summary>
    /// <returns></returns>
    public byte[] ToArray()
    {
        var buffer = new byte[16];
        var bufferPtr = Marshal.AllocHGlobal(16);
        Marshal.StructureToPtr(this, bufferPtr, true);
        Marshal.Copy(bufferPtr, buffer, 0, 16);
        Marshal.FreeHGlobal(bufferPtr);
        return buffer;
    }

    /// <summary>
    /// Saves the structure to a stream.
    /// </summary>
    /// <param name="stream"></param>
    public void SaveTo(Stream stream)
    {
        if (stream == null)
        {
            throw new ArgumentNullException("stream");
        }

        stream.Write(ToArray(), 0, 16);
    }
}
