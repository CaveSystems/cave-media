using Cave.IO;
using System;
using System.IO;

namespace Cave.Media.Codecs
{
    /// <summary>
    /// Provides an CCITT4 encoder
    /// </summary>
    public sealed class CCITT4Encoder : CCITT4, IDisposable
    {
        int m_State;
        MemoryStream m_Buffer;
        BitStreamWriterReverse m_Writer;

        void Initialize()
        {
            m_State = 1;
            m_Buffer = new MemoryStream();
            m_Writer = new BitStreamWriterReverse(m_Buffer);
        }

        void WriteBits(int count)
        {
            ushort[,] makeUpCodes, terminationCodes;
            if (m_State == 1)
            {
                //white
                makeUpCodes = WhiteMakeUpCodes;
                terminationCodes = WhiteTerminatingCodes;
            }
            else
            {
                //black
                makeUpCodes = BlackMakeUpCodes;
                terminationCodes = BlackTerminatingCodes;
            }

            //write more then 63 pixels ?
            if (count > 63)
            {
                //yes, find makeup to use
                int l_MakeUpIndex = makeUpCodes.GetLength(0);
                while (--l_MakeUpIndex > 0)
                {
                    if (makeUpCodes[l_MakeUpIndex, 2] <= count) break;
                }
                m_Writer.WriteBits(makeUpCodes[l_MakeUpIndex, 0], makeUpCodes[l_MakeUpIndex, 1]);
                count -= makeUpCodes[l_MakeUpIndex, 2];
            }
            //write termination
            m_Writer.WriteBits(terminationCodes[count, 0], terminationCodes[count, 1]);
        }

        byte[] Complete()
        {
            m_Writer.Flush();
            byte[] result = m_Buffer.ToArray();
            m_Writer = null;
            m_Buffer.Dispose();
            m_Buffer = null;
            return result;
        }

        /// <summary>
        /// Encodes a single pixel row
        /// </summary>
        /// <param name="data"></param>
        /// <returns></returns>
        public byte[] EncodeRow(byte[] data)
        {
            BitStreamReader reader = new BitStreamReader(new MemoryStream(data));
            int counter = 0;
            Initialize();
            //iterate until stream ends
            while (reader.Position < reader.Length)
            {
                if (reader.ReadBit() != m_State)
                {
                    //write out counted pixels
                    WriteBits(counter);
                    //found state change
                    m_State = 1 - m_State;
                    //reset counter
                    counter = 0;
                }
                counter++;
            }
            //write the last data
            if (counter > 0) WriteBits(counter);
            //return
            return Complete();
        }

        /// <summary>
        /// Disposes the buffer
        /// </summary>
        public void Dispose()
        {
            if (m_Buffer != null)
            {
                m_Buffer.Dispose();
                m_Buffer = null;
            }
        }
    }
}
