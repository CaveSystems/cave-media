using System;
using System.Collections.Generic;
using System.IO;

namespace Cave.Media
{
    /// <summary>
    /// Provides a reader for binary data with buffering (allows scrollback in case of an error).
    /// </summary>
    public sealed class DataFrameReader
    {
        #region private fields
        readonly LinkedList<byte[]> m_Buffers = new LinkedList<byte[]>();
        Stream m_Stream;
        int m_BufferLength = 0;
        long m_BufferEndPosition = 0;
        long m_EndOfStream;

        LinkedListNode<byte[]> m_ReadBufferNode = null;
        int m_ReadBufferPosition = 0;

        void Move(int index, out int bufferPosition, out LinkedListNode<byte[]> bufferNode)
        {
            int i = 0;
            bufferPosition = m_ReadBufferPosition;
            bufferNode = m_ReadBufferNode;
            while (i < index)
            {
                int skip = index - i;
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
        #endregion

        /// <summary>Gets or sets the source.</summary>
        /// <value>The source.</value>
        public object Source { get; set; }

        /// <summary>
        /// Fills the buffer with the specified number of bytes.
        /// </summary>
        /// <param name="size">Size to buffer.</param>
        public int FillBuffer(int size)
        {
            if (m_Stream == null)
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

            if (m_EndOfStream > 0)
            {
                int l_Readable = (int)(m_EndOfStream - m_BufferEndPosition);
                if (size > l_Readable)
                {
                    size = l_Readable;
                }

                if (size == 0)
                {
                    return 0;
                }
            }

            int l_Left = size;
            while (l_Left > 0)
            {
                byte[] buffer = new byte[Math.Min(16 * 1024, l_Left)];
                int len = m_Stream.Read(buffer, 0, buffer.Length);
                if (len == 0)
                {
                    break;
                }

                if (len == size)
                {
                    m_Buffers.AddLast(buffer);
                }
                else
                {
                    if (len == 0)
                    {
                        return 0;
                    }

                    byte[] newBuffer = new byte[len];
                    Array.Copy(buffer, newBuffer, len);
                    m_Buffers.AddLast(buffer);
                }
                m_BufferLength += len;
                m_BufferEndPosition += len;
                l_Left -= len;
            }

            if (m_ReadBufferNode == null)
            {
                m_ReadBufferNode = m_Buffers.First;
                m_ReadBufferPosition = 0;
            }

            return size - l_Left;
        }

        /// <summary>
        /// Ensures that the buffer contains at least the specified number of bytes.
        /// </summary>
        /// <param name="lenght">The minimum length available for reading.</param>
        public bool EnsureBuffer(int lenght)
        {
            int l_Needed = lenght - Available;
            if (l_Needed > 0)
            {
                FillBuffer(l_Needed);
            }

            return Available >= lenght;
        }

        /// <summary>
        /// Searches at the buffer for a (frame start) match using the specified <see cref="IDataFrameSearch"/>.
        /// </summary>
        /// <param name="search"></param>
        /// <returns></returns>
        public bool Contains(IDataFrameSearch search)
        {
            if (m_ReadBufferNode == null)
            {
                return false;
            }

            int bufferPosition = m_ReadBufferPosition;
            LinkedListNode<byte[]> bufferNode = m_ReadBufferNode;
            byte[] buffer = bufferNode.Value;

            while (true)
            {
                byte b = buffer[bufferPosition++];
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

        /// <summary>
        /// Reads a byte buffer from the reader. <see cref="EnsureBuffer"/> is called to check the available buffer size first.
        /// </summary>
        /// <param name="index">Index to start reading at.</param>
        /// <param name="count">Length of the buffer to read.</param>
        /// <returns></returns>
        public byte[] Read(int index, int count)
        {
            if (!EnsureBuffer(index + count))
            {
                throw new EndOfStreamException();
            }
            byte[] result = new byte[count];
            Move(index, out int bufferPosition, out LinkedListNode<byte[]> bufferNode);

            int l_Offset = 0;
            while (count > 0)
            {
                byte[] buffer = bufferNode.Value;
                int blockSize = Math.Min(buffer.Length - bufferPosition, count);
                Array.Copy(buffer, bufferPosition, result, l_Offset, blockSize);
                count -= blockSize;
                l_Offset += blockSize;
                bufferPosition += blockSize;
                if (bufferPosition == buffer.Length)
                {
                    bufferPosition = 0;
                    bufferNode = bufferNode.Next;
                }
            }
            return result;
        }

        /// <summary>
        /// Reads a byte from the buffer without calling <see cref="EnsureBuffer"/> first.
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        public byte ReadByte(int index)
        {
            if (index >= m_BufferLength)
            {
                throw new ArgumentOutOfRangeException(nameof(index));
            }
            Move(index, out int bufferPosition, out LinkedListNode<byte[]> bufferNode);

            return bufferNode.Value[bufferPosition];
        }

        /// <summary>
        /// Removes the first bytes.
        /// </summary>
        /// <param name="count"></param>
        public void Remove(int count)
        {
            Move(count, out int bufferPosition, out LinkedListNode<byte[]> bufferNode);

            m_ReadBufferNode = bufferNode;
            m_ReadBufferPosition = bufferPosition;

            while (m_Buffers.First != m_ReadBufferNode)
            {
                m_BufferLength -= m_Buffers.First.Value.Length;
                m_Buffers.RemoveFirst();
            }
        }

        /// <summary>
        /// Obtains buffered data beginning at index 0 and removes it.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffer(int count)
        {
            if (count == 0)
            {
                return new byte[0];
            }

            byte[] buffer = Read(0, count);
            Remove(count);
            return buffer;
        }

        /// <summary>
        /// Obtains the number of bytes currently available for reading.
        /// </summary>
        public int Available => m_BufferLength - m_ReadBufferPosition;

        /// <summary>
        /// Obtains all currently buffered data and removes it.
        /// </summary>
        /// <returns></returns>
        public byte[] GetBuffer()
        {
            int size = Available;
            return size == 0 ? (new byte[0]) : GetBuffer(size);
        }

        /// <summary>
        /// Creates a new <see cref="DataFrameReader"/> for the specified stream.
        /// </summary>
        /// <param name="stream">Stream to read from.</param>
        /// <param name="endOfStream">End of stream position (if known, -1 otherwise).</param>
        public DataFrameReader(Stream stream, long endOfStream)
        {
            m_EndOfStream = endOfStream;
            m_Stream = stream;
            Source = stream;
        }

        /// <summary>
        /// Creates a new <see cref="DataFrameReader"/> for the specified buffer.
        /// </summary>
        /// <param name="buffer"></param>
        public DataFrameReader(byte[] buffer)
        {
            Source = buffer;
            m_Buffers.AddLast(buffer);
            m_BufferLength = buffer.Length;
            m_ReadBufferNode = m_Buffers.First;
            m_BufferEndPosition = m_BufferLength;
        }

        /// <summary>
        /// Obtains the current start position (at the stream) of the buffer.
        /// </summary>
        public long BufferStartPosition => m_BufferEndPosition - m_BufferLength + m_ReadBufferPosition;

        /// <summary>
        /// Obtains the current end position (at the stream) of the buffer.
        /// </summary>
        public long BufferEndPosition => m_BufferEndPosition;

        /// <summary>
        /// Obtains the current buffer length.
        /// </summary>
        public int BufferLength => m_BufferLength;

        /// <summary>
        /// Returns "DataFrameReader &lt;Source&gt;".
        /// </summary>
        /// <returns>Returns "DataFrameReader &lt;Source&gt;".</returns>
        public override string ToString()
        {
            object source = Source;
            return source != null ? string.Format("DataFrameReader <{0}>", source) : "DataFrameReader";
        }

        /// <summary>
        /// Closes the <see cref="DataFrameReader"/> and the underlying stream.
        /// </summary>
        public void Close()
        {
            if (m_Stream != null)
            {
                m_Stream.Close();
                m_Stream.Dispose();
                m_Stream = null;
            }
        }
    }
}
