using System;
using System.Collections.Generic;

namespace Cave.Media.Audio.ID3.Frames;

/// <summary>
/// This frame contains the lyrics of the song or a text transcription of other vocal activities. The head includes an encoding descriptor and a content descriptor.
/// </summary>
public sealed class ID3v2SYLTFrame : ID3v2Frame
{
    #region Private Classes

    /// <summary>text/lyrics event containing the text and time stamp.</summary>
    sealed class Event : ID3v2Event
    {
        #region Internal Constructors

        internal Event(string text, long value, bool isTimeStamp)
                    : base(value, isTimeStamp)
        {
            Text = text;
        }

        #endregion Internal Constructors

        #region Public Fields

        /// <summary>Gets the event text.</summary>
        public readonly string Text;

        #endregion Public Fields

        #region Public Methods

        /// <summary>Creates a new text event.</summary>
        /// <param name="text">Lyrics.</param>
        /// <param name="value">Frame number.</param>
        /// <returns></returns>
        public static Event FromFrameNumber(string text, long value) => new Event(text, value, true);

        /// <summary>Creates a new lyrics event.</summary>
        /// <param name="text">Lyrics.</param>
        /// <param name="value">Time in milli seconds.</param>
        /// <returns></returns>
        public static Event FromTimeStamp(string text, long value) => new Event(text, value, true);

        #endregion Public Methods
    }

    #endregion Private Classes

    #region Private Fields

    ContentType contentType;
    string? descriptor;
    ID3v2Event[]? events;
    string? language;
    bool parsed = false;

    #endregion Private Fields

    #region Private Methods

    void Parse()
    {
        // encoding
        var encoding = (ID3v2EncodingType)Content[0];

        // language
        language = ID3v2Encoding.ISO88591.GetString(Content, 1, 3);

        // timestamp
        bool isTimeStamp;
        {
            var mode = Content[4];
            switch (mode)
            {
                case 0: isTimeStamp = false; break;
                case 1: isTimeStamp = true; break;
                default: throw new NotImplementedException(string.Format("Mode {0} is not implemented!", mode));
            }
        }

        // content type
        contentType = (ContentType)Content[5];

        // read descriptor
        var start = 6;
        start += ID3v2Encoding.Parse(encoding, Content, start, out descriptor);

        // read events
        var events = new List<Event>();
        while (start < Content.Length)
        {
            string text;
            start += ID3v2Encoding.Parse(encoding, Content, start, out text);
            long value = 0;
            for (var i = 0; i < 4; i++)
            {
                value = (value << 8) | Content[start++];
            }

            events.Add(new Event(text, value, isTimeStamp));
        }
        this.events = events.ToArray();
        parsed = true;
    }

    #endregion Private Methods

    #region Internal Constructors

    internal ID3v2SYLTFrame(ID3v2Frame frame)
            : base(frame)
    {
        if (frame.ID != "SYLT")
        {
            throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "SYLT"));
        }
    }

    #endregion Internal Constructors

    #region Public Enums

    /// <summary>Synchronised lyrics/text content type.</summary>
    public enum ContentType
    {
        /// <summary>unknown content type</summary>
        Unknown = 0,

        /// <summary>lyrics</summary>
        Lyrics = 1,

        /// <summary>text transcription</summary>
        TextTranscription = 2,

        /// <summary>movement/part name (e.g. "Adagio")</summary>
        Movement = 3,

        /// <summary>events (e.g. "Don Quijote enters the stage")</summary>
        Events = 4,

        /// <summary>chord (e.g. "Bb F Fsus")</summary>
        Chord = 5,

        /// <summary>trivia/'pop up' information</summary>
        Trivia = 6,

        /// <summary>URLs to webpages</summary>
        WebpageURLs = 7,

        /// <summary>URLs to images</summary>
        ImageURLs = 8,
    }

    #endregion Public Enums

    #region Public Properties

    /// <summary>Gets the descriptor.</summary>
    public string Descriptor
    {
        get
        {
            if (!parsed)
            {
                Parse();
            }

            return descriptor ?? string.Empty;
        }
    }

    /// <summary>Gets the list of events.</summary>
    public ID3v2Event[] Events
    {
        get
        {
            if (!parsed)
            {
                Parse();
            }

            return events ?? [];
        }
    }

    /// <summary>Gets the language (3 character language code).</summary>
    public string Language
    {
        get
        {
            if (!parsed)
            {
                Parse();
            }

            return language ?? string.Empty;
        }
    }

    /// <summary>Gets the content type.</summary>
    public ContentType Type
    {
        get
        {
            if (!parsed)
            {
                Parse();
            }

            return contentType;
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a string describing this frame.</summary>
    /// <returns>ID[Length] ContentType LNG:"Descriptor".</returns>
    public override string ToString() => base.ToString() + " " + Type.ToString() + " " + Language + ":\"" + Descriptor + '"';

    #endregion Public Methods
}
