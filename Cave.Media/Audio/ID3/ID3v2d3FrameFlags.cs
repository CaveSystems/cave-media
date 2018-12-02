using System;
namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Flags for ID3v2.3 Frames<br />
    /// If an unknown flag is set in the first byte the frame may not be changed without the bit cleared.
    /// </summary>
    
    [Flags]
    public enum ID3v2d3FrameFlags : ushort
    {
        /// <summary>
        /// No flags
        /// </summary>
        None = 0,

        /// <summary>
        /// All possible flags
        /// </summary>
        All = TagAlterPreservation | FileAlterPreservation | ReadOnly | Compression | Encryption | GroupingIdentity,

        /// <summary>
        /// <para>
        /// 0: Frame does not contain group information<br/>
        /// 1: Frame contains group information
        /// </para>
        /// This flag indicates whether or not this frame belongs in a group
        /// with other frames. If set a group identifier byte is added to the
        /// frame header. Every frame with the same group identifier belongs
        /// to the same group.
        /// </summary>
        GroupingIdentity = 0x20,

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
        Encryption = 0x40,

        /// <summary>
        /// <para>
        /// 0: Frame is not compressed.<br/>
        /// 1: Frame is compressed using zlib [zlib] with 4 bytes for
        /// 'decompressed size' appended to the frame header.
        /// </para>
        /// This flag indicates whether or not the frame is compressed.
        /// </summary>
        Compression = 0x80,

        /// <summary>
        /// This flag, if set, tells the software that the contents of this
        /// frame are intended to be read only. Changing the contents might
        /// break something, e.g. a signature. If the contents are changed,
        /// without knowledge of why the frame was flagged read only and
        /// without taking the proper means to compensate, e.g. recalculating
        /// the signature, the bit MUST be cleared.
        /// </summary>
        ReadOnly = 0x2000,

        /// <summary>
        /// <para>
        /// 0: Frame should be preserved.<br />
        /// 1: Frame should be discarded.
        /// </para>
        /// This flag tells the tag parser what to do with this frame if it is
        /// unknown and the file, excluding the tag, is altered. This does not
        /// apply when the audio is completely replaced with other audio data.
        /// </summary>
        FileAlterPreservation = 0x4000,

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
        TagAlterPreservation = 0x8000,
    }
}
