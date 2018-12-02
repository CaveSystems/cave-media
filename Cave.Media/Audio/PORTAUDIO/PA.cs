#region License PortAudio
/*
    PortAudio Portable Real-Time Audio Library
    Copyright(c) 1999-2011 Ross Bencina and Phil Burk
*/
#endregion

using System;
using System.Runtime.InteropServices;
using System.Security;

#pragma warning disable 1591

namespace Cave.Media.Audio.PORTAUDIO
{
    /// <summary>
    /// Provides direct access to the portaudio functions
    /// </summary>
    internal static class PA
    {
        const string NATIVE_LIBRARY = "portaudio";
        const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

        /// <summary>The global portaudio synchronize root</summary>
        public static readonly object SyncRoot = new object();

        /// <summary>
        /// The number buffers per second to use when writing to the device. 
        /// Higher numbers increase cpu usage but increase the accuracy, too and decrease the device latency.
        /// It is recommended not to use more than 100 buffers per second. (May result in cracks and clicks at the output.)
        /// </summary>
        public static int BuffersPerSecond = 10;

        public const int FormatIsSupported = 0;

        public const int FramesPerBufferUnspecified = 0;

        #region callbacks

        [UnmanagedFunctionPointer(CallingConvention.Cdecl)]
        internal delegate PAStreamCallbackResult StreamCallbackDelegate(IntPtr input, IntPtr output, uint frameCount, ref PAStreamCallbackTimeInfo timeInfo, PAStreamCallbackFlags statusFlags, IntPtr userData);

        internal delegate void StreamFinishedCallbackDelegate(IntPtr userData);

        #endregion

        #region interopped functions

        public static string VersionText
        {
            get
            {
                return Marshal.PtrToStringAnsi(SafeNativeMethods.Pa_GetVersionText());
            }
        }

        public static string GetErrorText(PAErrorCode errorCode)
        {
            return Marshal.PtrToStringAnsi(SafeNativeMethods.Pa_GetErrorText(errorCode));
        }


        public static PAHostApiInfo GetHostApiInfo(int hostApi)
        {
            return (PAHostApiInfo)Marshal.PtrToStructure(SafeNativeMethods.Pa_GetHostApiInfo(hostApi), typeof(PAHostApiInfo));
        }

        public static PAHostErrorInfo LastHostErrorInfo
        {
            get
            {
                return (PAHostErrorInfo)Marshal.PtrToStructure(SafeNativeMethods.Pa_GetLastHostErrorInfo(), typeof(PAHostErrorInfo));
            }
        }

        public static PADeviceInfo GetDeviceInfo(int dev)
        {
            IntPtr value = SafeNativeMethods.Pa_GetDeviceInfo(dev);
            return (PADeviceInfo)Marshal.PtrToStructure(value, typeof(PADeviceInfo));
        }

        public static PAStreamInfo GetStreamInfo(IntPtr stream)
        {
            return (PAStreamInfo)Marshal.PtrToStructure(SafeNativeMethods.Pa_GetStreamInfo(stream), typeof(PAStreamInfo));
        }

        #endregion

        [SuppressUnmanagedCodeSecurity]
        internal static class SafeNativeMethods
        {
            #region function imports

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetVersion();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetVersionText();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetErrorText(PAErrorCode errorCode);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_Initialize();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_Terminate();


            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetHostApiCount();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetDefaultHostApi();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetHostApiInfo(int hostApi);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_HostApiTypeIdToHostApiIndex(PAHostApiTypeId type);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_HostApiDeviceIndexToDeviceIndex(int hostApi, int hostApiDeviceIndex);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetLastHostErrorInfo();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetDeviceCount();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetDefaultInputDevice();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetDefaultOutputDevice();

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetDeviceInfo(int dev);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_IsFormatSupported(ref PAStreamParameters inputParameters, ref PAStreamParameters outputParameters, double sampleRate);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_OpenStream(out IntPtr stream, ref PAStreamParameters inputParameters, ref PAStreamParameters outputParameters, double sampleRate, uint framesPerBuffer, PAStreamFlags streamFlags, StreamCallbackDelegate streamCallback, IntPtr userData);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_OpenStream(out IntPtr stream, IntPtr inputParameters, ref PAStreamParameters outputParameters, double sampleRate, uint framesPerBuffer, PAStreamFlags streamFlags, StreamCallbackDelegate streamCallback, IntPtr userData);
            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_OpenStream(out IntPtr stream, ref PAStreamParameters inputParameters, IntPtr outputParameters, double sampleRate, uint framesPerBuffer, PAStreamFlags streamFlags, StreamCallbackDelegate streamCallback, IntPtr userData);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_OpenDefaultStream(out IntPtr stream, int numInputChannels, int numOutputChannels, uint sampleFormat, double sampleRate, uint framesPerBuffer, StreamCallbackDelegate streamCallback, IntPtr userData);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_CloseStream(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_SetStreamFinishedCallback(ref IntPtr stream, [MarshalAs(UnmanagedType.FunctionPtr)]StreamFinishedCallbackDelegate streamFinishedCallback);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_StartStream(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_StopStream(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_AbortStream(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_IsStreamStopped(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_IsStreamActive(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern IntPtr Pa_GetStreamInfo(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern double Pa_GetStreamTime(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern double Pa_GetStreamCpuLoad(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_ReadStream(IntPtr stream, [In] byte[] buffer, uint frames);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_WriteStream(IntPtr stream, [Out] byte[] buffer, uint frames);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetStreamReadAvailable(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern int Pa_GetStreamWriteAvailable(IntPtr stream);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern PAErrorCode Pa_GetSampleSize(PASampleFormat format);

            [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION), SuppressUnmanagedCodeSecurity]
            public static extern void Pa_Sleep(int mss);

            #endregion
        }
    }
}

#pragma warning restore 1591