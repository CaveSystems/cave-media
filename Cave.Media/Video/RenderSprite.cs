using System;

namespace Cave.Media.Video
{
    /// <summary>
    /// Provides an object for the renderer.
    /// </summary>
    public abstract class RenderSprite : IRenderSprite
    {
        /// <summary>
        /// Gets parent renderer.
        /// </summary>
        public IRenderer Renderer { get; }

        /// <summary>
        /// Gets the name of the object.
        /// </summary>
        public string Name { get; }

        /// <summary>Initializes a new instance of the <see cref="RenderSprite"/> class.</summary>
        /// <param name="renderer">The used renderer.</param>
        /// <param name="name">Name of the Object.</param>
        public RenderSprite(IRenderer renderer, string name)
        {
            Name = name ?? throw new ArgumentNullException("name");
            Renderer = renderer ?? throw new ArgumentNullException("renderer");
            Alpha = 1;
            Scale = Vector3.Create(1, 1, 1);
            Visible = true;
            Tint = 0;
        }

        /// <summary>
        /// Gets or sets a value indicating whether the <see cref="IRenderSprite"/> is visible or not.
        /// </summary>
        public bool Visible { get; set; }

        /// <summary>
        /// Gets or sets global alpha for the whole object. This is added on top of the alpha values already present at the texture data.
        /// </summary>
        public float Alpha { get; set; }

        /// <summary>
        /// Gets or sets global tint for the whole object.
        /// </summary>
        public ARGB Tint { get; set; }

        /// <summary>
        /// Gets or sets the rotation of the sprite (-1..0..1).
        /// </summary>
        public Vector3 Rotation { get; set; }

        /// <summary>
        /// Gets or sets the pos of the sprite (-1..0..1).
        /// </summary>
        public Vector3 Position { get; set; }

        /// <summary>
        /// Gets or sets the scale of the sprite.
        /// </summary>
        public Vector3 Scale { get; set; }

        /// <summary>
        /// Gets or sets the center point of the sprite (The center is mid body point of the sprite and the rotation center point. This does
        /// not affect scaling!).
        /// </summary>
        public Vector3 CenterPoint { get; set; }

        /// <summary>
        /// Gets a <see cref="Position"/> for a specific pixel location. Attention: the pixel location depends on the pixel resolution
        /// of the renderers backbuffer, not the screen or control size! You can check the renderers backbuffer resolution with.
        /// <see cref="IRenderer.Resolution"/>.
        /// </summary>
        /// <param name="x">The x location in pixels to set (<see cref="IRenderer.Resolution"/> left=0..X).</param>
        /// <param name="y">The y location in pixels to set (<see cref="IRenderer.Resolution"/> top=0..Y).</param>
        /// <param name="width">The width of the object in pixel.</param>
        /// <param name="height">The height of the object in pixel.</param>
        /// <returns>the new position.</returns>
        public Vector3 PositionFromBounds(float x, float y, float width = 0, float height = 0)
        {
            if (width == 0)
            {
                width = TextureSize.X;
            }

            if (height == 0)
            {
                height = TextureSize.Y;
            }

            var v = new Vector3
            {
                X = ((x - 0.5f + (width / 2.0f)) / Renderer.Resolution.X * 2.0f) - 1.0f,
                Y = -(((y - 0.5f + (height / 2.0f)) / Renderer.Resolution.Y * 2.0f) - 1.0f),
            };
            return v;
        }

        /// <summary>
        /// Gets the <see cref="Scale"/> from a specific pixel size. Attention: the pixel size depends on the pixel resolution
        /// of the renderers backbuffer, not the screen or control size! You can check the renderers backbuffer resolution with.
        /// </summary>
        /// <param name="width">The width of the object in pixel.</param>
        /// <param name="height">The height of the object in pixel.</param>
        /// <param name="mode">resize mode for the scaling.</param>
        /// <returns>the new scale.</returns>
        public Vector3 ScaleFromSize(float width = 0, float height = 0, ResizeMode mode = ResizeMode.None)
        {
            return ScaleFromSizeNorm(width / Renderer.Resolution.X, height / Renderer.Resolution.Y);
        }

        /// <summary>
        /// Gets the <see cref="Scale"/> from a specific size (0..1).
        /// The given size is normalized to the backbuffer size.
        /// </summary>
        /// <param name="width">The width of the object.</param>
        /// <param name="height">The height of the object.</param>
        /// <param name="mode">resize mode for the scaling.</param>
        /// <returns>the new scale.</returns>
        public Vector3 ScaleFromSizeNorm(float width = 1, float height = 1, ResizeMode mode = ResizeMode.None)
        {
            if (mode == ResizeMode.None)
            {
                return Vector3.Create(width, height, 0);
            }
            float x = 1f, y = 1f;
            float fbAspect = Renderer.Resolution.X / Renderer.Resolution.Y;
            if (TextureSize != Vector2.Empty)
            {
                fbAspect = fbAspect * (width / height) / (TextureSize.X / TextureSize.Y);
            }
            if (mode == ResizeMode.TouchFromInside)
            {
                if (fbAspect > 1)
                {
                    x = 1f / fbAspect;
                }
                else
                {
                    y = fbAspect;
                }
            }
            else
            {
                if (fbAspect > 1)
                {
                    y = fbAspect;
                }
                else
                {
                    x = 1f / fbAspect;
                }
            }
            return Vector3.Create(x * width, y * height, 0);
        }

        /// <summary>
        /// Uses <see cref="ScaleFromSize(float, float, ResizeMode)"/> and <see cref="PositionFromBounds(float, float, float, float)"/> to set the scale and position of the object.
        /// </summary>
        /// <param name="x">The x location in pixels to set (<see cref="IRenderer.Resolution"/> left=0..X).</param>
        /// <param name="y">The y location in pixels to set (<see cref="IRenderer.Resolution"/> top=0..Y).</param>
        /// <param name="width">The width of the object in pixel.</param>
        /// <param name="height">The height of the object in pixel.</param>
        public void SetBounds(float x, float y, float width = 0, float height = 0)
        {
            if (width == 0)
            {
                width = TextureSize.X;
            }

            if (height == 0)
            {
                height = TextureSize.Y;
            }

            Scale = ScaleFromSize(width, height);
            Position = PositionFromBounds(x, y, Scale.X * Renderer.Resolution.X, Scale.Y * Renderer.Resolution.Y);
        }

        #region IDisposable Member

        /// <summary>
        /// Disposes all unmanaged resources.
        /// </summary>
        /// <param name="disposing">set to true if disposing managed resources.</param>
        protected abstract void Dispose(bool disposing);

        /// <summary>
        /// Disposes the properties.
        /// </summary>
        public void Dispose()
        {
            Dispose(true);
            GC.SuppressFinalize(this);
        }

        #endregion

        #region abstract implementation

        /// <summary>
        /// Gets or sets a value indicating whether the texture is created in streaming mode or not.
        /// </summary>
        public abstract bool IsStreamingTexture { get; protected set; }

        /// <summary>
        /// Gets or sets the current texture size in pixel.
        /// </summary>
        public abstract Vector2 TextureSize { get; protected set; }

        /// <summary>
        /// Loads a new texture bitmap.
        /// </summary>
        /// <param name="bitmap">The bitmap to load.</param>
        public abstract void LoadTexture(Bitmap32 bitmap);

        /// <summary>
        /// Loads a texture copied from the current backbuffer.
        /// </summary>
        public abstract void LoadTextureFromBackbuffer();

        /// <summary>
        /// Clears the texture bitmap.
        /// </summary>
        public abstract void DeleteTexture();
        #endregion
    }
}
