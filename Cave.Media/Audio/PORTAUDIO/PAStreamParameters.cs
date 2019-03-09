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
    /// PortAudio Stream Parameters.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PAStreamParameters
    {
        /// <summary>The device.</summary>
        public int Device;

        /// <summary>The channel count.</summary>
        public int ChannelCount;

        /// <summary>The sample format.</summary>
        public PASampleFormat SampleFormat;

        /// <summary>The suggested latency (s).</summary>
        public double SuggestedLatency;

        /// <summary>The host API specific stream information.</summary>
        internal IntPtr HostApiSpecificStreamInfo;

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "[" + GetType().Name + "]" + Environment.NewLine +
                "Device: " + Device + Environment.NewLine +
                "ChannelCount: " + ChannelCount + Environment.NewLine +
                "SampleFormat: " + SampleFormat + Environment.NewLine +
                "SuggestedLatency: " + SuggestedLatency;
        }
    }
}
