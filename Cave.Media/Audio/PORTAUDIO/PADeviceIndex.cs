#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// PortAudio device index.
    /// </summary>
    internal enum PADeviceIndex : int
    {
        /// <summary>The no device</summary>
        NoDevice = -1,

        /// <summary>
        /// Use Host Api Specific Device Specification
        /// </summary>
        UseHostApiSpecificDeviceSpecification = -2,
    }
}
