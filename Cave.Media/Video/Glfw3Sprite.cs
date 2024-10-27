using System;
using System.Diagnostics;
using Cave.Media.OpenGL;

namespace Cave.Media.Video;

class Glfw3Sprite : RenderSprite
{
    #region Private Fields

    Glfw3Renderer renderer;
    int texture;

    #endregion Private Fields

    #region Private Methods

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

    unsafe void LoadNewTexture(Bitmap32 image)
    {
        DeleteTexture();
        CreateTexture();

        Trace.TraceInformation("Generating Texture...");
        gl2.BindTexture(GL._TEXTURE_2D, texture);
        renderer.CheckErrors("BindTexture");

        Trace.TraceInformation("retrieve raw pixels...");
        var data = image.GetImageData();
        if (data.Stride != data.Width * 4) throw new InvalidOperationException($"Stride {data.Stride} is not supported! Convert the image first!");
        var pixels = data.Pixels1;

        Trace.TraceInformation("set texture data {0}x{1} [{2}]...", image.Width, image.Height, data.DataLength);
        gl2.TexImage2D(GL._TEXTURE_2D, 0, GL._RGBA, image.Width, image.Height, 0, GL._BGRA, GL._UNSIGNED_BYTE, (IntPtr)pixels);
        renderer.CheckErrors("TexImage2D");
    }

    unsafe void UpdateTexture(Bitmap32 image)
    {
        Trace.TraceInformation("Updating Texture...");
        gl2.BindTexture(GL._TEXTURE_2D, texture);
        renderer.CheckErrors("BindTexture");

        Trace.TraceInformation("retrieve raw pixels...");
        var data = image.GetImageData();
        if (data.Stride != data.Width * 4) throw new InvalidOperationException($"Stride {data.Stride} is not supported! Convert the image first!");
        var pixels = data.Pixels1;

        Trace.TraceInformation("set texture data {0}x{1} [{2}]...", image.Width, image.Height, data.DataLength);
        gl2.TexSubImage2D(GL._TEXTURE_2D, 0, 0, 0, image.Width, image.Height, GL._BGRA, GL._UNSIGNED_BYTE, (IntPtr)pixels);
        renderer.CheckErrors("TexSubImage2D");
    }

    #endregion Private Methods

    #region Protected Methods

    protected override void Dispose(bool disposing) => DeleteTexture();

    #endregion Protected Methods

    #region Public Constructors

    public Glfw3Sprite(Glfw3Renderer renderer, string name)
                      : base(renderer, name)
    {
        this.renderer = renderer;
        IsStreamingTexture = false;
    }

    #endregion Public Constructors

    #region Public Properties

    public override bool IsStreamingTexture { get; protected set; }

    public int Texture => texture;

    public override Vector2 TextureSize { get; protected set; }

    #endregion Public Properties

    #region Public Methods

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

    public override void LoadTexture(Bitmap32 image)
    {
        if (renderer.MaxTextureSize > 0)
        {
            if (image.Width > renderer.MaxTextureSize)
            {
                throw new Exception(string.Format("Maximum pixel size exceeded! Width > {0}", renderer.MaxTextureSize));
            }
            if (image.Height > renderer.MaxTextureSize)
            {
                throw new Exception(string.Format("Maximum pixel size exceeded! Height > {0}", renderer.MaxTextureSize));
            }
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
        var w = (int)renderer.Resolution.X;
        var h = (int)renderer.Resolution.Y;
        gl2.CopyTexImage2D(GL._TEXTURE_2D, 0, GL._RGBA, 0, 0, w, h, 0);
        renderer.CheckErrors("CopyTexImage2D");

        Trace.TraceInformation("bind texture to attribute...");
        gl2.Uniform1i(renderer.ShaderTextureData, 0);
        renderer.CheckErrors("Uniform1i");

        TextureSize = Vector2.Create(w, h);
    }

    #endregion Public Methods
}
