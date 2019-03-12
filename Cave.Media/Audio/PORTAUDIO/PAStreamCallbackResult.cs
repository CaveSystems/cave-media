#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// PortAudio Stream Callback Result.
    /// </summary>
    internal enum PAStreamCallbackResult : uint
    {
        /// <summary>continue</summary>
        Continue = 0,

        /// <summary>complete</summary>
        Complete = 1,

        /// <summary>abort</summary>
        Abort = 2,
    }
}
