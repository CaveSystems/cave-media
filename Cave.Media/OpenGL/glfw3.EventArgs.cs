using System;

namespace Cave.Media.OpenGL
{
    public static partial class glfw3
    {
        public class MouseButtonEventArgs : EventArgs
        {
            public Vector2 Position { get; set; }
            public MouseButton Button { get; set; }
            public InputState State { get; set; }
            public KeyMods Mods { get; set; }
        }
    }
}
