using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    ///
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct TIMECODEDATA
    {
        /// <summary>The time code.</summary>
        TIMECODE time;

        /// <summary>The SMPTE flags.</summary>
        public SMPTEFLAGS SMPTEflags;

        /// <summary></summary>
        public int User;
    }
}
