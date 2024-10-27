using System;
using System.IO;
using System.Linq;
using Cave.IO;

namespace Cave.Media;

/// <summary>Provides very simple finger printing for images.</summary>
public class FingerPrint
{
    #region Private Fields

    static readonly IBitConverter Converter = BigEndian.Converter;
    string? base32data;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="FingerPrint"/> class.</summary>
    /// <param name="pixelSize">Size in pixels.</param>
    /// <param name="blocks">The blocks.</param>
    /// <param name="data">The data.</param>
    public FingerPrint(int pixelSize, uint[] blocks, byte[] data)
    {
        PixelSize = pixelSize;
        Blocks = blocks;
        Data = data;
        if (data.Length != ((pixelSize * pixelSize * 6) + 7) / 8)
        {
            throw new ArgumentOutOfRangeException("data.Length", "Data length is out of range!");
        }
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the 4 bits blocks (1 bits per 4x4 pin).</summary>
    public uint[] Blocks { get; private set; }

    /// <summary>Gets the full fingerprint data.</summary>
    public byte[] Data { get; private set; }

    /// <summary>Gets a name for the image file.</summary>
    public string FileName => Blocks.Select(b => b.ToHexString()).Join('-');

    /// <summary>Gets a identifier for the fingerprint (this is not collision free).</summary>
    public Guid Guid
    {
        get
        {
            var data = new byte[16];
            for (var i = 0; i < 4; i++)
            {
                Converter.GetBytes(Blocks[i]).CopyTo(data, i << 2);
            }

            return new Guid(data);
        }
    }

    /// <summary>Gets the size in pixels.</summary>
    public int PixelSize { get; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates a 32x32 fingerprint for the specified bitmap.</summary>
    /// <param name="bitmap">The bitmap.</param>
    /// <returns>Returns a fingerprint with 6 bits per pixel (32 px = 6144 bit = 768 byte = 1024 base32 chars).</returns>
    public static unsafe FingerPrint Create(IBitmap32 bitmap)
    {
        using var thumb = bitmap.Resize(32, 32, ResizeMode.TouchFromInside);
        var data = thumb.GetImageData();
        using var ms = new MemoryStream();
        // calculate fingerprint and distance matrix
        var writer = new BitStreamWriter(ms);
        var distanceMatrix = new float[16];
        {
            int x = 0, y = 0;
            ARGB last = 0;
            for (var n = 0; n < data.PixelCount; n++)
            {
                var pixel = data.Pixels2[n];
                if (++x > 15)
                {
                    x = 0;
                    ++y;
                }

                var r = pixel.Red >> 6;
                var g = pixel.Green >> 6;
                var b = pixel.Blue >> 6;
                writer.WriteBits(r, 2);
                writer.WriteBits(g, 2);
                writer.WriteBits(b, 2);

                unchecked
                {
                    var i = ((y << 1) & 0xC) + (x >> 2);
                    var distance = Math.Abs(pixel.GetDistance(last));
                    distanceMatrix[i] += distance;
                    last = pixel;
                }
            }
        }

        // normalize matrix
        var maxDistance = distanceMatrix.Max();
        for (var i = 0; i < distanceMatrix.Length; i++)
        {
            distanceMatrix[i] /= maxDistance;
        }

        // calculate blocks
        var blocks = new uint[4];
        var index = new int[] { 0, 2, 8, 10 };
        for (var i = 0; i < 4; i++)
        {
            var idx = index[i];
            var blockValue = (uint)(255 * distanceMatrix[idx]) << 24;
            blockValue |= (uint)(255 * distanceMatrix[idx + 1]) << 16;
            blockValue |= (uint)(255 * distanceMatrix[idx + 4]) << 8;
            blockValue |= (uint)(255 * distanceMatrix[idx + 5]);
            blocks[i] = blockValue;
        }

        return new FingerPrint(32, blocks, ms.ToArray());
    }

    /// <summary>Determines whether the specified <see cref="object"/>, is equal to this instance.</summary>
    /// <param name="obj">The <see cref="object"/> to compare with this instance.</param>
    /// <returns><c>true</c> if the specified <see cref="object"/> is equal to this instance; otherwise, <c>false</c>.</returns>
    public override bool Equals(object? obj) => obj is FingerPrint other && ToString().Equals(other.ToString());

    /// <summary>Returns a hash code for this instance.</summary>
    /// <returns>A hash code for this instance, suitable for use in hashing algorithms and data structures like a hash table.</returns>
    public override int GetHashCode() => ToString().GetHashCode();

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        if (base32data == null)
        {
            base32data = Base32.Safe.Encode(Data);
        }
        return base32data;
    }

    #endregion Public Methods
}
