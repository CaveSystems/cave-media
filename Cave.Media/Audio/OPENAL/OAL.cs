using System;
using System.Diagnostics.CodeAnalysis;
using System.Runtime.InteropServices;
using System.Security;
using System.Text;
using Cave.IO;

namespace Cave.Media.Audio.OPENAL;

/// <summary>Allows access to the open al native functions.</summary>
[SuppressMessage("Naming", "CA1707")]
public static class OAL
{
    #region Private Fields

    const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

    /// <summary>The native library name (windows openal32.dll, linux libopenal.so.x, macos libopenal.dylib.</summary>
    const string NATIVE_LIBRARY = "openal32";

    #endregion Private Fields

    #region Internal Classes

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        #region Private Methods

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr alcGetString([In] IntPtr device, ALCenum attribute);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        private static extern IntPtr alGetString(ALenum param);

        #endregion Private Methods

        #region Public Methods

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBuffer3f(ALbuffer buffer, ALenum param, float value1, float value2, float value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBuffer3i(ALbuffer buffer, ALenum param, int value1, int value2, int value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferData(ALbuffer buffer, ALenum format, [In] byte[] data, int size, int frequency);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferData(ALbuffer buffer, ALenum format, [In] IntPtr data, int size, int frequency);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferf(ALbuffer buffer, ALenum param, float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferfv(ALbuffer buffer, ALenum param, out float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferi(ALbuffer buffer, ALenum param, int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alBufferiv(ALbuffer buffer, ALenum param, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALenum alcCaptureCloseDevice([In] IntPtr device);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alcCaptureOpenDevice(byte[] devicename, int frequency, ALenum format, int buffersize);

        public static IntPtr alcCaptureOpenDevice(string deviceName, int frequency, ALenum format, int buffersize) => alcCaptureOpenDevice(Encoding.UTF8.GetBytes(deviceName + '\0'), frequency, format, buffersize);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcCaptureSamples([In] IntPtr device, [In] IntPtr buffer, int samples);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcCaptureStart([In] IntPtr device);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcCaptureStop([In] IntPtr device);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcCloseDevice([In] IntPtr device);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alcCreateContext([In] IntPtr device, [In] ref int attribute);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alcCreateContext([In] IntPtr device, [In] int[] attributes);

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
        public static extern ALCenum alcGetEnumValue([In] IntPtr device, string enumName);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALCenum alcGetError([In] IntPtr device);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcGetIntegerv([In] IntPtr device, ALCenum attribute, int size, out int data);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcGetIntegerv([In] IntPtr device, ALCenum attribute, int size, [Out] int[] data);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcGetIntegerv([In] IntPtr device, ALCenum attribute, int size, [Out] IntPtr data);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alcGetProcAddress([In] IntPtr device, string functionName);

        public static UTF8 alcGetString1(IntPtr device, ALCenum attribute) => MarshalStruct.ReadUtf8(alcGetString(device, attribute));

        public static UTF8[] alcGetStringv([In] IntPtr device, ALCenum attribute) => MarshalStruct.ReadUtf8Strings(alcGetString(device, attribute));

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALenum alcIsExtensionPresent([In] IntPtr device, string extensionName);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALenum alcMakeContextCurrent([In] IntPtr context);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alcOpenDevice(byte[] deviceName);

        public static IntPtr alcOpenDevice(string deviceName) => alcOpenDevice(Encoding.UTF8.GetBytes(deviceName + '\0'));

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcProcessContext([In] IntPtr context);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alcSuspendContext([In] IntPtr context);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteBuffers(int number, [In] ref ALbuffer buffer);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteBuffers(int number, [In] ALbuffer[] buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteBuffers(int number, [In] IntPtr buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteSources(int number, [In] ref ALsource sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteSources(int number, [In] ALsource[] sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDeleteSources(int number, [In] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDisable(ALenum capability);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDistanceModel(ALenum val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDopplerFactor(float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alDopplerVelocity(float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alEnable(ALenum capability);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenBuffers(int number, out ALbuffer buffer);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenBuffers(int number, [Out] ALbuffer[] buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenBuffers(int number, [Out] IntPtr buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenSources(int number, out ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenSources(int number, [Out] ALsource[] sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGenSources(int number, [Out] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALenum alGetBoolean(ALenum state);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBooleanv(ALenum state, out ALenum output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBooleanv(ALenum state, [Out] ALenum[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBooleanv(ALenum state, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBuffer3f(ALbuffer buffer, ALenum attribute, out float value1, out float value2, out float value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBuffer3i(ALbuffer buffer, ALenum attribute, out int value1, out int value2, out int value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferf(ALbuffer buffer, ALenum attribute, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferf(ALbuffer buffer, ALenum attribute, [Out] int[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferf(ALbuffer buffer, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferfv(ALbuffer buffer, ALenum attribute, out float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferfv(ALbuffer buffer, ALenum attribute, [Out] float[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferfv(ALbuffer buffer, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferi(ALbuffer buffer, ALenum attribute, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferi(ALbuffer buffer, ALenum attribute, [Out] int[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferi(ALbuffer buffer, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferiv(ALbuffer buffer, ALenum attribute, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferiv(ALbuffer buffer, ALenum attribute, [Out] int[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetBufferiv(ALbuffer buffer, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern double alGetDouble(ALenum state);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetDoublev(ALenum state, out double output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetDoublev(ALenum state, [Out] double[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetDoublev(ALenum state, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alGetEnumValue(string enumName);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern ALenum alGetError();

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern float alGetFloat(ALenum state);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetFloatv(ALenum state, out float output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetFloatv(ALenum state, [Out] float[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetFloatv(ALenum state, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alGetInteger(ALenum state);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetIntegerv(ALenum state, out int output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetIntegerv(ALenum state, [Out] int[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetIntegerv(ALenum state, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListener3f(ALenum attribute, out float output1, out float output2, out float output3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListener3f(ALenum attribute, [Out] float[] output1, [Out] float[] output2, [Out] float[] output3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListener3f(ALenum attribute, [Out] IntPtr output1, [Out] IntPtr output2, [Out] IntPtr output3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListener3i(ALenum attribute, out int output1, out int output2, out int output3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerf(ALenum attribute, out float output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerf(ALenum attribute, [Out] float[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerf(ALenum attribute, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerfv(ALenum attribute, out float output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerfv(ALenum attribute, [Out] float[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListenerfv(ALenum attribute, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneri(ALenum attribute, out int output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneri(ALenum attribute, [Out] int[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneri(ALenum attribute, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneriv(ALenum attribute, out int output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneriv(ALenum attribute, [Out] int[] output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetListeneriv(ALenum attribute, [Out] IntPtr output);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern IntPtr alGetProcAddress(string functionName);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSource3f(ALsource source, ALenum attribute, out float value1, out float value2, out float value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSource3f(ALsource source, ALenum attribute, [Out] float[] value1, [Out] float[] value2, [Out] float[] value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSource3f(ALsource source, ALenum attribute, [Out] IntPtr value1, [Out] IntPtr value2, [Out] IntPtr value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSource3i(ALsource source, ALenum attribute, out int value1, out int value2, out int value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcef(ALsource source, ALenum attribute, out float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcef(ALsource source, ALenum attribute, [Out] float[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcef(ALsource source, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcefv(ALsource source, ALenum attribute, out float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcefv(ALsource source, ALenum attribute, [Out] float[] values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcefv(ALsource source, ALenum attribute, [Out] IntPtr values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcei(ALsource source, ALenum attribute, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcei(ALsource source, ALenum attribute, [Out] int[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourcei(ALsource source, ALenum attribute, [Out] IntPtr val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourceiv(ALsource source, ALenum attribute, out int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourceiv(ALsource source, ALenum attribute, [Out] int[] val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alGetSourceiv(ALsource source, ALenum attribute, [Out] IntPtr val);

        public static UTF8 alGetString1(ALenum state) => MarshalStruct.ReadUtf8(alGetString(state));

        public static UTF8[] alGetStringv(ALenum state) => MarshalStruct.ReadUtf8Strings(alGetString(state));

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alIsBuffer(ALbuffer buffer);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alIsEnabled(ALenum capability);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Ansi, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alIsExtensionPresent(string extensionName);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern int alIsSource(ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListener3f(ALenum attribute, float value1, float value2, float value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListener3i(ALenum attribute, int value1, int value2, int value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListenerf(ALenum attribute, float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListenerfv(ALenum attribute, [In] ref float values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListenerfv(ALenum attribute, [In] float[] values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListenerfv(ALenum attribute, [In] IntPtr values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListeneri(ALenum attribute, int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alListeneriv(ALenum attribute, [In] ref int values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alQueuei(ALsource source, ALenum attribute, int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSource3f(ALsource source, ALenum attribute, float value1, float value2, float value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSource3i(ALsource source, ALenum attribute, int value1, int value2, int value3);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcef(ALsource source, ALenum attribute, float val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcefv(ALsource source, ALenum attribute, [In] ref float values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcefv(ALsource source, ALenum attribute, [In] float[] values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcefv(ALsource source, ALenum attribute, [In] IntPtr values);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcei(ALsource source, ALenum attribute, int val);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcei(ALsource source, ALenum attribute, ALenum val);

        /// <summary>This function pauses a source</summary>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePause(ALsource source);

        /// <summary>This function pauses a set of sources</summary>
        /// <param name="number">the number of sources to be paused</param>
        /// <param name="source">a pointer to an array of sources to be paused</param>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePausev(int number, [In] ref ALsource source);

        /// <summary>This function pauses a set of sources</summary>
        /// <param name="number">the number of sources to be paused</param>
        /// <param name="sources">a pointer to an array of sources to be paused</param>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePausev(int number, [In] ALsource[] sources);

        /// <summary>This function pauses a set of sources</summary>
        /// <param name="number">the number of sources to be paused</param>
        /// <param name="sources">a pointer to an array of sources to be paused</param>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePausev(int number, [In] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePlay(ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePlayv(int number, [In] ref ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePlayv(int number, [In] ALsource[] sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourcePlayv(int number, [In] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceQueueBuffers(ALsource source, int number, [In] ref ALbuffer buffer);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceQueueBuffers(ALsource source, int number, [In] ALbuffer[] buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceQueueBuffers(ALsource source, int number, [In] IntPtr buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceRewind(ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceRewindv(int number, [In] ref ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceRewindv(int number, [In] ALsource[] sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceRewindv(int number, [In] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceStop(ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceStopv(int number, [In] ref ALsource source);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceStopv(int number, [In] ALsource[] sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceStopv(int number, [In] IntPtr sources);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceUnqueueBuffers(ALsource source, int number, [In] ref ALbuffer buffer);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceUnqueueBuffers(ALsource source, int number, [In] ALbuffer[] buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSourceUnqueueBuffers(ALsource source, int number, [In] IntPtr buffers);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        [SuppressUnmanagedCodeSecurity]
        public static extern void alSpeedOfSound(float val);

        #endregion Public Methods
    }

    #endregion Internal Classes

    #region Public Fields

    /// <summary>The global open al synchronize root.</summary>
    public static readonly object SyncRoot = new object();

    #endregion Public Fields

    #region Public Properties

    /// <summary>
    /// The number buffers per second to use when writing to the device. Higher numbers increase cpu usage but increase the accuracy and decrease the device
    /// latency. It is recommended not to use more than 100 buffers per second. (May result in cracks and clicks at the output.)
    /// </summary>
    public static int BuffersPerSecond { get; set; } = 10;

    #endregion Public Properties

    #region Public Methods

    /// <summary>Retrieves a valid AL_FORMAT_* from the specified AudioConfiguration.</summary>
    /// <param name="config"></param>
    /// <returns></returns>
    public static ALenum AL_FORMAT(IAudioConfiguration config)
    {
        switch (config.Format)
        {
            case AudioSampleFormat.Int16:
                switch (config.ChannelSetup)
                {
                    case AudioChannelSetup.Mono: return ALenum.AL_FORMAT_MONO16;
                    case AudioChannelSetup.Stereo: return ALenum.AL_FORMAT_STEREO16;
                    default: throw new NotSupportedException("Unsupported channel setup!");
                }

            case AudioSampleFormat.Float:
                switch (config.ChannelSetup)
                {
                    case AudioChannelSetup.Mono: return ALenum.AL_FORMAT_MONO_FLOAT32;
                    case AudioChannelSetup.Stereo: return ALenum.AL_FORMAT_STEREO_FLOAT32;
                    default: throw new NotSupportedException("Unsupported channel setup!");
                }

            default: throw new NotSupportedException(string.Format("Unsupported sample format {0}!", config.Format));
        }
    }

    /// <summary>Checks for an OpenAL error and thows an appropriate exception if an error occured.</summary>
    public static void CheckError()
    {
        var error = SafeNativeMethods.alGetError();
        switch (error)
        {
            case ALenum.AL_NO_ERROR:
                return;

            case ALenum.AL_INVALID_ENUM:
                throw new InvalidOperationException("OpenAL: Invalid enum passed !");
            case ALenum.AL_INVALID_NAME:
                throw new InvalidOperationException("OpenAL: Invalid name passed !");
            case ALenum.AL_INVALID_VALUE:
                throw new InvalidOperationException("OpenAL: Invalid value passed !");
            case ALenum.AL_INVALID_OPERATION:
                throw new InvalidOperationException("OpenAL: Requested operation is invalid !");
            case ALenum.AL_OUT_OF_MEMORY:
                throw new InvalidOperationException("OpenAL: Out of memory !");
            default:
                throw new InvalidOperationException($"Unknown OpenAL error {error}!");
        }
    }

    #endregion Public Methods
}
