using System;
using System.Collections.Generic;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// This frame allows synchronisation with key events in a song or sound.
    /// </summary>

    public sealed class ID3v2ETCOFrame : ID3v2Frame
    {
        ID3v2Event[] m_Events = null;

        #region Event class

        /// <summary>
        /// Provides a ETCO Frame event.
        /// </summary>
        sealed class Event : ID3v2Event
        {
            /// <summary>
            /// Creates a new event from a timestamp value.
            /// </summary>
            /// <param name="type">The EventType.</param>
            /// <param name="value">The timestamp.</param>
            /// <returns></returns>
            public static Event FromTimeStamp(EventType type, long value)
            {
                return new Event(type, value, true);
            }

            /// <summary>
            /// Creates a new event from a frame number value.
            /// </summary>
            /// <param name="type">The EventType.</param>
            /// <param name="value">The frame number.</param>
            /// <returns></returns>
            public static Event FromFrameNumber(EventType type, long value)
            {
                return new Event(type, value, true);
            }

            internal Event(EventType type, long value, bool isTimeStamp)
                : base(value, isTimeStamp)
            {
                Type = type;
            }

            /// <summary>
            /// Obtains the <see cref="EventType"/>.
            /// </summary>
            public readonly EventType Type;
        }
        #endregion

        #region EventType enum

        /// <summary>
        /// Provides available event types.
        /// </summary>
        public enum EventType : byte
        {
            /// <summary>
            /// padding (has no meaning)
            /// </summary>
            Padding = 0x00,

            /// <summary>
            /// end of initial silence
            /// </summary>
            EndOfInitialSilence = 0x01,

            /// <summary>
            /// intro start
            /// </summary>
            IntroStart = 0x02,

            /// <summary>
            /// main part start
            /// </summary>
            MainPartStart = 0x03,

            /// <summary>
            /// outro start
            /// </summary>
            OutroStart = 0x04,

            /// <summary>
            /// outro end
            /// </summary>
            OutroEnd = 0x05,

            /// <summary>
            /// verse start
            /// </summary>
            VerseStart = 0x06,

            /// <summary>
            /// refrain start
            /// </summary>
            RefrainStart = 0x07,

            /// <summary>
            /// interlude start
            /// </summary>
            InterludeStart = 0x08,

            /// <summary>
            /// theme start
            /// </summary>
            ThemeStart = 0x09,

            /// <summary>
            /// variation start
            /// </summary>
            VariationStart = 0x0A,

            /// <summary>
            /// key change
            /// </summary>
            KeyChange = 0x0B,

            /// <summary>
            /// time change
            /// </summary>
            TimeChange = 0x0C,

            /// <summary>
            /// momentary unwanted noise (Snap, Crackle &amp; Pop)
            /// </summary>
            MomentaryUnwantedNoise = 0x0D,

            /// <summary>
            /// sustained noise
            /// </summary>
            SustainedNoise = 0x0E,

            /// <summary>
            /// sustained noise end
            /// </summary>
            SustainedNoiseEnd = 0x0F,

            /// <summary>
            /// intro end
            /// </summary>
            IntroEnd = 0x10,

            /// <summary>
            /// main part end
            /// </summary>
            MainPartEnd = 0x11,

            /// <summary>
            /// verse end
            /// </summary>
            VerseEnd = 0x12,

            /// <summary>
            /// refrain end
            /// </summary>
            RefrainEnd = 0x13,

            /// <summary>
            /// theme end
            /// </summary>
            ThemeEnd = 0x14,

            /// <summary>
            /// profanity
            /// </summary>
            Profanity = 0x15,

            /// <summary>
            /// profanity end
            /// </summary>
            ProfanityEnd = 0x16,

            /// <summary>
            /// User defined Event 0
            /// </summary>
            UserEvent0 = 0xE0,

            /// <summary>
            /// User defined Event 1
            /// </summary>
            UserEvent1 = 0xE1,

            /// <summary>
            /// User defined Event 2
            /// </summary>
            UserEvent2 = 0xE2,

            /// <summary>
            /// User defined Event 3
            /// </summary>
            UserEvent3 = 0xE3,

            /// <summary>
            /// User defined Event 4
            /// </summary>
            UserEvent4 = 0xE4,

            /// <summary>
            /// User defined Event 5
            /// </summary>
            UserEvent5 = 0xE5,

            /// <summary>
            /// User defined Event 6
            /// </summary>
            UserEvent6 = 0xE6,

            /// <summary>
            /// User defined Event 7
            /// </summary>
            UserEvent7 = 0xE7,

            /// <summary>
            /// User defined Event 8
            /// </summary>
            UserEvent8 = 0xE8,

            /// <summary>
            /// User defined Event 9
            /// </summary>
            UserEvent9 = 0xE9,

            /// <summary>
            /// User defined Event 10
            /// </summary>
            UserEvent10 = 0xEA,

            /// <summary>
            /// User defined Event 11
            /// </summary>
            UserEvent11 = 0xEB,

            /// <summary>
            /// User defined Event 12
            /// </summary>
            UserEvent12 = 0xEC,

            /// <summary>
            /// User defined Event 13
            /// </summary>
            UserEvent13 = 0xED,

            /// <summary>
            /// User defined Event 14
            /// </summary>
            UserEvent14 = 0xEE,

            /// <summary>
            /// User defined Event 15
            /// </summary>
            UserEvent15 = 0xEF,

            /// <summary>
            /// audio end (start of silence)
            /// </summary>
            AudioEnd = 0xFD,

            /// <summary>
            /// audio file ends
            /// </summary>
            AudioFileEnd = 0xFE,

            /// <summary>
            /// one more byte of events follows (all the following bytes with the value 0xFF have the same function)
            /// </summary>
            DoNotUse = 0xFF,
        }
        #endregion

        #region Parser functions
        void Parse()
        {
            bool isTimeStamp;
            byte mode = m_Content[0];
            switch (mode)
            {
                case 0: isTimeStamp = false; break;
                case 1: isTimeStamp = true; break;
                default: throw new NotImplementedException(string.Format("Mode {0} is not implemented!", mode));
            }
            var events = new List<Event>();
            int i = 1;
            while (i < m_Data.Length)
            {
                var type = (EventType)m_Data[i++];
                int value = 0;
                for (int n = 0; n < 4; n++)
                {
                    value = (value << 8) | m_Content[i++];
                }

                events.Add(new Event(type, value, isTimeStamp));
            }
            m_Events = events.ToArray();
        }

        #endregion

        internal ID3v2ETCOFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "ETCO")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "ETCO"));
            }
        }

        /// <summary>
        /// Obtains a list of all events.
        /// </summary>
        public ID3v2Event[] Events
        {
            get
            {
                if (m_Events == null)
                {
                    Parse();
                }

                return (Event[])m_Events.Clone();
            }
        }
    }
}
