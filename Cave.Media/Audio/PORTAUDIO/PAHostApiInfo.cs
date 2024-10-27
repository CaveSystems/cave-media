using System;
using System.Runtime.InteropServices;
using Cave.IO;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Host Api Information structure.</summary>
[StructLayout(LayoutKind.Sequential)]
internal struct PAHostApiInfo
{
    /// <summary>The structure version.</summary>
    public int StructVersion;

    /// <summary>The PAHostApiTypeId.</summary>
    public PAHostApiTypeId Type;

    IntPtr namePtr;

    /// <summary>The device count.</summary>
    public int DeviceCount;

    /// <summary>The default input device.</summary>
    public int DefaultInputDevice;

    /// <summary>The default output device.</summary>
    public int DefaultOutputDevice;

    /// <summary>Gets the name UTF8.</summary>
    /// <value>The name UTF8.</value>
    public UTF8 NameUtf8 => MarshalStruct.ReadUtf8(namePtr);

    /// <summary>Gets the name ANSI.</summary>
    /// <value>The name ANSI.</value>
    public string? NameAnsi => Marshal.PtrToStringAnsi(namePtr);

    /// <summary>Gets the name unicode.</summary>
    /// <value>The name unicode.</value>
    public string? NameUnicode => Marshal.PtrToStringUni(namePtr);

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        return "[" + GetType().Name + "]" + Environment.NewLine +
            "StructVersion: " + StructVersion + Environment.NewLine +
            "Type: " + Type + Environment.NewLine +
            "NamePtr: " + namePtr + Environment.NewLine +
            "DeviceCount: " + DeviceCount + Environment.NewLine +
            "DefaultInputDevice: " + DefaultInputDevice + Environment.NewLine +
            "DefaultOutputDevice: " + DefaultOutputDevice;
    }
}
