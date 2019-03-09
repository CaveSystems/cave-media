#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// PortAudio Host Api Type Identifier.
    /// </summary>
    internal enum PAHostApiTypeId : uint
    {
        /// <summary>in development</summary>
        InDevelopment = 0,

        /// <summary>direct sound</summary>
        DirectSound = 1,

        /// <summary>mme</summary>
        MME = 2,

        /// <summary>asio</summary>
        ASIO = 3,

        /// <summary>sound manager</summary>
        SoundManager = 4,

        /// <summary>core audio</summary>
        CoreAudio = 5,

        /// <summary>oss</summary>
        OSS = 7,

        /// <summary>alsa</summary>
        ALSA = 8,

        /// <summary>al</summary>
        AL = 9,

        /// <summary>be os</summary>
        BeOS = 10,
    }
}
