using Cave.IO;
using System;
using System.IO;

namespace Cave.Media.Codecs
{
    /// <summary>
    /// Provides an CCITT4 decoder.
    /// </summary>
    public sealed class CCITT4Decoder : CCITT4
    {
        CCITT4DecoderState m_State = CCITT4DecoderState.White;
        int m_CurrentValue = 0;
        int m_CurrentBitLength = 0;

        private void m_Reset(CCITT4DecoderState state)
        {
            m_State = state;
            m_CurrentValue = 0;
            m_CurrentBitLength = 0;
        }

        /// <summary>
        /// Decodes a single pixel row.
        /// </summary>
        public byte[] DecodeRow(byte[] data)
        {
            m_Reset(CCITT4DecoderState.White);
            var reader = new BitStreamReaderReverse(new MemoryStream(data));
            var writer = new BitStreamWriter(new MemoryStream());
            while (reader.Position < reader.Length)
            {
                // read a bit
                m_CurrentValue = (m_CurrentValue << 1) | reader.ReadBit();
                m_CurrentBitLength++;
                if (m_CurrentBitLength > 13)
                {
                    throw new FormatException();
                }

                // did we get a EndOfLine code ?
                if ((m_CurrentBitLength == EOL[1]) && (m_CurrentValue == EOL[0]))
                {
                    m_Reset(0);
                    continue;
                }
                if (m_State == 0)
                {
                    // white makeup search
                    for (int i = 0; i < WhiteMakeUpCodes.GetLength(0); i++)
                    {
                        if ((WhiteMakeUpCodes[i, 1] == m_CurrentBitLength) &&
                            (WhiteMakeUpCodes[i, 0] == m_CurrentValue))
                        {
                            writer.WriteBits(WhiteMakeUpCodes[i, 2], true);
                            m_Reset(CCITT4DecoderState.WhiteTerminationRequired);
                            break;
                        }
                    }
                    if (m_State != 0)
                    {
                        continue;
                    }
                }
                if ((int)m_State <= 1)
                {
                    // white termination search
                    for (int i = 0; i < WhiteTerminatingCodes.GetLength(0); i++)
                    {
                        if ((WhiteTerminatingCodes[i, 1] == m_CurrentBitLength) &&
                            (WhiteTerminatingCodes[i, 0] == m_CurrentValue))
                        {
                            writer.WriteBits(i, true);
                            m_Reset(CCITT4DecoderState.Black);
                            break;
                        }
                    }
                    if ((int)m_State != 1)
                    {
                        continue;
                    }
                }
                if ((int)m_State == 2)
                {
                    // black makeup search
                    for (int i = 0; i < BlackMakeUpCodes.GetLength(0); i++)
                    {
                        if ((BlackMakeUpCodes[i, 1] == m_CurrentBitLength) &&
                            (BlackMakeUpCodes[i, 0] == m_CurrentValue))
                        {
                            writer.WriteBits(BlackMakeUpCodes[i, 2], false);
                            m_Reset(CCITT4DecoderState.BlackTerminationRequired);
                            break;
                        }
                    }
                    if ((int)m_State != 2)
                    {
                        continue;
                    }
                }
                if ((int)m_State >= 2)
                {
                    // black termination search
                    for (int i = 0; i < BlackTerminatingCodes.GetLength(0); i++)
                    {
                        if ((BlackTerminatingCodes[i, 1] == m_CurrentBitLength) &&
                            (BlackTerminatingCodes[i, 0] == m_CurrentValue))
                        {
                            writer.WriteBits(i, false);
                            m_Reset(CCITT4DecoderState.White);
                            break;
                        }
                    }
                    if ((int)m_State != 3)
                    {
                        continue;
                    }
                }
            }
            return ((MemoryStream)writer.BaseStream).ToArray();
        }

    }
}
