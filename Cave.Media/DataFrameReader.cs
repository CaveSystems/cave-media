using System;
using System.Collections.Generic;
using System.IO;

#nullable enable

namespace Cave.Media;

/// <summary>Provides a reader for binary data with buffering (allows scrollback in case of an error).</summary>
public sealed class DataFrameReader
{
    #region Private Fields

    readonly LinkedList<byte[]> buffers = new LinkedList<byte[]>();
    readonly long endOfStream;
    LinkedListNode<byte[]>? readBufferNode = null;
    int readBufferPosition = 0;
    Stream? stream;

    #endregion Private Fields

    #region Private Methods

    void Move(int index, out int bufferPosition, out LinkedListNode<byte[]>? bufferNode)
    {
        var i = 0;
        bufferPosition = readBufferPosition;
        bufferNode = readBufferNode;
        while (i < index)
        {
            if (bufferNode == null) throw new InvalidOperationException("Buffer empty!");
            var skip = index - i;
            if (skip + bufferPosition >= bufferNode.Value.Length)
            {
                i += bufferNode.Value.Length - bufferPosition;
                bufferPosition = 0;
                bufferNode = bufferNode.Next;
            }
            else
            {
                bufferPosition += skip;
                i += skip;
            }
        }
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Creates a new <see cref="DataFrameReader"/> for the specified stream.</summary>
    /// <param name="stream">Stream to read from.</param>
    /// <param name="endOfStream">End of stream position (if known, -1 otherwise).</param>
    public DataFrameReader(Stream stream, long endOfStream)
    {
        this.endOfStream = endOfStream;
        this.stream = stream;
        Source = stream;
    }

    /// <summary>Creates a new <see cref="DataFrameReader"/> for the specified buffer.</summary>
    /// <param name="buffer"></param>
    public DataFrameReader(byte[] buffer)
    {
        Source = buffer;
        buffers.AddLast(buffer);
        BufferLength = buffer.Length;
        readBufferNode = buffers.First;
        BufferEndPosition = BufferLength;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets the number of bytes currently available for reading.</summary>
    public int Available => BufferLength - readBufferPosition;

    /// <summary>Gets the current end position (at the stream) of the buffer.</summary>
    public long BufferEndPosition { get; private set; } = 0;

    /// <summary>Gets the current buffer length.</summary>
    public int BufferLength { get; private set; } = 0;

    /// <summary>Gets the current start position (at the stream) of the buffer.</summary>
    public long BufferStartPosition => BufferEndPosition - BufferLength + readBufferPosition;

    /// <summary>Gets or sets the source.</summary>
    /// <value>The source.</value>
    public object Source { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Closes the <see cref="DataFrameReader"/> and the underlying stream.</summary>
    public void Close()
    {
        if (stream != null)
        {
            stream.Close();
            stream.Dispose();
            stream = null;
        }
    }

    /// <summary>Searches at the buffer for a (frame start) match using the specified <see cref="IDataFrameSearch"/>.</summary>
    /// <param name="search"></param>
    /// <returns></returns>
    public bool Contains(IDataFrameSearch search)
    {
        if (readBufferNode == null)
        {
            return false;
        }

        var bufferPosition = readBufferPosition;
        var bufferNode = readBufferNode;
        var buffer = bufferNode.Value;

        while (true)
        {
            var b = buffer[bufferPosition++];
            if (search.Check(b))
            {
                return true;
            }

            if (bufferPosition >= buffer.Length)
            {
                bufferNode = bufferNode.Next;
                if (bufferNode == null)
                {
                    return false;
                }

                buffer = bufferNode.Value;
                bufferPosition = 0;
            }
        }
    }

    /// <summary>Ensures that the buffer contains at least the specified number of bytes.</summary>
    /// <param name="lenght">The minimum length available for reading.</param>
    public bool EnsureBuffer(int lenght)
    {
        var needed = lenght - Available;
        if (needed > 0)
        {
            FillBuffer(needed);
        }

        return Available >= lenght;
    }

    /// <summary>Fills the buffer with the specified number of bytes.</summary>
    /// <param name="size">Size to buffer.</param>
    public int FillBuffer(int size)
    {
        if (stream == null)
        {
            return 0;
        }

        if (size <= 0)
        {
            return 0;
        }

        if (size < 1024)
        {
            size = 1024;
        }

        if (endOfStream > 0)
        {
            var readable = (int)(endOfStream - BufferEndPosition);
            if (size > readable)
            {
                size = readable;
            }

            if (size == 0)
            {
                return 0;
            }
        }

        var left = size;
        while (left > 0)
        {
            var buffer = new byte[Math.Min(16 * 1024, left)];
            var len = stream.Read(buffer, 0, buffer.Length);
            if (len == 0)
            {
                break;
            }

            if (len == size)
            {
                buffers.AddLast(buffer);
            }
            else
            {
                if (len == 0)
                {
                    return 0;
                }

                var newBuffer = new byte[len];
                Array.Copy(buffer, newBuffer, len);
                buffers.AddLast(buffer);
            }
            BufferLength += len;
            BufferEndPosition += len;
            left -= len;
        }

        if (readBufferNode == null)
        {
            readBufferNode = buffers.First;
            readBufferPosition = 0;
        }

        return size - left;
    }

    /// <summary>Gets buffered data beginning at index 0 and removes it.</summary>
    /// <returns></returns>
    public byte[] GetBuffer(int count)
    {
        if (count == 0)
        {
            return new byte[0];
        }

        var buffer = Read(0, count);
        Remove(count);
        return buffer;
    }

    /// <summary>Gets all currently buffered data and removes it.</summary>
    /// <returns></returns>
    public byte[] GetBuffer()
    {
        var size = Available;
        return size == 0 ? (new byte[0]) : GetBuffer(size);
    }

    /// <summary>Reads a byte buffer from the reader. <see cref="EnsureBuffer"/> is called to check the available buffer size first.</summary>
    /// <param name="index">Index to start reading at.</param>
    /// <param name="count">Length of the buffer to read.</param>
    /// <returns></returns>
    public byte[] Read(int index, int count)
    {
        if (!EnsureBuffer(index + count))
        {
            throw new EndOfStreamException();
        }
        var result = new byte[count];
        Move(index, out var bufferPosition, out var bufferNode);

        var offset = 0;
        while (count > 0)
        {
            if (bufferNode is null) throw new InvalidOperationException("Buffer is empty!");
            var buffer = bufferNode.Value;
            var blockSize = Math.Min(buffer.Length - bufferPosition, count);
            Array.Copy(buffer, bufferPosition, result, offset, blockSize);
            count -= blockSize;
            offset += blockSize;
            bufferPosition += blockSize;
            if (bufferPosition == buffer.Length)
            {
                bufferPosition = 0;
                bufferNode = bufferNode.Next;
            }
        }
        return result;
    }

    /// <summary>Reads a byte from the buffer without calling <see cref="EnsureBuffer"/> first.</summary>
    /// <param name="index"></param>
    /// <returns></returns>
    public byte ReadByte(int index)
    {
        if (index >= BufferLength)
        {
            throw new ArgumentOutOfRangeException(nameof(index));
        }
        Move(index, out var bufferPosition, out var bufferNode);
        if (bufferNode is null) throw new InvalidOperationException("Buffer is empty!");
        return bufferNode.Value[bufferPosition];
    }

    /// <summary>Removes the first bytes.</summary>
    /// <param name="count"></param>
    public void Remove(int count)
    {
        Move(count, out var bufferPosition, out var bufferNode);

        readBufferNode = bufferNode;
        readBufferPosition = bufferPosition;

        while (buffers.First != readBufferNode)
        {
            if (buffers.First is null) throw new InvalidOperationException("Buffer is empty!");
            BufferLength -= buffers.First.Value.Length;
            buffers.RemoveFirst();
        }
    }

    /// <summary>Returns "DataFrameReader &lt;Source&gt;".</summary>
    /// <returns>Returns "DataFrameReader &lt;Source&gt;".</returns>
    public override string ToString()
    {
        var source = Source;
        return source != null ? string.Format("DataFrameReader <{0}>", source) : "DataFrameReader";
    }

    #endregion Public Methods
}
