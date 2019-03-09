namespace Cave.Media.Audio.ID3
{
    /// <summary>
    /// Provides all available Flags for ID3v2 Frames (any version).
    /// </summary>
    public class ID3v2FrameFlags
    {
        /// <summary>
        /// Creates a <see cref="ID3v2FrameFlags"/> instance from the specified <see cref="ID3v2d4FrameFlags"/>.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static ID3v2FrameFlags FromID3v2d4(ID3v2d4FrameFlags flags)
        {
            ID3v2FrameFlags result = new ID3v2FrameFlags();
            result.Compression = (flags & ID3v2d4FrameFlags.Compression) != 0;
            result.DataLengthIndicator = (flags & ID3v2d4FrameFlags.DataLengthIndicator) != 0;
            result.Encryption = (flags & ID3v2d4FrameFlags.Encryption) != 0;
            result.FileAlterPreservation = (flags & ID3v2d4FrameFlags.FileAlterPreservation) != 0;
            result.GroupingIdentity = (flags & ID3v2d4FrameFlags.GroupingIdentity) != 0;
            result.ReadOnly = (flags & ID3v2d4FrameFlags.ReadOnly) != 0;
            result.TagAlterPreservation = (flags & ID3v2d4FrameFlags.TagAlterPreservation) != 0;
            result.Unsynchronisation = (flags & ID3v2d4FrameFlags.Unsynchronisation) != 0;
            return result;
        }

        /// <summary>
        /// Creates a <see cref="ID3v2FrameFlags"/> instance from the specified <see cref="ID3v2d3FrameFlags"/>.
        /// </summary>
        /// <param name="flags"></param>
        /// <returns></returns>
        public static ID3v2FrameFlags FromID3v2d3(ID3v2d3FrameFlags flags)
        {
            ID3v2FrameFlags result = new ID3v2FrameFlags();
            result.Compression = (flags & ID3v2d3FrameFlags.Compression) != 0;
            result.Encryption = (flags & ID3v2d3FrameFlags.Encryption) != 0;
            result.FileAlterPreservation = (flags & ID3v2d3FrameFlags.FileAlterPreservation) != 0;
            result.GroupingIdentity = (flags & ID3v2d3FrameFlags.GroupingIdentity) != 0;
            result.ReadOnly = (flags & ID3v2d3FrameFlags.ReadOnly) != 0;
            result.TagAlterPreservation = (flags & ID3v2d3FrameFlags.TagAlterPreservation) != 0;
            return result;
        }

        /// <summary>Gets a value indicating whether this instance is empty.</summary>
        /// <value><c>true</c> if this instance is empty; otherwise, <c>false</c>.</value>
        public bool IsEmpty
        {
            get
            {
                return !Unsynchronisation
                    && !DataLengthIndicator
                    && !TagAlterPreservation
                    && !FileAlterPreservation
                    && !ReadOnly
                    && !GroupingIdentity
                    && !Encryption
                    && !Compression;
            }
        }

        /// <summary>
        /// <para>
        /// 0: Frame has not been unsynchronised.
        /// 1: Frame has been unsyrchronised.
        /// </para>
        /// [Present since 2.4]<br />
        /// This flag indicates whether or not unsynchronisation was applied
        /// to this frame. See section 6 for details on unsynchronisation.
        /// If this flag is set all data from the end of this header to the
        /// end of this frame has been unsynchronised. Although desirable, the
        /// presence of a 'Data Length Indicator' is not made mandatory by
        /// unsynchronisation.
        /// </summary>
        public bool Unsynchronisation;

        /// <summary>
        /// <para>
        /// 0: There is no Data Length Indicator.
        /// 1: A data length Indicator has been added to the frame.
        /// </para>
        /// [Present since 2.4]<br />
        /// This flag indicates that a data length indicator has been added to
        /// the frame. The data length indicator is the value one would write
        /// as the 'Frame length' if all of the frame format flags were
        /// zeroed, represented as a 32 bit synchsafe integer.
        /// </summary>
        public bool DataLengthIndicator;

        /// <summary>
        /// <para>
        /// 0: Frame should be preserved.<br />
        /// 1: Frame should be discarded.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag tells the tag parser what to do with this frame if it is
        /// unknown and the tag is altered in any way. This applies to all
        /// kinds of alterations, including adding more padding and reordering
        /// the frames.
        /// </summary>
        public bool TagAlterPreservation;

        /// <summary>
        /// <para>
        /// 0: Frame should be preserved.<br />
        /// 1: Frame should be discarded.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag tells the tag parser what to do with this frame if it is
        /// unknown and the file, excluding the tag, is altered. This does not
        /// apply when the audio is completely replaced with other audio data.
        /// </summary>
        public bool FileAlterPreservation;

        /// <summary>
        /// <para>
        /// 0: Frame is read only.<br />
        /// 1: Frame not read only.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag, if set, tells the software that the contents of this
        /// frame are intended to be read only. Changing the contents might
        /// break something, e.g. a signature. If the contents are changed,
        /// without knowledge of why the frame was flagged read only and
        /// without taking the proper means to compensate, e.g. recalculating
        /// the signature, the bit MUST be cleared.
        /// </summary>
        public bool ReadOnly;

        /// <summary>
        /// <para>
        /// 0: Frame is not compressed.<br/>
        /// 1: Frame is compressed using zlib [zlib] with 4 bytes for
        /// 'decompressed size' appended to the frame header.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag indicates whether or not the frame is compressed.
        /// </summary>
        public bool GroupingIdentity;

        /// <summary>
        /// <para>
        /// 0: Frame is not encrypted.<br/>
        /// 1: Frame is encrypted.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag indicates wether or not the frame is enrypted. If set
        /// one byte indicating with which method it was encrypted will be
        /// appended to the frame header. See section 4.26. for more
        /// information about encryption method registration.
        /// </summary>
        public bool Encryption;

        /// <summary>
        /// <para>
        /// 0: Frame is not compressed.<br/>
        /// 1: Frame is compressed using zlib [zlib] with 4 bytes for
        /// 'decompressed size' appended to the frame header.
        /// </para>
        /// [Present since 2.3]<br />
        /// This flag indicates whether or not the frame is compressed.
        /// </summary>
        public bool Compression;

        /// <summary>Converts to ID3v2d4FrameFlags.</summary>
        /// <returns></returns>
        public ID3v2d4FrameFlags ToID3v2d4Flags()
        {
            ID3v2d4FrameFlags flags = 0;
            if (Compression)
            {
                flags |= ID3v2d4FrameFlags.Compression;
            }

            if (DataLengthIndicator)
            {
                flags |= ID3v2d4FrameFlags.DataLengthIndicator;
            }

            if (Encryption)
            {
                flags |= ID3v2d4FrameFlags.Encryption;
            }

            if (FileAlterPreservation)
            {
                flags |= ID3v2d4FrameFlags.FileAlterPreservation;
            }

            if (GroupingIdentity)
            {
                flags |= ID3v2d4FrameFlags.GroupingIdentity;
            }

            if (ReadOnly)
            {
                flags |= ID3v2d4FrameFlags.ReadOnly;
            }

            if (TagAlterPreservation)
            {
                flags |= ID3v2d4FrameFlags.TagAlterPreservation;
            }

            if (Unsynchronisation)
            {
                flags |= ID3v2d4FrameFlags.Unsynchronisation;
            }

            return flags;
        }

        /// <summary>Converts to ID3v2d3FrameFlags.</summary>
        /// <returns></returns>
        public ID3v2d3FrameFlags ToID3v2d3Flags()
        {
            ID3v2d3FrameFlags flags = 0;
            if (Compression)
            {
                flags |= ID3v2d3FrameFlags.Compression;
            }

            if (Encryption)
            {
                flags |= ID3v2d3FrameFlags.Encryption;
            }

            if (FileAlterPreservation)
            {
                flags |= ID3v2d3FrameFlags.FileAlterPreservation;
            }

            if (GroupingIdentity)
            {
                flags |= ID3v2d3FrameFlags.GroupingIdentity;
            }

            if (ReadOnly)
            {
                flags |= ID3v2d3FrameFlags.ReadOnly;
            }

            if (TagAlterPreservation)
            {
                flags |= ID3v2d3FrameFlags.TagAlterPreservation;
            }

            return flags;
        }
    }
}
