using Cave.IO;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides synchronized lyrics
    /// </summary>
    /// <seealso cref="IEnumerable{SynchronizedLyricsItem}" />
    public class SynchronizedLyrics : IEnumerable<SynchronizedLyricsItem>
    {
        /// <summary>Creates a new <see cref="SynchronizedLyrics"/> instance by parsing the specified file</summary>
        /// <param name="fileName">Name of the file.</param>
        /// <returns></returns>
        public static SynchronizedLyrics FromFile(string fileName)
        {
            return FromData(File.ReadAllBytes(fileName));
        }

        /// <summary>Creates a new <see cref="SynchronizedLyrics"/> instance by parsing the specified data</summary>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        public static SynchronizedLyrics FromData(byte[] data)
        {
            return FromStream(new MemoryStream(data));
        }

        /// <summary>Creates a new <see cref="SynchronizedLyrics"/> instance by parsing the specified stream</summary>
        /// <param name="stream">The stream.</param>
        /// <returns></returns>
        public static SynchronizedLyrics FromStream(MemoryStream stream)
        {
            List<SynchronizedLyricsItem> items = new List<SynchronizedLyricsItem>();
            long milliSecond = 0;

            DataReader reader = new DataReader(stream);
            if (reader.ReadString(3) != "SLT") throw new InvalidDataException("Invalid format!");
            if (reader.Read7BitEncodedUInt64() != 1) throw new InvalidDataException("Invalid version!");
            while (reader.Available > 0)
            {
                long milliSecondDistance = reader.Read7BitEncodedInt64();
                milliSecond += milliSecondDistance;

                SynchronizedLyricsItemBuilder item = new SynchronizedLyricsItemBuilder();
                item.TimeCode = new TimeSpan(milliSecond * TimeSpan.TicksPerMillisecond);
                while (true)
                {
                    ISynchronizedLyricsCommand command = SynchronizedLyricsCommand.Parse(reader);
                    if (command == null) break;
                    item.Commands.Add(command);
                }
                items.Add(item.ToSynchronizedLyricsItem());
            }
            return new SynchronizedLyrics(items);
        }

        readonly List<SynchronizedLyricsItem> m_Items;
        int m_CurrentPosition;

        /// <summary>Initializes a new instance of the <see cref="SynchronizedLyrics"/> class.</summary>
        /// <remarks>This constructor consumes the reference to the list not copying the items!</remarks>
        /// <param name="items">The items.</param>
        internal SynchronizedLyrics(List<SynchronizedLyricsItem> items)
        {
            m_Items = items;
        }

        /// <summary>Gets the version.</summary>
        /// <value>The version.</value>
        public int Version { get; private set; }

        /// <summary>Gets the position (time code) of the last played item.</summary>
        /// <value>The position (time code) of the last played item.</value>
        public TimeSpan Position
        {
            get
            {
                if (m_CurrentPosition == 0) return TimeSpan.Zero;
                if (m_CurrentPosition >= m_Items.Count) return TimeSpan.MaxValue;
                return m_Items[m_CurrentPosition - 1].TimeCode;
            }
        }

        /// <summary>Gets the items.</summary>
        /// <value>The items.</value>
        public ICollection<SynchronizedLyricsItem> Items { get { return m_Items.AsReadOnly(); } }

        /// <summary>Gibt einen Enumerator zurück, der die Auflistung durchläuft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.Generic.IEnumerator`1" />, der zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        public IEnumerator<SynchronizedLyricsItem> GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>Gibt einen Enumerator zurück, der eine Auflistung durchläuft.</summary>
        /// <returns>Ein <see cref="T:System.Collections.IEnumerator" />-Objekt, das zum Durchlaufen der Auflistung verwendet werden kann.</returns>
        IEnumerator IEnumerable.GetEnumerator()
        {
            return m_Items.GetEnumerator();
        }

        /// <summary>Saves the whole instance to the specified stream.</summary>
        /// <param name="stream">The stream.</param>
        public void Save(Stream stream)
        {
            DataWriter writer = new DataWriter(stream);
            writer.Write("SLT");
            writer.Write7BitEncoded32(1);
            TimeSpan timeCode = TimeSpan.Zero;
            foreach (SynchronizedLyricsItem item in m_Items)
            {
                item.Save(timeCode, writer);
                timeCode = item.TimeCode;
            }
        }

        /// <summary>Retrieves this instance as byte array.</summary>
        /// <returns></returns>
        public byte[] ToArray()
        {
            using (MemoryStream ms = new MemoryStream())
            {
                Save(ms);
                return ms.ToArray();
            }
        }

        /// <summary>Plays forward from <see cref="Position"/> to the specified timeCode.</summary>
        /// <param name="timeCode">The time code to play to.</param>
        /// <param name="backBuffer">The back buffer.</param>
        public void PlayTo(TimeSpan timeCode, ISynchronizedLyricsBackbuffer backBuffer)
        {
            for (; m_CurrentPosition < m_Items.Count; m_CurrentPosition++)
            {
                SynchronizedLyricsItem item = m_Items[m_CurrentPosition];
                if (item.TimeCode > timeCode) break;
                try
                {
                    backBuffer.Play(item);
                }
                catch (Exception ex)
                {
                    Trace.TraceError("Error playing synchronized lyrics item {0} {1} {2}", m_CurrentPosition, item, ex);
                }
            }
        }
    }
}
