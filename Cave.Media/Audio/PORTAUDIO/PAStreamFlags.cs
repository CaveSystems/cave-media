#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

using System;

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// Port Audio Stream Flags
    /// </summary>
    [Flags]
    internal enum PAStreamFlags : uint
    {
        /// <summary>No Flags</summary>
        None = 0,

        /// <summary>Enable clip off</summary>
        ClipOff = 0x00000001,

        /// <summary>Disable dithering</summary>
        DitherOff = 0x00000002,

        /// <summary>Enable never drop input</summary>
        NeverDropInput = 0x00000004,

        /// <summary>Prime output buffers using stream callback</summary>
        PrimeOutputBuffersUsingStreamCallback = 0x00000008,

        /// <summary>The platform specific flags mask</summary>
        PlatformSpecificFlags = 0xFFFF0000
    }
}
