﻿using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Threading;
using Cave.Media.OpenGL;

namespace Cave.Media.Video
{
	/// <summary>
	/// Provides a renderer using OpenGL and <see href="http://www.glfw.org/">GLFW3</see>
	/// </summary>
	public class Glfw3Renderer : IRenderer, IDisposable
	{
		static int m_InitCount = 0;
		
		#region private classes
		class Glfw3Device : IRenderDevice
		{
			public Glfw3Device(int number, glfw3.Monitor monitor)
			{
				Monitor = monitor;
				Number = number;
				string name = glfw3.GetMonitorName(monitor);
				glfw3.GetMonitorPhysicalSize(monitor, out int width, out int height);
				VideoMode = glfw3.GetVideoMode(monitor);

				Name = $"Monitor {number} {name} {width / 10}cm x {height / 10}cm ({VideoMode.Width}x{VideoMode.Height})";
			}

			public int Number { get; }

			public glfw3.Monitor Monitor { get; }

			public glfw3.VideoMode VideoMode { get; }

			public string Name { get; }
		}

		class Glfw3Sprite : RenderSprite
		{
			Glfw3Renderer renderer;
			string name;
			int texture;

			public Glfw3Sprite(Glfw3Renderer renderer, string name)
				: base(renderer, name)
			{
				this.renderer = renderer;
				this.name = name;
				IsStreamingTexture = false;
			}

			void CreateTexture()
			{
				Trace.TraceInformation("Generating Texture...");
				gl2.GenTextures(1, out texture);
				renderer.CheckErrors("GenTextures");
				gl2.BindTexture(GL._TEXTURE_2D, texture);
				renderer.CheckErrors("BindTexture");
				gl2.TexParameteri(GL._TEXTURE_2D, GL._TEXTURE_MIN_FILTER, GL._LINEAR);
				gl2.TexParameteri(GL._TEXTURE_2D, GL._TEXTURE_WRAP_R, GL._CLAMP_TO_EDGE);
				gl2.TexParameteri(GL._TEXTURE_2D, GL._TEXTURE_WRAP_S, GL._CLAMP_TO_EDGE);
				gl2.TexParameteri(GL._TEXTURE_2D, GL._TEXTURE_WRAP_T, GL._CLAMP_TO_EDGE);
				renderer.CheckErrors("TexParameteri");
			}

			public override void DeleteTexture()
			{
				if (texture != 0)
				{
					Trace.TraceInformation("Free old texture...");
					gl2.DeleteTextures(1, ref texture);
					texture = 0;
					renderer.CheckErrors("DeleteTextures");
                    TextureSize = Vector2.Empty;
				}
			}

			public int Texture { get => texture; }

			public override bool IsStreamingTexture { get; protected set; }
			public override Vector2 TextureSize { get; protected set; }

            private void LoadNewTexture(Bitmap32 image)
            {
                DeleteTexture();
                CreateTexture();

                Trace.TraceInformation("Generating Texture...");
                gl2.BindTexture(GL._TEXTURE_2D, texture);
                renderer.CheckErrors("BindTexture");

                Trace.TraceInformation("retrieve raw pixels...");
                int[] pixels = image.Data.Data;

                Trace.TraceInformation("set texture data {0}x{1} [{2}]...", image.Width, image.Height, pixels.Length);
                gl2.TexImage2D(GL._TEXTURE_2D, 0, GL._RGBA, image.Width, image.Height, 0, GL._BGRA, GL._UNSIGNED_BYTE, pixels);
                renderer.CheckErrors("TexImage2D");


                // Probably not needed.. to check
                //  Trace.TraceInformation("bind texture to attribute...");
                //  gl2.Uniform1i(renderer.shaderTextureData, 0);
                //  renderer.CheckErrors("Uniform1i");

            }

            private void UpdateTexture(Bitmap32 image)
            {
                Trace.TraceInformation("Updating Texture...");
                gl2.BindTexture(GL._TEXTURE_2D, texture);
                renderer.CheckErrors("BindTexture");

                Trace.TraceInformation("retrieve raw pixels...");
                int[] pixels = image.Data.Data;

                Trace.TraceInformation("set texture data {0}x{1} [{2}]...", image.Width, image.Height, pixels.Length);
                gl2.TexSubImage2D(GL._TEXTURE_2D, 0, 0, 0, image.Width, image.Height, GL._BGRA, GL._UNSIGNED_BYTE, pixels);
              //  gl2.TexImage2D(GL._TEXTURE_2D, 0, GL._RGBA, image.Width, image.Height, 0, GL._BGRA, GL._UNSIGNED_BYTE, pixels);
                renderer.CheckErrors("TexSubImage2D");
            }

			public override void LoadTexture(Bitmap32 image)
			{
                if (renderer.MaxTextureSize > 0)
                {
                    if (image.Width > renderer.MaxTextureSize) throw new Exception(string.Format("Maximum pixel size exceeded! Width > {0}", renderer.MaxTextureSize));
                    if (image.Height > renderer.MaxTextureSize) throw new Exception(string.Format("Maximum pixel size exceeded! Height > {0}", renderer.MaxTextureSize));
                }
                
                if ((TextureSize.X > 0) && (TextureSize.Y > 0) && (TextureSize.X == image.Width) && (TextureSize.Y == image.Height))
                {
                    UpdateTexture(image);
                }
                else
                {
                    LoadNewTexture(image);
                }

				TextureSize = Vector2.Create(image.Width, image.Height);
			}

			public override void LoadTextureFromBackbuffer()
			{
				DeleteTexture();
				CreateTexture();

				gl2.Flush();
				Trace.TraceInformation("Generating Texture...");
				gl2.BindTexture(GL._TEXTURE_2D, texture);
				renderer.CheckErrors("BindTexture");

				Trace.TraceInformation("copy pixels...");
				int w = (int)renderer.Resolution.X;
				int h = (int)renderer.Resolution.Y;
				gl2.CopyTexImage2D(GL._TEXTURE_2D, 0, GL._RGBA, 0, 0, w, h, 0);
				renderer.CheckErrors("CopyTexImage2D");

				Trace.TraceInformation("bind texture to attribute...");
				gl2.Uniform1i(renderer.shaderTextureData, 0);
				renderer.CheckErrors("Uniform1i");

				TextureSize = Vector2.Create(w, h);
			}

			protected override void Dispose(bool disposing)
			{
				DeleteTexture();
			}
		}
		#endregion

		#region private variables
		glfw3.Window window;
		bool disposedValue = false;
		glfw3.WindowCloseFunc funcWindowClose;
		glfw3.FramebufferSizeFunc funcWindowChanged;
		int shaderProgram;
		int shaderVertexPosition;
		int shaderTextureCoordinates;
		int shaderTextureData;
		int shaderTint;
		int shaderAlpha;
		int shaderTranslation;
		int shaderRotation;
		int shaderScale;
		#endregion

		#region private functions
		void Compile(int shader, string code)
		{
			string[] codeArray = new string[] { code };

			gl2.ShaderSource(shader, 1, codeArray, null);
			CheckErrors("ShaderSource");

			gl2.CompileShader(shader);
			CheckErrors("CompileShader");

			StringBuilder resultString = new StringBuilder(Int16.MaxValue);
			gl2.GetShaderInfoLog(shader, Int16.MaxValue, out int length, resultString);
			CheckErrors("GetShaderInfoLog");

			gl2.GetShaderiv(shader, GL._COMPILE_STATUS, out int result);
			CheckErrors("GetShaderiv");

			if (result != (int)GL._TRUE) throw new Exception(string.Format("Shader compile failed: {0}", resultString));
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
			shaderProgram = gl2.CreateProgram();
			gl2.AttachShader(shaderProgram, vertexShader);
			CheckErrors("AttachShader vertexShader");

			gl2.AttachShader(shaderProgram, fragmentShader);
			CheckErrors("AttachShader fragmentShader");

			gl2.LinkProgram(shaderProgram);
			CheckErrors("LinkProgram");

			{
				StringBuilder resultString = new StringBuilder(Int16.MaxValue);
				gl2.GetProgramInfoLog(shaderProgram, Int16.MaxValue, out int length, resultString);
				gl2.GetProgramiv(shaderProgram, GL._LINK_STATUS, out int result);
				if (result != (int)GL._TRUE) throw new Exception(string.Format("Program link failed: {0}", resultString));
			}

			Trace.TraceInformation("Releasing source shaders...");
			gl2.DeleteShader(vertexShader);
			CheckErrors("DeleteShader vertexShader");

			gl2.DeleteShader(fragmentShader);
			CheckErrors("DeleteShader fragmentShader");

			gl2.UseProgram(shaderProgram);
			shaderVertexPosition = gl2.GetAttribLocation(shaderProgram, "shaderVertexPosition");
			shaderTextureCoordinates = gl2.GetAttribLocation(shaderProgram, "shaderTextureCoordinates");
			shaderTextureData = gl2.GetUniformLocation(shaderProgram, "shaderTextureData");
			shaderTint = gl2.GetUniformLocation(shaderProgram, "shaderTint");
			shaderAlpha = gl2.GetUniformLocation(shaderProgram, "shaderAlpha");
			shaderTranslation = gl2.GetUniformLocation(shaderProgram, "shaderTranslation");
			shaderRotation = gl2.GetUniformLocation(shaderProgram, "shaderRotation");
			shaderScale = gl2.GetUniformLocation(shaderProgram, "shaderScale");
			CheckErrors("UseProgram");
		}

		void PrepareBuffers()
		{
			int[] buffers = new int[2];
			gl2.GenBuffers(2, buffers);

			{
				Vector2[] quad = new Vector2[] { Vector2.Create(-1, -1), Vector2.Create(1, -1), Vector2.Create(-1, 1), Vector2.Create(1, 1) };
				gl2.BindBuffer(GL._ARRAY_BUFFER, buffers[0]);
				// send vertices data to opengl
				// ! 2nd paramater is length of array in bytes, send as IntPtr (not pointer of size variable)
				gl2.BufferData(GL._ARRAY_BUFFER, new IntPtr(2 * 4 * 4), quad, GL._STATIC_DRAW);
				gl2.EnableVertexAttribArray(shaderVertexPosition);
				// hint opengl at structure of data in bound buffer to use as vertex attribute
				// attribute index,  3 values / vertex, type float, no normalization, no stride, no offset
				gl2.VertexAttribPointer(shaderVertexPosition, 2, GL._FLOAT, 0, 0, null);
			}

			{
				Vector2[] texture = new Vector2[] { Vector2.Create(0, 1), Vector2.Create(1, 1), Vector2.Create(0, 0), Vector2.Create(1, 0), };
				//Vector2[] texture = new Vector2[] { Vector2.Create(0, 0), Vector2.Create(1, 0), Vector2.Create(0, 1), Vector2.Create(1, 1), };
				gl2.BindBuffer(GL._ARRAY_BUFFER, buffers[1]);
				// send vertices data to opengl
				// ! 2nd paramater is length of array in bytes, send as IntPtr (not pointer of size variable)
				gl2.BufferData(GL._ARRAY_BUFFER, new IntPtr(2 * 4 * 4), texture, GL._STATIC_DRAW);
				gl2.EnableVertexAttribArray(shaderTextureCoordinates);
				// hint opengl at structure of data in bound buffer to use as vertex attribute
				// attribute index,  3 values / vertex, type float, no normalization, no stride, no offset
				gl2.VertexAttribPointer(shaderTextureCoordinates, 2, GL._FLOAT, 0, 0, null);
			}
		}

		void CheckErrors(string command)
		{
			List<Exception> errors = null;
			GL last = GL._NO_ERROR;
			while (true)
			{
				GL e = (GL)gl2.GetError();
				if (e == GL._NO_ERROR) break;
				if (errors == null) errors = new List<Exception>();
				errors.Add(new Exception(e.ToString()));
				if (e == last) break;
				last = e;
			}
			if (errors != null) throw new AggregateException(string.Format("Glfw3Renderer Exception at {0}", command), errors.ToArray());
		}

		void WindowClose(glfw3.Window window)
		{
			Closed?.Invoke(this, new EventArgs());
		}

		void WindowChanged(glfw3.Window window, int width, int height)
		{
			PrepareFramebuffer();
		}

		void PrepareFramebuffer()
		{
			glfw3.GetFramebufferSize(window, out int w, out int h);
			gl2.Viewport(0, 0, w, h);
			//Resolution = Vector2.Create(w, h);
		}
		#endregion

		/// <summary>
		/// Creates a new <see cref="Glfw3Renderer"/> instance.
		/// This always succeeds. Please check <see cref="IsAvailable"/> after construction.
		/// </summary>
		public Glfw3Renderer()
		{
			Name = "Glfw3Renderer";
			Description = "OpenGL 2.1 Renderer using glfw3.";
			if (glfw3.Init())
			{
				IsAvailable = true;
				Interlocked.Increment(ref m_InitCount);
			}
		}

		public int MaxTextureSize { get; private set; }

		/// <summary>
		/// Provides a callback after the user closed the window
		/// </summary>
		public event EventHandler<EventArgs> Closed;

		/// <summary>
		/// Resolution of the backbuffer
		/// </summary>
		public Vector2 Resolution { get; private set; }

		/// <summary>
		/// Name of the Renderer
		/// </summary>
		public string Name { get; }

		/// <summary>
		/// Description of the Renderer
		/// </summary>
		public string Description { get; }

		/// <summary>
		/// Retrieves whether the renderer is available or not
		/// </summary>
		public bool IsAvailable { get; }

		/// <summary>
		/// Clears the backbuffer with the specified color
		/// </summary>
		/// <param name="backColor"></param>
		public void Clear(ARGB backColor)
		{
			if (window == null) throw new InvalidOperationException("Not initialized!");
			glfw3.MakeContextCurrent(window);
			gl2.ClearColor(backColor.RedFloat, backColor.GreenFloat, backColor.BlueFloat, backColor.AlphaFloat);
			gl2.Clear(GL._COLOR_BUFFER_BIT | GL._DEPTH_BUFFER_BIT);
		}

		/// <summary>
		/// Closes the renderer
		/// </summary>
		public void Close()
		{
			Dispose();
		}

		/// <summary>
		/// Creates a new <see cref="IRenderSprite"/> instance.
		/// </summary>
		/// <param name="name">The name of the sprite</param>
		/// <returns></returns>
		public IRenderSprite CreateSprite(string name)
		{
			if (!window.IsValid) throw new Exception("Not initialized!");
			return new Glfw3Sprite(this, name);
		}

		/// <summary>
		/// Gets all available devices
		/// </summary>
		/// <returns></returns>
		public IRenderDevice[] GetDevices()
		{
			List<IRenderDevice> results = new List<IRenderDevice>();
			foreach (var monitor in glfw3.GetMonitors())
			{
				results.Add(new Glfw3Device(results.Count + 1, monitor));
			}
			return results.ToArray();
		}

		/// <summary>
		/// Initializes the renderer with the specified properties. Use <see cref="GetDevices()"/> to get a device list for use with this function.
		/// </summary>
		/// <param name="parent">The device to use</param>
		/// <param name="mode">The mode the renderer is created with</param>
		/// <param name="flags">The flags defining the behaviour during rendering</param>
		/// <param name="width">The width in pixel of the backbuffer</param>
		/// <param name="height">The height in pixel of the backbuffer</param>
		/// <param name="title">The title of the window</param>
		public void Initialize(IRenderDevice parent, RendererMode mode, RendererFlags flags, int width, int height, string title)
		{
			Glfw3Device device = parent as Glfw3Device;
			if (device == null) throw new ArgumentNullException("device");
			if (window.Ptr != IntPtr.Zero) throw new InvalidOperationException("Already initialized!");
			
			Resolution = new Vector2() { X = width, Y = height };
			switch (mode)
			{
				case RendererMode.FullScreen:
					window = glfw3.CreateWindow(width, height, title, device.Monitor);
					if (!window.IsValid) throw new Exception("No window available!");
					break;
				case RendererMode.WindowedFullScreen:
				{
					glfw3.WindowHint(glfw3.Hint.Decorated, false);
					window = glfw3.CreateWindow(width, height, title);
					if (!window.IsValid) throw new Exception("No window available!");
					glfw3.GetMonitorPos(device.Monitor, out int x, out int y);
					glfw3.SetWindowMonitor(window, glfw3.Monitor.None, x, y, device.VideoMode.Width, device.VideoMode.Height, 60);
					break;
				}
				case RendererMode.Window:
				{
					window = glfw3.CreateWindow(width, height, title);
					if (!window.IsValid) throw new Exception("No window available!");
					glfw3.GetMonitorPos(device.Monitor, out int x, out int y);
					glfw3.SetWindowMonitor(window, glfw3.Monitor.None, x + width / 4, y + height / 4, width / 2, height / 2, 60);
					break;
				}
				default: throw new Exception(string.Format("Unknown mode {0}", mode));
			}
			glfw3.MakeContextCurrent(window);
			glfw3.SwapInterval(flags.HasFlag(RendererFlags.WaitRetrace) ? 1 : 0);
			glfw3.SetFramebufferSizeCallback(window, funcWindowChanged = new glfw3.FramebufferSizeFunc(WindowChanged));
			glfw3.SetWindowCloseCallback(window, funcWindowClose = new glfw3.WindowCloseFunc(WindowClose));

			gl2.GetIntegerv(GL._MAX_TEXTURE_SIZE, out int maxTextureSize);
			CheckErrors("GL_MAX_TEXTURE_SIZE");
			MaxTextureSize = maxTextureSize;

			PrepareShaders();
			PrepareBuffers();
			//prepare viewport
			gl2.Enable(GL._BLEND);
			gl2.BlendFunc(GL._SRC_ALPHA, GL._ONE_MINUS_SRC_ALPHA);
			gl2.MatrixMode(GL._PROJECTION);
			gl2.LoadIdentity();
			gl2.Ortho(-1, 1, -1, 1, 0, -100);
			PrepareFramebuffer();
            Trace.TraceInformation("Initialized {0} using {1} resolution {2}x{3} using OpenGL {4} Shader {5}", parent, flags, width, height, gl2.GetString(GL._VERSION), gl2.GetString(GL._SHADING_LANGUAGE_VERSION));
		}		

		/// <summary>
		/// Displays the current backbuffer.
		/// </summary>
		public void Present()
		{
			if (window == null) throw new InvalidOperationException("Not initialized!");
			glfw3.MakeContextCurrent(window);
			glfw3.SwapBuffers(window);
			glfw3.PollEvents();
		}

		/// <summary>
		/// Renders sprite instances to the backbuffer
		/// </summary>
		/// <param name="sprites">The sprites to render</param>
		public void Render(params IRenderSprite[] sprites)
		{
			Render((IEnumerable<IRenderSprite>)sprites);
		}

		/// <summary>
		/// Renders sprite instances to the backbuffer
		/// </summary>
		/// <param name="sprites">The sprites to render</param>
		public void Render(IEnumerable<IRenderSprite> sprites)
		{
			if (window == null) throw new InvalidOperationException("Not initialized!");
			glfw3.MakeContextCurrent(window);
			foreach (Glfw3Sprite s in sprites)
			{
				gl2.BindTexture(GL._TEXTURE_2D, s.Texture);

				gl2.Uniform3fv(shaderTranslation, s.Position);
				gl2.Uniform3fv(shaderRotation, s.Rotation);
				gl2.Uniform3fv(shaderScale, s.Scale);

				gl2.Uniform1f(shaderAlpha, s.Alpha);
				gl2.Uniform4f(shaderTint, s.Tint.RedFloat, s.Tint.GreenFloat, s.Tint.BlueFloat, s.Tint.AlphaFloat);
				gl2.DrawArrays(GL._TRIANGLE_STRIP, 0, 4);
			}
		}

		#region IDisposable Support
		/// <summary>
		/// Disposes all unmanaged and if specified all managed resources of the renderer
		/// </summary>
		/// <param name="disposing"></param>
		protected virtual void Dispose(bool disposing)
		{
			if (!disposedValue)
			{
				if (disposing)
				{
				
				}
				//TODO: do we need to free the shaders ?
				if (0 == Interlocked.Decrement(ref m_InitCount))
				{
					glfw3.Terminate();
				}
				disposedValue = true;
			}
		}

		/// <summary>
		/// Disposes all unmanaged resources
		/// </summary>
		~Glfw3Renderer()
		{
			Dispose(false);
		}

		/// <summary>
		/// Disposes all unmanaged and managed resources of the renderer
		/// </summary>
		public void Dispose()
		{
			Dispose(true);
			GC.SuppressFinalize(this);
		}
		#endregion
	}
}
