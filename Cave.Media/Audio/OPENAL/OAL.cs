#region License OpenAL Soft
/*
    Uses openal soft (http://kcat.strangesoft.net/openal.html)
    A non-GPL license for this library is not available.
*/
#endregion

using Cave.IO;
using System;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;

namespace Cave.Media.Audio.OPENAL
{
    /// <summary>
    /// Allows access to the open al native functions.
    /// </summary>
    public static class OAL
    {
        /// <summary>The native library name (windows openal.dll, linux libopenal.so.x, macos libopenal.dylib.</summary>
        const string NATIVE_LIBRARY = "openal";
        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        /// <summary>The global open al synchronize root.</summary>
        public static readonly object SyncRoot = new object();

        /// <summary>
        /// The number buffers per second to use when writing to the device.
        /// Higher numbers increase cpu usage but increase the accuracy, too and decrease the device latency.
        /// It is recommended not to use more than 100 buffers per second. (May result in cracks and clicks at the output.)
        /// </summary>
        public static int BuffersPerSecond = 10;

        #region OpenAL AL* Constants
#pragma warning disable 1591, SA1310
        public const int AL_INVALID = -1;
        public const int AL_NONE = 0;
        public const int AL_FALSE = 0;
        public const int AL_TRUE = 1;

        /// <summary>(Source) the soruce type – AL_UNDETERMINED, AL_STATIC, or AL_STREAMING. </summary>
        public const int AL_SOURCE_TYPE = 0x1027;

        /// <summary>(Source) determines if the positions are relative to the listener default is AL_FALSE.</summary>
        public const int AL_SOURCE_RELATIVE = 0x202;

        /// <summary>(Source) the gain when inside the oriented cone. </summary>
        public const int AL_CONE_INNER_ANGLE = 0x1001;

        /// <summary>(Source) outer angle of the sound cone, in degrees default is 360.</summary>
        public const int AL_CONE_OUTER_ANGLE = 0x1002;

        /// <summary>(Source) Pitch multiplier always positive.</summary>
        public const int AL_PITCH = 0x1003;

        /// <summary>(Listener, Source) X, Y, Z position. </summary>
        public const int AL_POSITION = 0x1004;

        /// <summary>(Source) direction vector.</summary>
        public const int AL_DIRECTION = 0x1005;

        /// <summary>(Listener, Source) Velocity vector.</summary>
        public const int AL_VELOCITY = 0x1006;

        /// <summary>(Source) turns looping on (AL_TRUE) or off (AL_FALSE). </summary>
        public const int AL_LOOPING = 0x1007;
        public const int AL_STATIC = 0x1028;
        public const int AL_STREAMING = 0x1029;
        public const int AL_UNDETERMINED = 0x1030;

        /// <summary>(Source) the ID of the attached buffer. </summary>
        public const int AL_BUFFER = 0x1009;

        /// <summary>(Source, Listener) Master gain - value should be positive.</summary>
        public const int AL_GAIN = 0x100A;

        /// <summary>(Source) the minimum gain for this source. </summary>
        public const int AL_MIN_GAIN = 0x100D;

        /// <summary>(Source) the maximum gain for this source. </summary>
        public const int AL_MAX_GAIN = 0x100E;

        /// <summary>(Listener) Orientation expressed as 'at' and 'up' vectors. </summary>
        public const int AL_ORIENTATION = 0x100F;

        /// <summary>(Source) The distance under which the volume for the source would normally drop by half (before being influenced by rolloff factor or AL_MAX_DISTANCE).</summary>
        public const int AL_REFERENCE_DISTANCE = 0x1020;

        /// <summary>(Source) The rolloff rate for the source default is 1.0. </summary>
        public const int AL_ROLLOFF_FACTOR = 0x1021;

        /// <summary>(Source) the gain when outside the oriented cone. </summary>
        public const int AL_CONE_OUTER_GAIN = 0x1022;

        /// <summary>(Source) Used with the Inverse Clamped Distance Model to set the distance where there will no longer be any attenuation of the source.</summary>
        public const int AL_MAX_DISTANCE = 0x1023;
        public const int AL_CHANNEL_MASK = 0x3000;

        /// <summary>(Source) the state of the source (AL_STOPPED, AL_PLAYING, …).</summary>
        public const int AL_SOURCE_STATE = 0x1010;
        public const int AL_INITIAL = 0x1011;
        public const int AL_PLAYING = 0x1012;
        public const int AL_PAUSED = 0x1013;
        public const int AL_STOPPED = 0x1014;

        /// <summary>(Source) the number of buffers queued on this source. </summary>
        public const int AL_BUFFERS_QUEUED = 0x1015;

        /// <summary>(Source)  the number of buffers in the queue that have been processed.</summary>
        public const int AL_BUFFERS_PROCESSED = 0x1016;

        /// <summary>(Source) the playback position, expressed in seconds. </summary>
        public const int AL_SEC_OFFSET = 0x1024;

        /// <summary>(Source) the playback position, expressed in samples.</summary>
        public const int AL_SAMPLE_OFFSET = 0x1025;

        /// <summary>(Source) the playback position, expressed in bytes.</summary>
        public const int AL_BYTE_OFFSET = 0x1026;
        public const int AL_FORMAT_MONO8 = 0x1100;
        public const int AL_FORMAT_MONO16 = 0x1101;
        public const int AL_FORMAT_STEREO8 = 0x1102;
        public const int AL_FORMAT_STEREO16 = 0x1103;
        public const int AL_FORMAT_MONO_FLOAT32 = 0x10010;
        public const int AL_FORMAT_STEREO_FLOAT32 = 0x10011;

        /// <summary>(Buffer) Frequency of buffer in Hz.</summary>
        public const int AL_FREQUENCY = 0x2001;

        /// <summary>(Buffer) Bit depth of buffer.</summary>
        public const int AL_BITS = 0x2002;

        /// <summary>(Buffer) Number of channels in buffer > 1 is valid, but buffer won’t be positioned when played.</summary>
        public const int AL_CHANNELS = 0x2003;

        /// <summary>(Buffer) Size of buffer in bytes. </summary>
        public const int AL_SIZE = 0x2004;

        /// <summary>(Buffer) Original location where data was copied from generally useless, as was probably freed after buffer creation.</summary>
        public const int AL_DATA = 0x2005;
        public const int AL_UNUSED = 0x2010;
        public const int AL_QUEUED = 0x2011;
        public const int AL_PENDING = 0x2011;
        public const int AL_CURRENT = 0x2012;
        public const int AL_PROCESSED = 0x2012;

        /// <summary>(Error) there is not currently an error. </summary>
        public const int AL_NO_ERROR = AL_FALSE;

        /// <summary>(Error) a bad name (ID) was passed to an OpenAL function. </summary>
        public const int AL_INVALID_NAME = 0xa001;

        /// <summary>(Error) an invalid enum value was passed to an OpenAL function. </summary>
        public const int AL_ILLEGAL_ENUM = 0xA002;

        /// <summary>(Error) an invalid value was passed to an OpenAL function. </summary>
        public const int AL_INVALID_ENUM = 0xA002;
        public const int AL_INVALID_VALUE = 0xA003;
        public const int AL_ILLEGAcOMMAND = 0xA004;

        /// <summary>(Error) the requested operation is not valid. </summary>
        public const int AL_INVALID_OPERATION = 0xA004;

        /// <summary>(Error) the requested operation resulted in OpenAL running out of memory.</summary>
        public const int AL_OUT_OF_MEMORY = 0xA005;
        public const int AL_VENDOR = 0xB001;
        public const int AL_VERSION = 0xB002;
        public const int AL_RENDERER = 0xB003;
        public const int AL_EXTENSIONS = 0xB004;
        public const int AL_DOPPLER_FACTOR = 0xC000;
        public const int AL_DOPPLER_VELOCITY = 0xC001;
        public const int AL_SPEED_OF_SOUND = 0xC003;
        public const int AL_DISTANCE_SCALE = 0xC002;
        public const int AL_DISTANCE_MODEL = 0xD000;
        public const int AL_INVERSE_DISTANCE = 0xD001;
        public const int AL_INVERSE_DISTANCE_CLAMPED = 0xD002;
        public const int AL_LINEAR_DISTANCE = 0xD003;
        public const int AL_LINEAR_DISTANCE_CLAMPED = 0xD004;
        public const int AL_EXPONENT_DISTANCE = 0xD005;
        public const int AL_EXPONENT_DISTANCE_CLAMPED = 0xD006;
        public const int AL_ENV_ROOM_IASIG = 0x3001;
        public const int AL_ENV_ROOM_HIGH_FREQUENCY_IASIG = 0x3002;
        public const int AL_ENV_ROOM_ROLLOFF_FACTOR = 0x3003;
        public const int AL_ENV_DECAY_TIME_IASIG = 0x3004;
        public const int AL_ENV_DECAY_HIGH_FREQUENCY_RATIO_IASIG = 0x3005;
        public const int AL_ENV_REFLECTIONS_IASIG = 0x3006;
        public const int AL_ENV_REFLECTIONS_DELAY_IASIG = 0x3006;
        public const int AL_ENV_REVERB_IASIG = 0x3007;
        public const int AL_ENV_REVERB_DELAY_IASIG = 0x3008;
        public const int AL_ENV_DIFFUSION_IASIG = 0x3009;
        public const int AL_ENV_DENSITY_IASIG = 0x300A;
        public const int AL_ENV_HIGH_FREQUENCY_REFERENCE_IASIG = 0x300B;
        public const int AL_METERS_PER_UNIT = 0x20004;
        public const int AL_DIRECT_FILTER = 0x20005;
        public const int AL_AUXILIARY_SEND_FILTER = 0x20006;
        public const int AL_AIR_ABSORPTION_FACTOR = 0x20007;
        public const int AL_ROOM_ROLLOFF_FACTOR = 0x20008;
        public const int AL_CONE_OUTER_GAINHF = 0x20009;
        public const int AL_DIRECT_FILTER_GAINHF_AUTO = 0x2000A;
        public const int AL_AUXILIARY_SEND_FILTER_GAIN_AUTO = 0x2000B;
        public const int AL_AUXILIARY_SEND_FILTER_GAINHF_AUTO = 0x2000C;
        public const int AL_EFFECTSLOT_EFFECT = 0x0001;
        public const int AL_EFFECTSLOT_GAIN = 0x0002;
        public const int AL_EFFECTSLOT_AUXILIARY_SEND_AUTO = 0x0003;
        public const int AL_EFFECTSLOT_NULL = 0x0000;
        public const int AL_CHORUS_WAVEFORM = 0x0001;
        public const int AL_CHORUS_PHASE = 0x0002;
        public const int AL_CHORUS_RATE = 0x0003;
        public const int AL_CHORUS_DEPTH = 0x0004;
        public const int AL_CHORUS_FEEDBACK = 0x0005;
        public const int AL_CHORUS_DELAY = 0x0006;
        public const int AL_ECHO_DELAY = 0x0001;
        public const int AL_ECHO_LRDELAY = 0x0002;
        public const int AL_ECHO_DAMPING = 0x0003;
        public const int AL_ECHO_FEEDBACK = 0x0004;
        public const int AL_ECHO_SPREAD = 0x0005;
        public const int AL_FREQUENCY_SHIFTER_FREQUENCY = 0x0001;
        public const int AL_FREQUENCY_SHIFTER_LEFT_DIRECTION = 0x0002;
        public const int AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION = 0x0003;
        public const int AL_PITCH_SHIFTER_COARSE_TUNE = 0x0001;
        public const int AL_PITCH_SHIFTER_FINE_TUNE = 0x0002;
        public const int AL_AUTOWAH_ATTACK_TIME = 0x0001;
        public const int AL_AUTOWAH_RELEASE_TIME = 0x0002;
        public const int AL_AUTOWAH_RESONANCE = 0x0003;
        public const int AL_AUTOWAH_PEAK_GAIN = 0x0004;
        public const int AL_EQUALIZER_LOW_GAIN = 0x0001;
        public const int AL_EQUALIZER_LOW_CUTOFF = 0x0002;
        public const int AL_EQUALIZER_MID1_GAIN = 0x0003;
        public const int AL_EQUALIZER_MID1_CENTER = 0x0004;
        public const int AL_EQUALIZER_MID1_WIDTH = 0x0005;
        public const int AL_EQUALIZER_MID2_GAIN = 0x0006;
        public const int AL_EQUALIZER_MID2_CENTER = 0x0007;
        public const int AL_EQUALIZER_MID2_WIDTH = 0x0008;
        public const int AL_EQUALIZER_HIGH_GAIN = 0x0009;
        public const int AL_EQUALIZER_HIGH_CUTOFF = 0x000A;
        public const int AL_LOWPASS_GAIN = 0x0001;
        public const int AL_LOWPASS_GAINHF = 0x0002;
        public const int AL_BANDPASS_GAIN = 0x0001;
        public const int AL_BANDPASS_GAINLF = 0x0002;
        public const int AL_BANDPASS_GAINHF = 0x0003;
#pragma warning restore 1591, SA1310
        #endregion

        #region OpenAL ALC* Constants
#pragma warning disable 1591
        public const int ALC_INVALID = 0;
        public const int ALC_FALSE = 0;
        public const int ALC_TRUE = 1;
        public const int ALC_NO_ERROR = ALC_FALSE;
        public const int ALC_MAJOR_VERSION = 0x1000;
        public const int ALC_MINOR_VERSION = 0x1001;
        public const int ALC_ATTRIBUTES_SIZE = 0x1002;
        public const int ALC_ALaTTRIBUTES = 0x1003;
        public const int ALC_CAPTURE_DEVICE_SPECIFIER = 0x310;
        public const int ALC_CAPTURE_DEFAULT_DEVICE_SPECIFIER = 0x311;
        public const int ALC_CAPTURE_SAMPLES = 0x312;
        public const int ALC_DEFAULT_DEVICE_SPECIFIER = 0x1004;
        public const int ALC_DEVICE_SPECIFIER = 0x1005;
        public const int ALC_EXTENSIONS = 0x1006;
        public const int ALC_FREQUENCY = 0x1007;
        public const int ALC_REFRESH = 0x1008;
        public const int ALC_SYNC = 0x1009;
        public const int ALC_MONO_SOURCES = 0x1010;
        public const int ALC_STEREO_SOURCES = 0x1011;
        public const int ALC_INVALID_DEVICE = 0xA001;
        public const int ALC_INVALID_CONTEXT = 0xA002;
        public const int ALC_INVALID_ENUM = 0xA003;
        public const int ALC_INVALID_VALUE = 0xA004;
        public const int ALC_OUT_OF_MEMORY = 0xA005;
        public const int ALC_DEFAULT_ALL_DEVICES_SPECIFIER = 0x1012;
        public const int ALC_ALL_DEVICES_SPECIFIER = 0x1013;
        public const int ALC_EFX_MAJOR_VERSION = 0x20001;
        public const int ALC_EFX_MINOR_VERSION = 0x20002;
        public const int ALC_MAX_AUXILIARY_SENDS = 0x20003;
        public const string ALC_EXT_EFX_NAME = "ALC_EXT_EFX";
#pragma warning restore 1591
        #endregion

        /// <summary>
        /// Retrieves a valid AL_FORMAT_* from the specified AudioConfiguration.
        /// </summary>
        /// <param name="config"></param>
        /// <returns></returns>
        public static int AL_FORMAT(IAudioConfiguration config)
        {
            switch (config.Format)
            {
                case AudioSampleFormat.Int16:
                    switch (config.ChannelSetup)
                    {
                        case AudioChannelSetup.Mono: return AL_FORMAT_MONO16;
                        case AudioChannelSetup.Stereo: return AL_FORMAT_STEREO16;
                        default: throw new NotSupportedException("Unsupported channel setup!");
                    }

                case AudioSampleFormat.Float:
                    switch (config.ChannelSetup)
                    {
                        case AudioChannelSetup.Mono: return AL_FORMAT_MONO_FLOAT32;
                        case AudioChannelSetup.Stereo: return AL_FORMAT_STEREO_FLOAT32;
                        default: throw new NotSupportedException("Unsupported channel setup!");
                    }

                default: throw new NotSupportedException(string.Format("Unsupported sample format {0}!", config.Format));
            }

        }

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            #region OpenAL AL* Functions
#pragma warning disable 1591
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferData(int buffer, int format, [In] byte[] data, int size, int frequency);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferData(int buffer, int format, [In] IntPtr data, int size, int frequency);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferf(int bid, int param, float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBuffer3f(int bid, int param, float value1, float value2, float value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferfv(int bid, int param, out float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferi(int bid, int param, int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBuffer3i(int bid, int param, int value1, int value2, int value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alBufferiv(int bid, int param, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteBuffers(int number, [In] ref int buffer);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteBuffers(int number, [In] int[] buffers);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteBuffers(int number, [In] IntPtr buffers);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteSources(int number, [In] ref int sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteSources(int number, [In] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteSources(int number, [In] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDisable(int capability);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDistanceModel(int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDopplerFactor(float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDopplerVelocity(float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSpeedOfSound(float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alEnable(int capability);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenBuffers(int number, out int buffer);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenBuffers(int number, [Out] int[] buffers);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenBuffers(int number, [Out] IntPtr buffers);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenSources(int number, out int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenSources(int number, [Out] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGenSources(int number, [Out] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGetBoolean(int state);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBooleanv(int state, out int output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBooleanv(int state, [Out] int[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBooleanv(int state, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferf(int buffer, int attribute, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferf(int buffer, int attribute, [Out] int[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferf(int buffer, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBuffer3f(int buffer, int attribute, out float value1, out float value2, out float value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferfv(int buffer, int attribute, out float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferfv(int buffer, int attribute, [Out] float[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferfv(int buffer, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferi(int buffer, int attribute, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferi(int buffer, int attribute, [Out] int[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferi(int buffer, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBuffer3i(int buffer, int attribute, out int value1, out int value2, out int value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferiv(int buffer, int attribute, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferiv(int buffer, int attribute, [Out] int[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetBufferiv(int buffer, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern double alGetDouble(int state);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetDoublev(int state, out double output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetDoublev(int state, [Out] double[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetDoublev(int state, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGetEnumValue(string enumName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGetError();
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern float alGetFloat(int state);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetFloatv(int state, out float output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetFloatv(int state, [Out] float[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetFloatv(int state, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGetInteger(int state);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetIntegerv(int state, out int output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetIntegerv(int state, [Out] int[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetIntegerv(int state, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListener3f(int attribute, out float output1, out float output2, out float output3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListener3f(int attribute, [Out] float[] output1, [Out] float[] output2, [Out] float[] output3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListener3f(int attribute, [Out] IntPtr output1, [Out] IntPtr output2, [Out] IntPtr output3);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerf(int attribute, out float output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerf(int attribute, [Out] float[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerf(int attribute, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerfv(int attribute, out float output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerfv(int attribute, [Out] float[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListenerfv(int attribute, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneri(int attribute, out int output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneri(int attribute, [Out] int[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneri(int attribute, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListener3i(int attribute, out int output1, out int output2, out int output3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneriv(int attribute, out int output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneriv(int attribute, [Out] int[] output);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetListeneriv(int attribute, [Out] IntPtr output);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alGetProcAddress(string functionName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSource3f(int source, int attribute, out float value1, out float value2, out float value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSource3f(int source, int attribute, [Out] float[] value1, [Out] float[] value2, [Out] float[] value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSource3f(int source, int attribute, [Out] IntPtr value1, [Out] IntPtr value2, [Out] IntPtr value3);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcef(int source, int attribute, out float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcef(int source, int attribute, [Out] float[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcef(int source, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcefv(int source, int attribute, out float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcefv(int source, int attribute, [Out] float[] values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcefv(int source, int attribute, [Out] IntPtr values);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcei(int source, int attribute, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcei(int source, int attribute, [Out] int[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourcei(int source, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSource3i(int source, int attribute, out int value1, out int value2, out int value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourceiv(int source, int attribute, out int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourceiv(int source, int attribute, [Out] int[] val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alGetSourceiv(int source, int attribute, [Out] IntPtr val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            private static extern IntPtr alGetString(int state);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alHint(int target, int mode);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alIsBuffer(int buffer);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alIsEnabled(int capability);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alIsExtensionPresent(string extensionName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alIsSource(int id);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListener3f(int attribute, float value1, float value2, float value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListenerf(int attribute, float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListenerfv(int attribute, [In] ref float values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListenerfv(int attribute, [In] float[] values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListenerfv(int attribute, [In] IntPtr values);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListeneri(int attribute, int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListener3i(int attribute, int value1, int value2, int value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alListeneriv(int attribute, [In] ref int values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alQueuei(int source, int attribute, int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSource3f(int source, int attribute, float value1, float value2, float value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcef(int source, int attribute, float val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcefv(int source, int attribute, [In] ref float values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcefv(int source, int attribute, [In] float[] values);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcefv(int source, int attribute, [In] IntPtr values);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcei(int source, int attribute, int val);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSource3i(int source, int attribute, int value1, int value2, int value3);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePause(int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePausev(int number, [In] ref int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePausev(int number, [In] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePausev(int number, [In] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePlay(int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePlayv(int number, [In] ref int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePlayv(int number, [In] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourcePlayv(int number, [In] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceQueueBuffers(int source, int number, [In] ref int buffer);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceQueueBuffers(int source, int number, [In] int[] buffers);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceQueueBuffers(int source, int number, [In] IntPtr buffers);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceRewind(int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceRewindv(int number, [In] ref int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceRewindv(int number, [In] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceRewindv(int number, [In] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceStop(int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceStopv(int number, [In] ref int source);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceStopv(int number, [In] int[] sources);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceStopv(int number, [In] IntPtr sources);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceUnqueueBuffers(int source, int number, [In] ref int buffer);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceUnqueueBuffers(int source, int number, [In] int[] buffers);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alSourceUnqueueBuffers(int source, int number, [In] IntPtr buffers);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGenEnvironmentIASIG(int number, out int environments);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGenEnvironmentIASIG(int number, [Out] int[] environments);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alGenEnvironmentIASIG(int number, [Out] IntPtr environments);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteEnvironmentIASIG(int number, [In] ref int environments);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteEnvironmentIASIG(int number, [In] int[] environments);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alDeleteEnvironmentIASIG(int number, [In] IntPtr environments);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alIsEnvironmentIASIG(int environment);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alEnvironmentiIASIG(int environmentId, int attribute, int val);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alEnvironmentfIASIG(int environmentId, int attribute, int val);
#pragma warning restore 1591
            #endregion

            #region OpenAL ALC* Functions
#pragma warning disable 1591
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcCloseDevice([In] IntPtr device);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcCreateContext([In] IntPtr device, [In] ref int attribute);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcCreateContext([In] IntPtr device, [In] int[] attribute);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcCreateContext([In] IntPtr device, [In] IntPtr attribute);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcDestroyContext([In] IntPtr context);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcGetContextsDevice([In] IntPtr context);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcGetCurrentContext();
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alcGetEnumValue([In] IntPtr device, string enumName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alcGetError([In] IntPtr device);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcGetIntegerv([In] IntPtr device, int attribute, int size, out int data);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcGetIntegerv([In] IntPtr device, int attribute, int size, [Out] int[] data);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcGetIntegerv([In] IntPtr device, int attribute, int size, [Out] IntPtr data);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcGetProcAddress([In] IntPtr device, string functionName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            private static extern IntPtr alcGetString([In] IntPtr device, int attribute);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alcIsExtensionPresent([In] IntPtr device, string extensionName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alcMakeContextCurrent([In] IntPtr context);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcOpenDevice(byte[] deviceName);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcProcessContext([In] IntPtr context);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcSuspendContext([In] IntPtr context);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern IntPtr alcCaptureOpenDevice(byte[] devicename, int frequency, int format, int buffersize);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern int alcCaptureCloseDevice([In] IntPtr device);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcCaptureStart([In] IntPtr device);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcCaptureStop([In] IntPtr device);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
            [SuppressUnmanagedCodeSecurity]
            public static extern void alcCaptureSamples([In] IntPtr device, [In] IntPtr buffer, int samples);
#pragma warning restore 1591
            #endregion

            #region Additional wrappers for string functions
#pragma warning disable 1591

            public static IntPtr alcOpenDevice(string deviceName)
            {
                return alcOpenDevice(Encoding.UTF8.GetBytes(deviceName + '\0'));
            }

            public static IntPtr alcCaptureOpenDevice(string deviceName, int frequency, int format, int buffersize)
            {
                return alcCaptureOpenDevice(Encoding.UTF8.GetBytes(deviceName + '\0'), frequency, format, buffersize);
            }

            public static string alGetString1(int state) { return MarshalStruct.ReadUtf8(alGetString(state)); }

            public static string[] alGetStringv(int state) { return MarshalStruct.ReadUtf8Strings(alGetString(state)); }

            public static string alcGetString1(IntPtr device, int attribute) { return MarshalStruct.ReadUtf8(alcGetString(device, attribute)); }

            public static string[] alcGetStringv([In] IntPtr device, int attribute) { return MarshalStruct.ReadUtf8Strings(alcGetString(device, attribute)); }

            /// <summary>
            /// Checks for an OpenAL error and thows an appropriate exception if an error occured.
            /// </summary>
            public static void CheckError()
            {
                switch (alGetError())
                {
                    case AL_NO_ERROR:
                        return;
                    case AL_INVALID_ENUM:
                        throw new InvalidOperationException("OpenAL: Invalid enum passed !");
                    case AL_INVALID_NAME:
                        throw new InvalidOperationException("OpenAL: Invalid name passed !");
                    case AL_INVALID_VALUE:
                        throw new InvalidOperationException("OpenAL: Invalid value passed !");
                    case AL_INVALID_OPERATION:
                        throw new InvalidOperationException("OpenAL: Requested operation is invalid !");
                    case AL_OUT_OF_MEMORY:
                        throw new InvalidOperationException("OpenAL: Out of memory !");
                    default:
                        throw new NotImplementedException("Undefined OpenAL error !");
                }
            }
#pragma warning restore 1591
            #endregion
        }
    }
}
