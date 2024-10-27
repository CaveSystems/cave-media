// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;
using System.Runtime.InteropServices;
using System.Security;

namespace Cave.Media.Linux;

public static class libc
{
    #region Private Fields

    const CallingConvention CALLING_CONVENTION = CallingConvention.Cdecl;

    /// <summary>The native library name</summary>
    const string NATIVE_LIBRARY = "libc";

    #endregion Private Fields

    #region Internal Classes

    [SuppressUnmanagedCodeSecurity]
    internal static class SafeNativeMethods
    {
        #region Public Methods

        [DllImport("libc", EntryPoint = "__errno_location")]
        public static extern IntPtr __errno_location();

        [DllImport("libc", CallingConvention = CallingConvention.Cdecl)]
        public static extern int clock_gettime(int clockID, IntPtr result);

        /// <summary>Closes the specified handle.</summary>
        /// <param name="handle">The handle.</param>
        /// <returns></returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern int close(int handle);

        /// <summary>
        /// closes the current Syslog connection, if there is one. This includes closing the /dev/log socket, if it is open. closelog also sets the
        /// identification string for Syslog messages back to the default, if openlog was called with a non-NULL argument to ident. The default identification
        /// string is the program name taken from argv[0].
        /// </summary>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern void closelog();

        /// <summary>The ioctl function performs the generic I/O operation command on a given handle.</summary>
        /// <param name="handle">The handle.</param>
        /// <param name="cmd">The command.</param>
        /// <param name="data">The data.</param>
        /// <returns></returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern int ioctl(int handle, int cmd, IntPtr data);

        /// <summary>
        /// This function copies num bytes from source to dest. It assumes that the source and destination regions don't overlap; if you need to copy
        /// overlapping regions, use memmove instead. See section memmove.
        /// </summary>
        /// <param name="dest">The destination.</param>
        /// <param name="src">The source.</param>
        /// <param name="num">The number.</param>
        /// <returns>Returns the given destination pointer</returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern IntPtr memcpy(IntPtr dest, IntPtr src, int num);

        /// <summary>
        /// This function copies num bytes from source to dest. It assumes that the source and destination regions don't overlap; if you need to copy
        /// overlapping regions, use memmove instead. See section memmove.
        /// </summary>
        /// <param name="dest">The destination.</param>
        /// <param name="src">The source.</param>
        /// <param name="num">The number.</param>
        /// <returns>Returns the given destination pointer</returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static unsafe extern IntPtr memcpy(int* dest, int* src, int num);

        /// <summary>map files or devices into memory</summary>
        /// <param name="address">The address.</param>
        /// <param name="length">The length.</param>
        /// <param name="prot">The protection flags.</param>
        /// <param name="flags">The mapping flags.</param>
        /// <param name="fd">The handle.</param>
        /// <param name="offset">The offset.</param>
        /// <returns></returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern IntPtr mmap(IntPtr address, UIntPtr length, PROT prot, MAP flags, int fd, IntPtr offset);

        /// <summary>The open function creates and returns a new file descriptor for the file named by fileName.</summary>
        /// <param name="fileName">The fileName.</param>
        /// <param name="flags">The flags argument controls how the file is to be opened.</param>
        /// <returns>Returns a handle to the opened file</returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern int open(string fileName, int flags);

        /// <summary>opens or reopens a connection to Syslog in preparation for submitting messages.</summary>
        /// <param name="process">
        /// Ident is an arbitrary identification string which future syslog invocations will prefix to each message. This is intended to identify the source of
        /// the message, and people conventionally set it to the name of the program that will submit the messages. Please note that the string pointer ident
        /// will be retained internally by the Syslog routines. You must not free the memory that ident points to. It is also dangerous to pass a reference to
        /// an automatic variable since leaving the scope would mean ending the lifetime of the variable. If you want to change the ident string, you must call
        /// openlog again; overwriting the string pointed to by ident is not thread-safe.
        /// </param>
        /// <param name="option">SyslogOption</param>
        /// <param name="facility">SyslogFacility</param>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern void openlog(IntPtr process, IntPtr option, IntPtr facility);

        /// <summary>
        /// When your system has configurable system limits, you can use the sysconf function to find out the value that applies to any particular machine.
        /// </summary>
        /// <param name="parameter"></param>
        /// <returns></returns>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern long sysconf(SysConf parameter);

        /// <summary>submits a message to the Syslog facility. It does this by writing to the Unix domain socket /dev/log.</summary>
        /// <param name="priority">SyslogPriority</param>
        /// <param name="msg">Message to submit</param>
        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION, CharSet = CharSet.Auto, BestFitMapping = false, ThrowOnUnmappableChar = true)]
        public static extern void syslog(int priority, string msg);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern long time(IntPtr result);

        [DllImport(NATIVE_LIBRARY, CallingConvention = CALLING_CONVENTION)]
        public static extern int uname(IntPtr buf);

        #endregion Public Methods
    }

    #endregion Internal Classes

    #region Public Enums

    /// <summary>Memory page mapping flags</summary>
    [Flags]
    public enum MAP : uint
    {
        /// <summary>Share changes</summary>
        SHARED = 0x001,

        /// <summary>Changes are private</summary>
        PRIVATE = 0x002,

        /// <summary>Mask for type of mapping</summary>
        TYPE = 0x00f,

        /// <summary>Interpret addr exactly</summary>
        FIXED = 0x010,
    }

    /// <summary>Memory page protection flags</summary>
    [Flags]
    public enum PROT : uint
    {
        /// <summary>page can not be accessed</summary>
        NONE = 0x00,

        /// <summary>page can be read</summary>
        READ = 0x01,

        /// <summary>page can be written</summary>
        WRITE = 0x02,

        /// <summary>page can be executed</summary>
        EXEC = 0x04,

        /// <summary>page may be used for atomic ops</summary>
        SEM = 0x10,

        /// <summary>mprotect flag: extend change to start of growsdown vma</summary>
        GROWSDOWN = 0x01000000,

        /// <summary>mprotect flag: extend change to end of growsup vma</summary>
        GROWSUP = 0x02000000,
    }

    #endregion Public Enums

    #region Public Methods

    /// <summary>Gets the name of the unix kernel.</summary>
    /// <returns></returns>
    public static string GetUnixName()
    {
        var buf = Marshal.AllocHGlobal(8192);
        try
        {
            var i = SafeNativeMethods.uname(buf);
            return Marshal.PtrToStringAnsi(buf, i);
        }
        finally
        {
            Marshal.FreeHGlobal(buf);
        }
    }

    public static bool IsTime64()
    {
        var val = new uint[4];
        var handle = GCHandle.Alloc(val, GCHandleType.Pinned);
        SafeNativeMethods.clock_gettime(0, handle.AddrOfPinnedObject());
        handle.Free();
        return val[2] > 0;
    }

    #endregion Public Methods
}
