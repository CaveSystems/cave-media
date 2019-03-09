using System.Collections.Generic;

namespace Cave.Media.Audio.ID3
{

    sealed class ID3v2DeUnsync
    {
        public static int Int32(byte[] data, int start)
        {
            int value = 0;
            for (int i = 0; i < 4; i++)
            {
                value <<= 7;
                value |= data[start + i];
            }
            return value;
        }

        public static byte[] Buffer(byte[] data)
        {
            List<byte> buffer = new List<byte>(data.Length);
            bool unsync = false;
            foreach (byte b in data)
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
            byte[] result = new byte[unsyncedLength];
            int n = start;
            for (int i = 0; i < result.Length; i++)
            {
                byte b = reader.ReadByte(n++);
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
}
