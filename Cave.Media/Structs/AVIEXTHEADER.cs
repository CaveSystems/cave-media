using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// AVI stream header: riff chunk 'dmlh'
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVIEXTHEADER
    {
        /// <summary>
        /// total number of frames in the file
        /// </summary>
        public int GrandFrames;

        /// <summary>
        /// reserved for future use
        /// </summary>
        [MarshalAs(UnmanagedType.ByValArray, SizeConst = 244)]
        public byte[] Future;
    }
}
