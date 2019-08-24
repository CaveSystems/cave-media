using System;

namespace Cave.Media.OpenGL
{
    public static partial class glfw3
    {
        /// <summary>
        /// glfw3 size event args, z.B. window resize.
        /// </summary>
        public class SizeEventArgs : EventArgs
        {
            /// <summary>
            /// the width.
            /// </summary>
            public int Width { get; private set; }

            /// <summary>
            /// the height.
            /// </summary>
            public int Height { get; private set; }

            /// <summary>
            /// constructor.
            /// </summary>
            /// <param name="width"></param>
            /// <param name="height"></param>
            public SizeEventArgs(int width, int height)
            {
                Width = width;
                Height = height;
            }
        }


        /// <summary>
        /// glfw3 mouse button event args send when a mouse button event (down, up) occured.
        /// </summary>
        public class MouseButtonEventArgs : EventArgs
        {
            /// <summary>
            /// Gets mouse cursor position in window coordinates (<see cref="Vector2"/> in range [0,0;width,height]).
            /// </summary>
            public Vector2 Position { get; private set; }

            /// <summary>
            /// Gets mouse cursor position in normalized window coordinates (<see cref="Vector2"/> in range [0,0;1,1]).
            /// </summary>
            public Vector2 PositionNorm { get; private set; }

            /// <summary>
            /// Gets the mouse button that triggered this event.
            /// </summary>
            public MouseButton Button { get; private set; }

            /// <summary>
            /// Gets the input state <see cref="InputState"/> of the button when the event was triggered.
            /// </summary>
            public InputState State { get; private set; }

            /// <summary>
            /// Gets additional keys that were pressed when the event was triggered.
            /// </summary>
            public KeyMods Mods { get; private set; }

            /// <summary>
            /// Initializes a new instance of the <see cref="MouseButtonEventArgs"/> class.
            /// </summary>
            /// <param name="position">Cursor position.</param>
            /// <param name="positionNorm">Normalized position.</param>
            /// <param name="button">Active buttons.</param>
            /// <param name="state">Key and button actions.</param>
            /// <param name="mods">Modifier flags.</param>
            public MouseButtonEventArgs(Vector2 position, Vector2 positionNorm, MouseButton button, InputState state, KeyMods mods)
            {
                Position = position;
                PositionNorm = positionNorm;
                Button = button;
                State = state;
                Mods = mods;
            }
        }
    }
}
