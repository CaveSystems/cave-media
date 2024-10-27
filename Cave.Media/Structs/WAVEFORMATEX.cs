using System.Runtime.InteropServices;

namespace Cave.Media.Structs;

/// <summary>
/// The WAVEFORMATEX structure defines the format of waveform-audio data. Only format information common to all waveform-audio data formats is included in this structure. For formats that require additional information, this structure is included as the first member in another structure, along with the additional information.
/// Formats that support more than two channels or sample sizes of more than 16 bits can be described in a WAVEFORMATEXTENSIBLE structure, which includes the WAVEFORMAT structure.
/// </summary>
[StructLayout(LayoutKind.Sequential, Pack = 1)]
public struct WAVEFORMATEX
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

    /// <summary>
    /// Bits per sample for the <see cref="FormatTag"/> format type. If <see cref="FormatTag"/> is WAVE_FORMAT_PCM, then <see cref="BitsPerSample"/> should be equal to 8 or 16. For non-PCM formats, this member must be set according to the manufacturer's specification of the format tag. If wFormatTag is WAVE_FORMAT_EXTENSIBLE, this value can be any integer multiple of 8 and represents the container size, not necessarily the sample size; for example, a 20-bit sample size is in a 24-bit container. Some compression schemes cannot define a value for wBitsPerSample, so this member can be 0.
    /// </summary>
    public short BitsPerSample;

    /// <summary>
    /// Size, in bytes, of extra format information appended to the end of the WAVEFORMATEX structure. This information can be used by non-PCM formats to store extra attributes for the wFormatTag. If no extra information is required by the wFormatTag, this member must be set to 0. For WAVE_FORMAT_PCM formats (and only WAVE_FORMAT_PCM formats), this member is ignored. When this structure is included in a WAVEFORMATEXTENSIBLE structure, this value must be at least 22.
    /// </summary>
    public short Size;
}
