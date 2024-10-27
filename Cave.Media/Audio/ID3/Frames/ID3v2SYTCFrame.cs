using System;
using System.Collections.Generic;

namespace Cave.Media.Audio.ID3.Frames;

/// <summary>
/// ID3v2: Synchronised tempo codes frame.<br />
/// For a more accurate description of the tempo of a musical piece this frame might be used.
/// </summary>
public sealed class ID3v2SYTCFrame : ID3v2Frame
{
    ID3v2Event[]? events;

    #region Event class

    /// <summary>
    /// Synchronised tempo event containing the tempo descriptor and time stamp.
    /// </summary>
    sealed class Event : ID3v2Event
    {
        /// <summary>
        /// Creates a new synchronised tempo event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Event FromTimeStamp(EventType type, long value) => new Event(type, value, true);

        /// <summary>
        /// Creates a new synchronised tempo event.
        /// </summary>
        /// <param name="type"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public static Event FromFrameNumber(EventType type, long value) => new Event(type, value, true);

        internal Event(EventType type, long value, bool isTimeStamp)
            : base(value, isTimeStamp)
        {
            Type = type;
        }

        /// <summary>
        /// Gets the <see cref="EventType"/>.
        /// </summary>
        public readonly EventType Type;

        /// <summary>
        /// Beats per minute after this event occured.
        /// </summary>
        public int BeatsPerMinute => (int)Type;
    }
    #endregion

    #region EventType enum

    /// <summary>
    /// Synchronised tempo codes.
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
    ID3v2Event[] Parse()
    {
        bool isTimeStamp;
        var mode = Content[0];
        switch (mode)
        {
            case 0: isTimeStamp = false; break;
            case 1: isTimeStamp = true; break;
            default: throw new NotImplementedException(string.Format("Mode {0} is not implemented!", mode));
        }
        var events = new List<Event>();
        var i = 1;
        while (i < Content.Length)
        {
            ushort beat = Content[i++];
            if (beat == 0xFF)
            {
                beat += Content[i++];
            }

            var type = (EventType)beat;
            var value = 0;
            for (var n = 0; n < 4; n++)
            {
                value = (value << 8) | Content[i++];
            }

            events.Add(new Event(type, value, isTimeStamp));
        }
        return events.ToArray();
    }

    #endregion

    internal ID3v2SYTCFrame(ID3v2Frame frame)
        : base(frame)
    {
        if (frame.ID != "SYTC")
        {
            throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "SYTC"));
        }
    }

    /// <summary>
    /// Gets a list of all events.
    /// </summary>
    public ID3v2Event[] Events
    {
        get
        {
            events ??= Parse();
            return events is ID3v2Event[] result ? result : [];
        }
    }
}
