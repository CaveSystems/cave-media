#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// PortAudio error codes.
    /// </summary>
    internal enum PAErrorCode : int
    {
        /// <summary>no error</summary>
        NoError = 0,

        /// <summary>not initialized</summary>
        NotInitialized = -10000,

        /// <summary>unanticited host error</summary>
        UnanticitedHostError,

        /// <summary>invalid channel count</summary>
        InvalidChannelCount,

        /// <summary>invalid sample rate</summary>
        InvalidSampleRate,

        /// <summary>invalid device</summary>
        InvalidDevice,

        /// <summary>invalid flag</summary>
        InvalidFlag,

        /// <summary>sample format not supported</summary>
        SampleFormatNotSupported,

        /// <summary>bad io device combination</summary>
        BadIODeviceCombination,

        /// <summary>insufficient memory</summary>
        InsufficientMemory,

        /// <summary>buffer too big</summary>
        BufferTooBig,

        /// <summary>buffer too small</summary>
        BufferTooSmall,

        /// <summary>null callback</summary>
        NullCallback,

        /// <summary>bad stream Pointer</summary>
        BadStreamPtr,

        /// <summary>timed out</summary>
        TimedOut,

        /// <summary>internal error</summary>
        InternalError,

        /// <summary>device unavailable</summary>
        DeviceUnavailable,

        /// <summary>incompatible host API specific stream information</summary>
        IncompatibleHostApiSpecificStreamInfo,

        /// <summary>stream is stopped</summary>
        StreamIsStopped,

        /// <summary>stream is not stopped</summary>
        StreamIsNotStopped,

        /// <summary>input overflowed</summary>
        InputOverflowed,

        /// <summary>output underflowed</summary>
        OutputUnderflowed,

        /// <summary>host API not found</summary>
        HostApiNotFound,

        /// <summary>invalid host API</summary>
        InvalidHostApi,

        /// <summary>can not read from a callback stream</summary>
        CanNotReadFromACallbackStream,

        /// <summary>can not write to a callback stream</summary>
        CanNotWriteToACallbackStream,

        /// <summary>can not read from an output only stream</summary>
        CanNotReadFromAnOutputOnlyStream,

        /// <summary>can not write to an input only stream</summary>
        CanNotWriteToAnInputOnlyStream,

        /// <summary>incompatible stream host API</summary>
        IncompatibleStreamHostApi,
    }
}
