using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// The WAVEFORMAT structure describes the format of waveform-audio data. Only format information common to all waveform-audio data formats is included in this structure. This structure has been superseded by the WAVEFORMATEX structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    [Obsolete("This structure has been superseded by the WAVEFORMATEX structure.")]
    public struct WAVEFORMAT
    {
        /// <summary>
        /// Format type. The following type is defined:
        /// WAVE_FORMAT_PCM: Waveform-audio data is PCM.
        /// </summary>
        public WAVEFORMATTAG FormatTag;

        /// <summary>
        /// Number of channels in the waveform-audio data. Mono data uses one channel and stereo data uses two channels.
        /// </summary>
        public short Channels;

        /// <summary>
        /// Sample rate, in samples per second.
        /// </summary>
        public int SamplesPerSec;

        /// <summary>
        /// Required average data transfer rate, in bytes per second. For example, 16-bit stereo at 44.1 kHz has an average data rate of 176,400 bytes per second (2 channels — 2 bytes per sample per channel — 44,100 samples per second).
        /// </summary>
        public int AvgBytesPerSec;

        /// <summary>
        /// Block alignment, in bytes. The block alignment is the minimum atomic unit of data. For PCM data, the block alignment is the number of bytes used by a single sample, including data for both channels if the data is stereo. For example, the block alignment for 16-bit stereo PCM is 4 bytes (2 channels — 2 bytes per sample).
        /// </summary>
        public short BlockAlign;
    }
}
