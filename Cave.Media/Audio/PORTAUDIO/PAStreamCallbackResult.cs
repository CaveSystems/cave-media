namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Stream Callback Result.</summary>
internal enum PAStreamCallbackResult : uint
{
    /// <summary>continue</summary>
    Continue = 0,

    /// <summary>complete</summary>
    Complete = 1,

    /// <summary>abort</summary>
    Abort = 2,
}
