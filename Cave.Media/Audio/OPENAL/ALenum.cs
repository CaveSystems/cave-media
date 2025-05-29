#pragma warning disable CS1591, CA1069, CA1707

namespace Cave.Media.Audio.OPENAL;

public enum ALenum : int
{
    AL_AIR_ABSORPTION_FACTOR = 0x20007,

    AL_AUTOWAH_ATTACK_TIME = 0x0001,

    AL_AUTOWAH_PEAK_GAIN = 0x0004,

    AL_AUTOWAH_RELEASE_TIME = 0x0002,

    AL_AUTOWAH_RESONANCE = 0x0003,

    AL_AUXILIARY_SEND_FILTER = 0x20006,

    AL_AUXILIARY_SEND_FILTER_GAIN_AUTO = 0x2000B,

    AL_AUXILIARY_SEND_FILTER_GAINHF_AUTO = 0x2000C,

    AL_BANDPASS_GAIN = 0x0001,

    AL_BANDPASS_GAINHF = 0x0003,

    AL_BANDPASS_GAINLF = 0x0002,

    /// <summary>(Buffer) Bit depth of buffer.</summary>
    AL_BITS = 0x2002,

    /// <summary>(Source) the ID of the attached buffer.</summary>
    AL_BUFFER = 0x1009,

    /// <summary>(Source) the number of buffers in the queue that have been processed.</summary>
    AL_BUFFERS_PROCESSED = 0x1016,

    /// <summary>(Source) the number of buffers queued on this source.</summary>
    AL_BUFFERS_QUEUED = 0x1015,

    /// <summary>(Source) the playback position, expressed in bytes.</summary>
    AL_BYTE_OFFSET = 0x1026,

    AL_CHANNEL_MASK = 0x3000,

    /// <summary>(Buffer) Number of channels in buffer &gt; 1 is valid, but buffer won’t be positioned when played.</summary>
    AL_CHANNELS = 0x2003,

    AL_CHORUS_DELAY = 0x0006,

    AL_CHORUS_DEPTH = 0x0004,

    AL_CHORUS_FEEDBACK = 0x0005,

    AL_CHORUS_PHASE = 0x0002,

    AL_CHORUS_RATE = 0x0003,

    AL_CHORUS_WAVEFORM = 0x0001,

    /// <summary>(Source) the gain when inside the oriented cone.</summary>
    AL_CONE_INNER_ANGLE = 0x1001,

    /// <summary>(Source) outer angle of the sound cone, in degrees default is 360.</summary>
    AL_CONE_OUTER_ANGLE = 0x1002,

    /// <summary>(Source) the gain when outside the oriented cone.</summary>
    AL_CONE_OUTER_GAIN = 0x1022,

    AL_CONE_OUTER_GAINHF = 0x20009,

    AL_CURRENT = 0x2012,

    /// <summary>(Buffer) Original location where data was copied from generally useless, as was probably freed after buffer creation.</summary>
    AL_DATA = 0x2005,

    AL_DIRECT_FILTER = 0x20005,

    AL_DIRECT_FILTER_GAINHF_AUTO = 0x2000A,

    /// <summary>(Source) direction vector.</summary>
    AL_DIRECTION = 0x1005,

    AL_DISTANCE_MODEL = 0xD000,

    AL_DISTANCE_SCALE = 0xC002,

    AL_DOPPLER_FACTOR = 0xC000,

    AL_DOPPLER_VELOCITY = 0xC001,

    AL_ECHO_DAMPING = 0x0003,

    AL_ECHO_DELAY = 0x0001,

    AL_ECHO_FEEDBACK = 0x0004,

    AL_ECHO_LRDELAY = 0x0002,

    AL_ECHO_SPREAD = 0x0005,

    AL_EFFECTSLOT_AUXILIARY_SEND_AUTO = 0x0003,

    AL_EFFECTSLOT_EFFECT = 0x0001,

    AL_EFFECTSLOT_GAIN = 0x0002,

    AL_EFFECTSLOT_NULL = 0x0000,

    AL_ENV_DECAY_HIGH_FREQUENCY_RATIO_IASIG = 0x3005,

    AL_ENV_DECAY_TIME_IASIG = 0x3004,

    AL_ENV_DENSITY_IASIG = 0x300A,

    AL_ENV_DIFFUSION_IASIG = 0x3009,

    AL_ENV_HIGH_FREQUENCY_REFERENCE_IASIG = 0x300B,

    AL_ENV_REFLECTIONS_DELAY_IASIG = 0x3006,

    AL_ENV_REFLECTIONS_IASIG = 0x3006,

    AL_ENV_REVERB_DELAY_IASIG = 0x3008,

    AL_ENV_REVERB_IASIG = 0x3007,

    AL_ENV_ROOM_HIGH_FREQUENCY_IASIG = 0x3002,

    AL_ENV_ROOM_IASIG = 0x3001,

    AL_ENV_ROOM_ROLLOFF_FACTOR = 0x3003,

    AL_EQUALIZER_HIGH_CUTOFF = 0x000A,

    AL_EQUALIZER_HIGH_GAIN = 0x0009,

    AL_EQUALIZER_LOW_CUTOFF = 0x0002,

    AL_EQUALIZER_LOW_GAIN = 0x0001,

    AL_EQUALIZER_MID1_CENTER = 0x0004,

    AL_EQUALIZER_MID1_GAIN = 0x0003,

    AL_EQUALIZER_MID1_WIDTH = 0x0005,

    AL_EQUALIZER_MID2_CENTER = 0x0007,

    AL_EQUALIZER_MID2_GAIN = 0x0006,

    AL_EQUALIZER_MID2_WIDTH = 0x0008,

    AL_EXPONENT_DISTANCE = 0xD005,

    AL_EXPONENT_DISTANCE_CLAMPED = 0xD006,

    AL_EXTENSIONS = 0xB004,

    /// <summary>False value</summary>
    AL_FALSE = 0,

    AL_FORMAT_MONO_FLOAT32 = 0x10010,

    AL_FORMAT_MONO16 = 0x1101,

    AL_FORMAT_MONO8 = 0x1100,

    AL_FORMAT_STEREO_FLOAT32 = 0x10011,

    AL_FORMAT_STEREO16 = 0x1103,

    AL_FORMAT_STEREO8 = 0x1102,

    /// <summary>(Buffer) Frequency of buffer in Hz.</summary>
    AL_FREQUENCY = 0x2001,

    AL_FREQUENCY_SHIFTER_FREQUENCY = 0x0001,

    AL_FREQUENCY_SHIFTER_LEFT_DIRECTION = 0x0002,

    AL_FREQUENCY_SHIFTER_RIGHT_DIRECTION = 0x0003,

    /// <summary>(Source, Listener) Master gain - value should be positive.</summary>
    AL_GAIN = 0x100A,

    AL_ILLEGAcOMMAND = 0xA004,

    /// <summary>(Error) an invalid enum value was passed to an OpenAL function.</summary>
    AL_ILLEGAL_ENUM = 0xA002,

    AL_INITIAL = 0x1011,

    /// <summary>Invalid value</summary>
    AL_INVALID = -1,

    /// <summary>(Error) an invalid value was passed to an OpenAL function.</summary>
    AL_INVALID_ENUM = 0xA002,

    /// <summary>(Error) a bad name (ID) was passed to an OpenAL function.</summary>
    AL_INVALID_NAME = 0xa001,

    /// <summary>(Error) the requested operation is not valid.</summary>
    AL_INVALID_OPERATION = 0xA004,

    AL_INVALID_VALUE = 0xA003,

    AL_INVERSE_DISTANCE = 0xD001,

    AL_INVERSE_DISTANCE_CLAMPED = 0xD002,

    AL_LINEAR_DISTANCE = 0xD003,

    AL_LINEAR_DISTANCE_CLAMPED = 0xD004,

    /// <summary>(Source) turns looping on (AL_TRUE) or off (AL_FALSE).</summary>
    AL_LOOPING = 0x1007,

    AL_LOWPASS_GAIN = 0x0001,

    AL_LOWPASS_GAINHF = 0x0002,

    /// <summary>(Source) Used with the Inverse Clamped Distance Model to set the distance where there will no longer be any attenuation of the source.</summary>
    AL_MAX_DISTANCE = 0x1023,

    /// <summary>(Source) the maximum gain for this source.</summary>
    AL_MAX_GAIN = 0x100E,

    AL_METERS_PER_UNIT = 0x20004,

    /// <summary>(Source) the minimum gain for this source.</summary>
    AL_MIN_GAIN = 0x100D,

    /// <summary>(Error) there is not currently an error.</summary>
    AL_NO_ERROR = AL_FALSE,

    /// <summary>None value</summary>
    AL_NONE = 0,

    /// <summary>(Listener) Orientation expressed as 'at' and 'up' vectors.</summary>
    AL_ORIENTATION = 0x100F,

    /// <summary>(Error) the requested operation resulted in OpenAL running out of memory.</summary>
    AL_OUT_OF_MEMORY = 0xA005,

    AL_PAUSED = 0x1013,

    AL_PENDING = 0x2011,

    /// <summary>(Source) Pitch multiplier always positive.</summary>
    AL_PITCH = 0x1003,

    AL_PITCH_SHIFTER_COARSE_TUNE = 0x0001,

    AL_PITCH_SHIFTER_FINE_TUNE = 0x0002,

    AL_PLAYING = 0x1012,

    /// <summary>(Listener, Source) X, Y, Z position.</summary>
    AL_POSITION = 0x1004,

    AL_PROCESSED = 0x2012,

    AL_QUEUED = 0x2011,

    /// <summary>(Source) The distance under which the volume for the source would normally drop by half (before being influenced by rolloff factor or AL_MAX_DISTANCE).</summary>
    AL_REFERENCE_DISTANCE = 0x1020,

    AL_RENDERER = 0xB003,

    /// <summary>(Source) The rolloff rate for the source default is 1.0.</summary>
    AL_ROLLOFF_FACTOR = 0x1021,

    AL_ROOM_ROLLOFF_FACTOR = 0x20008,

    /// <summary>(Source) the playback position, expressed in samples.</summary>
    AL_SAMPLE_OFFSET = 0x1025,

    /// <summary>(Source) the playback position, expressed in seconds.</summary>
    AL_SEC_OFFSET = 0x1024,

    /// <summary>(Buffer) Size of buffer in bytes.</summary>
    AL_SIZE = 0x2004,

    /// <summary>(Source) determines if the positions are relative to the listener default is AL_FALSE.</summary>
    AL_SOURCE_RELATIVE = 0x202,

    /// <summary>(Source) the state of the source (AL_STOPPED, AL_PLAYING, …).</summary>
    AL_SOURCE_STATE = 0x1010,

    /// <summary>(Source) the soruce type – AL_UNDETERMINED, AL_STATIC, or AL_STREAMING.</summary>
    AL_SOURCE_TYPE = 0x1027,

    /// <summary>
    /// Reference (propagation) speed used in the Doppler calculation.The source and listener velocities should be expressed in the same units as the speed of sound.
    /// </summary>
    AL_SPEED_OF_SOUND = 0xC003,

    /// <summary>Static sound buffer. Not <see cref="AL_STREAMING"/></summary>
    AL_STATIC = 0x1028,

    /// <summary>Buffer is stopped, not <see cref="AL_PLAYING"/></summary>
    AL_STOPPED = 0x1014,

    /// <summary>Streaming sound buffer, not <see cref="AL_STATIC"/></summary>
    AL_STREAMING = 0x1029,

    /// <summary>True value</summary>
    AL_TRUE = 1,

    AL_UNDETERMINED = 0x1030,

    AL_UNUSED = 0x2010,

    /// <summary>(Listener, Source) Velocity vector.</summary>
    AL_VELOCITY = 0x1006,

    AL_VENDOR = 0xB001,

    AL_VERSION = 0xB002,
}
