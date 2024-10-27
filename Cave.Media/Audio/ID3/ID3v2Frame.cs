using System;

namespace Cave.Media.Audio.ID3;

/// <summary>
/// Provides an ID3v2 frame implementation.
/// </summary>
public class ID3v2Frame
{
    /// <summary>Initializes a new instance of the <see cref="ID3v2Frame"/> class.</summary>
    /// <param name="frame">The frame.</param>
    /// <exception cref="ArgumentNullException">Frame.</exception>
    public ID3v2Frame(ID3v2Frame frame)
    {
        if (frame == null)
        {
            throw new ArgumentNullException("Frame");
        }

        RawData = frame.RawData;
        Content = frame.Content;
        Header = frame.Header;
    }

    /// <summary>Initializes a new instance of the <see cref="ID3v2Frame" /> class.</summary>
    /// <param name="header">The header.</param>
    /// <param name="reader">The reader.</param>
    /// <exception cref="ArgumentNullException">Header.</exception>
    /// <exception cref="NotSupportedException">ID3v2.{0} is not supported!</exception>
    public ID3v2Frame(ID3v2Header header, DataFrameReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("Reader");
        }

        Header = new ID3v2FrameHeader(header, reader);

        // prepare content (has to be decoded, decrypted, decompressed, ...
        Content = reader.Read(Header.HeaderSize, Header.ContentSize);

        switch (header.Version)
        {
            case 2: /*nothing to do, raw plain content data*/ break;
            case 3: ParseVersion3(reader); break;
            case 4: ParseVersion4(reader); break;
            default: throw new NotSupportedException(string.Format("ID3v2.{0} is not supported!", header.Version));
        }

        // copy raw data and remove from reader
        RawData = reader.GetBuffer(Header.HeaderSize + Header.ContentSize);
    }

    /// <summary>Initializes a new instance of the <see cref="ID3v2Frame" /> class.</summary>
    /// <param name="header">The header.</param>
    /// <param name="data">The data.</param>
    public ID3v2Frame(ID3v2Header header, byte[] data)
    {
        Header = new ID3v2FrameHeader(header, data);
        if (Header.ContentSize + Header.HeaderSize != data.Length)
        {
            throw new ArgumentOutOfRangeException("data", $"Invalid size of data! Expected {Header.ContentSize + Header.HeaderSize} bytes, got {data.Length}!");
        }

        RawData = data;
        Content = new byte[Header.ContentSize];
        Array.Copy(RawData, Header.HeaderSize, Content, 0, Header.ContentSize);
    }

    #region parser functions

    /// <summary>
    /// Provides decompression.
    /// </summary>
    /// <param name="data">The data to be decompressed.</param>
    /// <returns>Retruns decompressed data.</returns>
    protected byte[] Decompress(byte[] data) => throw new NotSupportedException("ID3v2 Compressed Data is not jet supported!");

    /// <summary>
    /// Provides decryption.
    /// </summary>
    /// <param name="data">The data to be decompressed.</param>
    /// <returns>Retruns decrypted data.</returns>
    protected byte[] Decrypt(byte[] data) => throw new NotSupportedException("ID3v2 Encrypted Data is not jet supported!");

    void ParseVersion3(DataFrameReader reader)
    {
        if (Header.Flags.Compression)
        {
            Content = Decompress(Content);
        }

        if (Header.Flags.Encryption)
        {
            Content = Decrypt(Content);
        }
    }

    /// <summary>
    /// Parses the specified buffer starting at index to load all data for this frame
    /// This function will throw exceptions on parser errors.
    /// </summary>
    /// <param name="reader">FrameReader to read from.</param>
    void ParseVersion4(DataFrameReader reader)
    {
        if ((Header.TagHeader.Flags & ID3v2HeaderFlags.Unsynchronisation) == 0)
        {
            // no full unsync done, check if we have to unsync now
            if (Header.Flags.Unsynchronisation)
            {
                Content = ID3v2DeUnsync.Buffer(Content);
            }
        }
        if (Header.Flags.Compression)
        {
            Content = Decompress(Content);
        }

        if (Header.Flags.Encryption)
        {
            Content = Decrypt(Content);
        }
    }

    #endregion

    #region public properties

    /// <summary>Gets or sets the frame header.</summary>
    /// <value>The frame header.</value>
    public ID3v2FrameHeader Header { get; set; }

    /// <summary>Gets the identifier.</summary>
    /// <value>The identifier.</value>
    public string ID => Header.ID;

    /// <summary>Gets the flags.</summary>
    /// <value>The flags.</value>
    public ID3v2FrameFlags Flags => Header.Flags;

    /// <summary>Gets the length of the raw data including header and body encoded, encrypted, compressed, ...</summary>
    /// <value>The length of the raw data.</value>
    public int Length => Header.HeaderSize + Header.ContentSize;

    /// <summary>Gets the length of the content.</summary>
    /// <value>The length of the content.</value>
    public int ContentLength => Header.ContentSize;

    /// <summary>Gets or sets the raw data.</summary>
    /// <value>The raw data.</value>
    public byte[] RawData { get; set; }

    /// <summary>Gets or sets the (decrypted, decoded, deunsynced, uncompressed, ...) content.</summary>
    /// <value>The content.</value>
    public byte[] Content { get; set; }

    #endregion

    /// <summary>
    /// Gets a string describing this frame.
    /// </summary>
    /// <returns>ID[Length].</returns>
    public override string ToString() => Header.ToString();

    /// <summary>
    /// Gets the hashcode for this instance.
    /// </summary>
    /// <returns></returns>
    public override int GetHashCode() => RawData.GetHashCode();
}
