using System;
namespace Cave.Media.Audio.ID3.Frames;

/// <summary>
/// With these frames dynamic data such as webpages with touring
/// information, price information or plain ordinary news can be added to
/// the tag.
/// </summary>
public class ID3v2WebFrame : ID3v2Frame
{
    string? address;

    void Parse() => ID3v2Encoding.Parse(0, Content, 0, out address);

    internal ID3v2WebFrame(ID3v2Frame frame)
        : base(frame)
    {
        if ((frame.ID[0] != 'W') || (frame.ID == "WXXX"))
        {
            throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "W*"));
        }
    }

    /// <summary>
    /// Gets the URL / address this frame contains.
    /// </summary>
    public string Address
    {
        get
        {
            if (address == null)
            {
                Parse();
            }

            return address ?? string.Empty;
        }
    }

    /// <summary>
    /// Gets a string describing this frame.
    /// </summary>
    /// <returns>ID[Length] "URL".</returns>
    public override string ToString() => base.ToString() + " \"" + Address + '"';
}
