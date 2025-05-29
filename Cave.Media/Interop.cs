#pragma warning disable CS1591

using System;
using System.Runtime.CompilerServices;
using System.Runtime.InteropServices;

namespace Cave.Media;

/// <summary>Provides native interop functions</summary>
public static class Interop
{
    #region Public Classes

    public static class SafeNativeMethods
    {
        #region Public Methods

        [DllImport("msvcrt", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, int count);

        [DllImport("msvcrt", EntryPoint = "memcpy", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static unsafe extern IntPtr memcpy(byte* dest, byte* src, int count);

        [DllImport("msvcrt", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static extern IntPtr memset(IntPtr dest, int c, int count);

        [DllImport("msvcrt", EntryPoint = "memset", CallingConvention = CallingConvention.Cdecl, SetLastError = false)]
        public static unsafe extern IntPtr memset(int* dest, int c, int count);

        #endregion Public Methods
    }

    #endregion Public Classes
}
