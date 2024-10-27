using System;
using System.IO;

namespace Cave.Media.Audio.MP3;

/// <summary>Provides a mp3 audio frame (containing encoded mp3 audio data).</summary>
public sealed class MP3AudioFrame : AudioFrame
{
    #region Private Fields

    MP3BitReserve? bits;
    byte[]? data;
    MP3AudioFrameHeader? header;
    TimeSpan m_Duration = TimeSpan.Zero;

    #endregion Private Fields

    #region Public Constructors

    /// <summary>Creates a new empty frame.</summary>
    public MP3AudioFrame()
    {
    }

    /// <summary>Creates a new empty frame.</summary>
    public MP3AudioFrame(byte[] data)
    {
        this.data = data;
        header = new MP3AudioFrameHeader(data);
        if (header.Validation != MP3AudioFrameHeadervalidation.Valid)
        {
            throw new InvalidDataException();
        }

        var dataLength = header.Length;
        if (dataLength == 0 || data.Length != dataLength)
        {
            throw new InvalidDataException();
        }
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Retrieves the bits of the frame exclusing header.</summary>
    /// <value>The bits.</value>
    public MP3BitReserve Bits
    {
        get
        {
            if (bits == null)
            {
                if (Header.Protection)
                {
                    bits = new MP3BitReserve(data, 6);
                }
                else
                {
                    bits = new MP3BitReserve(data, 4);
                }
            }
            return bits;
        }
    }

    /// <summary>Gets an array with the data for this instance.</summary>
    /// <returns></returns>
    public override byte[] Data => data ?? [];

    /// <summary>Gets the duration of the audio frame.</summary>
    public override TimeSpan Duration
    {
        get
        {
            if (m_Duration == TimeSpan.Zero)
            {
                m_Duration = new TimeSpan(Header.SampleCount * TimeSpan.TicksPerSecond / Header.SamplingRate);
            }
            return m_Duration;
        }
    }

    /// <summary>Gets the <see cref="MP3AudioFrameHeader"/>.</summary>
    public MP3AudioFrameHeader Header => header ?? throw new InvalidOperationException("Header not initialized!");

    /// <summary>Gets whether the padding bit at the header was corrected during Parse().</summary>
    public bool InvalidPaddingCorrected { get; private set; }

    /// <summary>Returns true.</summary>
    public override bool IsAudio => true;

    /// <summary>Returns false (mp3 audio frames may differ in size depending on layer, bitrate and samplerate).</summary>
    public override bool IsFixedLength => false;

    /// <summary>Returns true.</summary>
    public override bool IsValid => true;

    /// <summary>Length of the frame in bytes.</summary>
    public override int Length => data?.Length ?? 0;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Parses the specified stream to load all fields for this instance.</summary>
    /// <param name="reader">FrameReader to read from.</param>
    public override bool Parse(DataFrameReader reader)
    {
        if (reader == null)
        {
            throw new ArgumentNullException("Reader");
        }

        if (!reader.EnsureBuffer(4))
        {
            return false;
        }

        var headerData = reader.Read(0, 4);
        header = new MP3AudioFrameHeader(headerData);
        if (header.Validation != MP3AudioFrameHeadervalidation.Valid)
        {
            return false;
        }

        var dataLength = header.Length;
        if (dataLength == 0)
        {
            return false;
        }

        if (!reader.EnsureBuffer(dataLength))
        {
            return false;
        }

        data = reader.Read(0, dataLength);

        // check next header
        if (reader.EnsureBuffer(dataLength + 4))
        {
            var nextHeaderBuffer = reader.Read(dataLength, 4);
            var next = new MP3AudioFrameHeader(nextHeaderBuffer);
            if (next.Validation != MP3AudioFrameHeadervalidation.Valid)
            {
                if ((nextHeaderBuffer[0] == 'I') && (nextHeaderBuffer[1] == 'D') && (nextHeaderBuffer[2] == '3'))
                {
                    // ID3 v2 tag incoming
                }
                else if ((nextHeaderBuffer[0] == 'T') && (nextHeaderBuffer[1] == 'A') && (nextHeaderBuffer[2] == 'G'))
                {
                    // ID3 v1 tag incoming
                }
                else
                {
                    // next header is invalid, check if the padding bit is set incorrectly there is a high pobability that the padding bit is invalid if the
                    // framestart is not directly after our buffer but one byte late
                    var newStart = dataLength + (header.Padding ? -1 : 1);
                    nextHeaderBuffer = reader.Read(newStart, 4);
                    next = new MP3AudioFrameHeader(nextHeaderBuffer);
                    if (next.Validation == MP3AudioFrameHeadervalidation.Valid)
                    {
                        if (!header.Padding)
                        {
                            // frame has a padding byte but the header padding bit is not set
                            data = reader.Read(0, newStart);
                        }
                        else
                        {
                            // frame has no padding byte but the header padding bit is set
                            data = reader.Read(0, newStart);
                        }
                        InvalidPaddingCorrected = true;
                    }
                }
            }
        }

        reader.Remove(data.Length);
        return true;
    }

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString() => $"MP3AudioFrame {header}";

    #endregion Public Methods
}
