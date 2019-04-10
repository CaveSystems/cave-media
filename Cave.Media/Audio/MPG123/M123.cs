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
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Runtime.InteropServices;
using System.Security;
using System.Threading;
using Cave.IO;

namespace Cave.Media.Audio.MPG123
{
    /// <summary>
    /// Provides direct access to the mpg123 functions.
    /// </summary>
    public sealed class M123
    {
        /// <summary>The native library name (windows mpg123.dll, linux libmpg123.so, macos libmpg123.dylib.</summary>
        const string NATIVE_LIBRARY = "mpg123";

        static int initializationCounter;

        /// <summary>Gets the name of the log source.</summary>
        /// <value>The name of the log source.</value>
        public string LogSourceName { get { return "MPG123"; } }

        #region enums

        /// <summary>
        /// An enum over all sample types possibly known to mpg123. The values are designed as bit flags to allow bitmasking for encoding families.<br />
        /// Note that (your build of) libmpg123 does not necessarily support all these. Usually, you can expect the 8bit encodings and signed 16 bit.
        /// Also 32bit float will be usual beginning with mpg123-1.7.0 . What you should bear in mind is that (SSE, etc) optimized routines
        /// may be absent for some formats. We do have SSE for 16, 32 bit and float, though. 24 bit integer is done via postprocessing of 32 bit output --
        /// just cutting the last byte, no rounding, even. If you want better, do it yourself.<br />
        /// All formats are in native byte order. If you need different endinaness, you can simply postprocess the output buffers
        /// (libmpg123 wouldn't do anything else). mpg123_encsize() can be helpful there. <br />
        /// </summary>
        public enum ENC
        {
            /// <summary>
            /// No or unknown encoding
            /// </summary>
            NONE = 0,

            /// <summary>
            /// Some 8 bit integer encoding.
            /// </summary>
            DEFAULT_8 = 0x00f,

            /// <summary>
            /// Some 16 bit integer encoding.
            /// </summary>
            DEFAULT_16 = 0x040,

            /// <summary>
            /// Some 24 bit integer encoding.
            /// </summary>
            DEFAULT_24 = 0x4000,

            /// <summary>
            /// Some 32 bit integer encoding.
            /// </summary>
            DEFAULT_32 = 0x100,

            /// <summary>
            /// Some signed integer encoding.
            /// </summary>
            SIGNED = 0x080,

            /// <summary>
            /// Some float encoding.
            /// </summary>
            FLOAT = 0xe00,

            /// <summary>
            /// signed 16 bit
            /// </summary>
            SIGNED_16 = DEFAULT_16 | SIGNED | 0x10,

            /// <summary>
            /// unsigned 16 bit
            /// </summary>
            UNSIGNED_16 = DEFAULT_16 | 0x20,

            /// <summary>
            /// unsigned 8 bit
            /// </summary>
            UNSIGNED_8 = 0x01,

            /// <summary>
            /// signed 8 bit
            /// </summary>
            SIGNED_8 = SIGNED | 0x02,

            /// <summary>
            /// ulaw 8 bit
            /// </summary>
            ULAW_8 = 0x04,

            /// <summary>
            /// alaw 8 bit
            /// </summary>
            ALAW_8 = 0x08,

            /// <summary>
            /// signed 32 bit
            /// </summary>
            SIGNED_32 = DEFAULT_32 | SIGNED | 0x1000,

            /// <summary>
            /// unsigned 32 bit
            /// </summary>
            UNSIGNED_32 = DEFAULT_32 | 0x2000,

            /// <summary>
            /// signed 24 bit
            /// </summary>
            SIGNED_24 = DEFAULT_24 | SIGNED | 0x1000,

            /// <summary>
            /// unsigned 24 bit
            /// </summary>
            UNSIGNED_24 = DEFAULT_24 | 0x2000,

            /// <summary>
            /// 32bit float
            /// </summary>
            FLOAT_32 = 0x200,

            /// <summary>
            /// 64bit float
            /// </summary>
            FLOAT_64 = 0x400,

            /// <summary>
            /// Any encoding on the list.
            /// </summary>
            ANY =
                SIGNED_16 | UNSIGNED_16 | UNSIGNED_8 | SIGNED_8 | ULAW_8 | ALAW_8 |
                SIGNED_32 | UNSIGNED_32 | SIGNED_24 | UNSIGNED_24 | FLOAT_32 | FLOAT_64,
        }

        /// <summary>
        /// Feature set available for query with mpg123_feature.
        /// </summary>
        public enum FEATURE
        {
            /// <summary>
            /// mpg123 expects path names to be given in UTF-8 encoding instead of plain native.
            /// </summary>
            ABI_UTF8OPEN = 0,

            /// <summary>
            /// 8bit output
            /// </summary>
            OUTPUT_8BIT,

            /// <summary>
            /// 16bit output
            /// </summary>
            OUTPUT_16BIT,

            /// <summary>
            /// 32bit output
            /// </summary>
            OUTPUT_32BIT,

            /// <summary>
            /// support for building a frame index for accurate seeking
            /// </summary>
            INDEX,

            /// <summary>
            /// id3v2 parsing
            /// </summary>
            PARSE_ID3V2,

            /// <summary>
            /// mpeg layer-1 decoder enabled
            /// </summary>
            DECODE_LAYER1,

            /// <summary>
            /// mpeg layer-2 decoder enabled
            /// </summary>
            DECODE_LAYER2,

            /// <summary>
            /// mpeg layer-3 decoder enabled
            /// </summary>
            DECODE_LAYER3,

            /// <summary>
            /// accurate decoder rounding
            /// </summary>
            DECODE_ACCURATE,

            /// <summary>
            /// downsample (sample omit)
            /// </summary>
            DECODE_DOWNSAMPLE,

            /// <summary>
            /// flexible rate decoding
            /// </summary>
            DECODE_NTOM,

            /// <summary>
            /// ICY support
            /// </summary>
            PARSE_ICY,

            /// <summary>
            /// Reader with timeout (network).
            /// </summary>
            TIMEOUT_READ,
        }

        /// <summary>
        /// Channel count flags: They can be combined into one number (3) to indicate mono and stereo...
        /// </summary>
        [Flags]
        public enum CHANNELCOUNT
        {
            /// <summary>
            /// No channels or error
            /// </summary>
            NONE = 0,

            /// <summary>
            /// Mono
            /// </summary>
            MONO = 1,

            /// <summary>
            /// Stereo
            /// </summary>
            STEREO = 2,

            /// <summary>
            /// Mono | Stereo
            /// </summary>
            BOTH = MONO | STEREO,
        }

        /// <summary>
        /// Flag bits for MPG123_FLAGS, use the usual binary or to combine.
        /// </summary>
        [Flags]
        public enum FLAGS : int
        {
            /// <summary>
            /// Force some mono mode: This is a test bitmask for seeing if any mono forcing is active.
            /// </summary>
            FORCE_MONO = 0x7,

            /// <summary>
            /// Force playback of left channel only.
            /// </summary>
            MONO_LEFT = 0x1,

            /// <summary>
            /// Force playback of right channel only.
            /// </summary>
            MONO_RIGHT = 0x2,

            /// <summary>
            /// Force playback of mixed mono.
            /// </summary>
            MONO_MIX = 0x4,

            /// <summary>
            /// Force stereo output.
            /// </summary>
            FORCE_STEREO = 0x8,

            /// <summary>
            /// Force 8bit formats.
            /// </summary>
            FORCE_8BIT = 0x10,

            /// <summary>
            /// Suppress any printouts (overrules verbose).
            /// </summary>
            QUIET = 0x20,

            /// <summary>
            /// Enable gapless decoding (default on if libmpg123 has support).
            /// </summary>
            GAPLESS = 0x40,

            /// <summary>
            /// Disable resync stream after error.
            /// </summary>
            NO_RESYNC = 0x80,

            /// <summary>
            /// Enable small buffer on non-seekable streams to allow some peek-ahead (for better MPEG sync).
            /// </summary>
            SEEKBUFFER = 0x100,

            /// <summary>
            /// Enable fuzzy seeks (guessing byte offsets or using approximate seek points from Xing TOC)
            /// </summary>
            FUZZY = 0x200,

            /// <summary>
            /// Force floating point output (32 or 64 bits depends on mpg123 internal precision).
            /// </summary>
            FORCE_FLOAT = 0x400,

            /// <summary>
            /// Do not translate ID3 text data to UTF-8. ID3 strings will contain the raw text data, with the first byte containing the ID3 encoding code.
            /// </summary>
            PLAIN_ID3TEXT = 0x800,

            /// <summary>
            /// Ignore any stream length information contained in the stream, which can be contained in a 'TLEN' frame of an ID3v2 tag or a Xing tag
            /// </summary>
            IGNORE_STREAMLENGTH = 0x1000,

            /// <summary>
            /// Do not parse ID3v2 tags, just skip them.
            /// </summary>
            SKIid3V2 = 0x2000,

            /// <summary>
            /// Do not parse the LAME/Xing info frame, treat it as normal MPEG data.
            /// </summary>
            IGNORE_INFOFRAME = 0x4000,

            /// <summary>
            /// Allow automatic internal resampling of any kind (default on if supported). Especially when going lowlevel with replacing output buffer, you might want to unset this flag. Setting MPG123_DOWNSAMPLE or MPG123_FORCE_RATE will override this.
            /// </summary>
            AUTO_RESAMPLE = 0x8000,
        }

        /// <summary>
        /// Enumeration of the parameters types that it is possible to set/get.
        /// </summary>
        public enum PARAMS
        {
            /// <summary>
            /// set verbosity value for enabling messages to stderr, &gt;= 0 makes sense (integer)
            /// </summary>
            VERBOSE = 0,

            /// <summary>
            /// set all flags, p.ex val = GAPLESS | MONO_MIX (integer)
            /// </summary>
            FLAGS,

            /// <summary>
            /// add some flags (integer)
            /// </summary>
            ADD_FLAGS,

            /// <summary>
            /// when value &gt; 0, force output rate to that value (integer)
            /// </summary>
            FORCE_RATE,

            /// <summary>
            /// 0=native rate, 1=half rate, 2=quarter rate (integer)
            /// </summary>
            DOWN_SAMPLE,

            /// <summary>
            /// one of the RVA choices above (integer)
            /// </summary>
            RVA,

            /// <summary>
            /// play a frame N times (integer)
            /// </summary>
            DOWNSPEED,

            /// <summary>
            /// play every Nth frame (integer)
            /// </summary>
            UPSPEED,

            /// <summary>
            /// start with this frame (skip frames before that, integer)
            /// </summary>
            START_FRAME,

            /// <summary>
            /// decode only this number of frames (integer)
            /// </summary>
            DECODE_FRAMES,

            /// <summary>
            /// stream contains ICY metadata with this interval (integer)
            /// </summary>
            ICY_INTERVAL,

            /// <summary>
            /// the scale for output samples (amplitude - integer or float according to mpg123 output format, normally integer)
            /// </summary>
            OUTSCALE,

            /// <summary>
            /// timeout for reading from a stream (not supported on win32, integer)
            /// </summary>
            TIMEOUT,

            /// <summary>
            /// remove some flags (inverse of MPG123_ADD_FLAGS, integer)
            /// </summary>
            REMOVE_FLAGS,

            /// <summary>
            /// Try resync on frame parsing for that many bytes or until end of stream (&lt;0 ... integer). This can enlarge the limit for skipping junk on beginning, too (but not reduce it).
            /// </summary>
            RESYNC_LIMIT,

            /// <summary>
            /// Set the frame index size (if supported). Values &lt;0 mean that the index is allowed to grow dynamically in these steps (in positive direction, of course) -- Use this when you really want a full index with every individual frame.
            /// </summary>
            INDEX_SIZE,

            /// <summary>
            /// Decode/ignore that many frames in advance for layer 3. This is needed to fill bit reservoir after seeking, for example (but also at least one frame in advance is needed to have all "normal" data for layer 3). Give a positive integer value, please.
            /// </summary>
            PREFRAMES,

            /// <summary>
            /// For feeder mode, keep that many buffers in a pool to avoid frequent malloc/free. The pool is allocated on mpg123_open_feed(). If you change this parameter afterwards, you can trigger growth and shrinkage during decoding. The default value could change any time. If you care about this, then set it. (integer)
            /// </summary>
            FEEDPOOL,

            /// <summary>
            /// Minimal size of one internal feeder buffer, again, the default value is subject to change. (integer)
            /// </summary>
            FEEDBUFFER,
        }

        /// <summary>
        /// Enumeration of the message and error codes and returned by libmpg123 functions.
        /// </summary>
        public enum RESULT : int
        {
            /// <summary>
            /// Message: Track ended. Stop decoding.
            /// </summary>
            DONE = -12,

            /// <summary>
            /// Message: Output format will be different on next call. Note that some libmpg123 versions between 1.4.3 and 1.8.0 insist on you calling mpg123_getformat() after getting this message code. Newer verisons behave like advertised: You have the chance to call mpg123_getformat(), but you can also just continue decoding and get your data.
            /// </summary>
            NEW_FORMAT = -11,

            /// <summary>
            /// Message: For feed reader: "Feed me more!" (call mpg123_feed() or mpg123_decode() with some new input data).
            /// </summary>
            NEED_MORE = -10,

            /// <summary>
            /// Generic Error
            /// </summary>
            ERR = -1,

            /// <summary>
            /// Success
            /// </summary>
            OK = 0,

            /// <summary>
            /// Unable to set up output format!
            /// </summary>
            BAD_OUTFORMAT,

            /// <summary>
            /// Invalid channel number specified.
            /// </summary>
            BAD_CHANNEL,

            /// <summary>
            /// Invalid sample rate specified.
            /// </summary>
            BAD_RATE,

            /// <summary>
            /// Unable to allocate memory for 16 to 8 converter table!
            /// </summary>
            ERR_16TO8TABLE,

            /// <summary>
            /// Bad parameter id!
            /// </summary>
            BAD_PARAM,

            /// <summary>
            /// Bad buffer given -- invalid pointer or too small size.
            /// </summary>
            BAD_BUFFER,

            /// <summary>
            /// Out of memory -- some malloc() failed.
            /// </summary>
            OUT_OF_MEM,

            /// <summary>
            /// You didn't initialize the library!
            /// </summary>
            NOT_INITIALIZED,

            /// <summary>
            /// Invalid decoder choice.
            /// </summary>
            BAD_DECODER,

            /// <summary>
            /// Invalid mpg123 handle.
            /// </summary>
            BAD_HANDLE,

            /// <summary>
            /// Unable to initialize frame buffers (out of memory?).
            /// </summary>
            NO_BUFFERS,

            /// <summary>
            /// Invalid RVA mode.
            /// </summary>
            BAD_RVA,

            /// <summary>
            /// This build doesn't support gapless decoding.
            /// </summary>
            NO_GAPLESS,

            /// <summary>
            /// Not enough buffer space.
            /// </summary>
            NO_SPACE,

            /// <summary>
            /// Incompatible numeric data types.
            /// </summary>
            BAD_TYPES,

            /// <summary>
            /// Bad equalizer band.
            /// </summary>
            BAD_BAND,

            /// <summary>
            /// Null pointer given where valid storage address needed.
            /// </summary>
            ERR_NULL,

            /// <summary>
            /// Error reading the stream.
            /// </summary>
            ERR_READER,

            /// <summary>
            /// Cannot seek from end (end is not known).
            /// </summary>
            NO_SEEK_FROM_END,

            /// <summary>
            /// Invalid 'whence' for seek function.
            /// </summary>
            BAD_WHENCE,

            /// <summary>
            /// Build does not support stream timeouts.
            /// </summary>
            NO_TIMEOUT,

            /// <summary>
            /// File access error.
            /// </summary>
            BAD_FILE,

            /// <summary>
            /// Seek not supported by stream.
            /// </summary>
            NO_SEEK,

            /// <summary>
            /// No stream opened.
            /// </summary>
            NO_READER,

            /// <summary>
            /// Bad parameter handle.
            /// </summary>
            BAD_PARS,

            /// <summary>
            /// Bad parameters to mpg123_index() and mpg123_set_index()
            /// </summary>
            BAD_INDEX_PAR,

            /// <summary>
            /// Lost track in bytestream and did not try to resync.
            /// </summary>
            OUT_OF_SYNC,

            /// <summary>
            /// Resync failed to find valid MPEG data.
            /// </summary>
            RESYNC_FAIL,

            /// <summary>
            /// No 8bit encoding possible.
            /// </summary>
            NO_8BIT,

            /// <summary>
            /// Stack aligmnent error
            /// </summary>
            BAD_ALIGN,

            /// <summary>
            /// NULL input buffer with non-zero size...
            /// </summary>
            NULbUFFER,

            /// <summary>
            /// Relative seek not possible (screwed up file offset)
            /// </summary>
            NO_RELSEEK,

            /// <summary>
            /// You gave a null pointer somewhere where you shouldn't have.
            /// </summary>
            NULL_POINTER,

            /// <summary>
            /// Bad key value given.
            /// </summary>
            BAD_KEY,

            /// <summary>
            /// No frame index in this build.
            /// </summary>
            NO_INDEX,

            /// <summary>
            /// Something with frame index went wrong.
            /// </summary>
            INDEX_FAIL,

            /// <summary>
            /// Something prevents a proper decoder setup
            /// </summary>
            BAD_DECODER_SETUP,

            /// <summary>
            /// This feature has not been built into libmpg123.
            /// </summary>
            MISSING_FEATURE,

            /// <summary>
            /// A bad value has been given, somewhere.
            /// </summary>
            BAD_VALUE,

            /// <summary>
            /// Low-level seek failed.
            /// </summary>
            LSEEK_FAILED,

            /// <summary>
            /// Custom I/O not prepared.
            /// </summary>
            BAD_CUSTOM_IO,

            /// <summary>
            /// Offset value overflow during translation of large file API calls -- your client program cannot handle that large file.
            /// </summary>
            LFS_OVERFLOW,

            /// <summary>
            /// Some integer overflow.
            /// </summary>
            INT_OVERFLOW,
        }

        /// <summary>
        /// RVA choices.
        /// </summary>
        public enum RVA
        {
            /// <summary>
            /// RVA disabled (default).
            /// </summary>
            OFF = 0,

            /// <summary>
            /// Use mix/track/radio gain.
            /// </summary>
            MIX = 1,

            /// <summary>
            /// Use album/audiophile gain
            /// </summary>
            ALBUM = 2,
        }
        #endregion

        #region classes

        /// <summary>
        /// mpg123 advanced parameter API:
        /// <para>
        /// Direct access to a parameter set without full handle around it. Possible uses:<br />
        /// * Influence behaviour of library _during_ initialization of handle (MPG123_VERBOSE).<br />
        /// * Use one set of parameters for multiple handles.
        /// </para>
        /// </summary>
        public sealed class M123_PARS
        {
            /// <summary>Gets the name of the log source.</summary>
            /// <value>The name of the log source.</value>
            public string LogSourceName { get { return "MPG123_PARS"; } }

            /// <summary>
            /// Obtains the handle.
            /// </summary>
            public IntPtr Handle { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="M123_PARS"/> class.
            /// </summary>
            public M123_PARS()
            {
                Handle = IntPtr.Zero;
            }

            /// <summary>
            /// Finalizes an instance of the <see cref="M123_PARS"/> class.
            /// </summary>
            ~M123_PARS()
            {
                if (Handle != IntPtr.Zero)
                {
                    Trace.WriteLine(string.Format("MPG123_PARS struct was not disposed via mpg123_delete_pars()!"));
                }
            }

            /// <summary>
            /// Creates new MPG123_PARS with the specified handle.
            /// </summary>
            /// <param name="handle"></param>
            public M123_PARS(IntPtr handle)
            {
                this.Handle = handle;
            }

            /// <summary>
            /// Obtains whether the handle is valid or not.
            /// </summary>
            public bool Valid
            {
                get
                {
                    return Handle != IntPtr.Zero;
                }
            }

            #region IDisposable Member

            internal void Disposed()
            {
                Handle = IntPtr.Zero;
            }

            #endregion
        }
        #endregion

        /// <summary>
        /// Obtains the MPG123_ENC value for the specified IAudioConfiguration.
        /// </summary>
        /// <param name="config">The audio configuration.</param>
        /// <returns></returns>
        /// <exception cref="NotSupportedException"></exception>
        public static ENC GetEncoding(IAudioConfiguration config)
        {
            switch (config.Format)
            {
                case AudioSampleFormat.Float: return ENC.FLOAT_32;
                case AudioSampleFormat.Int8: return ENC.SIGNED_8;
                case AudioSampleFormat.Int16: return ENC.SIGNED_16;
                case AudioSampleFormat.Int24: return ENC.SIGNED_24;
                case AudioSampleFormat.Int32: return ENC.SIGNED_32;
                default: throw new NotSupportedException(string.Format("SampleFormat {0} is not supported!", config.Format));
            }
        }

        /// <summary>
        /// Obtains the MPG123_CHANNELCOUNT value for the specified IAudioConfiguration.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static CHANNELCOUNT GetChannelConfig(IAudioConfiguration config)
        {
            switch (config.ChannelSetup)
            {
                case AudioChannelSetup.Mono: return CHANNELCOUNT.MONO;
                case AudioChannelSetup.Stereo: return CHANNELCOUNT.STEREO;
                default: throw new NotSupportedException(string.Format("Audio channel config {0} is not supported!", config.ChannelSetup));
            }
        }

        /// <summary>Checks the result of a mpg 123 operation and throws an exception on error.</summary>
        /// <param name="result">The result.</param>
        /// <exception cref="Exception"></exception>
        public static void CheckResult(RESULT result)
        {
            if (result != RESULT.OK)
            {
                throw new Exception(string.Format("MPG123 Error: {0}", SafeNativeMethods.mpg123_plain_strerror(result)));
            }
        }

        /// <summary>
        /// Initialized the mpg123 library (you have to call Deinitialize for each call to this function).
        /// </summary>
        public static void Initialize()
        {
            if (Interlocked.Increment(ref initializationCounter) > 1)
            {
                return;
            }

            var errors = new List<Exception>();

            // Initialize
            try
            {
                CheckResult(SafeNativeMethods.mpg123_init());
                return;
            }
            catch (Exception ex)
            {
                Interlocked.Decrement(ref initializationCounter);
                Trace.WriteLine("Library initialization exception.\n" + ex);
                throw;
            }
        }

        /// <summary>Deinitializes mpg123 library if the InitializeCount reaches 0.</summary>
        /// <exception cref="InvalidOperationException"></exception>
        public static void Deinitialize()
        {
            if (Interlocked.Decrement(ref initializationCounter) <= 0)
            {
                SafeNativeMethods.mpg123_exit();
            }
        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            static string[] m_DecodeStringArray(IntPtr pointer)
            {
                var result = new List<string>();
                int index = 0;
                IntPtr item = Marshal.ReadIntPtr(pointer, index);
                index += IntPtr.Size;
                while (item != IntPtr.Zero)
                {
                    string str = Marshal.PtrToStringAnsi(item);
                    if (!string.IsNullOrEmpty(str))
                    {
                        result.Add(str);
                    }

                    item = Marshal.ReadIntPtr(pointer, index);
                    index += IntPtr.Size;
                }
                return result.ToArray();
            }

            static void m_DecodeIntArray(IntPtr pointer, int[] array)
            {
                int count = 0;
                int index = 0;
                if (IntPtr.Size == 4 || Platform.IsMicrosoft)
                {
                    while (count < array.Length)
                    {
                        int value = Marshal.ReadInt32(pointer, index);
                        index += 4;
                        array[count++] = value;
                    }
                }
                else if (IntPtr.Size == 8)
                {
                    while (count < array.Length)
                    {
                        int value = (int)Marshal.ReadInt64(pointer, index);
                        index += 8;
                        array[count++] = value;
                    }
                }
                else
                {
                    throw new NotImplementedException();
                }
            }

            #region mpg123 library and handle setup

            /// <summary>
            /// Function to initialise the mpg123 library. This function is not thread-safe. Call it exactly once per process, before any other (possibly threaded) work with the library.
            /// </summary>
            /// <returns>MPG123_RESULT.OK if successful, otherwise an error number. </returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_init();

            /// <summary>
            /// Function to close down the mpg123 library. This function is not thread-safe. Call it exactly once per process.
            /// </summary>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern void mpg123_exit();

            /// <summary>
            /// Create a handle with optional choice of decoder (named by a string, see mpg123_decoders() or mpg123_supported_decoders()). and optional retrieval of an error code to feed to mpg123_plain_strerror(). Optional means: Any of or both the parameters may be NULL.
            /// </summary>
            /// <param name="decoderName">Decoder name (optional).</param>
            /// <param name="error">Result.</param>
            /// <returns>Returns a non-NULL pointer when successful. </returns>
            [DllImport(NATIVE_LIBRARY, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl)]
            public static extern IntPtr mpg123_new([MarshalAs(UnmanagedType.LPStr)]string decoderName, out RESULT error);

            /// <summary>
            /// Delete handle, mh is either a valid mpg123 handle or NULL.
            /// </summary>
            /// <param name="handle"></param>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern void mpg123_delete(IntPtr handle);

            /// <summary>
            /// Set a specific parameter, for a specific mpg123_handle, using a parameter type key chosen from the MPG123_PARAMS enumeration, to the specified value.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <param name="floatValue"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_param(IntPtr handle, PARAMS type, IntPtr value, double floatValue);

            /// <summary>
            /// Get a specific parameter, for a specific mpg123_handle. See the MPG123_PARAMS enumeration for a list of available parameters.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <param name="floatValue"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_getparam(IntPtr handle, PARAMS type, out IntPtr value, ref double floatValue);

            /// <summary>
            /// Query libmpg123 feature, 1 for success, 0 for unimplemented functions.
            /// </summary>
            /// <param name="key"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern bool mpg123_feature(FEATURE key);

            #endregion

            #region mpg123 error handling

            /// <summary>
            /// Return a string describing that error errcode means.
            /// </summary>
            /// <param name="errCode"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern string mpg123_plain_strerror(RESULT errCode);

            /// <summary>
            /// Give string describing what error has occured in the context of the handle. When a function operating on an mpg123 handle returns MPG123_ERR, you should check for the actual reason via char *errmsg = mpg123_strerror(mh) This function will catch mh == NULL and return the message for MPG123_BAD_HANDLE.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern string mpg123_strerror(IntPtr handle);

            /// <summary>
            /// Return the plain errcode intead of a string.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_errcode(IntPtr handle);

            #endregion

            #region mpg123 decoder selection

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_decoders", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr m_mpg123_decoders();

            /// <summary>
            /// Returns an array of generally available decoder names (plain 8bit ASCII).
            /// </summary>
            /// <returns>Returns a list of available decoders.</returns>
            public static string[] mpg123_decoders()
            {
                return m_DecodeStringArray(m_mpg123_decoders());
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_supported_decoders", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr m_mpg123_supported_decoders();

            /// <summary>
            /// Return a NULL-terminated array of the decoders supported by the CPU (plain 8bit ASCII).
            /// </summary>
            /// <returns>Returns a list of supported decoders.</returns>
            public static string[] mpg123_supported_decoders()
            {
                return m_DecodeStringArray(m_mpg123_supported_decoders());
            }

            /// <summary>
            /// Set the chosen decoder to 'decoder_name'.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="decoderName"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_decoder(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string decoderName);

            /// <summary>
            /// Get the currently active decoder engine name. The active decoder engine can vary depening on output constraints, mostly non-resampling, integer output is accelerated via 3DNow &amp; Co. but for other modes a fallback engine kicks in. Note that this can return a decoder that is ony active in the hidden and not available as decoder choice from the outside.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns>The decoder name or NULL on error. </returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern string mpg123_current_decoder(IntPtr handle);

            #endregion

            #region mpg123 output audio format

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_rates", CallingConvention = CallingConvention.Cdecl)]
            static extern void m_mpg123_rates(out IntPtr list, out UIntPtr count);

            /// <summary>
            /// An array of supported standard sample rates These are possible native sample rates of MPEG audio files. You can still force mpg123 to resample to a different one, but by default you will only get audio in one of these samplings.
            /// </summary>
            /// <returns></returns>
            public static int[] mpg123_rates()
            {
                IntPtr list;
                UIntPtr count;
                m_mpg123_rates(out list, out count);
                int[] result = new int[count.ToUInt32()];
                m_DecodeIntArray(list, result);
                return result;
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_encodings", CallingConvention = CallingConvention.Cdecl)]
            static extern void m_mpg123_encodings(out IntPtr list, out UIntPtr count);

            /// <summary>
            /// An array of supported audio encodings. An audio encoding is one of the fully qualified members of mpg123_enc_enum (MPG123_ENC_SIGNED_16, not MPG123_SIGNED).
            /// </summary>
            /// <returns>Returns a list of all available encodings.</returns>
            public static ENC[] mpg123_encodings()
            {
                IntPtr list;
                UIntPtr count;
                m_mpg123_encodings(out list, out count);
                int[] array = new int[count.ToUInt32()];
                m_DecodeIntArray(list, array);
                ENC[] result = new ENC[array.Length];
                for (int i = 0; i < array.Length; i++)
                {
                    result[i] = (ENC)array[i];
                }
                return result;
            }

            /// <summary>
            /// Return the size (in bytes) of one mono sample of the named encoding.
            /// </summary>
            /// <param name="encoding">The encoding value to analyze. </param>
            /// <returns>Returns positive size of encoding in bytes, 0 on invalid encoding. </returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern int mpg123_encsize(ENC encoding);

            /// <summary>
            /// Configure a mpg123 handle to accept no output format at all, use before specifying supported formats with mpg123_format.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_format_none(IntPtr handle);

            /// <summary>
            /// Configure mpg123 handle to accept all formats (also any custom rate you may set) -- this is default.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_format_all(IntPtr handle);

            /// <summary>
            /// Set the audio format support of a mpg123_handle in detail:.
            /// </summary>
            /// <param name="handle">Audio decoder handle. </param>
            /// <param name="sampleRate">The sample rate value (in Hertz). </param>
            /// <param name="channels">A combination of MPG123_STEREO and MPG123_MONO. </param>
            /// <param name="encodings">A combination of accepted encodings for rate and channels, p.ex MPG123_ENC_SIGNED16 | MPG123_ENC_ULAW_8 (or 0 for no support). Please note that some encodings may not be supported in the library build and thus will be ignored here. </param>
            /// <returns>MPG123_RESULT.OK on success, MPG123_RESULT.ERR if there was an error. </returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_format(IntPtr handle, IntPtr sampleRate, CHANNELCOUNT channels, ENC encodings);

            /// <summary>
            /// Check to see if a specific format at a specific rate is supported by mpg123_handle.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="sampleRate"></param>
            /// <param name="encoding"></param>
            /// <returns>MPG123_CHANNELCOUNT.NONE for no support (that includes invalid parameters), MPG123_CHANNELCOUNT.STEREO, MPG123_CHANNELCOUNT.MONO or MPG123_CHANNELCOUNT.BOTH. </returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern CHANNELCOUNT mpg123_format_support(IntPtr handle, long sampleRate, ENC encoding);

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_getformat", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_getformat(IntPtr handle, out IntPtr sampleRate, out IntPtr channels, out IntPtr encoding);

            /// <summary>
            /// Get the current output format.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            public static IAudioConfiguration mpg123_getformat(IntPtr handle)
            {
                IntPtr sampleRate;
                IntPtr channelConfig;
                IntPtr encoding;

                RESULT result = m_mpg123_getformat(handle, out sampleRate, out channelConfig, out encoding);
                if (result != RESULT.OK)
                {
                    throw new Exception(mpg123_plain_strerror(result));
                }

                AudioSampleFormat format;
                switch ((ENC)encoding.ToInt32())
                {
                    case ENC.SIGNED_8: format = AudioSampleFormat.Int8; break;
                    case ENC.SIGNED_16: format = AudioSampleFormat.Int16; break;
                    case ENC.SIGNED_24: format = AudioSampleFormat.Int24; break;
                    case ENC.SIGNED_32: format = AudioSampleFormat.Int32; break;
                    case ENC.FLOAT_32: format = AudioSampleFormat.Float; break;
                    case ENC.FLOAT_64: format = AudioSampleFormat.Double; break;
                    default: format = AudioSampleFormat.Unknown; break;
                }

                AudioChannelSetup channel;
                switch ((CHANNELCOUNT)channelConfig.ToInt32())
                {
                    case CHANNELCOUNT.MONO: channel = AudioChannelSetup.Mono; break;
                    case CHANNELCOUNT.STEREO: channel = AudioChannelSetup.Stereo; break;
                    default: channel = AudioChannelSetup.None; break;
                }
                return new AudioConfiguration(sampleRate.ToInt32(), format, channel);
            }

            #endregion

            #region mpg123 file input and decoding

            /// <summary>
            /// Open and prepare to decode the specified file by filesystem path.
            /// This does not open HTTP urls; libmpg123 contains no networking code.
            /// If you want to decode internet streams, use mpg123_open_fd() or mpg123_open_feed().
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="path"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_open(IntPtr handle, [MarshalAs(UnmanagedType.LPStr)]string path);

            /// <summary>
            /// Use an already opened file descriptor as the bitstream input mpg123_close() will _not_ close the file descriptor.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="fileDescriptor"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_open_fd(IntPtr handle, IntPtr fileDescriptor);

            /// <summary>
            /// Use an opaque handle as bitstream input. This works only with the replaced I/O from mpg123_replace_reader_handle()!
            /// mpg123_close() will call the cleanup callback for your handle (if you gave one).
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="iohandle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_open_handle(IntPtr handle, IntPtr iohandle);

            /// <summary>
            /// Open a new bitstream and prepare for direct feeding.
            /// This works together with mpg123_decode();
            /// you are responsible for reading and feeding the input bitstream.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_open_feed(IntPtr handle);

            /// <summary>
            /// Closes the source, if libmpg123 opened it.
            /// </summary>
            /// <param name="handle"></param>
            /// <returns></returns>
            [DllImport(NATIVE_LIBRARY, CallingConvention = CallingConvention.Cdecl)]
            public static extern RESULT mpg123_close(IntPtr handle);

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_read")]
            static extern RESULT m_mpg123_read(IntPtr handle, IntPtr outMemory, IntPtr outMemSize, out IntPtr bytesDone);

            /// <summary>
            /// Read from stream and decode up to BufferSize bytes.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="outBuffer"></param>
            /// <param name="bufferSize"></param>
            /// <returns></returns>
            public static RESULT mpg123_read(IntPtr handle, FifoBuffer outBuffer, int bufferSize)
            {
                IntPtr memory = Marshal.AllocHGlobal(bufferSize);
                IntPtr bytesDone;
                RESULT result = m_mpg123_read(handle, memory, new IntPtr(bufferSize), out bytesDone);
                outBuffer.Enqueue(memory, bytesDone.ToInt32());
                return result;
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_feed", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_feed(IntPtr handle, ref byte[] buffer, IntPtr bufferSize);

            /// <summary>
            /// Feed data for a stream that has been opened with mpg123_open_feed().
            /// It's give and take: You provide the bytestream, mpg123 gives you the decoded samples.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="inBuffer"></param>
            /// <returns></returns>
            public static RESULT mpg123_feed(IntPtr handle, byte[] inBuffer)
            {
                return m_mpg123_feed(handle, ref inBuffer, new IntPtr(inBuffer.Length));
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_decode", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_decode(IntPtr handle, IntPtr inMemory, IntPtr inMemSize, IntPtr outMemory, IntPtr outMemSize, out IntPtr bytesDone);

            /// <summary>
            /// Decode MPEG Audio from inmemory to outmemory. This is very close to a drop-in replacement for old mpglib.
            /// When you give zero-sized output buffer the input will be parsed until decoded data is available.
            /// This enables you to get MPG123_NEW_FORMAT (and query it) without taking decoded data.
            /// Think of this function being the union of mpg123_read() and mpg123_feed() (which it actually is, sort of;-).
            /// You can actually always decide if you want those specialized functions in separate steps or one call this one here.
            /// </summary>
            /// <param name="handle"></param>
            /// <param name="input">Input buffer (buffer where decoded data will be stored).</param>
            /// <param name="output">Output buffer (buffer to be decoded).</param>
            /// <param name="bufferSize">The output buffer size.</param>
            /// <returns></returns>
            public static RESULT mpg123_decode(IntPtr handle, FifoBuffer input, FifoBuffer output, int bufferSize)
            {
                // get input buffer
                int inSize = input.Length;
                IntPtr inMemory = Marshal.AllocHGlobal(inSize);
                input.Dequeue(inSize, inMemory);

                // prepare output buffer
                IntPtr outMemory = Marshal.AllocHGlobal(bufferSize);
                IntPtr done;

                // decode
                RESULT result = m_mpg123_decode(handle, inMemory, new IntPtr(inSize), outMemory, new IntPtr(bufferSize), out done);
                if (result == RESULT.ERR)
                {
                    throw new Exception(mpg123_strerror(handle));
                }
                while (result == RESULT.OK)
                {
                    int doneCount = done.ToInt32();
                    if (doneCount > 0)
                    {
                        output.Enqueue(outMemory, doneCount);
                    }
                    result = m_mpg123_decode(handle, inMemory, IntPtr.Zero, outMemory, new IntPtr(bufferSize), out done);
                }
                {
                    int doneCount = done.ToInt32();
                    if (doneCount > 0)
                    {
                        output.Enqueue(outMemory, doneCount);
                    }
                }

                // free buffers
                Marshal.FreeHGlobal(outMemory);
                Marshal.FreeHGlobal(inMemory);
                return result;
            }

            #endregion

            #region mpg123 status and information

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_safe_buffer", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr m_mpg123_safe_buffer();

            /// <summary>
            /// Get the safe output buffer size for all cases (when you want to replace the internal buffer).
            /// </summary>
            /// <returns></returns>
            public static int mpg123_safe_buffer()
            {
                return m_mpg123_safe_buffer().ToInt32();
            }

            #endregion

            #region mpg123 advanced parameter API

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_parnew", CharSet = CharSet.Ansi, ThrowOnUnmappableChar = true, BestFitMapping = false, CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr m_mpg123_parnew(ref IntPtr pars, [MarshalAs(UnmanagedType.LPStr)]string decoder, out IntPtr error);

            /// <summary>
            /// Create a handle with preset parameters.
            /// </summary>
            /// <param name="preset"></param>
            /// <param name="decoder"></param>
            /// <param name="handle"></param>
            /// <returns></returns>
            public static RESULT mpg123_parnew(M123_PARS preset, [MarshalAs(UnmanagedType.LPStr)]string decoder, out IntPtr handle)
            {
                if (!preset.Valid)
                {
                    throw new ArgumentException(string.Format("Preset invalid!"));
                }

                IntPtr l_PresetHandle = preset.Handle;
                IntPtr result;
                handle = m_mpg123_parnew(ref l_PresetHandle, decoder, out result);
                return (RESULT)result.ToInt32();
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_new_pars", CallingConvention = CallingConvention.Cdecl)]
            static extern IntPtr m_mpg123_new_pars(out IntPtr error);

            /// <summary>
            /// Allocate memory for and return a new MPG123_PARS.
            /// </summary>
            /// <param name="preset"></param>
            /// <returns></returns>
            public static RESULT mpg123_new_pars(out M123_PARS preset)
            {
                IntPtr resultCode;
                IntPtr l_Preset = m_mpg123_new_pars(out resultCode);
                var result = (RESULT)resultCode.ToInt32();
                if (result == RESULT.OK)
                {
                    preset = new M123_PARS(l_Preset);
                }
                else
                {
                    preset = new M123_PARS();
                }
                return result;
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_delete_pars", CallingConvention = CallingConvention.Cdecl)]
            static extern void m_mpg123_delete_pars(ref IntPtr preset);

            /// <summary>
            /// Delete and free up memory used by a MPG123_PARS data structure.
            /// </summary>
            /// <param name="preset"></param>
            public static void mpg123_delete_pars(M123_PARS preset)
            {
                if (!preset.Valid)
                {
                    throw new ArgumentException(string.Format("Preset invalid!"));
                }

                IntPtr l_PresetHandle = preset.Handle;
                m_mpg123_delete_pars(ref l_PresetHandle);
                preset.Disposed();
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_fmt_none", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_fmt_none(ref IntPtr preset);

            /// <summary>
            /// Configure mpg123 parameters to accept no output format at all, use before specifying supported formats with mpg123_format.
            /// </summary>
            /// <param name="preset"></param>
            /// <returns></returns>
            public static RESULT mpg123_fmt_none(M123_PARS preset)
            {
                if (!preset.Valid)
                {
                    throw new ArgumentException(string.Format("Preset invalid!"));
                }

                IntPtr l_PresetHandle = preset.Handle;
                return m_mpg123_fmt_none(ref l_PresetHandle);
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_fmt_all", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_fmt_all(ref IntPtr preset);

            /// <summary>
            /// Configure mpg123 parameters to accept no output format at all, use before specifying supported formats with mpg123_format.
            /// </summary>
            /// <param name="preset"></param>
            /// <returns></returns>
            public static RESULT mpg123_fmt_all(M123_PARS preset)
            {
                if (!preset.Valid)
                {
                    throw new ArgumentException(string.Format("Preset invalid!"));
                }

                IntPtr l_PresetHandle = preset.Handle;
                return m_mpg123_fmt_all(ref l_PresetHandle);
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_fmt", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_fmt(ref IntPtr preset, IntPtr sampleRate, CHANNELCOUNT channelConfig, ENC encodings);

            /// <summary>
            /// Set the audio format support of a mpg123_pars in detail.
            /// </summary>
            /// <param name="preset"></param>
            /// <param name="config"></param>
            /// <returns></returns>
            public static RESULT mpg123_fmt(M123_PARS preset, IAudioConfiguration config)
            {
                IntPtr l_PresetHandle = preset.Handle;
                CHANNELCOUNT channelConfig = GetChannelConfig(config);
                ENC l_Encoding = GetEncoding(config);
                return m_mpg123_fmt(ref l_PresetHandle, new IntPtr(config.SamplingRate), channelConfig, l_Encoding);
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_fmt_support", CallingConvention = CallingConvention.Cdecl)]
            static extern CHANNELCOUNT m_mpg123_fmt_support(ref IntPtr preset, IntPtr sampleRate, ENC encoding);

            /// <summary>
            /// Check to see if a specific format at a specific rate is supported by MPG123_PARS.
            /// </summary>
            /// <param name="preset"></param>
            /// <param name="config"></param>
            /// <returns></returns>
            public static bool mpg123_fmt_support(M123_PARS preset, IAudioConfiguration config)
            {
                IntPtr l_PresetHandle = preset.Handle;
                ENC l_Encoding = GetEncoding(config);
                CHANNELCOUNT availableChannelConfig = m_mpg123_fmt_support(ref l_PresetHandle, new IntPtr(config.SamplingRate), l_Encoding);
                CHANNELCOUNT channelConfig = GetChannelConfig(config);
                return (availableChannelConfig & channelConfig) == channelConfig;
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_par", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_par(ref IntPtr preset, PARAMS type, IntPtr value, double floatValue);

            /// <summary>
            /// Set a specific parameter, for a specific mpg123_pars, using a parameter type key chosen from the mpg123_parms enumeration, to the specified value.
            /// </summary>
            /// <param name="preset"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <param name="floatValue"></param>
            /// <returns></returns>
            public static RESULT mpg123_par(M123_PARS preset, PARAMS type, int value, double floatValue)
            {
                IntPtr l_PresetHandle = preset.Handle;
                return m_mpg123_par(ref l_PresetHandle, type, new IntPtr(value), floatValue);
            }

            [DllImport(NATIVE_LIBRARY, EntryPoint = "mpg123_getpar", CallingConvention = CallingConvention.Cdecl)]
            static extern RESULT m_mpg123_getpar(ref IntPtr preset, PARAMS type, out IntPtr value, out double floatValue);

            /// <summary>
            /// Get a specific parameter, for a specific mpg123_pars. See the MPG123_PARAMS enumeration for a list of available parameters.
            /// </summary>
            /// <param name="preset"></param>
            /// <param name="type"></param>
            /// <param name="value"></param>
            /// <param name="floatValue"></param>
            /// <returns></returns>
            public static RESULT mpg123_getpar(M123_PARS preset, PARAMS type, out int value, out double floatValue)
            {
                IntPtr presetHandle = preset.Handle;
                IntPtr p;
                RESULT result = m_mpg123_getpar(ref presetHandle, type, out p, out floatValue);
                value = p.ToInt32();
                return result;
            }
            #endregion
        }
    }
}
