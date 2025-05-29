using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;
using Cave.IO;
using Cave.Media.Audio.MP3;
using Cave.Media.Audio.MPG123;

namespace Cave.Media.Audio;

/// <summary>Provides an <see cref="IAudioDecoder"/> implementation for MPG123.</summary>
public sealed class Mpg123 : CriticalFinalizerObject, IAudioDecoder, IDisposable
{
    #region Private Fields

    static bool? isAvailable;

    bool disposed;
    bool initialized;
    IAudioConfiguration? currentConfig;
    TimeSpan currentTimeStamp = TimeSpan.Zero;
    readonly FifoBuffer decodeFifoBuffer = new();
    IntPtr decoderHandle = IntPtr.Zero;
    IFrameSource? source;
    bool useFloatingPoint;

    #endregion Private Fields

    #region Private Destructors

    /// <summary>Finalizes an instance of the <see cref="Mpg123"/> class.</summary>
    ~Mpg123()
    {
        ReleaseHandle();
    }

    #endregion Private Destructors

    #region Private Methods

    /// <summary>buffers a frame into mpg123.</summary>
    void BufferFrame()
    {
        for (var i = 0; i < 1;)
        {
            var frame = source!.GetNextFrame();
            if (frame == null)
            {
                break;
            }

            Decoding?.Invoke(this, new AudioFrameEventArgs(frame));
            if (frame.IsAudio)
            {
                decodeFifoBuffer.Enqueue(frame.Data, true);
                i++;
            }
        }
    }

    void ReleaseHandle()
    {
        if (decoderHandle != IntPtr.Zero)
        {
            M123.SafeNativeMethods.mpg123_close(decoderHandle);
            decoderHandle = IntPtr.Zero;
        }
    }

    void UpdateFormat() => currentConfig = M123.SafeNativeMethods.mpg123_getformat(decoderHandle);

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="Mpg123"/> class.</summary>
    public Mpg123() { }

    /// <summary>Initializes a new instance of the <see cref="Mpg123"/> class.</summary>
    public Mpg123(bool useFloatingPoint)
    {
        this.useFloatingPoint = useFloatingPoint;
    }

    #endregion Public Constructors

    #region Public Events

    /// <summary>Occurs when [decoding a frame].</summary>
    public event EventHandler<AudioFrameEventArgs>? Decoding;

    #endregion Public Events

    #region Public Properties

    /// <summary>Gets the description for the lame encoder.</summary>
    public string Description => "This is the fast and Free (LGPL license) real time MPEG Audio Layer 1, 2 and 3 decoding library." + Environment.NewLine +
                "It uses floating point or integer math, along with several special optimizations (3DNow, SSE, ARM, ...) to make it most efficient.";

    /// <summary>Gets the featurelist of the mpg123 decoder.</summary>
    public string Features => "Very fast mpeg audio decoder." + Environment.NewLine +
                "Really efficient with a growing number of assembler optimizations (pentium, MMX, AltiVec, ...)" + Environment.NewLine +
                "MPEG1,2 and 2.5 layer III decoding." + Environment.NewLine +
                "CBR (constant bitrate) and two types of variable bitrate, VBR and ABR.";

    /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
    /// <value><c>true</c> if this instance is available; otherwise, <c>false</c>.</value>
    public bool IsAvailable
    {
        get
        {
            if (!isAvailable.HasValue)
            {
                try { isAvailable = M123.SafeNativeMethods.mpg123_decoders().Length > 0; }
                catch (Exception ex)
                {
                    Trace.WriteLine("Error checking mpg123 library.\n" + ex);
                    isAvailable = false;
                }
            }
            return isAvailable.Value;
        }
    }

    /// <summary>Gets the name of the log source.</summary>
    /// <value>The name of the log source.</value>
    public string LogSourceName => "Mpg123";

    /// <summary>Returns the mpeg 1,2,2.5 layer 3 mime types.</summary>
    public string[] MimeTypes => new string[]
            {
                "audio/mpeg",
                "audio/mpeg3",
                "audio/x-mpeg",
                "audio/x-mpeg3",
            };

    /// <summary>Gets the encoder name.</summary>
    public string Name => "MPG123";

    /// <summary>Gets the name of the source currently beeing decoded. This is used for error messages.</summary>
    public string? SourceName { get; set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Starts the decoding process.</summary>
    /// <param name="fileName">Name of the file.</param>
    public void BeginDecode(string fileName) => BeginDecode(new MP3Reader(fileName));

    /// <summary>Starts the decoding process.</summary>
    /// <param name="sourceStream">The source Stream providing the encoded data.</param>
    /// <exception cref="Exception">Source + SourceName + : Decoding already started!.</exception>
    public void BeginDecode(Stream sourceStream) => BeginDecode(new MP3Reader(sourceStream));

    /// <summary>Starts the decoding process.</summary>
    /// <param name="source">The source.</param>
    /// <exception cref="InvalidOperationException">Source: Decoding already started!.</exception>
    public void BeginDecode(IFrameSource source)
    {
        if (disposed)
        {
            throw new ObjectDisposedException(LogSourceName);
        }

        if (initialized)
        {
            throw new InvalidOperationException(string.Format("Source {0}: Decoding already started!", SourceName));
        }

        if (SourceName != null)
        {
            SourceName = source.Name;
        }

        initialized = true;
        M123.Initialize();

        this.source = source;

        // open new decoder handle
        M123.RESULT result;
        decoderHandle = M123.SafeNativeMethods.mpg123_new(null, out result);
        M123.CheckResult(result);

        // reset formats
        M123.CheckResult(M123.SafeNativeMethods.mpg123_format_none(decoderHandle));

        // allow all mp3 native samplerates
        var mode = useFloatingPoint ? M123.ENC.FLOAT_32 : M123.ENC.SIGNED_16;
        foreach (var sampleRate in M123.SafeNativeMethods.mpg123_rates())
        {
            M123.CheckResult(M123.SafeNativeMethods.mpg123_format(decoderHandle, new IntPtr(sampleRate), M123.CHANNELCOUNT.STEREO, mode));
        }

        // open feed
        result = M123.SafeNativeMethods.mpg123_open_feed(decoderHandle);
        M123.CheckResult(result);
    }

    /// <summary>Closes the underlying stream and calls Dispose.</summary>
    public void Close()
    {
        if (disposed)
        {
            throw new ObjectDisposedException(LogSourceName);
        }

        if (initialized)
        {
            M123.Deinitialize();
            initialized = false;
        }
        if (source != null)
        {
            source.Close();
            source = null;
        }
    }

    /// <summary>Decodes audio data.</summary>
    /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available.</returns>
    public IAudioData? Decode()
    {
        if (!initialized) throw new InvalidOperationException("Not initialized. Start BeginDecode() first!");
        if (disposed)
        {
            throw new ObjectDisposedException(LogSourceName);
        }

        BufferFrame();

        // end of file ? -> yes exit
        if (decodeFifoBuffer.Length == 0)
        {
            return null;
        }

        var outBuffer = new FifoBuffer();
        var loop = true;
        while (loop)
        {
            M123.RESULT result;
            result = M123.SafeNativeMethods.mpg123_decode(decoderHandle, decodeFifoBuffer, outBuffer, 8192);
            switch (result)
            {
                case M123.RESULT.NEED_MORE:
                    if (outBuffer.Length > 0)
                    {
                        loop = false;
                        break;
                    }
                    BufferFrame();
                    if (decodeFifoBuffer.Length == 0)
                    {
                        return null;
                    }

                    break;

                case M123.RESULT.NEW_FORMAT: UpdateFormat(); break;
                default: M123.CheckResult(result); throw new InvalidOperationException();
            }
        }
        if (outBuffer.Length > 0)
        {
            var resultData = new AudioData(currentConfig!.SamplingRate, currentConfig.Format, currentConfig.ChannelSetup, currentTimeStamp, 0, -1, outBuffer.ToArray());
            currentTimeStamp += resultData.Duration;
            return resultData;
        }
        return null;
    }

    /// <summary>Disposes this instance.</summary>
    public void Dispose()
    {
        if (disposed)
        {
            return;
        }

        Close();
        disposed = true;
        ReleaseHandle();
        GC.SuppressFinalize(this);
    }

    #endregion Public Methods
}
