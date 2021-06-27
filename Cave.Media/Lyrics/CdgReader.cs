using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using Cave.IO;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides a SynchronizedLyrics reader for CDG files.
    /// </summary>
    public class CdgReader
    {
        /// <summary>Reads all frames.</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static SynchronizedLyrics ReadAllFrames(string fileName)
        {
            using Stream stream = File.OpenRead(fileName);
            return ReadAllFrames(stream);
        }

        /// <summary>Reads all frames.</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static SynchronizedLyrics ReadAllFrames(Stream stream)
        {
            var items = new List<SynchronizedLyricsItem>();
            var reader = new CdgReader(stream);
            CdgPacket packet;
            TimeSpan time;
            while (reader.GetNextPacket(out packet, out time))
            {
                var builder = new SynchronizedLyricsItemBuilder();
                builder.TimeCode = time;

                switch (packet.Instruction)
                {
                    case CdgInstruction.MemoryPreset: reader.ParseMemoryPreset(builder, packet); break;
                    case CdgInstruction.BorderPreset: break;
                    case CdgInstruction.TileBlock: reader.ParseTileBlock(builder, packet, false); break;
                    case CdgInstruction.TileBlockXor: reader.ParseTileBlock(builder, packet, true); break;
                    case CdgInstruction.ScrollPreset: reader.ParseScroll(builder, packet, false); break;
                    case CdgInstruction.ScrollCopy: reader.ParseScroll(builder, packet, true); break;
                    case CdgInstruction.DefineTransparentColor: reader.ParseDefineTransparentColor(builder, packet); break;
                    case CdgInstruction.LoadLowerColorTable: reader.ParseLoadColorTable(builder, packet, false); break;
                    case CdgInstruction.LoadHigherColorTable: reader.ParseLoadColorTable(builder, packet, true); break;
                    case CdgInstruction.Unknown: continue;

                    default:
                        Trace.TraceError("Unknown command <red>{0}", packet.Instruction);
                        continue;
                }
                if (builder.Commands.Count > 0)
                {
                    items.Add(builder.ToSynchronizedLyricsItem());
                }
            }
            return new SynchronizedLyrics(items);
        }

        #region parsers
        sbyte currentOffsetHorizontal;
        sbyte currentOffsetVertical;

        void ParseLoadColorTable(SynchronizedLyricsItemBuilder sl, CdgPacket packet, bool higherTable)
        {
            var offset = higherTable ? 8 : 0;
            var palette = new ARGB[8];
            for (var i = 0; i < 8; i++)
            {
                var r = (byte)((packet.Data[i * 2] & 0x3C) << 2);
                var g = (byte)(((packet.Data[i * 2] & 0x03) << 6) | (packet.Data[(i * 2) + 1] & 0x30));
                var b = (byte)((packet.Data[(i * 2) + 1] & 0x0F) << 4);
                palette[i] = ARGB.FromColor(r, g, b);
            }
            sl.Commands.Add(new SlcReplacePaletteColors((byte)offset, palette));
        }

        void ParseDefineTransparentColor(SynchronizedLyricsItemBuilder sl, CdgPacket packet) => sl.Commands.Add(new SlcWithColorIndex(SynchronizedLyricsCommandType.SetTransparentColor, (byte)(packet.Data[0] & 0x0F)));

        void ParseScroll(SynchronizedLyricsItemBuilder sl, CdgPacket packet, bool roll)
        {
            var colorIndex = packet.Data[0] & 0x0F;
            var cdgHScroll = packet.Data[1] & 0x3F;
            var cdgVScroll = packet.Data[2] & 0x3F;

            sbyte hScroll = 0;
            sbyte vScroll = 0;
            switch (cdgHScroll >> 4)
            {
                default:
                case 0: /*no scroll*/ break;
                case 1: /*right 6px*/ hScroll = 6; break;
                case 2: /*left 6px*/ hScroll = -6; break;
            }
            switch (cdgVScroll >> 4)
            {
                default:
                case 0: /*no scroll*/ break;
                case 1: /*down 12px*/ vScroll = 12; break;
                case 2: /*up 12px*/ vScroll = -12; break;
            }
            if (hScroll != 0 || vScroll != 0)
            {
                if (roll)
                {
                    sl.Commands.Add(new SlcScreenRoll(hScroll, vScroll));
                }
                else
                {
                    sl.Commands.Add(new SlcScreenScroll((byte)colorIndex, hScroll, vScroll));
                }
            }

            var hOffset = (sbyte)((cdgHScroll & 0xF) % 6);
            var vOffset = (sbyte)((cdgVScroll & 0xF) % 12);
            if (hOffset != currentOffsetHorizontal || vOffset != currentOffsetVertical)
            {
                sl.Commands.Add(new SlcScreenOffset(hOffset, vOffset));
                currentOffsetHorizontal = hOffset;
                currentOffsetVertical = vOffset;
            }
        }
        const int BufferWidth = 300;
        const int BufferHeight = 216;
        void ParseTileBlock(SynchronizedLyricsItemBuilder sl, CdgPacket packet, bool xor)
        {
            var color0 = (byte)(packet.Data[0] & 0x0F);
            var color1 = (byte)(packet.Data[1] & 0x0F);
            var y = (packet.Data[2] & 0x1F) * 12;
            var x = (packet.Data[3] & 0x3F) * 6;
            var w = 6;
            var h = 12;
            if (x + w > BufferWidth || y + h > BufferHeight)
            {
                Trace.TraceError(string.Format("Subchannel decode error x={0} y={1}", x, y));
                return;
            }
            var data = new byte[12];
            Array.Copy(packet.Data, 4, data, 0, 12);
            var cmd = xor ? SynchronizedLyricsCommandType.SetSprite2ColorsXOR : SynchronizedLyricsCommandType.SetSprite2Colors;
            sl.Commands.Add(new SlcSetSprite2Colors(cmd, w, h, x, y, color0, color1, data));
        }

        void ParseMemoryPreset(SynchronizedLyricsItemBuilder sl, CdgPacket packet)
        {
            var repeat = packet.Data[1] & 0x0F;
            if (repeat == 0)
            {
                sl.Commands.Add(new SlcWithColorIndex(SynchronizedLyricsCommandType.ClearScreen, (byte)(packet.Data[0] & 0x0F)));
            }
        }
        #endregion

        const int PacketPerSecond = 300;
        DataReader m_Reader;
        int m_PacketNumber = 0;

        /// <summary>Initializes a new instance of the <see cref="CdgReader"/> class.</summary>
        /// <param name="stream">The stream to read from.</param>
        public CdgReader(Stream stream)
        {
            m_Reader = new DataReader(stream);
        }

        /// <summary>Gets the next packet or null if the end of the stream was reached.</summary>
        /// <param name="packet">The packet.</param>
        /// <param name="time">The time.</param>
        /// <returns></returns>
        /// <exception cref="ObjectDisposedException"></exception>
        public bool GetNextPacket(out CdgPacket packet, out TimeSpan time)
        {
            if (m_Reader == null)
            {
                throw new ObjectDisposedException(nameof(CdgReader));
            }

            // can we check the position in the stream ? yes -> can we read another packet ? no -> exit
            if (m_Reader.BaseStream.CanSeek && m_Reader.Available < 24)
            {
                packet = default;
                time = default;
                return false;
            }
            try
            {
                packet = m_Reader.ReadStruct<CdgPacket>();
                time = new TimeSpan(m_PacketNumber++ * TimeSpan.TicksPerSecond / PacketPerSecond);
                return true;
            }
            catch (EndOfStreamException)
            {
                packet = default;
                time = default;
                return false;
            }
        }

        /// <summary>Closes this instance.</summary>
        public void Close()
        {
            m_Reader.Close();
            m_Reader = null;
        }
    }
}
