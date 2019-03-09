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
        BoxAlignment m_Alignment;

        /// <summary>
        /// Creates a new <see cref="RenderText"/> instance.
        /// </summary>
        public RenderText(IRenderSprite sprite)
        {
            Sprite = sprite;
            FontName = null;
            FontSize = 8.25f;
            ForeColor = Color.White;
            BackColor = Color.Transparent;
            m_Alignment = BoxAlignment.Center;
        }

        /// <summary>
        /// Provides access to the underlying sprite.
        /// Use this to call <see cref="IRenderer.Render(IRenderSprite[])"/> and to set translation, rotation, scale, ...
        /// </summary>
        public IRenderSprite Sprite { get; private set; }

        /// <summary>
        /// Font size.
        /// </summary>
        public float FontSize { get; set; }

        /// <summary>
        /// Name of the font.
        /// </summary>
        public string FontName { get; set; }

        /// <summary>
        /// Foreground color to use.
        /// </summary>
        public ARGB ForeColor { get; set; }

        /// <summary>
        /// Background color to use.
        /// </summary>
        public ARGB BackColor { get; set; }

        /// <summary>
        /// Text.
        /// </summary>
        public string Text { get; set; }

        /// <summary>
        /// Gets/sets the text alignment.
        /// </summary>
        public BoxAlignment Alignment
        {
            get { return m_Alignment; }
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
                m_Alignment = value;
            }
        }

        /// <inheritdoc/>
        public string LogSourceName => "RenderText";

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
        public void Update()
        {
            if (Sprite == null)
            {
                throw new ObjectDisposedException("RenderText");
            }

            Trace.TraceInformation("Update text font:{0} size:{1} fg:{2} bg:{3} text:{4}", FontName, FontSize, ForeColor, BackColor, Text);
            var bitmap = Bitmap32.Create(FontName, FontSize, ForeColor, BackColor, Text);
            Sprite.LoadTexture(bitmap);
            Sprite.Scale = Sprite.ScaleFromSize(bitmap.Width, bitmap.Height);
        }
    }
}
