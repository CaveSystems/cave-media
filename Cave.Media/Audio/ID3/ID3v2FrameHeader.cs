using System;
using System.IO;

namespace Cave.Media.Audio.ID3;

/// <summary>Provides a ID3v2 frame header structure.</summary>
public class ID3v2FrameHeader
{
    #region Private Fields

    byte[] data;

    #endregion Private Fields

    #region Private Methods

    void ParseVersion2(byte[] data)
    {
        HeaderSize = 6;
        ID = ASCII.GetString(data, 0, 3);
        Flags = new ID3v2FrameFlags();
        var size = 0;
        for (var i = 3; i < 6; i++)
        {
            size = (size << 8) | data[i];
        }

        ContentSize = size;
    }

    void ParseVersion3(byte[] data)
    {
        HeaderSize = 10;
        ID = ASCII.GetString(data, 0, 4);
        var flags = (ID3v2d3FrameFlags)((data[8] << 8) | data[9]);
        Flags = ID3v2FrameFlags.FromID3v2d3(flags);
        var size = 0;
        for (var i = 4; i < 8; i++)
        {
            size = (size << 8) | data[i];
        }

        ContentSize = size;
    }

    void ParseVersion4(byte[] data)
    {
        HeaderSize = 10;
        ID = ASCII.GetCleanString(data, 0, 4);
        var flags = (ID3v2d4FrameFlags)((data[8] << 8) | data[9]);
        Flags = ID3v2FrameFlags.FromID3v2d4(flags);
        var size = 0;
        for (var i = 4; i < 8; i++)
        {
            if (data[i] > 0x7F)
            {
                throw new InvalidDataException("ID3v2.4 Header with invalid non-unsynced size value found.");
            }
            size = (size << 7) | data[i];
        }
        ContentSize = size;
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="ID3v2FrameHeader"/> class.</summary>
    /// <param name="header">The header.</param>
    /// <param name="reader">The reader.</param>
    /// <exception cref="NotSupportedException"></exception>
    public ID3v2FrameHeader(ID3v2Header header, DataFrameReader reader) : this(header, header.Version switch
    {
        2 => reader.Read(0, 6),
        3 or 4 => reader.Read(0, 10),
        _ => throw new NotSupportedException(string.Format("Unsupported ID3v2 Version {0}", header.Version))
    })
    { /* no body required */ }

    /// <summary>Initializes a new instance of the <see cref="ID3v2FrameHeader"/> class.</summary>
    /// <param name="header">The header.</param>
    /// <param name="data">The data.</param>
    /// <exception cref="NotSupportedException"></exception>
    public ID3v2FrameHeader(ID3v2Header header, byte[] data)
    {
        this.data = data ?? throw new ArgumentNullException(nameof(data));
        TagHeader = header;
        switch (header.Version)
        {
            case 2: ParseVersion2(data); break;
            case 3: ParseVersion3(data); break;
            case 4: ParseVersion4(data); break;
            default: throw new NotSupportedException(string.Format("Unsupported ID3v2 Version {0}", header.Version));
        }
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the size of the frame.</summary>
    /// <value>The size of the frame.</value>
    public int ContentSize { get; private set; }

    /// <summary>Gets the data.</summary>
    /// <value>The data.</value>
    public byte[] Data => (byte[])data.Clone();

    /// <summary>Gets the flags.</summary>
    /// <value>The flags.</value>
    public ID3v2FrameFlags Flags { get; private set; } = new();

    /// <summary>Gets the size of the header.</summary>
    /// <value>The size of the header.</value>
    public int HeaderSize { get; private set; }

    /// <summary>Gets or sets the identifier.</summary>
    /// <value>The identifier.</value>
    public string ID { get; set; } = string.Empty;

    /// <summary>Gets the global tag header.</summary>
    /// <value>The tag header.</value>
    public ID3v2Header TagHeader { get; private set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Creates the specified header.</summary>
    /// <param name="header">The header.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="contentSize">Size of the frame content.</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static ID3v2FrameHeader Create(ID3v2Header header, string id, ID3v2FrameFlags flags, int contentSize)
    {
        switch (header.Version)
        {
            case 2: return CreateVersion2(header, id, contentSize);
            case 3: return CreateVersion3(header, id, flags, contentSize);
            case 4: return CreateVersion4(header, id, flags, contentSize);
            default: throw new NotSupportedException(string.Format("Unsupported ID3v2 Version {0}", header.Version));
        }
    }

    /// <summary>Creates a version2 header.</summary>
    /// <param name="header">The header.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="contentSize">Size of the frame content.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid identifier!.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ID3v2FrameHeader CreateVersion2(ID3v2Header header, string id, int contentSize)
    {
        if (id.Length != 3)
        {
            throw new ArgumentException("Invalid identifier!", nameof(id));
        }

        var data = ASCII.GetBytes(id + "  ");
        for (var i = 5; i >= 3; i--)
        {
            data[i] = (byte)(contentSize & 0xFF);
            contentSize >>= 8;
        }
        if (contentSize > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(contentSize));
        }

        return new ID3v2FrameHeader(header, data);
    }

    /// <summary>Creates a version3 header.</summary>
    /// <param name="header">The header.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="contentSize">Size of the frame content.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid identifier!.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ID3v2FrameHeader CreateVersion3(ID3v2Header header, string id, ID3v2FrameFlags flags, int contentSize)
    {
        if (id.Length != 4)
        {
            throw new ArgumentException("Invalid identifier!", nameof(id));
        }

        var data = ASCII.GetBytes(id + "      ");
        var f = (ushort)flags.ToID3v2d3Flags();
        data[9] = (byte)(f & 0xFF);
        data[8] = (byte)(f >> 8);
        for (var i = 7; i >= 4; i--)
        {
            data[i] = (byte)(contentSize & 0xFF);
            contentSize >>= 8;
        }
        if (contentSize > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(contentSize));
        }

        return new ID3v2FrameHeader(header, data);
    }

    /// <summary>Creates a version4 header.</summary>
    /// <param name="header">The header.</param>
    /// <param name="id">The identifier.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="contentSize">Size of the frame content.</param>
    /// <returns></returns>
    /// <exception cref="ArgumentException">Invalid identifier!.</exception>
    /// <exception cref="ArgumentOutOfRangeException"></exception>
    public static ID3v2FrameHeader CreateVersion4(ID3v2Header header, string id, ID3v2FrameFlags flags, int contentSize)
    {
        if (id.Length != 4)
        {
            throw new ArgumentException("Invalid identifier!", nameof(id));
        }

        var data = ASCII.GetBytes(id + "      ");
        var f = (ushort)flags.ToID3v2d4Flags();
        data[9] = (byte)(f & 0xFF);
        data[8] = (byte)(f >> 8);
        for (var i = 7; i >= 4; i--)
        {
            data[i] = (byte)(contentSize & 0x7F);
            contentSize >>= 7;
        }
        if (contentSize > 0)
        {
            throw new ArgumentOutOfRangeException(nameof(contentSize));
        }

        return new ID3v2FrameHeader(header, data);
    }

    /// <summary>Gets the size of the header.</summary>
    /// <param name="header">The header.</param>
    /// <returns></returns>
    /// <exception cref="NotSupportedException"></exception>
    public static int GetHeaderSize(ID3v2Header header)
    {
        switch (header.Version)
        {
            case 2: return 6;
            case 3:
            case 4: return 10;
            default: throw new NotSupportedException(string.Format("Unsupported ID3v2 Version {0}", header.Version));
        }
    }

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString() => $"ID3v2.{TagHeader.Version}Frame {ID} [{ContentSize}]";

    #endregion Public Methods
}
