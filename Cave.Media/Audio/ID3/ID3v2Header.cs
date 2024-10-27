using System;
using System.IO;
using Cave.Media.Audio.MP3;

namespace Cave.Media.Audio.ID3;

/// <summary>Provides an ID3v2 header implementation.</summary>
public class ID3v2Header : MP3MetaFrame
{
    #region Private Fields

    int bodySize;
    byte[]? data;
    byte revision;

    #endregion Private Fields

    #region Private Methods

    ID3v2HeaderFlags CheckFlags(byte b)
    {
        switch (Version)
        {
            case 0:
            case 1:
                throw new InvalidDataException(string.Format("Invalid ID3v2.{0} tag!", Version));

            case 2:
                if ((b & 0x3F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
                }

                break;

            case 3:
                if ((b & 0x1F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
                }

                break;

            case 4:
                if ((b & 0x0F) != 0)
                {
                    throw new InvalidDataException(string.Format("Invalid flags present at ID3v2.{0} tag!", Version));
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
        if ((data[0] != (byte)'I') || (data[1] != (byte)'D') || (data[2] != (byte)'3'))
        {
            throw new InvalidDataException(string.Format("Missing ID3 identifier!"));
        }

        Version = data[3];
        revision = data[4];
        Flags = CheckFlags(data[5]);
        bodySize = ID3v2DeUnsync.Int32(data, 6);
    }

    #endregion Protected Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
    public ID3v2Header() { }

    /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
    /// <param name="data">The data.</param>
    public ID3v2Header(byte[] data)
    {
        this.data = new byte[10];
        Array.Copy(data, this.data, 10);
        ParseData();
    }

    /// <summary>Initializes a new instance of the <see cref="ID3v2Header"/> class.</summary>
    /// <param name="version">The version.</param>
    /// <param name="revision">The revision.</param>
    /// <param name="flags">The flags.</param>
    /// <param name="size">The size.</param>
    /// <exception cref="NotSupportedException"></exception>
    public ID3v2Header(byte version, byte revision, ID3v2HeaderFlags flags, int size)
    {
        Version = version;
        this.revision = revision;
        Flags = flags;
        bodySize = size;

        switch (Version)
        {
            case 2:
                Flags = (ID3v2HeaderFlags)((int)Flags & 0xC0);
                throw new NotImplementedException("Missing ID3v2.2 implementation.");
            case 3:
                Flags = (ID3v2HeaderFlags)((int)Flags & 0xE0);
                break;

            case 4:
                Flags = (ID3v2HeaderFlags)((int)Flags & 0xF0);
                break;

            default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", Version));
        }
        data = new byte[10];
        data[0] = (byte)'I';
        data[1] = (byte)'D';
        data[2] = (byte)'3';
        data[3] = Version;
        data[4] = 0;
        data[5] = (byte)((int)Flags & 0xF0);
        ID3v2EnUnsync.Int32(bodySize, data, 6);
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>
    /// The ID3v2 tag size is the size of the complete tag after unsychronisation, including padding, excluding the header but not excluding the extended header
    /// (total tag size - 10). Only 28 bits (representing up to 256MB) are used in the size description to avoid the introducuction of 'false syncsignals'.
    /// </summary>
    public int BodySize => bodySize;

    /// <summary>Gets an array with the data for this instance.</summary>
    /// <returns></returns>
    public override byte[] Data => data ?? [];

    /// <summary>Gets/sets the ID3v2 revision used.</summary>
    public ID3v2HeaderFlags Flags { get; private set; }

    /// <summary>Gets the size of the header.</summary>
    /// <value>The size of the header.</value>
    public int HeaderSize => 10;

    /// <summary>Returns true (header length is fixed).</summary>
    public override bool IsFixedLength => true;

    /// <summary>returns 10.</summary>
    public override int Length => 10;

    /// <summary>Gets/sets the ID3v2 revision used.</summary>
    public byte Revision => revision;

    /// <summary>Gets/sets the ID3v2 (major) version used.</summary>
    public byte Version { get; private set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Parses the specified buffer starting at index to load all data for this frame.</summary>
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
        return true;
    }

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString() => $"ID3v2 Header Version {Version} Revision {Revision} [{BodySize}] {Flags}";

    #endregion Public Methods
}
