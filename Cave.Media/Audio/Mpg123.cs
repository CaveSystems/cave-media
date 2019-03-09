#region License mpg123
/*
    Uses mpg123 (http://www.mpg123.de)
    copyright 1995-2010 by the mpg123 project
    free software under the terms of the LGPL 2.1

    This program/library/sourcecode is free software; you can redistribute it
    and/or modify it under the terms of the GNU Lesser General Public
    License as published by the Free Software Foundation; either
    version 2.1 of the License, or (at your option) any later version.

    You may not use this program/library/sourcecode except in compliance
    with the License. The License is included in the LICENSE.LGPL21 file
    found at the installation directory or the distribution package.

    This library is distributed in the hope that it will be useful,
    but WITHOUT ANY WARRANTY; without even the implied warranty of
    MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the GNU
    Lesser General Public License for more details.

    A non-GPL license for this library is not available.
*/
#endregion

using Cave.IO;
using Cave.Media.Audio.MP3;
using Cave.Media.Audio.MPG123;
using System;
using System.Diagnostics;
using System.IO;
using System.Runtime.ConstrainedExecution;

namespace Cave.Media.Audio
{
    /// <summary>
    /// Provides an <see cref="IAudioDecoder" /> implementation for MPG123.
    /// </summary>
    public sealed class Mpg123 : CriticalFinalizerObject, IAudioDecoder, IDisposable
    {
        static bool? m_IsAvailable;

        bool m_Initialized;
        bool m_UseFloatingPoint;
        bool m_Disposed;

        /// <summary>Initializes a new instance of the <see cref="Mpg123"/> class.</summary>
        public Mpg123() { }

        /// <summary>Initializes a new instance of the <see cref="Mpg123"/> class.</summary>
        public Mpg123(bool useFloatingPoint)
        {
            m_UseFloatingPoint = useFloatingPoint;
        }

        /// <summary>
        /// Releases the handle
        /// </summary>
        ~Mpg123()
        {
            ReleaseHandle();
        }

        #region private implementation

        IntPtr m_DecoderHandle = IntPtr.Zero;
        IAudioConfiguration m_CurrentConfig;

        void ReleaseHandle()
        {
            if (m_DecoderHandle != IntPtr.Zero)
            {
                M123.SafeNativeMethods.mpg123_close(m_DecoderHandle);
                m_DecoderHandle = IntPtr.Zero;
            }
        }

        void UpdateFormat()
        {
            m_CurrentConfig = M123.SafeNativeMethods.mpg123_getformat(m_DecoderHandle);
        }
        #endregion

        /// <summary>Occurs when [decoding a frame].</summary>
        public event EventHandler<AudioFrameEventArgs> Decoding;

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName { get { return "Mpg123"; } }

        #region IAudioDecoder Member

        /// <summary>Gets a value indicating whether this decoder is available on this platform/installation or not.</summary>
        /// <value>
        /// <c>true</c> if this instance is available; otherwise, <c>false</c>.
        /// </value>
        public bool IsAvailable
        {
            get
            {
                if (!m_IsAvailable.HasValue)
                {
                    try { m_IsAvailable = M123.SafeNativeMethods.mpg123_decoders().Length > 0; }
                    catch (Exception ex)
                    {
                        Trace.WriteLine("Error checking mpg123 library.\n" + ex);
                        m_IsAvailable = false;
                    }
                }
                return m_IsAvailable.Value;
            }
        }

        /// <summary>
        /// Obtains the description for the lame encoder.
        /// </summary>
        public string Description
        {
            get
            {
                return
                    "This is the fast and Free (LGPL license) real time MPEG Audio Layer 1, 2 and 3 decoding library." + Environment.NewLine +
                    "It uses floating point or integer math, along with several special optimizations (3DNow, SSE, ARM, ...) to make it most efficient.";
            }
        }

        /// <summary>
        /// Obtains the featurelist of the mpg123 decoder.
        /// </summary>
        public string Features
        {
            get
            {
                return
                    "Very fast mpeg audio decoder." + Environment.NewLine +
                    "Really efficient with a growing number of assembler optimizations (pentium, MMX, AltiVec, ...)" + Environment.NewLine +
                    "MPEG1,2 and 2.5 layer III decoding." + Environment.NewLine +
                    "CBR (constant bitrate) and two types of variable bitrate, VBR and ABR.";
            }
        }

        /// <summary>
        /// Returns the mpeg 1,2,2.5 layer 3 mime types.
        /// </summary>
        public string[] MimeTypes
        {
            get
            {
                return new string[]
                {
                    "audio/mpeg",
                    "audio/mpeg3",
                    "audio/x-mpeg",
                    "audio/x-mpeg3",
                };
            }
        }

        /// <summary>Gets the name of the source currently beeing decoded. This is used for error messages.</summary>
        public string SourceName { get; set; }

        /// <summary>
        /// Obtains the encoder name.
        /// </summary>
        public string Name
        {
            get { return "MPG123"; }
        }

        FifoBuffer m_DecodeFifoBuffer;
        TimeSpan m_CurrentTimeStamp = TimeSpan.Zero;
        IFrameSource m_Source;

        /// <summary>Starts the decoding process.</summary>
        /// <param name="fileName">Name of the file.</param>
        public void BeginDecode(string fileName)
        {
            BeginDecode(new MP3Reader(fileName));
        }

        /// <summary>Starts the decoding process.</summary>
        /// <param name="sourceStream">The source Stream providing the encoded data.</param>
        /// <exception cref="Exception">Source  + SourceName + : Decoding already started!.</exception>
        public void BeginDecode(Stream sourceStream)
        {
            BeginDecode(new MP3Reader(sourceStream));
        }

        /// <summary>Starts the decoding process.</summary>
        /// <param name="source">The source.</param>
        /// <exception cref="InvalidOperationException">Source: Decoding already started!.</exception>
        public void BeginDecode(IFrameSource source)
        {
            if (m_Disposed)
            {
                throw new ObjectDisposedException(LogSourceName);
            }

            if (m_Initialized)
            {
                throw new InvalidOperationException(string.Format("Source {0}: Decoding already started!", SourceName));
            }

            if (SourceName != null)
            {
                SourceName = source.Name;
            }

            m_Initialized = true;
            M123.Initialize();

            m_Source = source;

            // open new decoder handle
            M123.RESULT result;
            m_DecoderHandle = M123.SafeNativeMethods.mpg123_new(null, out result);
            M123.CheckResult(result);

            // reset formats
            M123.CheckResult(M123.SafeNativeMethods.mpg123_format_none(m_DecoderHandle));

            // allow all mp3 native samplerates
            M123.ENC mode = m_UseFloatingPoint ? M123.ENC.FLOAT_32 : M123.ENC.SIGNED_16;
            foreach (int sampleRate in M123.SafeNativeMethods.mpg123_rates())
            {
                M123.CheckResult(M123.SafeNativeMethods.mpg123_format(m_DecoderHandle, new IntPtr(sampleRate), M123.CHANNELCOUNT.STEREO, mode));
            }

            // open feed
            result = M123.SafeNativeMethods.mpg123_open_feed(m_DecoderHandle);
            M123.CheckResult(result);
            m_DecodeFifoBuffer = new FifoBuffer();
        }

        /// <summary>
        /// buffers a frame into mpg123.
        /// </summary>
        void BufferFrame()
        {
            for (int i = 0; i < 1;)
            {
                AudioFrame frame = m_Source.GetNextFrame();
                if (frame == null)
                {
                    break;
                }

                Decoding?.Invoke(this, new AudioFrameEventArgs(frame));
                if (frame.IsAudio)
                {
                    m_DecodeFifoBuffer.Enqueue(frame.Data);
                    i++;
                }
            }
        }

        /// <summary>
        /// Decodes audio data.
        /// </summary>
        /// <returns>Returns a decoded IAudioData buffer or null if no more buffer available.</returns>
        public IAudioData Decode()
        {
            if (m_Disposed)
            {
                throw new ObjectDisposedException(LogSourceName);
            }

            BufferFrame();

            // end of file ? -> yes exit
            if (m_DecodeFifoBuffer.Length == 0)
            {
                return null;
            }

            FifoBuffer outBuffer = new FifoBuffer();
            bool l_Loop = true;
            while (l_Loop)
            {
                M123.RESULT result;
                result = M123.SafeNativeMethods.mpg123_decode(m_DecoderHandle, m_DecodeFifoBuffer, outBuffer, 8192);
                switch (result)
                {
                    case M123.RESULT.NEED_MORE:
                        if (outBuffer.Length > 0)
                        {
                            l_Loop = false;
                            break;
                        }
                        BufferFrame();
                        if (m_DecodeFifoBuffer.Length == 0)
                        {
                            return null;
                        }

                        break;
                    case M123.RESULT.NEW_FORMAT: UpdateFormat(); break;
                    default: M123.CheckResult(result); throw new InvalidOperationException();
                }
            }
            if (outBuffer.Length > 0)
            {
                AudioData resultData = new AudioData(m_CurrentConfig.SamplingRate, m_CurrentConfig.Format, m_CurrentConfig.ChannelSetup, m_CurrentTimeStamp, 0, -1, outBuffer.ToArray());
                m_CurrentTimeStamp += resultData.Duration;
                return resultData;
            }
            return null;
        }

        /// <summary>Closes the underlying stream and calls Dispose.</summary>
        public void Close()
        {
            if (m_Disposed)
            {
                throw new ObjectDisposedException(LogSourceName);
            }

            if (m_Initialized)
            {
                M123.Deinitialize();
                m_Initialized = false;
            }
            if (m_Source != null)
            {
                m_Source.Close();
                m_Source = null;
                m_DecodeFifoBuffer = null;
            }
        }
        #endregion

        #region IDisposable Member

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            if (m_Disposed)
            {
                return;
            }

            Close();
            m_Disposed = true;
            ReleaseHandle();
            GC.SuppressFinalize(this);
        }

        #endregion
    }
}
