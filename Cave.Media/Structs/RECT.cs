using System.Drawing;
using System.Runtime.InteropServices;

namespace Cave.Media.Structs
{
    /// <summary>
    /// The RECT structure defines a win32 rectangle.
    /// </summary>
    [StructLayout(LayoutKind.Sequential, Pack = 1)]
    public struct RECT
    {
        /// <summary>
        /// Sets all values of the struct.
        /// </summary>
        /// <param name="left">left coordinate.</param>
        /// <param name="top">top coordinate.</param>
        /// <param name="right">right coordinate.</param>
        /// <param name="bottom">bottom coordinate.</param>
        public void Set(int left, int top, int right, int bottom)
        {
            Left = left;
            Top = top;
            Right = right;
            Bottom = bottom;
        }

        /// <summary>
        /// specifies the left coordinate of the rectangle.
        /// </summary>
        public int Left;

        /// <summary>
        /// specifies the top coordinate of the rectangle.
        /// </summary>
        public int Top;

        /// <summary>
        /// specifies the right coordinate of the rectangle.
        /// </summary>
        public int Right;

        /// <summary>
        /// specifies the bottom coordinate of the rectangle.
        /// </summary>
        public int Bottom;

        /// <summary>
        /// retrieves the width of the rectangle.
        /// </summary>
        public int Width { get { return Right - Left; } }

        /// <summary>
        /// retrieves the height of the rectangle.
        /// </summary>
        public int Height { get { return Bottom - Top; } }

        /// <summary>
        /// retrieves the location (Point) of the rectangle.
        /// </summary>
        public Point Location { get { return new Point(Left, Top); } }

        /// <summary>
        /// retrieves the size of the rectangle.
        /// </summary>
        public Size Size { get { return new Size(Width, Height); } }

        /// <summary>
        /// retrieves the RECT structure as managed Rectangle object.
        /// </summary>
        public Rectangle Rectangle { get { return new Rectangle(Left, Top, Width, Height); } }

        /// <summary>
        /// Obtains the rect ccordinates (x1,y1)-(x2,y2).
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return "(" + Left + "," + Top + ")-(" + Right + "," + Bottom + ")";
        }
    }
}
