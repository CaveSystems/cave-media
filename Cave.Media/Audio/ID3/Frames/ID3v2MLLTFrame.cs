using System;

namespace Cave.Media.Audio.ID3.Frames
{
    /// <summary>
    /// <para>
    /// This Frame is absolute nonsense and therefore I will not implement it.
    /// If this should be implemented someday allow backward references (negative FrameCount and TimeStamp),
    /// much bigger values (e.g. I want to skip 2h = 7200000ms = 276923 Frames up to 775384400 bytes),
    /// remove redundant data (time OR framecount).
    /// </para>
    /// <para>
    /// Original description:<br />
    /// This ID3v2 frame includes references that the
    /// software can use to calculate positions in the file. After the frame
    /// header follows a descriptor of how much the 'frame counter' should be
    /// increased for every reference. If this value is two then the first
    /// reference points out the second frame, the 2nd reference the 4th
    /// frame, the 3rd reference the 6th frame etc. In a similar way the
    /// 'bytes between reference' and 'milliseconds between reference' points
    /// out bytes and milliseconds respectively.
    /// </para>
    /// </summary>
    public sealed class ID3v2MLLTFrame : ID3v2Frame
    {
        internal ID3v2MLLTFrame(ID3v2Frame frame)
            : base(frame)
        {
            if (frame.ID != "MLLT")
            {
                throw new FormatException(string.Format("Cannot typecast frame {0} to {1}!", frame.ID, "MLLT"));
            }
        }

        /// <summary>
        /// Gets a string describing this frame.
        /// </summary>
        /// <returns>ID[Length] "Not implemented".</returns>
        public override string ToString() => base.ToString() + " \"Not implemented\"";
    }
}
