using Cave.Media.OpenGL;

namespace Cave.Media.Video
{
    class Glfw3Device : IRenderDevice
    {
        public Glfw3Device(int number, glfw3.Monitor monitor)
        {
            Monitor = monitor;
            Number = number;
            var name = glfw3.GetMonitorName(monitor);
            glfw3.GetMonitorPhysicalSize(monitor, out var width, out var height);
            VideoMode = glfw3.GetVideoMode(monitor);

            Name = $"Monitor {number} {name} {width / 10}cm x {height / 10}cm ({VideoMode.Width}x{VideoMode.Height})";
        }

        public int Number { get; }

        public glfw3.Monitor Monitor { get; }

        public glfw3.VideoMode VideoMode { get; }

        public string Name { get; }
    }
}
