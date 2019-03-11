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
    /// PortAudio Stream Callback Time Information.
    /// </summary>
    [StructLayout(LayoutKind.Sequential)]
    internal struct PAStreamCallbackTimeInfo
    {
        /// <summary>The current time since start (s).</summary>
        public double CurrentTime;

        /// <summary>The input buffer adc time (s).</summary>
        public double InputBufferAdcTime;

        /// <summary>The output buffer dac time (s).</summary>
        public double OutputBufferDacTime;

        /// <summary>Returns a <see cref="string" /> that represents this instance.</summary>
        /// <returns>A <see cref="string" /> that represents this instance.</returns>
        public override string ToString()
        {
            return "[" + GetType().Name + "]" + Environment.NewLine +
                "CurrentTime: " + CurrentTime + Environment.NewLine +
                "InputBufferAdcTime: " + InputBufferAdcTime + Environment.NewLine +
                "OutputBufferDacTime: " + OutputBufferDacTime;
        }
    }
}
