using System;
using System.IO;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3;

/// <summary>
/// Provides an ID3v2 footer implementation. <br/> To speed up the process of locating an ID3v2 tag when searching from the end of a file, a footer can be added
/// to the tag. It is REQUIRED to add a footer to an appended tag, i.e. a tag located after all audio data. The footer is a copy of the header, but with a
/// different identifier.
/// </summary>
public class ID3v2Footer : MP3MetaFrame
{
    #region Private Fields

    int bodySize;
    byte[]? data;
    ID3v2HeaderFlags flags;
    byte revision;
    byte version;

    #endregion Private Fields

    #region Private Methods

    ID3v2HeaderFlags CheckFlags(byte b)
    {
        switch (version)
        {
            case 0:
            case 1:
                throw new InvalidDataException(string.Format("Invalid ID3v2.{0} tag!", version));

            case 2:
                if ((b & 0x3F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", version));
                }

                break;

            case 3:
                if ((b & 0x1F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", version));
                }

                break;

            case 4:
                if ((b & 0x0F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", version));
                }

                break;
        }
        return (ID3v2HeaderFlags)b;
    }

    #endregion Private Methods

    #region Protected Methods

    /// <summary>Internally parses the current data and loads all fields.</summary>
    protected void ParseData()
    {
        if (data is null) throw new NullReferenceException("Data is unset!");
        if ((data[0] != (byte)'3') || (data[1] != (byte)'D') || (data[2] != (byte)'I'))
        {
            throw new InvalidDataException(string.Format("Missing ID3 identifier!"));
        }

        version = data[3];
        revision = data[4];
        flags = CheckFlags(data[5]);
        bodySize = ID3v2DeUnsync.Int32(data, 6);
    }

    #endregion Protected Methods

    #region Public Properties

    /// <summary>
    /// The ID3v2 tag size is the size of the complete tag after unsychronisation, including padding, excluding the header but not excluding the extended header
    /// (total tag size - 10). Only 28 bits (representing up to 256MB) are used in the size description to avoid the introducuction of 'false syncsignals'.
    /// </summary>
    public int BodySize
    {
        get => bodySize;
        set
        {
            bodySize = value;
            data = null;
        }
    }

    /// <summary>Gets an array with the data for this instance.</summary>
    /// <returns></returns>
    public override byte[] Data
    {
        get
        {
            if (data == null)
            {
                switch (version)
                {
                    case 2:
                        flags = (ID3v2HeaderFlags)((int)flags & 0xC0);
                        break;

                    case 3:
                        flags = (ID3v2HeaderFlags)((int)flags & 0xE0);
                        break;

                    case 4:
                        flags = (ID3v2HeaderFlags)((int)flags & 0xF0);
                        break;

                    default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", version));
                }
                data = new byte[10];
                data[0] = (byte)'3';
                data[1] = (byte)'D';
                data[2] = (byte)'I';
                data[3] = version;
                data[4] = 0;
                data[5] = (byte)((int)flags & 0xF0);
                ID3v2EnUnsync.Int32(bodySize, data, 6);
                ParseData();
            }
            return (byte[])data.Clone();
        }
    }

    /// <summary>Gets/sets the ID3v2 revision used.</summary>
    public ID3v2HeaderFlags Flags
    {
        get => flags;
        set
        {
            flags = value;
            data = null;
        }
    }

    /// <summary>Returns true.</summary>
    public override bool IsFixedLength => true;

    /// <summary>returns 10.</summary>
    public override int Length => 10;

    /// <summary>Gets/sets the ID3v2 revision used.</summary>
    public byte Revision
    {
        get => revision;
        set
        {
            revision = value;
            data = null;
        }
    }

    /// <summary>Gets/sets the ID3v2 (major) version used.</summary>
    public byte Version
    {
        get => version;
        set
        {
            version = value;
            data = null;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Parses the specified buffer starting at index to load all data for this frame This function will throw exceptions on parser errors.</summary>
    /// <param name="reader">FrameReader to read from.</param>
    public override bool Parse(DataFrameReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("Reader");
        }

        if (!reader.EnsureBuffer(10))
        {
            return false;
        }

        data = reader.Read(0, 10);
        ParseData();
        reader.Remove(10);
        return false;
    }

    #endregion Public Methods
}
