using System;
using System.Collections.Generic;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// ID3v2: Synchronised tempo codes frame.<br />
    /// For a more accurate description of the tempo of a musical piece this frame might be used.
    /// </summary>
    
    public sealed class ID3v2SYTCFrame : ID3v2Frame
    {
        ID3v2Event[] m_Events = null;

        #region Event class
        /// <summary>
        /// Synchronised tempo event containing the tempo descriptor and time stamp.
        /// </summary>
        sealed class Event : ID3v2Event
        {
            /// <summary>
            /// Creates a new synchronised tempo event
            /// </summary>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <returns></returns>
            public static Event FromTimeStamp(EventType type, long value)
            {
                return new Event(type, value, true);
            }

            /// <summary>
            /// Creates a new synchronised tempo event
            /// </summary>
            /// <param name="type"></param>
            /// <param name="value"></param>
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
            /// Obtains the <see cref="EventType"/>
            /// </summary>
            public readonly EventType Type;

            /// <summary>
            /// Beats per minute after this event occured
            /// </summary>
            public int BeatsPerMinute { get { return (int)Type; } }
        }
        #endregion

        #region EventType enum
        /// <summary>
        /// Synchronised tempo codes
        /// </summary>
        public enum EventType : ushort
        {
            /// <summary>
            /// No beats at all
            /// </summary>
            BeatFree = 0,

            /// <summary>
            /// Only a single beat stroke followed by a beat-free period
            /// </summary>
            SingleBeatStroke = 1,
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
            List<Event> l_Events = new List<Event>();
            int i = 1;
            while (i < m_Content.Length)
            {
                ushort beat = m_Content[i++];
                if (beat == 0xFF) beat += m_Content[i++];
                EventType type = (EventType)beat;
                int value = 0;
                for (int n = 0; n < 4; n++) value = (value << 8) | m_Content[i++];
                l_Events.Add(new Event(type, value, isTimeStamp));
            }
        }

        #endregion

        internal ID3v2SYTCFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "SYTC") throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "SYTC"));
        }

        /// <summary>
        /// Obtains a list of all events
        /// </summary>
        public ID3v2Event[] Events
        {
            get
            {
                if (m_Events == null) Parse();
                return (ID3v2Event[])m_Events.Clone();
            }
        }
    }
}
