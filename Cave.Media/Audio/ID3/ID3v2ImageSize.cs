namespace Cave.Media.Audio.ID3;

/// <summary>Provides idiotic image size restrictions.</summary>
public sealed class ID3v2ImageSize
{
    #region Private Constructors

    ID3v2ImageSize(int width, int height, bool isFixed)
    {
        Width = width;
        Height = height;
        Fixed = isFixed;
    }

    #endregion Private Constructors

    #region Public Fields

    /// <summary>Gets whether the image has exactly the specified <see cref="Width"/> and <see cref="Height"/> (true) or if it may be smaller (false).</summary>
    public readonly bool Fixed;

    /// <summary>Height of the image (check <see cref="Fixed"/> if it may be smaller).</summary>
    public readonly int Height;

    /// <summary>Width of the image (check <see cref="Fixed"/> if it may be smaller).</summary>
    public readonly int Width;

    #endregion Public Fields

    #region Public Properties

    /// <summary>No restrictions at all, this one makes sense.</summary>
    public static ID3v2ImageSize None => new ID3v2ImageSize(0, 0, false);

    /// <summary>All images have the size 64x64.</summary>
    public static ID3v2ImageSize SizeFixed64 => new ID3v2ImageSize(64, 64, true);

    /// <summary>All images are smaller than 256x256.</summary>
    public static ID3v2ImageSize SizeVar256 => new ID3v2ImageSize(256, 256, false);

    /// <summary>All images are smaller than 64x64.</summary>
    public static ID3v2ImageSize SizeVar64 => new ID3v2ImageSize(64, 64, false);

    #endregion Public Properties
}
