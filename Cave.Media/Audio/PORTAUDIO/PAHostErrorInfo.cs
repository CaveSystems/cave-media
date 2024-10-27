using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Host Error Information structure.</summary>
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

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        return "[" + GetType().Name + "]" + Environment.NewLine +
            "HostApiType: " + HostApiType + Environment.NewLine +
            "ErrorCode: " + ErrorCode + Environment.NewLine +
            "ErrorText: " + ErrorText;
    }
}
