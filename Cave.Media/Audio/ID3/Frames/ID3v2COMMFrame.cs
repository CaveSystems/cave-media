using System;

namespace Cave.Media.Audio.ID3.Frames;

/// <summary>Comments frame: <br/> This frame is intended for any kind of full text information that does not fit in any other frame.</summary>
public sealed class ID3v2COMMFrame : ID3v2Frame
{
    #region Private Fields

    string? description;
    string? language;
    string[]? lines;

    #endregion Private Fields

    #region Private Methods

    void Parse()
    {
        var encoding = (ID3v2EncodingType)Content[0];
        language = ID3v2Encoding.ISO88591.GetString(Content, 1, 3);
        var len = ID3v2Encoding.Parse(encoding, Content, 4, out description);
        ID3v2Encoding.Parse(encoding, Content, 4 + len, out var text);
        lines = text.SplitNewLine();
    }

    #endregion Private Methods

    #region Internal Constructors

    internal ID3v2COMMFrame(ID3v2Frame frame)
        : base(frame)
    {
        if (frame.ID != "COMM")
        {
            throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "COMM"));
        }
    }

    #endregion Internal Constructors

    #region Public Properties

    /// <summary>Gets the description of the text this frame contains.</summary>
    public string Description
    {
        get
        {
            if (description == null)
            {
                Parse();
            }

            return description ?? string.Empty;
        }
    }

    /// <summary>Gets the language (3 character code).</summary>
    public string Language
    {
        get
        {
            if (language == null)
            {
                Parse();
            }

            return language ?? string.Empty;
        }
    }

    /// <summary>Gets the text this frame contains.</summary>
    public string[] Lines
    {
        get
        {
            if (lines == null)
            {
                Parse();
            }

            return lines ?? [];
        }
    }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Gets a string describing this frame.</summary>
    /// <returns>ID[Length] "Text".</returns>
    public override string ToString() => base.ToString() + " \"" + Description + '"';

    #endregion Public Methods
}
