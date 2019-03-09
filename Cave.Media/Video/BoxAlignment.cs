using System;
namespace Cave.Media.Video
{
    /// <summary>
    /// Provides available text alignments.
    /// </summary>
    [Flags]
    public enum BoxAlignment
    {
        /// <summary>
        /// Align centered
        /// </summary>
        Center = 0x00,

        /// <summary>
        /// Align left
        /// </summary>
        Left = 0x01,

        /// <summary>
        /// Align right
        /// </summary>
        Right = 0x02,

        /// <summary>
        /// Alignment X Achsis flags
        /// </summary>
        XFlags = 0x0F,

        /// <summary>
        /// Align top
        /// </summary>
        Top = 0x10,

        /// <summary>
        /// Align bottom
        /// </summary>
        Bottom = 0x20,

        /// <summary>
        /// Alignment Y Achsis flags
        /// </summary>
        YFlags = 0xF0,

        /// <summary>
        /// Align top left
        /// </summary>
        TopLeft = Top | Left,

        /// <summary>
        /// Align top right
        /// </summary>
        TopRight = Top | Right,

        /// <summary>
        /// Align bottom left
        /// </summary>
        BottomLeft = Bottom | Left,

        /// <summary>
        /// Align bottom right
        /// </summary>
        BottomRight = Bottom | Right,

        /// <summary>
        /// Align top left
        /// </summary>
        LeftTop = TopLeft,

        /// <summary>
        /// Align top right
        /// </summary>
        RightTop = TopRight,

        /// <summary>
        /// Align bottom left
        /// </summary>
        LeftBottom = BottomLeft,

        /// <summary>
        /// Align bottom right
        /// </summary>
        RightBottom = BottomRight,
    }
}
