using System;
namespace Cave.Media.Audio.ID3;

/// <summary>
/// Flags for ID3v2.4 Frames<br />
/// If an unknown flag is set in the first byte the frame may not be changed without the bit cleared.
/// </summary>
[Flags]
public enum ID3v2d4FrameFlags : ushort
{
    /// <summary>
    /// No flags
    /// </summary>
    None = 0,

    /// <summary>
    /// All possible flags
    /// </summary>
    All = TagAlterPreservation | FileAlterPreservation | ReadOnly | GroupingIdentity | Compression | Encryption | Unsynchronisation | DataLengthIndicator,

    /// <summary>
    /// <para>
    /// 0: There is no Data Length Indicator.
    /// 1: A data length Indicator has been added to the frame.
    /// </para>
    /// This flag indicates that a data length indicator has been added to
    /// the frame. The data length indicator is the value one would write
    /// as the 'Frame length' if all of the frame format flags were
    /// zeroed, represented as a 32 bit synchsafe integer.
    /// </summary>
    DataLengthIndicator = 0x01,

    /// <summary>
    /// <para>
    /// 0: Frame has not been unsynchronised.
    /// 1: Frame has been unsyrchronised.
    /// </para>
    /// This flag indicates whether or not unsynchronisation was applied
    /// to this frame. See section 6 for details on unsynchronisation.
    /// If this flag is set all data from the end of this header to the
    /// end of this frame has been unsynchronised. Although desirable, the
    /// presence of a 'Data Length Indicator' is not made mandatory by
    /// unsynchronisation.
    /// </summary>
    Unsynchronisation = 0x02,

    /// <summary>
    /// <para>
    /// 0: Frame is not encrypted.<br/>
    /// 1: Frame is encrypted.
    /// </para>
    /// This flag indicates wether or not the frame is enrypted. If set
    /// one byte indicating with which method it was encrypted will be
    /// appended to the frame header. See section 4.26. for more
    /// information about encryption method registration.
    /// </summary>
    Encryption = 0x04,

    /// <summary>
    /// <para>
    /// 0: Frame is not compressed.<br/>
    /// 1: Frame is compressed using zlib [zlib] with 4 bytes for
    /// 'decompressed size' appended to the frame header.
    /// </para>
    /// This flag indicates whether or not the frame is compressed.
    /// </summary>
    Compression = 0x08,

    /// <summary>
    /// <para>
    /// 0: Frame is not compressed.<br/>
    /// 1: Frame is compressed using zlib [zlib] with 4 bytes for
    /// 'decompressed size' appended to the frame header.
    /// </para>
    /// This flag indicates whether or not the frame is compressed.
    /// </summary>
    GroupingIdentity = 0x40,

    /// <summary>
    /// This flag, if set, tells the software that the contents of this
    /// frame are intended to be read only. Changing the contents might
    /// break something, e.g. a signature. If the contents are changed,
    /// without knowledge of why the frame was flagged read only and
    /// without taking the proper means to compensate, e.g. recalculating
    /// the signature, the bit MUST be cleared.
    /// </summary>
    ReadOnly = 0x1000,

    /// <summary>
    /// <para>
    /// 0: Frame should be preserved.<br />
    /// 1: Frame should be discarded.
    /// </para>
    /// This flag tells the tag parser what to do with this frame if it is
    /// unknown and the file, excluding the tag, is altered. This does not
    /// apply when the audio is completely replaced with other audio data.
    /// </summary>
    FileAlterPreservation = 0x2000,

    /// <summary>
    /// <para>
    /// 0: Frame should be preserved.<br />
    /// 1: Frame should be discarded.
    /// </para>
    /// This flag tells the tag parser what to do with this frame if it is
    /// unknown and the tag is altered in any way. This applies to all
    /// kinds of alterations, including adding more padding and reordering
    /// the frames.
    /// </summary>
    TagAlterPreservation = 0x4000,
}
