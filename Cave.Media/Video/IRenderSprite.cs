using System;

namespace Cave.Media.Video;

/// <summary>
/// Provides an interface for sprites.
/// </summary>
public interface IRenderSprite : IDisposable
{
    /// <summary>
    /// Gets parent renderer.
    /// </summary>
    IRenderer Renderer { get; }

    /// <summary>
    /// Gets the name of the object.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets or sets a value indicating whether the <see cref="IRenderSprite"/> is visible or not.
    /// </summary>
    bool Visible { get; set; }

    /// <summary>
    /// Gets or sets global alpha for the whole object. This is added on top of the alpha values already present at the texture data.
    /// </summary>
    float Alpha { get; set; }

    /// <summary>
    /// Gets or sets global tint for the whole object.
    /// </summary>
    ARGB Tint { get; set; }

    /// <summary>
    /// Gets or sets the rotation of the sprite (-1..0..1).
    /// </summary>
    Vector3 Rotation { get; set; }

    /// <summary>
    /// Gets or sets the pos of the sprite (-1..0..1).
    /// </summary>
    Vector3 Position { get; set; }

    /// <summary>
    /// Gets or sets the scale of the sprite.
    /// </summary>
    Vector3 Scale { get; set; }

    /// <summary>
    /// Gets or sets the center point of the sprite (The center is mid body point of the sprite and the rotation center point. This does
    /// not affect scaling!).
    /// </summary>
    Vector3 CenterPoint { get; set; }

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
    Vector3 PositionFromBounds(float x, float y, float width = 0, float height = 0);

    /// <summary>
    /// Gets the <see cref="Scale"/> from a specific pixel size. Attention: the pixel size depends on the pixel resolution
    /// of the renderers backbuffer, not the screen or control size! You can check the renderers backbuffer resolution with.
    /// </summary>
    /// <param name="width">The width of the object in pixel.</param>
    /// <param name="height">The height of the object in pixel.</param>
    /// <param name="mode">resize mode for the scaling.</param>
    /// <returns>the new scale.</returns>
    Vector3 ScaleFromSize(float width = 0, float height = 0, ResizeMode mode = ResizeMode.None);

    /// <summary>
    /// Gets the <see cref="Scale"/> from a specific size (0..1).
    /// The given size is normalized to the backbuffer size.
    /// </summary>
    /// <param name="width">The width of the object.</param>
    /// <param name="height">The height of the object.</param>
    /// <param name="mode">resize mode for the scaling.</param>
    /// <returns>the new scale.</returns>
    Vector3 ScaleFromSizeNorm(float width = 0, float height = 0, ResizeMode mode = ResizeMode.None);

    /// <summary>
    /// Uses <see cref="ScaleFromSize(float, float, ResizeMode)"/> and <see cref="PositionFromBounds(float, float, float, float)"/> to set the scale and position of the object.
    /// </summary>
    /// <param name="x">The x location in pixels to set (<see cref="IRenderer.Resolution"/> left=0..X).</param>
    /// <param name="y">The y location in pixels to set (<see cref="IRenderer.Resolution"/> top=0..Y).</param>
    /// <param name="width">The width of the object in pixel.</param>
    /// <param name="height">The height of the object in pixel.</param>
    void SetBounds(float x, float y, float width = 0, float height = 0);

    /// <summary>
    /// Gets a value indicating whether the texture is created in streaming mode or not.
    /// </summary>
    bool IsStreamingTexture { get; }

    /// <summary>
    /// Loads a new texture bitmap.
    /// </summary>
    /// <param name="bitmap">The bitmap to load.</param>
    void LoadTexture(IBitmap32 bitmap);

    /// <summary>
    /// Clears the texture bitmap.
    /// </summary>
    void DeleteTexture();

    /// <summary>
    /// Loads a texture copied from the current backbuffer.
    /// </summary>
    void LoadTextureFromBackbuffer();

    /// <summary>
    /// Gets the current texture size in pixel.
    /// </summary>
    Vector2 TextureSize { get; }
}
