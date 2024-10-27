using System;
using System.Collections.Generic;

namespace Cave.Media.Video;

/// <summary>
/// Provides an interface for a renderer.
/// </summary>
public interface IRenderer
{
    /// <summary>
    /// Provides a callback for the close event of the renderer
    /// </summary>
    event EventHandler<EventArgs> Closed;

    /// <summary>
    /// Gets a list of all available devices.
    /// </summary>
    /// <returns>array of devices.</returns>
    IRenderDevice[] GetDevices();

    /// <summary>
    /// Initializes the renderer.
    /// </summary>
    /// <param name="parent">The device the <see cref="IRenderDevice"/> will use.</param>
    /// <param name="mode">The mode the renderer is created with.</param>
    /// <param name="flags">The flags defining the behaviour during rendering.</param>
    /// <param name="width">The width in pixel of the backbuffer.</param>
    /// <param name="height">The height in pixel of the backbuffer.</param>
    /// <param name="title">The title of the window.</param>
    void Initialize(IRenderDevice parent, RendererMode mode, RendererFlags flags, int width, int height, string title);

    /// <summary>
    /// Gets the resolution (in pixel) of the renderers backbuffer surface.
    /// </summary>
    Vector2 Resolution { get; }

    /// <summary>
    /// gets or sets the aspect correction mode.
    /// </summary>
    ResizeMode AspectCorrection { get; set; }

    /// <summary>
    /// Closes the renderer.
    /// </summary>
    void Close();

    /// <summary>
    /// Renders the specified object to the backbuffer.
    /// </summary>
    /// <param name="sprites">The <see cref="IRenderSprite"/>s to render.</param>
    void Render(params IRenderSprite[] sprites);

    /// <summary>
    /// Renders the specified object to the backbuffer.
    /// </summary>
    /// <param name="sprites">The <see cref="IRenderSprite"/>s to render.</param>
    void Render(IEnumerable<IRenderSprite> sprites);

    /// <summary>
    /// Presents the backbuffer.
    /// </summary>
    void Present();

    /// <summary>Clears the backbuffer with a given color.</summary>
    /// <param name="backColor">the color to use.</param>
    void Clear(ARGB backColor);

    /// <summary>
    /// Creates a new <see cref="IRenderSprite"/> object for this <see cref="IRenderer"/>.
    /// </summary>
    /// <param name="name">name of the new sprite.</param>
    /// /// <returns>new sprite.</returns>
    IRenderSprite CreateSprite(string name);

    /// <summary>
    /// Gets the name of the <see cref="IRenderer"/>.
    /// </summary>
    string Name { get; }

    /// <summary>
    /// Gets a brief description of the <see cref="IRenderer"/>.
    /// </summary>
    string Description { get; }

    /// <summary>
    /// Gets a value indicating whether the renderer is available or not.
    /// </summary>
    bool IsAvailable { get; }
}
