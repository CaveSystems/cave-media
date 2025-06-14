using System;
using System.IO;

#nullable enable

namespace Cave.Media.Audio.ID3;

/// <summary>Provides all available Flags for ID3v2 Extended Headers (any version).</summary>
public class ID3v2ExtendedHeaderFlags
{
    #region Private Methods

    static byte[] ReadCRC32(byte[] data, ref int i)
    {
        var buffer = ReadFlag(data, ref i);
        if (buffer.Length != 5)
        {
            throw new InvalidDataException(string.Format("Invalid CRC32 data!"));
        }

        for (var n = 0; n < 5; n++)
        {
            if ((buffer[i] & 0x80) != 0)
            {
                throw new InvalidDataException(string.Format("Invalid CRC32 data {0}!", StringExtensions.ToHexString(buffer)));
            }
        }

        var result = new byte[4];
        result[0] = (byte)((buffer[0] << 4) | ((buffer[1] >> 3) & 0xF));
        result[1] = (byte)((buffer[1] << 5) | ((buffer[2] >> 2) & 0x1F));
        result[2] = (byte)((buffer[2] << 6) | ((buffer[3] >> 1) & 0x3F));
        result[3] = (byte)((buffer[3] << 7) | (buffer[4] & 0x7F));
        return result;
    }

    static byte[] ReadFlag(byte[] data, ref int i)
    {
        var len = data[i];
        var result = new byte[len];
        Array.Copy(data, i, result, 0, len);
        i += len;
        return result;
    }

    static void SkipFlag(byte[] data, ref int i)
    {
        var len = data[i];
        i += len;
    }

    #endregion Private Methods

    #region Public Fields

    /// <summary>
    /// <para>null: There is no CRC data. byte[4]: CRC data of the frame.</para>
    /// [Present since 2.3] <br/> If this flag is set, a CRC-32 [ISO-3309] data is included in the extended header. The CRC is calculated on all the data
    /// between the header and footer as indicated by the header's tag length field, minus the extended header. Note that this includes the padding (if there is
    /// any), but excludes the footer.
    /// </summary>
    public readonly byte[]? CRC32;

    /// <summary>
    /// <para>
    /// 0: Tag is not an update.
    /// 1: Tag is an update.
    /// </para>
    /// [Present since 2.4] <br/> If this flag is set, the present tag is an update of a tag found earlier in the present file or stream. If frames defined as
    /// unique are found in the present tag, they are to override any corresponding ones found in the earlier tag. This flag has no corresponding data.
    /// </summary>
    public readonly bool TagIsUpdate;

    /// <summary>
    /// <para>
    /// 0: Tag is not restricted. <br/>
    /// 1: Tag is restricted.
    /// </para>
    /// [Present since 2.4] <br/> For some applications it might be desired to restrict a tag in more ways than imposed by the ID3v2 specification. Note that
    /// the presence of these restrictions does not affect how the tag is decoded, merely how it was restricted before encoding. If this flag is set the tag is
    /// restricted as follows:.
    /// </summary>
    public readonly ID3v2ExtendedHeaderRestrictions? TagRestrictions;

    #endregion Public Fields

    #region Public Constructors

    /// <summary>Creates new empty ID3v2ExtendedHeaderFlags.</summary>
    public ID3v2ExtendedHeaderFlags()
    {
    }

    /// <summary>Creates new ID3v2ExtendedHeaderFlags with the specified settings.</summary>
    /// <param name="cRC32">The crc32 of all frames (tag without headers, padding and footer).</param>
    public ID3v2ExtendedHeaderFlags(byte[] cRC32)
    {
        CRC32 = cRC32;
    }

    /// <summary>Creates new ID3v2ExtendedHeaderFlags with the specified settings.</summary>
    /// <param name="tagIsUpdate"></param>
    /// <param name="crc32">The crc32 of all frames (tag without headers, padding and footer).</param>
    /// <param name="tagRestrictions"></param>
    public ID3v2ExtendedHeaderFlags(bool tagIsUpdate, byte[]? crc32, ID3v2ExtendedHeaderRestrictions? tagRestrictions)
    {
        TagIsUpdate = tagIsUpdate;
        CRC32 = crc32;
        TagRestrictions = tagRestrictions;
    }

    #endregion Public Constructors

    #region Public Methods

    /// <summary>Loads ID3v2ExtendedHeaderFlags from the specified ID3v2.3 extended header.</summary>
    /// <param name="extendedHeader"></param>
    /// <returns></returns>
    public static ID3v2ExtendedHeaderFlags FromID3v23(byte[] extendedHeader)
    {
        if (extendedHeader == null)
        {
            throw new ArgumentNullException("ExtendedHeader");
        }

        var index = 10;
        var extFlags = (extendedHeader[4] << 8) | extendedHeader[5];

        // crc present?
        if ((extFlags & 0x8000) != 0)
        {
            var crc32 = new byte[4];
            for (var i = 0; i < 4; i++)
            {
                crc32[i] = extendedHeader[index++];
            }
            return new ID3v2ExtendedHeaderFlags(crc32);
        }
        return new ID3v2ExtendedHeaderFlags();
    }

    /// <summary>Loads ID3v2ExtendedHeaderFlags from the specified ID3v2.4 extended header.</summary>
    /// <param name="extendedHeader"></param>
    /// <returns></returns>
    public static ID3v2ExtendedHeaderFlags FromID3v24(byte[] extendedHeader)
    {
        if (extendedHeader == null)
        {
            throw new ArgumentNullException("ExtendedHeader");
        }

        if (extendedHeader[4] != 1)
        {
            throw new InvalidDataException(string.Format("Invalid number of flag bytes!"));
        }

        var i = 6;

        // flag 1000 0000
        if ((extendedHeader[5] & 0x80) != 0)
        {
            SkipFlag(extendedHeader, ref i);
        }

        // flag 0100 0000
        var tagIsUpdate = (extendedHeader[5] & 0x40) != 0;
        if (tagIsUpdate)
        {
            SkipFlag(extendedHeader, ref i);
        }

        // flag 0010 0000
        byte[]? crc32;
        if ((extendedHeader[5] & 0x20) != 0)
        {
            crc32 = ReadCRC32(extendedHeader, ref i);
        }
        else
        {
            crc32 = null;
        }

        // flag 0001 0000
        ID3v2ExtendedHeaderRestrictions? tagRestrictions;
        if ((extendedHeader[5] & 0x10) == 0)
        {
            tagRestrictions = null;
        }
        else
        {
            var bytes = ReadFlag(extendedHeader, ref i);
            if (bytes.Length != 1)
            {
                throw new InvalidDataException(string.Format("Invalid data length"));
            }

            tagRestrictions = ID3v2ExtendedHeaderRestrictions.FromID3v24(bytes[0]);
        }

        // read unknown flags (0000 xxxx)
        for (var n = 0x08; n != 0; n >>= 1)
        {
            SkipFlag(extendedHeader, ref i);
        }
        if (i != extendedHeader.Length)
        {
            throw new InvalidDataException(string.Format("Unexpected additional data at flag!"));
        }

        return new ID3v2ExtendedHeaderFlags(tagIsUpdate, crc32, tagRestrictions);
    }

    #endregion Public Methods
}
