#region Notes
/*
    Adapted from the public c code by Jeff Tsay.
*/
#endregion

using System;

namespace Cave.Media.Audio.MP3
{
    /// <summary> 
    /// Implementation of a Bit Reservoir for the Mpeg Layer III decoder.
    /// We use one byte of ar byte array per bit. Zero means bit is unset, non-zero means the bit is set.
    /// Although this may seem waseful, this can be a factor of two quicker than packing 8 bits to a byte and extracting. 
    /// </summary>
    public sealed class MP3BitReserve
    {
        /// <summary>The current write position at the buffer</summary>
        int m_WritePosition;

        /// <summary>The current read position at the buffer</summary>
        int m_ReadPosition;

        /// <summary>The buffer</summary>
        byte[] m_Buffer;

        /// <summary>Gets the current read position.</summary>
        /// <value>The read position (bitnumber).</value>
        public int ReadPosition { get { return m_ReadPosition; } }

        /// <summary>
        /// Initializes a new instance of the <see cref="MP3BitReserve"/> class.
        /// </summary>
        public MP3BitReserve()
        {
            m_Buffer = new byte[4096 * 8];
        }

        /// <summary>Initializes a new instance of the <see cref="MP3BitReserve"/> class.</summary>
        /// <param name="buffer">The byte buffer.</param>
        /// <param name="offset">The offset to start at.</param>
        public MP3BitReserve(byte[] buffer, int offset)
        {
            if (offset < 0)
            {
                throw new ArgumentOutOfRangeException();
            }

            m_Buffer = new byte[(buffer.Length - offset) * 8];
            for (int i = offset; i < buffer.Length; i++)
            {
                WriteByte(buffer[i]);
            }
        }

        /// <summary>Skips the specified count [bits].</summary>
        /// <param name="count">The bit count.</param>
        /// <returns></returns>
        public void Skip(int count)
        {
            unchecked
            {
                m_ReadPosition += count;
                while (m_ReadPosition >= m_Buffer.Length)
                {
                    m_ReadPosition -= m_Buffer.Length;
                }
            }
        }

        /// <summary>Read a number bits from the bit stream.</summary>
        /// <param name="count">the number of bits</param>
        /// <returns>Returns an int containing all retrieved bits</returns>
        public int ReadBits(int count)
        {
            unchecked
            {
                int value = 0;
                for (int i = 0; i < count; i++)
                {
                    value <<= 1;
                    if (m_Buffer[m_ReadPosition++] != 0)
                    {
                        value |= 1;
                    }

                    if (m_ReadPosition >= m_Buffer.Length)
                    {
                        m_ReadPosition = 0;
                    }
                }
                return value;
            }
        }

        /// <summary>Returns next bit from reserve.</summary>
        /// <returns>Returns 0 if next bit is reset, or 1 if next bit is set.</returns>
        public bool ReadBit()
        {
            unchecked
            {
                byte result = m_Buffer[m_ReadPosition++];
                if (m_ReadPosition >= m_Buffer.Length)
                {
                    m_ReadPosition = 0;
                }

                return result != 0;
            }
        }

        /// <summary>Moves a number bits to another bit reserve.</summary>
        /// <param name="count">The bit count.</param>
        /// <param name="other">The target.</param>
        public void MoveBits(int count, MP3BitReserve other)
        {
            while (count-- > 0)
            {
                other.m_Buffer[other.m_WritePosition++] = m_Buffer[m_ReadPosition++];
                if (m_ReadPosition >= m_Buffer.Length)
                {
                    m_ReadPosition = 0;
                }

                if (other.m_WritePosition >= other.m_Buffer.Length)
                {
                    other.m_WritePosition = 0;
                }
            }
        }

        /// <summary>Write 8 bits into the bit stream.</summary>
        /// <param name="b">The byte.</param>
        public void WriteByte(int b)
        {
            unchecked
            {
                m_Buffer[m_WritePosition++] = (byte)(b & 0x80);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x40);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x20);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x10);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x08);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x04);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x02);
                m_Buffer[m_WritePosition++] = (byte)(b & 0x01);
                if (m_WritePosition >= m_Buffer.Length)
                {
                    m_WritePosition = 0;
                }
            }
        }

        /// <summary>Rewind N bits in Stream.</summary>
        /// <param name="numberOfBits">The number of bits.</param>
        public void Rewind(int numberOfBits)
        {
            unchecked
            {
                m_ReadPosition -= numberOfBits;
            }
        }
    }
}