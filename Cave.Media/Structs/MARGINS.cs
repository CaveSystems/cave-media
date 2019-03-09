using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// Margins struct needed for api calls.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct MARGINS
    {
        /// <summary>
        /// creates a new Margins struct with the specified values.
        /// </summary>
        /// <param name="l">Left.</param>
        /// <param name="t">Top.</param>
        /// <param name="r">Right.</param>
        /// <param name="b">Bottom.</param>
        public MARGINS(int l, int t, int r, int b) { LEFT = l;
            TOP = t;
            RIGHT = r;
            BOTTOM = b; }

        /// <summary>
        /// distance to the left border.
        /// </summary>
        public int LEFT;

        /// <summary>
        /// distance to the right border.
        /// </summary>
        public int RIGHT;

        /// <summary>
        /// distance to the top border.
        /// </summary>
        public int TOP;

        /// <summary>
        /// distance to the bottom border.
        /// </summary>
        public int BOTTOM;
    }
}
