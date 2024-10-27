using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// The AVIMAINHEADER structure defines global information in an AVI file. AVI stream header: riff chunk 'avih'.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct AVIMAINHEADER
{
    /// <summary>
    /// Specifies the number of microseconds between frames. This value indicates the overall timing for the file.
    /// </summary>
    public int MicroSecPerFrame;

    /// <summary>
    /// Specifies the approximate maximum data rate of the file. This value indicates the number of bytes per second the system must handle to present an AVI sequence as specified by the other parameters contained in the main header and stream header chunks.
    /// </summary>
    public int MaxBytesPerSec;

    /// <summary>
    /// Specifies the alignment for data, in bytes. Pad the data to multiples of this value.
    /// </summary>
    public int PaddingGranularity;

    /// <summary>
    /// ontains a bitwise combination of zero or more of the <see cref="AVIFLAGS"/> flags.
    /// </summary>
    public AVIFLAGS Flags;

    /// <summary>
    /// Specifies the total number of frames of data in the file.
    /// </summary>
    public int TotalFrames;

    /// <summary>
    /// Specifies the initial frame for interleaved files. Noninterleaved files should specify zero. If you are creating interleaved files, specify the number of frames in the file prior to the initial frame of the AVI sequence in this member.
    /// </summary>
    public int InitialFrames;

    /// <summary>
    /// Specifies the number of streams in the file. For example, a file with audio and video has two streams.
    /// </summary>
    public int Streams;

    /// <summary>
    /// Specifies the suggested buffer size for reading the file. Generally, this size should be large enough to contain the largest chunk in the file. If set to zero, or if it is too small, the playback software will have to reallocate memory during playback, which will reduce performance. For an interleaved file, the buffer size should be large enough to read an entire record, and not just a chunk.
    /// </summary>
    public int SuggestedBufferSize;

    /// <summary>
    /// Specifies the width of the AVI file in pixels.
    /// </summary>
    public int Width;

    /// <summary>
    /// Specifies the height of the AVI file in pixels.
    /// </summary>
    public int Height;

    /// <summary>
    /// Reserved. Set this array to zero.
    /// </summary>
    [MarshalAs(UnmanagedType.ByValArray, SizeConst = 4)]
    public int[] Reserved;
}
