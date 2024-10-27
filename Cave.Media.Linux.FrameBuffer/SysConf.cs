// https://github.com/torvalds/linux/blob/master/include/linux/fb.h

using System;

namespace Cave.Media.Linux;

/// <summary>
/// Provides all _SC_ constants
/// </summary>
public enum SysConf
{
    /// <summary>
    /// Maximum bytes of argument to exec().
    /// </summary>
    ARG_MAX,

    /// <summary>
    /// Maximum number of simultaneous processes per user ID.
    /// </summary>
    CHILD_MAX,

    /// <summary>
    /// Number of yiffies per second
    /// </summary>
    CLK_TCK,

    /// <summary>
    /// Maximum number of supplemental groups.
    /// </summary>
    NGROUPS_MAX,

    /// <summary>
    /// Maximum number of open files per process.
    /// </summary>
    OPEN_MAX,

    /// <summary>
    /// indicates the maximum number of streams that a process can have open simultaneously. 
    /// </summary>
    STREAM_MAX,

    /// <summary>
    /// indicates the maximum length of the name of a time zone. 
    /// </summary>
    TZNAME_MAX,

    /// <summary>
    /// Return 1 if job control is available on this system, otherwise -1.
    /// </summary>
    JOB_CONTROL,

    /// <summary>
    /// Returns 1 if saved set-group and saved set-user ID is available, otherwise -1.
    /// </summary>
    SAVED_IDS,

    /// <summary>
    /// bool: Supports Realtime Signals
    /// </summary>
    REALTIME_SIGNALS,

    /// <summary>
    /// bool: Supports Process Scheduling
    /// </summary>
    PRIORITY_SCHEDULING,

    /// <summary>
    /// bool: Supports Timers
    /// </summary>
    TIMERS,

    /// <summary>
    /// bool: Supports Asynchronous I/O
    /// </summary>
    ASYNCHRONOUS_IO,

    /// <summary>
    /// bool: Supports Prioritized I/O
    /// </summary>
    PRIORITIZED_IO,

    /// <summary>
    /// bool: Supports Synchronized I/O
    /// </summary>
    SYNCHRONIZED_IO,

    /// <summary>
    /// bool: Supports File Synchronization
    /// </summary>
    FSYNC,

    /// <summary>
    /// bool: Supports Memory Mapped Files
    /// </summary>
    MAPPED_FILES,

    /// <summary>
    /// bool: Supports Process Memory Locking
    /// </summary>
    MEMLOCK,

    /// <summary>
    /// bool: Supports Range Memory Locking
    /// </summary>
    MEMLOCK_RANGE,

    /// <summary>
    /// bool: Supports Memory Protection
    /// </summary>
    MEMORY_PROTECTION,

    /// <summary>
    /// bool: Supports Message Passing
    /// </summary>
    MESSAGE_PASSING,

    /// <summary>
    /// Max number of POSIX semaphores a process can have
    /// </summary>
    SEMAPHORES,

    /// <summary>
    /// Max number of shared memory objects a process can have
    /// </summary>
    SHARED_MEMORY_OBJECTS,

    /// <summary>
    /// Max number of I/O operations in a single list I/O call supported
    /// </summary>
    AIO_LISTIO_MAX,

    /// <summary>
    /// Max number of outstanding asynchronous I/O operations supported
    /// </summary>
    AIO_MAX,

    /// <summary>
    /// Max amount by which process can decrease its asynchronous I/O priority level from its own scheduling priority
    /// </summary>
    AIO_PRIO_DELTA_MAX,

    /// <summary>
    /// Max number of timer expiration overruns
    /// </summary>
    DELAYTIMER_MAX,

    /// <summary>
    /// Max number of open message queues a process can hold
    /// </summary>
    MQ_OPEN_MAX,

    /// <summary>
    /// Max number of message priorities supported
    /// </summary>
    MQ_PRIO_MAX,

    /// <summary>
    /// The version of ISO/IEC 9945 (POSIX 1003.1) with which the system attempts to comply.
    /// </summary>
    VERSION,

    /// <summary>
    /// The virtual memory page size.
    /// </summary>
    PAGESIZE,

    /// <summary>
    /// Max number of realtime signals reserved for application use
    /// </summary>
    RTSIG_MAX,

    /// <summary>
    /// Max number of POSIX semaphores a process can have
    /// </summary>
    SEM_NSEMS_MAX,

    /// <summary>
    /// Max value a POSIX semaphore can have
    /// </summary>
    SEM_VALUE_MAX,

    /// <summary>
    /// Max number of queued signals that a  process can send and have pending at receiver(s) at a time
    /// </summary>
    SIGQUEUE_MAX,

    /// <summary>
    /// Max number of timer per process supported
    /// </summary>
    TIMER_MAX,

    /// <summary>
    /// Maximum obase values allowed by bc
    /// </summary>
    BC_BASE_MAX,

    /// <summary>
    /// Max number of elements permitted in array by bc
    /// </summary>
    BC_DIM_MAX,

    /// <summary>
    /// Max scale value allowed by bc
    /// </summary>
    BC_SCALE_MAX,

    /// <summary>
    /// Max length of string constant allowed by bc
    /// </summary>
    BC_STRING_MAX,

    /// <summary>
    /// Max number of weights that can be assigned to entry of the LC_COLLATE order keyword in locale definition file
    /// </summary>
    COLL_WEIGHTS_MAX,

    /// <summary>
    /// Inquire about the maximum number of weights that can be assigned to an entry of the LC_COLLATE category `order' keyword in a locale definition. The GNU C library does not presently support locale definitions. 
    /// </summary>
    EQUIV_CLASS_MAX,

    /// <summary>
    /// Max number of parentheses by expr
    /// </summary>
    EXPR_NEST_MAX,

    /// <summary>
    /// Max length of input line
    /// </summary>
    LINE_MAX,

    /// <summary>
    /// Max number of repeated occurrences of a regular expression permitted when using interval notation \{m,n\}
    /// </summary>
    RE_DUP_MAX,

    /// <summary>
    /// Inquire about the parameter corresponding to maximal length allowed for a character class name in an extended locale specification. These extensions are not yet standardized and so this option is not standardized as well. 
    /// </summary>
    CHARCLASS_NAME_MAX,

    /// <summary>
    /// Integer value indicates version of ISO POSIX-2 standard (C language binding)
    /// </summary>
    POSIX2_VERSION,

    /// <summary>
    /// Supports the C language binding option
    /// </summary>
    POSIX2_C_BIND,

    /// <summary>
    /// Supports the C language development utilities option
    /// </summary>
    POSIX2_C_DEV,

    /// <summary>
    /// Supports FORTRAN Development Utilities Option
    /// </summary>
    POSIX2_FORT_DEV,

    /// <summary>
    /// Supports FORTRAN Run-time Utilities Option
    /// </summary>
    POSIX2_FORT_RUN,

    /// <summary>
    /// Supports Software Development Utility Option
    /// </summary>
    POSIX2_SW_DEV,

    /// <summary>
    /// Supports creation of locales by the localedef utility
    /// </summary>
    POSIX2_LOCALEDEF,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII. 
    /// </summary>
    PII,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_XTI. 
    /// </summary>
    PII_XTI,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_SOCKET. 
    /// </summary>
    PII_SOCKET,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_INTERNET. 
    /// </summary>
    PII_INTERNET,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_OSI. 
    /// </summary>
    PII_OSI,
    POLL,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_SELECT. 
    /// </summary>
    SELECT,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_UIO_MAXIOV. 
    /// </summary>
    UIO_MAXIOV,

    /// <summary>
    /// Inquire the value of the value associated with the IOV_MAX variable. 
    /// </summary>
    IOV_MAX = UIO_MAXIOV,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_INTERNET_STREAM. 
    /// </summary>
    PII_INTERNET_STREAM,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_INTERNET_DGRAM. 
    /// </summary>
    PII_INTERNET_DGRAM,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_OSI_COTS. 
    /// </summary>
    PII_OSI_COTS,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_OSI_CLTS. 
    /// </summary>
    PII_OSI_CLTS,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_PII_OSI_M. 
    /// </summary>
    PII_OSI_M,

    /// <summary>
    /// Inquire the value of the value associated with the T_IOV_MAX variable. 
    /// </summary>
    T_IOV_MAX,

    /// <summary>
    /// Max number of threads per process
    /// </summary>
    THREADS,

    /// <summary>
    /// Inquire about the parameter corresponding to POSIX_THREAD_SAFE_FUNCTIONS. 
    /// </summary>
    THREAD_SAFE_FUNCTIONS,

    /// <summary>
    /// Max size of group entry buffer
    /// </summary>
    GETGR_R_SIZE_MAX,

    /// <summary>
    /// Max size of password entry buffer
    /// </summary>
    GETPW_R_SIZE_MAX,

    /// <summary>
    /// Max length of login name
    /// </summary>
    LOGIN_NAME_MAX,

    /// <summary>
    /// Max length of tty device name
    /// </summary>
    TTY_NAME_MAX,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_DESTRUCTOR_ITERATIONS. 
    /// </summary>
    THREAD_DESTRUCTOR_ITERATIONS,

    /// <summary>
    /// Max number of data keys per process
    /// </summary>
    THREAD_KEYS_MAX,

    /// <summary>
    /// Min byte size of thread stack storage
    /// </summary>
    THREAD_STACK_MIN,

    /// <summary>
    /// Max number of threads per process
    /// </summary>
    THREAD_THREADS_MAX,

    /// <summary>
    /// Inquire about the parameter corresponding to a _POSIX_THREAD_ATTR_STACKADDR. 
    /// </summary>
    THREAD_ATTR_STACKADDR,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_ATTR_STACKSIZE. 
    /// </summary>
    THREAD_ATTR_STACKSIZE,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_PRIORITY_SCHEDULING. 
    /// </summary>
    THREAD_PRIORITY_SCHEDULING,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_PRIO_INHERIT. 
    /// </summary>
    THREAD_PRIO_INHERIT,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_PRIO_PROTECT. 
    /// </summary>
    THREAD_PRIO_PROTECT,

    /// <summary>
    /// Inquire about the parameter corresponding to _POSIX_THREAD_PROCESS_SHARED. 
    /// </summary>
    THREAD_PROCESS_SHARED,

    /// <summary>
    /// Number of processors configured
    /// </summary>
    NPROCESSORS_CONF,

    /// <summary>
    /// Number of processors online
    /// </summary>
    NPROCESSORS_ONLN,

    /// <summary>
    /// Total number of pages of physical memory in system
    /// </summary>
    PHYS_PAGES,

    /// <summary>
    /// Number of physical memory pages not currently in use by system
    /// </summary>
    AVPHYS_PAGES,

    /// <summary>
    /// Max number of functions that can be registered with atexit()
    /// </summary>
    ATEXIT_MAX,

    /// <summary>
    /// Max number of significant bytes in a password
    /// </summary>
    PASS_MAX,

    /// <summary>
    /// Integer value indicates version of X/Open Portability Guide to which implementation conforms
    /// </summary>
    XOPEN_VERSION,

    /// <summary>
    /// Integer value indicates version of XCU specification to which implementation conforms
    /// </summary>
    XOPEN_XCU_VERSION,

    /// <summary>
    /// Supports X/Open CAE Specification, August 1994, System Interfaces and Headers, Issue 4, Version 2
    /// </summary>
    XOPEN_UNIX,

    /// <summary>
    /// Supports X/Open Encryption Feature Group
    /// </summary>
    XOPEN_CRYPT,

    /// <summary>
    /// Supports X/Open Enhanced Internationalization Feature Group
    /// </summary>
    XOPEN_ENH_I18N,

    /// <summary>
    /// Supports X/Open Shared Memory Feature Group
    /// </summary>
    XOPEN_SHM,

    /// <summary>
    /// Supports at least one terminal
    /// </summary>
    POSIX2_CHAR_TERM,

    /// <summary>
    /// Integer value indicates version of ISO POSIX-2 standard (Commands)
    /// </summary>
    POSIX2_C_VERSION,

    /// <summary>
    /// Supports User Portability Utilities Option
    /// </summary>
    POSIX2_UPE,

    /// <summary>
    /// Inquire about the parameter corresponding to _XOPEN_XPG2. 
    /// </summary>
    XOPEN_XPG2,

    /// <summary>
    /// Inquire about the parameter corresponding to _XOPEN_XPG3. 
    /// </summary>
    XOPEN_XPG3,

    /// <summary>
    /// Inquire about the parameter corresponding to _XOPEN_XPG4. 
    /// </summary>
    XOPEN_XPG4,

    /// <summary>
    /// Inquire about the number of bits in a variable of type char. 
    /// </summary>
    CHAR_BIT,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type char. 
    /// </summary>
    CHAR_MAX,

    /// <summary>
    /// Inquire about the minimum value which can be stored in a variable of type char. 
    /// </summary>
    CHAR_MIN,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type int. 
    /// </summary>
    INT_MAX,

    /// <summary>
    /// Inquire about the minimum value which can be stored in a variable of type int. 
    /// </summary>
    INT_MIN,

    /// <summary>
    /// Inquire about the number of bits in a variable of type long int. 
    /// </summary>
    LONG_BIT,

    /// <summary>
    /// Inquire about the number of bits in a variable of a register word. 
    /// </summary>
    WORD_BIT,

    /// <summary>
    /// Inquire the maximum length of a multi-byte representation of a wide character value. 
    /// </summary>
    MB_LEN_MAX,

    /// <summary>
    /// Inquire about the value used to internally represent the zero priority level for the process execution. 
    /// </summary>
    NZERO,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type ssize_t. 
    /// </summary>
    SSIZE_MAX,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type signed char. 
    /// </summary>
    SCHAR_MAX,

    /// <summary>
    /// Inquire about the minimum value which can be stored in a variable of type signed char. 
    /// </summary>
    SCHAR_MIN,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type short int. 
    /// </summary>
    SHRT_MAX,

    /// <summary>
    /// Inquire about the minimum value which can be stored in a variable of type short int. 
    /// </summary>
    SHRT_MIN,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type unsigned char. 
    /// </summary>
    UCHAR_MAX,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type unsigned int. 
    /// </summary>
    UINT_MAX,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type unsigned long int. 
    /// </summary>
    ULONG_MAX,

    /// <summary>
    /// Inquire about the maximum value which can be stored in a variable of type unsigned short int. 
    /// </summary>
    USHRT_MAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NaRGMAX. 
    /// </summary>
    NaRGMAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NL_LANGMAX. 
    /// </summary>
    NL_LANGMAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NL_MSGMAX. 
    /// </summary>
    NL_MSGMAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NL_NMAX. 
    /// </summary>
    NL_NMAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NL_SETMAX. 
    /// </summary>
    NL_SETMAX,

    /// <summary>
    /// Inquire about the parameter corresponding to NtextMAX. 
    /// </summary>
    NtextMAX,

    /// <summary>
    /// If sysconf(_SC_XBS5_ILP32_OFF32) returns -1 the meaning of this value is un specified . Otherwise, this value is the set of initial options to be specified to the cc and c89 utilities to build an application using a programming model with 32-bit int, long, pointer, and off_t types.
    /// </summary>
    XBS5_ILP32_OFF32,

    /// <summary>
    /// If sysconf(_SC_XBS5_ILP32_OFFBIG) returns -1 the meaning of this value is un specified . Otherwise, this value is the set of initial options to be specified to the cc and c89 utilities to build an application using a programming model with 32-bit int, long, and pointer types, and an off_t type using at least 64 bits.
    /// </summary>
    XBS5_ILP32_OFFBIG,

    /// <summary>
    /// If sysconf(_SC_XBS5_LP64_OFF64) returns -1 the meaning of this value is un specified . Otherwise, this value is the set of initial options to be specified to the cc and c89 utilities to build an application using a programming model with 64-bit int, long, pointer, and off_t types.
    /// </summary>
    XBS5_LP64_OFF64,

    /// <summary>
    /// If sysconf(_SC_XBS5_LPBIG_OFFBIG) returns -1 the meaning of this value is un specified . Otherwise, this value is the set of initial options to be specified to the cc and c89 utilities to build an application using a programming model with an int type using at least 32 bits and long, pointer, and off_t types using at least 64 bits.
    /// </summary>
    XBS5_LPBIG_OFFBIG,

    /// <summary>
    /// Inquire about the parameter corresponding to _XOPEN_LEGACY. 
    /// </summary>
    XOPEN_LEGACY,

    /// <summary>
    /// Supports X/Open POSIX Realtime Feature Group
    /// </summary>
    XOPEN_REALTIME,

    /// <summary>
    /// Supports X/Open POSIX Reatime Threads Feature Group
    /// </summary>
    XOPEN_REALTIME_THREADS,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    ADVISORY_INFO,

    /// <summary>
    /// Supports Barriers option
    /// </summary>
    BARRIERS,
    BASE,

    C_LANG_SUPPORT,
    C_LANG_SUPPORT_R,

    /// <summary>
    /// Supports Clock Selection option
    /// </summary>
    CLOCK_SELECTION,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    CPUTIME,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    THREAD_CPUTIME,

    DEVICE_IO,
    DEVICE_SPECIFIC,
    DEVICE_SPECIFIC_R,
    FD_MGMT,
    FIFO,
    PIPE,
    FILE_ATTRIBUTES,
    FILE_LOCKING,
    FILE_SYSTEM,
    MONOTONIC_CLOCK,
    MULTI_PROCESS,
    SINGLE_PROCESS,
    NETWORKING,
    READER_WRITER_LOCKS,
    SPIN_LOCKS,

    /// <summary>
    /// Supports Regular Expression Handling option
    /// </summary>
    REGEXP,
    REGEX_VERSION,
    SHELL,
    SIGNALS,

    /// <summary>
    /// This option describes support for process creation in a context where it is difficult or impossible to use fork(), for example, because no MMU is present. 
    /// If _POSIX_SPAWN is in effect, then the include file spawn.h and the following functions are present: 
    /// </summary>
    SPAWN,

    /// <summary>
    /// The scheduling policy SCHED_SPORADIC is supported. This option implies the _POSIX_PRIORITY_SCHEDULING option
    /// </summary>
    SPORADIC_SERVER,

    /// <summary>
    /// This option implies the _POSIX_THREAD_PRIORITY_SCHEDULING option.
    /// </summary>
    THREAD_SPORADIC_SERVER,
    SYSTEM_DATABASE,
    SYSTEM_DATABASE_R,
    TIMEOUTS,
    TYPED_MEMORY_OBJECTS,
    USER_GROUPS,
    USER_GROUPS_R,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS_ACCOUNTING,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS_LOCATE,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS_MESSAGE,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS_TRACK,

    /// <summary>
    /// Max number of symbolic links that can be reliably traversed in the resolution of a pathname in the absence of a loop
    /// </summary>
    SYMLOOP_MAX,
    STREAMS,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    POSIX2_PBS_CHECKPOINT,

    /// <summary>
    /// Supports  X/Open ILP32 w/32-bit offset build environment
    /// </summary>
    V6_ILP32_OFF32,

    /// <summary>
    /// Supports  X/Open ILP32 w/64-bit offset build environment
    /// </summary>
    V6_ILP32_OFFBIG,

    /// <summary>
    /// Supports  X/Open LP64 w/64-bit offset build environment
    /// </summary>
    V6_LP64_OFF64,

    /// <summary>
    /// Same as V6_LP64_OFF64
    /// </summary>
    V6_LPBIG_OFFBIG,

    /// <summary>
    /// Maximum length of a host name (excluding terminating null)
    /// </summary>
    HOST_NAME_MAX,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    TRACE,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    TRACE_EVENT_FILTER,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    TRACE_INHERIT,

    /// <summary>
    /// No longer supported
    /// </summary>
    [Obsolete]
    TRACE_LOG,

    LEVEL1_ICACHE_SIZE,
    LEVEL1_ICACHE_ASSOC,
    LEVEL1_ICACHE_LINESIZE,
    LEVEL1_DCACHE_SIZE,
    LEVEL1_DCACHE_ASSOC,
    LEVEL1_DCACHE_LINESIZE,
    LEVEL2_CACHE_SIZE,
    LEVEL2_CACHE_ASSOC,
    LEVEL2_CACHE_LINESIZE,
    LEVEL3_CACHE_SIZE,
    LEVEL3_CACHE_ASSOC,
    LEVEL3_CACHE_LINESIZE,
    LEVEL4_CACHE_SIZE,
    LEVEL4_CACHE_ASSOC,
    LEVEL4_CACHE_LINESIZE
}
