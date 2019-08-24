using System;
using System.IO;
using Cave.IO;

namespace Cave.Media.Codecs
{
    /// <summary>
    /// Provides an CCITT4 decoder.
    /// </summary>
    public sealed class CCITT4Decoder : CCITT4
    {
        CCITT4DecoderState state = CCITT4DecoderState.White;
        int currentValue = 0;
        int currentBitLength = 0;

        private void Reset(CCITT4DecoderState state)
        {
            this.state = state;
            currentValue = 0;
            currentBitLength = 0;
        }

        /// <summary>
        /// Decodes a single pixel row.
        /// </summary>
        public byte[] DecodeRow(byte[] data)
        {
            Reset(CCITT4DecoderState.White);
            var reader = new BitStreamReaderReverse(new MemoryStream(data));
            var writer = new BitStreamWriter(new MemoryStream());
            while (reader.Position < reader.Length)
            {
                // read a bit
                currentValue = (currentValue << 1) | reader.ReadBit();
                currentBitLength++;
                if (currentBitLength > 13)
                {
                    throw new FormatException();
                }

                // did we get a EndOfLine code ?
                if ((currentBitLength == EOL[1]) && (currentValue == EOL[0]))
                {
                    Reset(0);
                    continue;
                }
                if (state == 0)
                {
                    // white makeup search
                    for (int i = 0; i < WhiteMakeUpCodes.GetLength(0); i++)
                    {
                        if ((WhiteMakeUpCodes[i, 1] == currentBitLength) &&
                            (WhiteMakeUpCodes[i, 0] == currentValue))
                        {
                            writer.WriteBits(WhiteMakeUpCodes[i, 2], true);
                            Reset(CCITT4DecoderState.WhiteTerminationRequired);
                            break;
                        }
                    }
                    if (state != 0)
                    {
                        continue;
                    }
                }
                if ((int)state <= 1)
                {
                    // white termination search
                    for (int i = 0; i < WhiteTerminatingCodes.GetLength(0); i++)
                    {
                        if ((WhiteTerminatingCodes[i, 1] == currentBitLength) &&
                            (WhiteTerminatingCodes[i, 0] == currentValue))
                        {
                            writer.WriteBits(i, true);
                            Reset(CCITT4DecoderState.Black);
                            break;
                        }
                    }
                    if ((int)state != 1)
                    {
                        continue;
                    }
                }
                if ((int)state == 2)
                {
                    // black makeup search
                    for (int i = 0; i < BlackMakeUpCodes.GetLength(0); i++)
                    {
                        if ((BlackMakeUpCodes[i, 1] == currentBitLength) &&
                            (BlackMakeUpCodes[i, 0] == currentValue))
                        {
                            writer.WriteBits(BlackMakeUpCodes[i, 2], false);
                            Reset(CCITT4DecoderState.BlackTerminationRequired);
                            break;
                        }
                    }
                    if ((int)state != 2)
                    {
                        continue;
                    }
                }
                if ((int)state >= 2)
                {
                    // black termination search
                    for (int i = 0; i < BlackTerminatingCodes.GetLength(0); i++)
                    {
                        if ((BlackTerminatingCodes[i, 1] == currentBitLength) &&
                            (BlackTerminatingCodes[i, 0] == currentValue))
                        {
                            writer.WriteBits(i, false);
                            Reset(CCITT4DecoderState.White);
                            break;
                        }
                    }
                    if ((int)state != 3)
                    {
                        continue;
                    }
                }
            }
            return ((MemoryStream)writer.BaseStream).ToArray();
        }

    }
}
