using System;

namespace Cave.Media.Audio.ID3.Frames;

/// <summary>
/// This frame contains the lyrics of the song or a text transcription of other vocal activities. The head includes an encoding descriptor and a content descriptor.
/// </summary>
public sealed class ID3v2USLTFrame : ID3v2Frame
{
    #region Private Fields

    string? descriptor;
    string? language;
    string[]? lines;
    bool parsed = false;

    #endregion Private Fields

    #region Private Methods

    void Parse()
    {
        var encoding = (ID3v2EncodingType)Content[0];
        language = ID3v2Encoding.ISO88591.GetString(Content, 1, 3);
        var start = 4 + ID3v2Encoding.Parse(encoding, Content, 4, out descriptor);
        string text;
        ID3v2Encoding.Parse(encoding, Content, start, out text);
        lines = text.Split('\n');
    }

    #endregion Private Methods

    #region Internal Constructors

    internal ID3v2USLTFrame(ID3v2Frame frame)
        : base(frame)
    {
        if (frame.ID != "USLT")
        {
            throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "USLT"));
        }
    }

    #endregion Internal Constructors

    #region Public Properties

    /// <summary>Gets the lyric descripto.</summary>
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

    /// <summary>Gets the lyrics language.</summary>
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

    /// <summary>Gets the full song text.</summary>
    public string[] Lines
    {
        get
        {
            if (!parsed)
            {
                Parse();
            }

            return lines ?? [];
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a string describing this frame.</summary>
    /// <returns>ID[Length] LNG:"Descriptor".</returns>
    public override string ToString() => base.ToString() + " " + Language + ":\"" + Descriptor + '"';

    #endregion Public Methods
}
