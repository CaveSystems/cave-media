using System;
using System.Collections.Generic;

namespace Cave.Media.Audio.ID3;

sealed class ID3v2EnUnsync
{
    public static void Int32(int value, byte[] data, int start)
    {
        for (var i = 3; i >= 0; i--)
        {
            data[start + i] = (byte)value;
            value >>= 7;
        }
        if (value > 0)
        {
            throw new Exception(string.Format("Invalid value!"));
        }
    }

    public static byte[] Buffer(byte[] data)
    {
        var buffer = new List<byte>(data.Length * 2);
        foreach (var b in data)
        {
            buffer.Add(b);
            if (b == 0xFF)
            {
                buffer.Add(0);
            }
        }
        return buffer.ToArray();
    }
}
