using Cave.Text;
using System;
using System.IO;
using System.Text;

namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides available ID3v2 encodings
    /// </summary>
    
    public sealed class ID3v2Encoding
    {
        /// <summary>
        /// Obtains the default encoding
        /// </summary>
        public static Encoding ISO88591 { get { return Encoding.GetEncoding("ISO-8859-1"); } }

        /// <summary>
        /// Obtains a specific encoding
        /// </summary>
        /// <param name="encoding">The ID3v2 encoding number</param>
        /// <returns>The dotnet encoding instance</returns>
        public static Encoding Get(ID3v2EncodingType encoding)
        {
            switch (encoding)
            {
                case ID3v2EncodingType.ISO88591_OLD:
                case ID3v2EncodingType.ISO88591: return ISO88591;
                case ID3v2EncodingType.Unicode: return Encoding.Unicode;
                case ID3v2EncodingType.BigEndianUnicode: return Encoding.BigEndianUnicode;
                case ID3v2EncodingType.UTF8: return Encoding.UTF8;
                default: throw new NotSupportedException(string.Format("Encoding {0} is not supported!", encoding));
            }
        }

        /// <summary>Selects the best encoding for the given text.</summary>
        /// <param name="header">The header.</param>
        /// <param name="text">The text.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        /// <exception cref="NotImplementedException"></exception>
        internal static ID3v2EncodingType Select(ID3v2Header header, string text)
        {
            if (CanUseISO(text))
            {
                return ID3v2EncodingType.ISO88591;
            }
            else
            {
                switch (header.Version)
                {
                    case 3: return ID3v2EncodingType.Unicode;
                    case 4: return ID3v2EncodingType.Unicode;
                    default: throw new NotSupportedException();
                }
            }
        }

        /// <summary>Determines whether value can be encoded using iso encoding or not.</summary>
        /// <param name="value">The value.</param>
        /// <returns><c>true</c> if value can be represented in iso encoding; otherwise, <c>false</c>.</returns>
        public static bool CanUseISO(string value)
        {
            Encoding enc = Encoding.GetEncoding("ISO-8859-1");
            return (value == enc.GetString(enc.GetBytes(value)));
        }

        /// <summary>Gets the bytes.</summary>
        /// <param name="encoding">The encoding.</param>
        /// <param name="value">The value.</param>
        /// <param name="termination">if set to <c>true</c> [use string zero termination].</param>
        /// <returns></returns>
        public static byte[] GetBytes(ID3v2EncodingType encoding, string value, bool termination)
        {
            if (termination) value += "\0";
            return Get(encoding).GetBytes(value);
        }

        /// <summary>
        /// Obtains whether 8 bit char index search (true) shall be used or 16 bit (false)
        /// </summary>
        /// <param name="encoding">The ID3v2 encoding number</param>
        /// <returns>Returns true if 8 bit char index search may be used</returns>
        public static bool Is8Bit(ID3v2EncodingType encoding)
        {
            switch (encoding)
            {
                case ID3v2EncodingType.UTF8:
                case ID3v2EncodingType.ISO88591_OLD:
                case ID3v2EncodingType.ISO88591: return true;

                case ID3v2EncodingType.Unicode: 
                case ID3v2EncodingType.BigEndianUnicode: return false;

                default: throw new NotSupportedException(string.Format("Encoding {0} is not supported!", encoding));
            }
        }

        /// <summary>
        /// Finds the first null char at a 16bit charset
        /// </summary>
        /// <param name="data"></param>
        /// <param name="start"></param>
        /// <returns></returns>
        public static int IndexOfNull16Bit(byte[] data, int start)
        {
            if (data == null) throw new ArgumentNullException("Data");
            int l_Index = start;
            while (l_Index < data.Length)
            {
                if ((data[l_Index] == 0) && (data[l_Index + 1] == 0)) return l_Index;
                l_Index += 2;
            }
            return -1;
        }

        /// <summary>
        /// Parses a string from the specified data and returns the length in bytes
        /// </summary>
        /// <param name="encoding">The ID3v2 encoding number</param>
        /// <param name="data">The bytes to parse</param>
        /// <param name="index">The start index to begin at</param>
        /// <param name="text">The string output</param>
        /// <returns>Returns the number of bytes parsed</returns>
        public static int Parse(ID3v2EncodingType encoding, byte[] data, int index, out string text)
        {
            if (data == null) throw new ArgumentNullException("Data");
            int len;
            int l_MarkerLength;
            if (Is8Bit(encoding))
            {
                len = Array.IndexOf<byte>(data, 0, index) - index;
                l_MarkerLength = 1;
            }
            else
            {
                len = IndexOfNull16Bit(data, index) - index;
                l_MarkerLength = 2;
            }
            Encoding enc = Get(encoding);
            if (len < 0)
            {
                len = data.Length - index;
                l_MarkerLength = 0;
                if (len == 0)
                {
                    text = "";
                    return 0;
                }
            }
            text = enc.GetString(data, index, len).Trim('\uFFFE', '\uFEFF', '\u200B', '\u180E', '\u202F', '\u205F', ' ', '\t');
            if (text.StartsWith(ASCII.Strings.UTF8BOM)) text = text.Substring(ASCII.Strings.UTF8BOM.Length);
            return len + l_MarkerLength;
        }

        /// <summary>
        /// Parses an 8 bit character string from the specified data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetLength8BitString(byte[] data, int index)
        {
            int i = Array.IndexOf<byte>(data, 0, index);
            if (i < 0) throw new InvalidDataException(string.Format("Error while parsing 8 bit character stream!"));
            return i - index;
        }

        /// <summary>
        /// Parses an 16 bit character string from the specified data
        /// </summary>
        /// <param name="data"></param>
        /// <param name="index"></param>
        /// <returns></returns>
        public static int GetLength16BitString(byte[] data, int index)
        {
            int i = IndexOfNull16Bit(data, index);
            if (i < 0) throw new InvalidDataException(string.Format("Error while parsing 16 bit character stream!"));
            return i - index;
        }

        ID3v2Encoding() { throw new NotSupportedException(); }
    }
}
