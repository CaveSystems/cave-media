#region Notes
/*
    Adapted from the public c code by Jeff Tsay.
*/
#endregion

using System;

namespace Cave.Media.Audio.MP3;

/// <summary>
/// Implementation of a Bit Reservoir for the Mpeg Layer III decoder.
/// We use one byte of ar byte array per bit. Zero means bit is unset, non-zero means the bit is set.
/// Although this may seem waseful, this can be a factor of two quicker than packing 8 bits to a byte and extracting.
/// </summary>
public sealed class MP3BitReserve
{
    /// <summary>The current write position at the buffer.</summary>
    int writePosition;

    /// <summary>The buffer.</summary>
    byte[] buffer;

    /// <summary>Gets the current read position.</summary>
    /// <value>The read position (bitnumber).</value>
    public int ReadPosition { get; private set; }

    /// <summary>
    /// Initializes a new instance of the <see cref="MP3BitReserve"/> class.
    /// </summary>
    public MP3BitReserve()
    {
        buffer = new byte[4096 * 8];
    }

    /// <summary>Initializes a new instance of the <see cref="MP3BitReserve"/> class.</summary>
    /// <param name="buffer">The byte buffer.</param>
    /// <param name="offset">The offset to start at.</param>
    public MP3BitReserve(byte[]? buffer, int offset)
    {
        if (buffer is null) throw new ArgumentNullException(nameof(buffer));
        if (offset < 0) throw new ArgumentOutOfRangeException(nameof(offset));

        this.buffer = new byte[(buffer.Length - offset) * 8];
        for (var i = offset; i < buffer.Length; i++)
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
            ReadPosition += count;
            while (ReadPosition >= buffer.Length)
            {
                ReadPosition -= buffer.Length;
            }
        }
    }

    /// <summary>Read a number bits from the bit stream.</summary>
    /// <param name="count">the number of bits.</param>
    /// <returns>Returns an int containing all retrieved bits.</returns>
    public int ReadBits(int count)
    {
        unchecked
        {
            var value = 0;
            for (var i = 0; i < count; i++)
            {
                value <<= 1;
                if (buffer[ReadPosition++] != 0)
                {
                    value |= 1;
                }

                if (ReadPosition >= buffer.Length)
                {
                    ReadPosition = 0;
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
            var result = buffer[ReadPosition++];
            if (ReadPosition >= buffer.Length)
            {
                ReadPosition = 0;
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
            other.buffer[other.writePosition++] = buffer[ReadPosition++];
            if (ReadPosition >= buffer.Length)
            {
                ReadPosition = 0;
            }

            if (other.writePosition >= other.buffer.Length)
            {
                other.writePosition = 0;
            }
        }
    }

    /// <summary>Write 8 bits into the bit stream.</summary>
    /// <param name="b">The byte.</param>
    public void WriteByte(int b)
    {
        unchecked
        {
            buffer[writePosition++] = (byte)(b & 0x80);
            buffer[writePosition++] = (byte)(b & 0x40);
            buffer[writePosition++] = (byte)(b & 0x20);
            buffer[writePosition++] = (byte)(b & 0x10);
            buffer[writePosition++] = (byte)(b & 0x08);
            buffer[writePosition++] = (byte)(b & 0x04);
            buffer[writePosition++] = (byte)(b & 0x02);
            buffer[writePosition++] = (byte)(b & 0x01);
            if (writePosition >= buffer.Length)
            {
                writePosition = 0;
            }
        }
    }

    /// <summary>Rewind N bits in Stream.</summary>
    /// <param name="numberOfBits">The number of bits.</param>
    public void Rewind(int numberOfBits)
    {
        unchecked
        {
            ReadPosition -= numberOfBits;
        }
    }
}
