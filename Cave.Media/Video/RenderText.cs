using System;
using System.Diagnostics;
using System.Drawing;

namespace Cave.Media.Video
{
    /// <summary>
    /// Provides an abstract implementation of <see cref="IRenderText"/>.
    /// </summary>
    public class RenderText : IRenderText, IDisposable
    {
        BoxAlignment alignment;

        /// <summary>
        /// Initializes a new instance of the <see cref="RenderText"/> class.
        /// </summary>
        /// <param name="sprite">the sprite the text is rendered on.</param>
        public RenderText(IRenderSprite sprite)
        {
            Sprite = sprite;
            FontName = null;
            FontSize = 8.25f;
            ForeColor = Color.White;
            BackColor = Color.Transparent;
            alignment = BoxAlignment.Center;
        }

        /// <summary>
        /// Gets the underlying sprite.
        /// Use this to call <see cref="IRenderer.Render(IRenderSprite[])"/> and to set translation, rotation, scale, ...
        /// </summary>
        public IRenderSprite Sprite { get; private set; }

        /// <summary>
        /// Gets or sets thr Font size.
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Gets or sets the name of the font.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Gets or sets foreground color to use.
        /// </summary>
        public ARGB ForeColor { get; set; }

        /// <summary>
        /// Gets or sets background color to use.
        /// </summary>
        public ARGB BackColor { get; set; }

        /// <summary>
        /// Gets or sets text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets or sets the text alignment.
        /// </summary>
        public BoxAlignment Alignment
        {
            get => alignment;

            set
            {
                if (Sprite == null)
                {
                    throw new ObjectDisposedException("RenderText");
                }
                float x = 0;
                float y = 0;
                float z = 0;
                switch (value & BoxAlignment.XFlags)
                {
                    case BoxAlignment.Left:
                        x = -1;
                        break;
                    case BoxAlignment.Center:
                        x = 0;
                        break;
                    case BoxAlignment.Right:
                        x = 1;
                        break;
                    default:
                        throw new Exception(string.Format("Invalid alignment '{0}'!", value & BoxAlignment.XFlags));
                }
                switch (value & BoxAlignment.YFlags)
                {
                    case BoxAlignment.Top:
                        y = 1;
                        break;
                    case BoxAlignment.Center:
                        y = 0;
                        break;
                    case BoxAlignment.Bottom:
                        y = -1;
                        break;
                    default:
                        throw new Exception(string.Format("Invalid alignment '{0}'!", value & BoxAlignment.YFlags));
                }
                Sprite.CenterPoint = Vector3.Create(x, y, z);
                alignment = value;
            }
        }

        /// <summary>
        /// Disposes this instance.
        /// </summary>
        public void Dispose()
        {
            Sprite?.Dispose();
            Sprite = null;
        }

        /// <summary>
        /// Updates the texture and scale of the underlying Object3D.
        /// </summary>
        /// <param name="maxWidth">max width in pixels.</param>
        /// <param name="maxHeight">max height in pixels.</param>
        public void Update(int maxWidth = 0, int maxHeight = 0)
        {
            if (Sprite == null)
            {
                throw new ObjectDisposedException("RenderText");
            }

            Trace.TraceInformation("Update text font:{0} size:{1} fg:{2} bg:{3} text:{4}", FontName, FontSize, ForeColor, BackColor, Text);
            var bitmap = Bitmap32.Create(FontName, FontSize, ForeColor, BackColor, Text);
            if ((maxWidth > 0) && (maxHeight > 0))
            {
                if ((bitmap.Width > maxWidth) || (bitmap.Height > maxHeight))
                {
                    var b = new Bitmap32(Math.Min(bitmap.Width, maxWidth), Math.Min(bitmap.Height, maxHeight));
                    b.Draw(bitmap, 0, 0);
                    bitmap.Dispose();
                    bitmap = b;
                }
            }
            Sprite.LoadTexture(bitmap);
            Sprite.Scale = Sprite.ScaleFromSize(bitmap.Width, bitmap.Height);
        }
    }
}
