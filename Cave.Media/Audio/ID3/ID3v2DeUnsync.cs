using System.Collections.Generic;

namespace Cave.Media.Audio.ID3;

sealed class ID3v2DeUnsync
{
    public static int Int32(byte[] data, int start)
    {
        var value = 0;
        for (var i = 0; i < 4; i++)
        {
            value <<= 7;
            value |= data[start + i];
        }
        return value;
    }

    public static byte[] Buffer(byte[] data)
    {
        var buffer = new List<byte>(data.Length);
        var unsync = false;
        foreach (var b in data)
        {
            if (unsync)
            {
                buffer.Add(b);
                unsync = false;
                continue;
            }
            if (b == 0xFF)
            {
                unsync = true;
            }
            else
            {
                buffer.Add(b);
            }
        }
        return buffer.ToArray();
    }

    public static byte[] Reader(DataFrameReader reader, int start, int unsyncedLength)
    {
        var result = new byte[unsyncedLength];
        var n = start;
        for (var i = 0; i < result.Length; i++)
        {
            var b = reader.ReadByte(n++);
            if (b != 0xFF)
            {
                result[i] = b;
            }
            else
            {
                result[i] = reader.ReadByte(n++);
            }
        }
        return result;
    }
}
