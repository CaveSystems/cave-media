using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading;
using Cave.Media.OpenGL;

namespace Cave.Media.Video
{
    /// <summary>
    /// Provides a renderer using OpenGL and <see href="http://www.glfw.org/">GLFW3</see>.
    /// </summary>
    public class Glfw3Renderer : IRenderer, IDisposable
    {
        static int initCount = 0;

        #region private variables

        bool disposedValue = false;
        glfw3.WindowCloseFunc funcWindowClose;
        glfw3.FramebufferSizeFunc funcWindowChange;
        glfw3.MouseButtonFunc funcMouseButtonChange;
        glfw3.CursorPosFunc funcCursorPosChange;
        glfw3.ScrollFunc funcScrollEvent;
        glfw3.KeyFunc funcKeyEvent;

        ResizeMode aspectCorrection = ResizeMode.None;
        Vector2 aspectCorrectionVector = Vector2.Create(1f, 1f);

        #endregion

        #region internal properties

        internal glfw3.Window Window { get; private set; }
        internal int ShaderProgramId { get; private set; }
        internal int ShaderVertexPositionId { get; private set; }
        internal int ShaderTextureCoordinatesId { get; private set; }
        internal int ShaderTextureDataId { get; private set; }
        internal int ShaderTintId { get; private set; }
        internal int ShaderAlphaId { get; private set; }
        internal int ShaderTranslationId { get; private set; }
        internal int ShaderCenterPointId { get; private set; }
        internal int ShaderRotationId { get; private set; }
        internal int ShaderScaleId { get; private set; }
        internal int WorldTranslationId { get; private set; }
        internal int WorldScaleId { get; private set; }
        #endregion

        #region private functions

        void Compile(int shader, string code)
        {
            var codeArray = new string[] { code };

            gl2.ShaderSource(shader, 1, codeArray, null);
            CheckErrors("ShaderSource");

            gl2.CompileShader(shader);
            CheckErrors("CompileShader");

            var resultString = new StringBuilder(short.MaxValue);
            gl2.GetShaderInfoLog(shader, short.MaxValue, out _, resultString);
            CheckErrors("GetShaderInfoLog");

            gl2.GetShaderiv(shader, GL._COMPILE_STATUS, out var result);
            CheckErrors("GetShaderiv");

            if (result != (int)GL._TRUE)
            {
                throw new Exception(string.Format("Shader compile failed: {0}", resultString));
            }
        }

        void PrepareShaders()
        {
            Trace.TraceInformation("Compiling VertexShader...");
            var vertexShader = gl2.CreateShader((int)GL._VERTEX_SHADER);
            {
                Compile(vertexShader, Properties.Resources.Glfw3VertexShader);
            }

            Trace.TraceInformation("Compiling FragmentShader...");
            var fragmentShader = gl2.CreateShader((int)GL._FRAGMENT_SHADER);
            {
                Compile(fragmentShader, Properties.Resources.Glfw3FragmentShader);
            }

            Trace.TraceInformation("Linking Shaders...");
            ShaderProgramId = gl2.CreateProgram();
            gl2.AttachShader(ShaderProgramId, vertexShader);
            CheckErrors("AttachShader vertexShader");

            gl2.AttachShader(ShaderProgramId, fragmentShader);
            CheckErrors("AttachShader fragmentShader");

            gl2.LinkProgram(ShaderProgramId);
            CheckErrors("LinkProgram");

            var resultString = new StringBuilder(short.MaxValue);
            gl2.GetProgramInfoLog(ShaderProgramId, short.MaxValue, out _, resultString);
            gl2.GetProgramiv(ShaderProgramId, GL._LINK_STATUS, out var result);
            if (result != (int)GL._TRUE)
            {
                throw new Exception(string.Format("Program link failed: {0}", resultString));
            }

            Trace.TraceInformation("Releasing source shaders...");
            gl2.DeleteShader(vertexShader);
            CheckErrors("DeleteShader vertexShader");

            gl2.DeleteShader(fragmentShader);
            CheckErrors("DeleteShader fragmentShader");

            gl2.UseProgram(ShaderProgramId);
            ShaderVertexPositionId = gl2.GetAttribLocation(ShaderProgramId, "shaderVertexPosition");
            ShaderTextureCoordinatesId = gl2.GetAttribLocation(ShaderProgramId, "shaderTextureCoordinates");
            ShaderTextureDataId = gl2.GetUniformLocation(ShaderProgramId, "shaderTextureData");
            ShaderTintId = gl2.GetUniformLocation(ShaderProgramId, "shaderTint");
            ShaderAlphaId = gl2.GetUniformLocation(ShaderProgramId, "shaderAlpha");
            ShaderTranslationId = gl2.GetUniformLocation(ShaderProgramId, "shaderTranslation");
            ShaderCenterPointId = gl2.GetUniformLocation(ShaderProgramId, "shaderCenterPoint");
            ShaderRotationId = gl2.GetUniformLocation(ShaderProgramId, "shaderRotation");
            ShaderScaleId = gl2.GetUniformLocation(ShaderProgramId, "shaderScale");
            WorldTranslationId = gl2.GetUniformLocation(ShaderProgramId, "worldTranslation");
            WorldScaleId = gl2.GetUniformLocation(ShaderProgramId, "worldScale");
            CheckErrors("UseProgram");
        }

        void PrepareBuffers()
        {
            var buffers = new int[2];
            gl2.GenBuffers(2, buffers);
            {
                var quad = new Vector2[] { Vector2.Create(-1, -1), Vector2.Create(1, -1), Vector2.Create(-1, 1), Vector2.Create(1, 1) };
                gl2.BindBuffer(GL._ARRAY_BUFFER, buffers[0]);

                // send vertices data to opengl
                // ! 2nd paramater is length of array in bytes, send as IntPtr (not pointer of size variable)
                gl2.BufferData(GL._ARRAY_BUFFER, new IntPtr(2 * 4 * 4), quad, GL._STATIC_DRAW);
                gl2.EnableVertexAttribArray(ShaderVertexPositionId);

                // hint opengl at structure of data in bound buffer to use as vertex attribute
                // attribute index,  3 values / vertex, type float, no normalization, no stride, no offset
                gl2.VertexAttribPointer(ShaderVertexPositionId, 2, GL._FLOAT, 0, 0, null);
            }

            {
                var texture = new Vector2[] { Vector2.Create(0, 1), Vector2.Create(1, 1), Vector2.Create(0, 0), Vector2.Create(1, 0), };
                gl2.BindBuffer(GL._ARRAY_BUFFER, buffers[1]);

                // send vertices data to opengl
                // ! 2nd paramater is length of array in bytes, send as IntPtr (not pointer of size variable)
                gl2.BufferData(GL._ARRAY_BUFFER, new IntPtr(2 * 4 * 4), texture, GL._STATIC_DRAW);
                gl2.EnableVertexAttribArray(ShaderTextureCoordinatesId);

                // hint opengl at structure of data in bound buffer to use as vertex attribute
                // attribute index,  3 values / vertex, type float, no normalization, no stride, no offset
                gl2.VertexAttribPointer(ShaderTextureCoordinatesId, 2, GL._FLOAT, 0, 0, null);
            }
        }

        internal void CheckErrors(string command)
        {
            List<Exception> errors = null;
            var last = GL._NO_ERROR;
            while (true)
            {
                var e = (GL)gl2.GetError();
                if (e == GL._NO_ERROR)
                {
                    break;
                }
                if (errors == null)
                {
                    errors = new List<Exception>();
                }
                errors.Add(new Exception(e.ToString()));
                if (e == last)
                {
                    break;
                }
                last = e;
            }
            if (errors != null)
            {
                throw new AggregateException(string.Format("Glfw3Renderer Exception at {0}", command), errors.ToArray());
            }
        }

        void WindowClose(glfw3.Window window) => Closed?.Invoke(this, new EventArgs());

        void WindowChange(glfw3.Window window, int width, int height) => PrepareFramebuffer();

        Vector2 GetMousePosition(glfw3.Window window)
        {
            glfw3.GetCursorPos(window, out var x, out var y);
            return Vector2.Create((float)x, (float)y);
        }

        void MouseButtonChange(glfw3.Window window, glfw3.MouseButton button, glfw3.InputState state, glfw3.KeyMods mods)
        {
            var mousePosition = GetMousePosition(window);
            var mousePositionNorm = Vector2.Create(mousePosition.X / Resolution.X, mousePosition.Y / Resolution.Y);
            MouseButtonChanged?.Invoke(this, new glfw3.MouseButtonEventArgs(mousePosition, mousePositionNorm, button, state, mods));
        }

        private void CursorPosChange(glfw3.Window window, double xpos, double ypos)
        {
            var mousePosition = Vector2.Create((float)xpos, (float)ypos);
            var mousePositionNorm = Vector2.Create(mousePosition.X / Resolution.X, mousePosition.Y / Resolution.Y);
            CursorPosChanged?.Invoke(this, new glfw3.CursorPosEventArgs(mousePosition, mousePositionNorm));
        }

        private void ScrollEventTriggered(glfw3.Window window, double xoffset, double yoffset)
        {
            var scrollOffset = Vector2.Create((float)xoffset, (float)yoffset);
            ScrollEvent?.Invoke(this, new glfw3.ScrollEventArgs(scrollOffset));
        }

        private void KeyEventTriggered(glfw3.Window window, glfw3.KeyCode key, int scancode, glfw3.InputState state, glfw3.KeyMods mods)
        {
            KeyEvent?.Invoke(this, new glfw3.KeyEventArgs(key, scancode, state, mods));
        }

        void PrepareFramebuffer()
        {
            glfw3.GetFramebufferSize(Window, out var w, out var h);
            gl2.Viewport(0, 0, w, h);
            Resolution = Vector2.Create(w, h);
            UpdateAspectCorrection();
            FrameBufferChanged?.Invoke(this, new glfw3.SizeEventArgs(w, h));
        }

        void UpdateAspectCorrection()
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.MakeContextCurrent(Window);
            var aspect = Resolution.X / Resolution.Y;
            switch (aspectCorrection)
            {
                case ResizeMode.None:
                    aspectCorrectionVector.X = 1f;
                    aspectCorrectionVector.Y = 1f;
                    break;
                case ResizeMode.TouchFromInside:
                    if (aspect > 1)
                    {
                        aspectCorrectionVector.X = aspect;
                    }
                    if (aspect < 1)
                    {
                        aspectCorrectionVector.Y = 1f / aspect;
                    }
                    break;
                case ResizeMode.TouchFromOutside:
                    if (aspect > 1)
                    {
                        aspectCorrectionVector.Y = 1 / aspect;
                    }
                    if (aspect < 1)
                    {
                        aspectCorrectionVector.X = aspect;
                    }
                    break;
                default:
                    throw new NotImplementedException("unknown aspect correction mode!");
            }
            gl2.LoadIdentity();
            gl2.Ortho(-aspectCorrectionVector.X, aspectCorrectionVector.X, -aspectCorrectionVector.Y, aspectCorrectionVector.Y, 0, -100);

        }

        #endregion

        /// <summary>
        /// Initializes a new instance of the <see cref="Glfw3Renderer"/> class.
        /// This always succeeds. Please check <see cref="IsAvailable"/> after construction.
        /// </summary>
        public Glfw3Renderer()
        {
            Name = "Glfw3Renderer";
            Description = "OpenGL 2.1 Renderer using glfw3.";
            WorldTranslation = Vector2.Empty;
            WorldScale = Vector2.Create(1, 1);
            if (glfw3.Init())
            {
                IsAvailable = true;
                Interlocked.Increment(ref initCount);
            }
        }

        /// <summary>
        /// Gets the maximum texture size for a openGL texture on the current device.
        /// </summary>
        public int MaxTextureSize { get; private set; }

        /// <summary>
        /// Provides a callback after the user closed the window
        /// </summary>
        public event EventHandler<EventArgs> Closed;

        /// <summary>
        /// Provides a callback for mouse button events
        /// </summary>
        public event EventHandler<glfw3.MouseButtonEventArgs> MouseButtonChanged;

        /// <summary>
        /// Provides a callback when the framebuffer size changes
        /// </summary>
        public event EventHandler<glfw3.SizeEventArgs> FrameBufferChanged;

        /// <summary>
        /// Provides a callback for cursor position change events
        /// </summary>
        public event EventHandler<glfw3.CursorPosEventArgs> CursorPosChanged;

        /// <summary>
        /// Provides a callback for cursor position change events
        /// </summary>
        public event EventHandler<glfw3.ScrollEventArgs> ScrollEvent;

        /// <summary>
        /// Provides a callback for key events
        /// </summary>
        public event EventHandler<glfw3.KeyEventArgs> KeyEvent;

        /// <summary>
        /// Gets the resolution of the backbuffer.
        /// </summary>
        public Vector2 Resolution { get; private set; }

        /// <summary>
        /// Provides a worldwide translation vector
        /// </summary>
        public Vector2 WorldTranslation { get; set; }

        /// <summary>
        /// Provides a worldwide scaling vector (zoom)
        /// </summary>
        public Vector2 WorldScale { get; set; }

        /// <summary>
        /// Gets or sets the aspect correction mode to use.
        /// </summary>
        public ResizeMode AspectCorrection
        {
            get => aspectCorrection;
            set
            {
                aspectCorrection = value;
                if (Window.IsValid)
                {
                    UpdateAspectCorrection();
                }
            }
        }

        /// <summary>
        /// Gets the name of the Renderer.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Gets the description of the Renderer.
        /// </summary>
        public string Description { get; }

        /// <summary>
        /// Gets a value indicating whether the renderer is available or not.
        /// </summary>
        public bool IsAvailable { get; }

        /// <summary>
        /// Clears the backbuffer with the specified color.
        /// </summary>
        /// <param name="backColor">the color to clear the background with.</param>
        public void Clear(ARGB backColor)
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.MakeContextCurrent(Window);
            gl2.ClearColor(backColor.RedFloat, backColor.GreenFloat, backColor.BlueFloat, backColor.AlphaFloat);
            gl2.Clear(GL._COLOR_BUFFER_BIT | GL._DEPTH_BUFFER_BIT);
        }

        /// <summary>
        /// Closes the renderer.
        /// </summary>
        public void Close() => Dispose();

        /// <summary>
        /// Creates a new <see cref="IRenderSprite"/> instance.
        /// </summary>
        /// <param name="name">The name of the sprite.</param>
        /// <returns>a IrenderSprite.</returns>
        public IRenderSprite CreateSprite(string name)
        {
            if (!Window.IsValid)
            {
                throw new Exception("Not initialized!");
            }
            return new Glfw3Sprite(this, name);
        }

        /// <summary>
        /// Gets all available devices.
        /// </summary>
        /// <returns>array of <see cref="IRenderDevice"/>.</returns>
        public IRenderDevice[] GetDevices()
        {
            var results = new List<IRenderDevice>();
            foreach (var monitor in glfw3.GetMonitors())
            {
                results.Add(new Glfw3Device(results.Count + 1, monitor));
            }
            return results.ToArray();
        }

        /// <summary>
        /// Initializes the renderer with the specified properties. Use <see cref="GetDevices()"/> to get a device list for use with this function.
        /// </summary>
        /// <param name="parent">The device to use.</param>
        /// <param name="mode">The mode the renderer is created with.</param>
        /// <param name="flags">The flags defining the behaviour during rendering.</param>
        /// <param name="width">The width in pixel of the backbuffer.</param>
        /// <param name="height">The height in pixel of the backbuffer.</param>
        /// <param name="title">The title of the window.</param>
        public void Initialize(IRenderDevice parent, RendererMode mode, RendererFlags flags, int width, int height, string title)
        {
            if ((parent == null) || !(parent is Glfw3Device device))
            {
                throw new ArgumentNullException("device");
            }
            if (Window.Ptr != IntPtr.Zero)
            {
                throw new InvalidOperationException("Already initialized!");
            }
            Resolution = new Vector2() { X = width, Y = height };
            switch (mode)
            {
                case RendererMode.FullScreen:
                    Window = glfw3.CreateWindow(width, height, title, device.Monitor);
                    if (!Window.IsValid)
                    {
                        throw new Exception("No window available!");
                    }
                    break;
                case RendererMode.WindowedFullScreen:
                {
                    glfw3.WindowHint(glfw3.Hint.Decorated, false);
                    Window = glfw3.CreateWindow(width, height, title);
                    if (!Window.IsValid)
                    {
                        throw new Exception("No window available!");
                    }
                    glfw3.GetMonitorPos(device.Monitor, out var x, out var y);
                    glfw3.SetWindowMonitor(Window, glfw3.Monitor.None, x, y, device.VideoMode.Width, device.VideoMode.Height, 60);
                    break;
                }
                case RendererMode.Window:
                {
                    Window = glfw3.CreateWindow(width, height, title);
                    if (!Window.IsValid)
                    {
                        throw new Exception("No window available!");
                    }
                    glfw3.GetMonitorPos(device.Monitor, out var x, out var y);
                    glfw3.SetWindowMonitor(Window, glfw3.Monitor.None, 100, 100, width, height, 60);
                    break;
                }
                default: throw new Exception(string.Format("Unknown mode {0}", mode));
            }
            glfw3.MakeContextCurrent(Window);
            glfw3.SwapInterval(flags.HasFlag(RendererFlags.WaitRetrace) ? 1 : 0);
            glfw3.SetFramebufferSizeCallback(Window, funcWindowChange = new glfw3.FramebufferSizeFunc(WindowChange));
            glfw3.SetWindowCloseCallback(Window, funcWindowClose = new glfw3.WindowCloseFunc(WindowClose));
            glfw3.SetMouseButtonCallback(Window, funcMouseButtonChange = new glfw3.MouseButtonFunc(MouseButtonChange));
            glfw3.SetCursorPosCallback(Window, funcCursorPosChange = new glfw3.CursorPosFunc(CursorPosChange));
            glfw3.SetScrollCallback(Window, funcScrollEvent = new glfw3.ScrollFunc(ScrollEventTriggered));
            glfw3.SetKeyCallback(Window, funcKeyEvent = new glfw3.KeyFunc(KeyEventTriggered));

            gl2.GetIntegerv(GL._MAX_TEXTURE_SIZE, out var maxTextureSize);
            CheckErrors("GL_MAX_TEXTURE_SIZE");
            MaxTextureSize = maxTextureSize;
            Trace.TraceInformation("Max Texture Size is {0}", maxTextureSize);

            gl2.GetIntegerv(GL._STENCIL_BITS, out var stencilBits);
            CheckErrors("GL_STENCIL_BITS");
            Trace.TraceInformation("Stencil Bit Size is {0}", stencilBits);

            PrepareShaders();
            PrepareBuffers();

            // prepare viewport
            gl2.Enable(GL._BLEND);
            gl2.BlendFunc(GL._SRC_ALPHA, GL._ONE_MINUS_SRC_ALPHA);
            gl2.MatrixMode(GL._PROJECTION);
            UpdateAspectCorrection();
            PrepareFramebuffer();
            Trace.TraceInformation("Initialized {0} using {1} resolution {2}x{3} using OpenGL {4} Shader {5}", parent, flags, width, height, gl2.GetString(GL._VERSION), gl2.GetString(GL._SHADING_LANGUAGE_VERSION));
        }


        /// <summary>
        /// Sets the size of the underlying glfw3 window.
        /// </summary>
        /// <param name="width">new width in pixels.</param>
        /// <param name="height">new height in pixels.</param>
        public void SetWindowSize(int width, int height)
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.SetWindowSize(Window, width, height);
        }

        /// <summary>
        /// Sets the title of the underlying glfw3 window.
        /// </summary>
        /// <param name="title">string to set as new window title.</param>
        public void SetWindowTitle(string title)
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.SetWindowTitle(Window, title);
        }

        /// <summary>
        /// Displays the current backbuffer.
        /// </summary>
        public void Present()
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.MakeContextCurrent(Window);
            glfw3.SwapBuffers(Window);
            glfw3.PollEvents();
        }

        /// <summary>
        /// Renders sprite instances to the backbuffer.
        /// </summary>
        /// <param name="sprites">The sprites to render.</param>
        public void Render(params IRenderSprite[] sprites) => Render((IEnumerable<IRenderSprite>)sprites);

        /// <summary>
        /// Renders sprite instances to the backbuffer.
        /// </summary>
        /// <param name="sprites">The sprites to render.</param>
        public void Render(IEnumerable<IRenderSprite> sprites)
        {
            if (!Window.IsValid)
            {
                throw new InvalidOperationException("Not initialized!");
            }
            glfw3.MakeContextCurrent(Window);
            gl2.Uniform3fv(WorldTranslationId, Vector3.Create(WorldTranslation.X, WorldTranslation.Y, 0));
            gl2.Uniform3fv(WorldScaleId, Vector3.Create(WorldScale.X, WorldScale.Y, 0));

            foreach (var s in sprites.Cast<Glfw3Sprite>())
            {
                if (s.Visible)
                {
                    gl2.BindTexture(GL._TEXTURE_2D, s.Texture);

                    gl2.Uniform3fv(ShaderTranslationId, s.Position);
                    gl2.Uniform3fv(ShaderCenterPointId, Vector3.Create(s.CenterPoint.X, -s.CenterPoint.Y, s.CenterPoint.Z));
                    gl2.Uniform3fv(ShaderRotationId, s.Rotation);
                    gl2.Uniform3fv(ShaderScaleId, s.Scale);

                    gl2.Uniform1f(ShaderAlphaId, s.Alpha);
                    gl2.Uniform4f(ShaderTintId, s.Tint.RedFloat, s.Tint.GreenFloat, s.Tint.BlueFloat, s.Tint.AlphaFloat);
                    gl2.DrawArrays(GL._TRIANGLE_STRIP, 0, 4);
                }
            }
        }

        /// <summary>
        /// calculates projected coordinates (-1..1) from window coordinates (pixels)
        /// </summary>
        /// <param name="windowCoordinates"></param>
        /// <returns></returns>
        public Vector2 CalculateProjectionCoordinates(Vector2 windowCoordinates)
        {
            var x = (((2f * windowCoordinates.X / Resolution.X) - 1f) * aspectCorrectionVector.X / WorldScale.X) - WorldTranslation.X;
            var y = (((2f * windowCoordinates.Y / Resolution.Y) - 1f) * aspectCorrectionVector.Y / WorldScale.Y) + WorldTranslation.Y;
            return Vector2.Create(x, -y);
        }

        /// <summary>
        /// calculates window coordinates (pixels) from projected coordinates (-1..1)
        /// </summary>
        /// <param name="projectionCoordinates"></param>
        /// <returns></returns>
        public Vector2 CalculateWindowCoordinates(Vector2 projectionCoordinates)
        {
            var x = Resolution.X * 0.5f * ((projectionCoordinates.X / aspectCorrectionVector.X) + 1f);
            var y = Resolution.Y * 0.5f * ((-projectionCoordinates.Y / aspectCorrectionVector.Y) + 1f);
            return Vector2.Create(x, y);

        }

        #region IDisposable Support

        /// <summary>
        /// Disposes all unmanaged and if specified all managed resources of the renderer.
        /// </summary>
        /// <param name="disposing">set to true if disposing managed resources.</param>
        protected virtual void Dispose(bool disposing)
        {
            if (!disposedValue)
            {
                if (disposing)
                {
                    // release managed resources
                }
                if (Interlocked.Decrement(ref initCount) == 0)
                {
                    glfw3.Terminate();
                }
                disposedValue = true;
            }
        }

        /// <summary>
        /// Finalizes an instance of the <see cref="Glfw3Renderer"/> class.
        /// Disposes all unmanaged resources.
        /// </summary>
        ~Glfw3Renderer()
        {
            Dispose(false);
        }

        /// <summary>
        /// Disposes all unmanaged and managed resources of the renderer.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }
        #endregion
    }
}
