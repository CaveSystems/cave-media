using System;
using System.Runtime.InteropServices;
using Cave.IO;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Device Information.</summary>
[StructLayout(LayoutKind.Sequential)]
internal struct PADeviceInfo
{
    /// <summary>The structure version.</summary>
    public int StructVersion;

    IntPtr m_NamePtr;

    /// <summary>The host API.</summary>
    public int HostApi;

    /// <summary>The maximum input channels.</summary>
    public int MaxInputChannels;

    /// <summary>The maximum output channels.</summary>
    public int MaxOutputChannels;

    /// <summary>The default low input latency (s).</summary>
    public double DefaultLowInputLatency;

    /// <summary>The default low output latency (s).</summary>
    public double DefaultLowOutputLatency;

    /// <summary>The default high input latency (s).</summary>
    public double DefaultHighInputLatency;

    /// <summary>The default high output latency (s).</summary>
    public double DefaultHighOutputLatency;

    /// <summary>The default sample rate (HZ).</summary>
    public double DefaultSampleRate;

    /// <summary>Gets the name</summary>
    /// <value>The name</value>
    public UTF8 NameUtf8 => MarshalStruct.ReadUtf8(m_NamePtr);

    /// <summary>Gets the name ANSI.</summary>
    /// <value>The name ANSI.</value>
    public string? NameAnsi => Marshal.PtrToStringAnsi(m_NamePtr);

    /// <summary>Gets the name unicode.</summary>
    /// <value>The name unicode.</value>
    public string? NameUnicode => Marshal.PtrToStringUni(m_NamePtr);

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        return "[" + GetType().Name + "] " + Environment.NewLine +
            "Name: " + m_NamePtr + Environment.NewLine +
            "HostApi: " + HostApi + Environment.NewLine +
            "MaxInputChannels: " + MaxInputChannels + Environment.NewLine +
            "MaxOutputChannels: " + MaxOutputChannels + Environment.NewLine +
            "DefaultLowInputLatency: " + DefaultLowInputLatency + Environment.NewLine +
            "DefaultLowOutputLatency: " + DefaultLowOutputLatency + Environment.NewLine +
            "DefaultHighInputLatency: " + DefaultHighInputLatency + Environment.NewLine +
            "DefaultHighOutputLatency: " + DefaultHighOutputLatency + Environment.NewLine +
            "DefaultSampleRate: " + DefaultSampleRate;
    }
}
