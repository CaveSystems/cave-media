using System;
using System.IO;
using Cave.IO;

namespace Cave.Media.Codecs;

/// <summary>
/// Provides an CCITT4 encoder.
/// </summary>
public sealed class CCITT4Encoder : CCITT4, IDisposable
{
    FifoStream buffer;
    bool disposed;
    int state = 1;

    public CCITT4Encoder()
    {
        buffer = new();
    }

    void WriteBits(BitStreamWriterReverse writer, int count)
    {
        ushort[,] makeUpCodes, terminationCodes;
        if (state == 1)
        {
            // white
            makeUpCodes = WhiteMakeUpCodes;
            terminationCodes = WhiteTerminatingCodes;
        }
        else
        {
            // black
            makeUpCodes = BlackMakeUpCodes;
            terminationCodes = BlackTerminatingCodes;
        }

        // write more then 63 pixels ?
        if (count > 63)
        {
            // yes, find makeup to use
            var makeUpIndex = makeUpCodes.GetLength(0);
            while (--makeUpIndex > 0)
            {
                if (makeUpCodes[makeUpIndex, 2] <= count)
                {
                    break;
                }
            }
            writer.WriteBits(makeUpCodes[makeUpIndex, 0], makeUpCodes[makeUpIndex, 1]);
            count -= makeUpCodes[makeUpIndex, 2];
        }

        // write termination
        writer.WriteBits(terminationCodes[count, 0], terminationCodes[count, 1]);
    }

    byte[] Complete(BitStreamWriterReverse writer)
    {
        writer.Flush();
        var result = buffer.ToArray();
        buffer.Clear();
        return result;
    }

    /// <summary>
    /// Encodes a single pixel row.
    /// </summary>
    /// <param name="data"></param>
    /// <returns></returns>
    public byte[] EncodeRow(byte[] data)
    {
        if (disposed) throw new ObjectDisposedException(nameof(CCITT4Decoder));
        var writer = new BitStreamWriterReverse(buffer);
        var reader = new BitStreamReader(new MemoryStream(data));
        var counter = 0;

        // iterate until stream ends
        while (reader.Position < reader.Length)
        {
            if (reader.ReadBit() != state)
            {
                // write out counted pixels
                WriteBits(writer, counter);

                // found state change
                state = 1 - state;

                // reset counter
                counter = 0;
            }
            counter++;
        }

        // write the last data
        if (counter > 0)
        {
            WriteBits(writer, counter);
        }

        // return
        return Complete(writer);
    }

    /// <summary>
    /// Disposes the buffer.
    /// </summary>
    public void Dispose()
    {
        GC.SuppressFinalize(this);
        if (!disposed)
        {
            disposed = true;
            buffer.Dispose();
        }
    }
}
