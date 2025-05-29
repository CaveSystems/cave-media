using System;
using System.Runtime.InteropServices;

namespace Cave.Media.Audio.PORTAUDIO;

/// <summary>PortAudio Stream Information.</summary>
[StructLayout(LayoutKind.Sequential)]
internal struct PAStreamInfo
{
    /// <summary>The structure version.</summary>
    public int StructVersion;

    /// <summary>The input latency (s).</summary>
    public double InputLatency;

    /// <summary>The output latency (s).</summary>
    public double OutputLatency;

    /// <summary>The sample rate (Hz).</summary>
    public double SampleRate;

    /// <summary>Returns a <see cref="string"/> that represents this instance.</summary>
    /// <returns>A <see cref="string"/> that represents this instance.</returns>
    public override string ToString()
    {
        return "[" + GetType().Name + "]" + Environment.NewLine +
            "StructVersion: " + StructVersion + Environment.NewLine +
            "InputLatency: " + InputLatency + Environment.NewLine +
            "OutputLatency: " + OutputLatency + Environment.NewLine +
            "SampleRate: " + SampleRate;
    }
}
