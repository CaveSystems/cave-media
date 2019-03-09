#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// PortAudio Host Error Information structure.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PAHostErrorInfo
    {
        /// <summary>The host API type.</summary>
        public PAHostApiTypeId HostApiType;

        /// <summary>The error code.</summary>
        public PAErrorCode ErrorCode;

        /// <summary>The error text.</summary>
        [MarshalAs(UnmanagedType.LPStr)]
        public string ErrorText;

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "[" + GetType().Name + "]" + Environment.NewLine +
                "HostApiType: " + HostApiType + Environment.NewLine +
                "ErrorCode: " + ErrorCode + Environment.NewLine +
                "ErrorText: " + ErrorText;
        }
    }
}
