#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// The AVISTREAMHEADER structure contains information about one stream in an AVI file. AVI stream header: riff chunk 'strh'.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1, Size = 56)]
    public struct AVISTREAMHEADER
    {
        /// <summary>
        /// Contains a FOURCC that specifies the type of the data contained in the stream. The following standard AVI values for video and audio are defined.
        /// AUDS, MIDS, TXTS, VIDS see FOURCC.
        /// </summary>
        public uint fccType;

        /// <summary>
        /// Optionally, contains a FOURCC that identifies a specific data handler. The data handler is the preferred handler for the stream. For audio and video streams, this specifies the codec for decoding the stream.
        /// </summary>
        public uint fccHandler;

        /// <summary>
        /// Applicable flags for the stream. The bits in the high-order word of these flags are specific to the type of data contained in the stream. The following flags are defined:.
        /// </summary>
        public AVISTREAMHEADERFLAGS Flags;

        /// <summary>
        /// Priority of the stream.
        /// </summary>
        public short Priority;

        /// <summary>
        /// Language.
        /// </summary>
        public short Language;

        /// <summary>
        /// Specifies how far audio data is skewed ahead of the video frames in interleaved files. Typically, this is about 0.75 seconds. If you are creating interleaved files, specify the number of frames in the file prior to the initial frame of the AVI sequence in this member. For more information, see the remarks for the dwInitialFrames member of the AVIMAINHEADER structure.
        /// </summary>
        public int InitialFrames;

        /// <summary>
        /// Time scale applicable for the stream. Dividing dwRate by dwScale gives the playback rate in number of samples per second.
        /// </summary>
        public int Scale;

        /// <summary>
        /// Rate in an integer format. To obtain the rate in samples per second, divide this value by the value in Scale.
        /// </summary>
        public int Rate;

        /// <summary>
        /// Sample number of the first frame of the AVI file. The units are defined by dwRate and dwScale. Normally, this is zero, but it can specify a delay time for a stream that does not start concurrently with the file.
        /// </summary>
        public int Start;

        /// <summary>
        /// Length of this stream. The units are defined by dwRate and dwScale.
        /// </summary>
        public int Length;

        /// <summary>
        /// Recommended buffer size, in bytes, for the stream. Typically, this member contains a value corresponding to the largest chunk in the stream. Using the correct buffer size makes playback more efficient. Use zero if you do not know the correct buffer size.
        /// </summary>
        public int SuggestedBufferSize;

        /// <summary>
        /// Quality indicator of the video data in the stream. Quality is represented as a number between 0 and 10,000. For compressed data, this typically represents the value of the quality parameter passed to the compression software. If set to –1, drivers use the default quality value.
        /// </summary>
        public int Quality;

        /// <summary>
        /// Size, in bytes, of a single data sample. If the value of this member is zero, the samples can vary in size and each data sample (such as a video frame) must be in a separate chunk. A nonzero value indicates that multiple samples of data can be grouped into a single chunk within the file.
        /// </summary>
        public int SampleSize;
    }
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
