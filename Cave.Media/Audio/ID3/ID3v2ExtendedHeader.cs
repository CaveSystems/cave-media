using System;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3;

/// <summary>
/// The extended header contains information that can provide further insight in the structure of the tag, but is not vital to the correct parsing of the tag
/// information; hence the extended header is optional.
/// </summary>
public sealed class ID3v2ExtendedHeader : MP3MetaFrame
{
    #region Private Fields

    readonly ID3v2Header header;
    byte[]? data;
    ID3v2ExtendedHeaderFlags flags = new ID3v2ExtendedHeaderFlags();

    #endregion Private Fields

    #region Private Methods

    bool ParseVersion2(DataFrameReader reader) => throw new NotImplementedException("TODO");

    bool ParseVersion3(DataFrameReader reader)
    {
        if (!reader.EnsureBuffer(4))
        {
            return false;
        }

        // calc size
        var sizeBytes = reader.Read(0, 4);
        var size = 0;
        for (var i = 0; i < 4; i++)
        {
            size = (size << 8) | sizeBytes[i];
        }

        size += 4;

        // get data
        if (!reader.EnsureBuffer(size))
        {
            return false;
        }

        data = reader.GetBuffer(size);

        // get flags
        flags = ID3v2ExtendedHeaderFlags.FromID3v23(data);
        return true;
    }

    bool ParseVersion4(DataFrameReader reader)
    {
        if (!reader.EnsureBuffer(4))
        {
            return false;
        }

        // calc size
        var size = ID3v2DeUnsync.Int32(reader.Read(0, 4), 0);

        // get data
        if (!reader.EnsureBuffer(size))
        {
            return false;
        }

        data = reader.GetBuffer(size);

        // get flags
        flags = ID3v2ExtendedHeaderFlags.FromID3v24(data);
        return true;
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Creates a new empty instance.</summary>
    /// <param name="header"></param>
    public ID3v2ExtendedHeader(ID3v2Header header)
    {
        this.header = header ?? throw new ArgumentNullException("Header");
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>TODO.</summary>
    /// <returns></returns>
    public override byte[] Data
    {
        get
        {
            if (data == null)
            {
                switch (header.Version)
                {
                    // TODO: implement data creation
                    default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", header.Version));
                }
            }
            return (byte[])data.Clone();
        }
    }

    /// <summary>Gets/sets the ID3v2 revision used.</summary>
    public ID3v2ExtendedHeaderFlags Flags
    {
        get => flags;
        set
        {
            flags = value;
            data = null;
        }
    }

    /// <summary>Returns false (extended header may vary in size).</summary>
    public override bool IsFixedLength => false;

    /// <summary>Size of the extended header.</summary>
    public override int Length => data?.Length ?? 0;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Parses the specified buffer starting at index to load all data for this frame.</summary>
    /// <param name="reader">FrameReader to read from.</param>
    public override bool Parse(DataFrameReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("Stream");
        }

        flags = new ID3v2ExtendedHeaderFlags();

        switch (header.Version)
        {
            case 2: return ParseVersion2(reader);
            case 3: return ParseVersion3(reader);
            case 4: return ParseVersion4(reader);
            default: return false;
        }
    }

    #endregion Public Methods
}
