using System;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Stream Callback Flags.</summary>
[Flags]
internal enum PAStreamCallbackFlags : uint
{
    /// <summary>An input underflow occured</summary>
    InputUnderflow = 0x00000001,

    /// <summary>An input overflow occured</summary>
    InputOverflow = 0x00000002,

    /// <summary>An output underflow occured</summary>
    OutputUnderflow = 0x00000004,

    /// <summary>An output overflow occured</summary>
    OutputOverflow = 0x00000008,

    /// <summary>priming the output buffer</summary>
    PrimingOutput = 0x00000010,
}
