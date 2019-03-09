using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// Old AVI index entry.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct AVIOLDINDEXENTRY
    {
        /// <summary>
        /// Chunk id.
        /// </summary>
        public int ChunkId;

        /// <summary>
        /// <see cref="AVIOLDINDEXENTRYFLAGS"/>.
        /// </summary>
        public AVIOLDINDEXENTRYFLAGS Flags;

        /// <summary>
        /// offset of riff chunk header for the data.
        /// </summary>
        public int Offset;

        /// <summary>
        /// size of the data (excluding riff header size).
        /// </summary>
        public int Size;
    }
}
