using System;

namespace Cave.Media.Structs;

/// <summary>
/// Flags for AVI files.
/// </summary>
[Flags]
public enum AVIFLAGS : int
{
    /// <summary>
    /// AVI has an index.
    /// </summary>
    HASINDEX = 0x00000010,

    /// <summary>
    /// Indicates the index should be used to determine the order of the presentation of the data. When this flag is set, it implies the physical ordering of the chunks in the file does not correspond to the presentation order.
    /// </summary>
    MUSTUSEINDEX = 0x00000020,

    /// <summary>
    /// Indicates the AVI file has been interleaved. The system can stream interleaved data from a CD-ROM more efficiently than non-interleaved data.
    /// </summary>
    ISINTERLEAVED = 0x00000100,

    /// <summary>
    /// Unknown flags, information welcome, mailto:info@caveprojects.org
    /// </summary>
    TRUSTCKTYPE = 0x00000800,

    /// <summary>
    /// Indicates the AVI file contains copyrighted data.  When this flag is set, applications should not let users duplicate the file or the data in the file.
    /// </summary>
    COPYRIGHTED = 0x00010000,

    /// <summary>
    /// Indicates the AVI file is a specially allocated file used for capturing real-time video. Typically, capture files have been defragmented by the user so video capture data can be efficiently streamed into the file. If this flag is set, an application should warn the user before writing over the file.
    /// </summary>
    WASCAPTUREFILE = 0x00020000,
}
