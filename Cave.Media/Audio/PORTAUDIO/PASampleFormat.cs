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
    /// PortAudio Sample Format.
    /// </summary>
    [Flags]
    internal enum PASampleFormat : uint
    {
        /// <summary>The float32 sample format</summary>
        Float32 = 0x00000001,

        /// <summary>The int32 sample format</summary>
        Int32 = 0x00000002,

        /// <summary>The int24 sample format</summary>
        Int24 = 0x00000004,

        /// <summary>The int16 sample format</summary>
        Int16 = 0x00000008,

        /// <summary>The int8 sample format = 1 signed byte per sample</summary>
        Int8 = 0x00000010,

        /// <summary>The unsigned int8 sample format = 1 unsigned byte per sample</summary>
        UInt8 = 0x00000020,

        /// <summary>The custom format sample format</summary>
        CustomFormat = 0x00010000,

        /// <summary>The non interleaved sample format</summary>
        NonInterleaved = 0x80000000,
    }
}
