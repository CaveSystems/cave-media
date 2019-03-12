using System;
using System.Drawing;

namespace Cave.Media.Lyrics
{
    /// <summary>
    /// Provides a SynchronizedLyrics version 1 backbuffer.
    /// </summary>
    /// <seealso cref="ISynchronizedLyricsBackbuffer" />
    public class SynchronizedLyricsBackbufferV1 : ISynchronizedLyricsBackbuffer
    {
        const int BufferWidth = 300;
        const int BufferHeight = 216;
        const int BufferSize = BufferWidth * BufferHeight;
        const int ScreenWidth = BufferWidth - 12;
        const int ScreenHeight = BufferHeight - 24;

        byte[] m_Buffer = new byte[BufferSize];
        ARGB[] m_Palette = new ARGB[256];
        byte m_TransparentColor;
        byte m_ClearColor;
        sbyte m_OffsetHorizontal;
        sbyte m_OffsetVertical;

        /// <summary>Gets or sets the global alpha value (0 = transparent, 255 = opaque).</summary>
        /// <value>The alpha.</value>
        public byte GlobalAlpha { get; set; }

        /// <summary>Gets the size of the screen.</summary>
        /// <value>The size of the screen.</value>
        public Size ScreenSize { get { return new Size(ScreenWidth, ScreenHeight); } }

        /// <summary>Gets or sets a value indicating whether [to use transparent color override].</summary>
        /// <value>
        /// <c>true</c> if [transparent color override]; otherwise, <c>false</c>.
        /// </value>
        /// <remarks>
        /// Transparent color override replaces all background colors with the value specified by <see cref="TransparentColorValue" />.
        /// </remarks>
        public bool TransparentColorOverride { get; set; }

        /// <summary>Gets or sets the transparent color.</summary>
        /// <value>The transparent color.</value>
        public ARGB TransparentColorValue { get; set; }

        /// <summary>Initializes a new instance of the <see cref="SynchronizedLyricsBackbufferV1"/> class.</summary>
        public SynchronizedLyricsBackbufferV1()
        {
            GlobalAlpha = 255;
        }

        /// <summary>Plays the specified sl.</summary>
        /// <param name="sl">The sl.</param>
        public void Play(SynchronizedLyricsItem sl)
        {
            foreach (ISynchronizedLyricsCommand cmd in sl.Commands)
            {
                Play(cmd);
            }
        }

        /// <summary>Plays the specified command.</summary>
        /// <param name="cmd">The command.</param>
        /// <exception cref="System.NotImplementedException"></exception>
        public void Play(ISynchronizedLyricsCommand cmd)
        {
            switch (cmd.Type)
            {
                case SynchronizedLyricsCommandType.None: return;
                case SynchronizedLyricsCommandType.ClearScreen: ClearScreen((SlcWithColorIndex)cmd); break;
                case SynchronizedLyricsCommandType.ReplacePaletteColor: ReplacePaletteColor((SlcReplacePaletteColor)cmd); break;
                case SynchronizedLyricsCommandType.ReplacePaletteColors: ReplacePaletteColors((SlcReplacePaletteColors)cmd); break;
                case SynchronizedLyricsCommandType.SetTransparentColor: SetTransparentColor((SlcWithColorIndex)cmd); break;
                case SynchronizedLyricsCommandType.SetSprite2Colors: SetSprite2Colors((SlcSetSprite2Colors)cmd); break;
                case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: SetSprite2Colors((SlcSetSprite2Colors)cmd); break;
                case SynchronizedLyricsCommandType.ScreenOffset: SetScreenOffset((SlcScreenOffset)cmd); break;
                case SynchronizedLyricsCommandType.ScreenScroll: ScreenScroll((SlcScreenScroll)cmd); break;
                case SynchronizedLyricsCommandType.ScreenRoll: ScreenRoll((SlcScreenRoll)cmd); break;
                default: throw new NotImplementedException();
            }
        }

        private void ScreenRoll(SlcScreenRoll cmd)
        {
            return;
            throw new NotImplementedException();
        }

        private void ScreenScroll(SlcScreenScroll cmd)
        {
            int targetOffset = 0;
            targetOffset += cmd.Horizontal;
            targetOffset += cmd.Vertical * BufferWidth;

            if (targetOffset == 0)
            {
                return;
            }

            if (targetOffset > 0)
            {
                // target offset > 0 : need to move from last to first pixel
                for (int target = BufferSize - 1; target >= 0; target--)
                {
                    int source = target - targetOffset;
                    if (source >= 0)
                    {
                        m_Buffer[target] = m_Buffer[source];
                    }
                    else
                    {
                        m_Buffer[target] = cmd.ColorIndex;
                    }
                }
            }
            else
            {
                // target offset < 0 : need to move from first to last pixel
                for (int target = 0; target < BufferSize; target++)
                {
                    int source = target - targetOffset;
                    if (source < BufferSize)
                    {
                        m_Buffer[target] = m_Buffer[source];
                    }
                    else
                    {
                        m_Buffer[target] = cmd.ColorIndex;
                    }
                }
            }
            Invalidate();
        }

        private void SetScreenOffset(SlcScreenOffset cmd)
        {
            m_OffsetHorizontal = cmd.Horizontal;
            m_OffsetVertical = cmd.Vertical;
            Invalidate();
        }

        private void ReplacePaletteColors(SlcReplacePaletteColors cmd)
        {
            int i = cmd.ColorIndex;
            foreach (ARGB color in cmd.PaletteUpdate)
            {
                m_Palette[i++] = color;
            }
        }

        private void SetSprite2Colors(SlcSetSprite2Colors cmd)
        {
            byte[] colors = new byte[] { cmd.Color0, cmd.Color1 };

            int bufferOffset = cmd.X + (cmd.Y * BufferWidth);
            int b = 0;
            for (int y = 0; y < cmd.Height; y++)
            {
                // start at highest bit
                int shift = cmd.Width % 8;
                byte current = cmd.BitArray[b];

                for (int x = 0; x < cmd.Width; x++)
                {
                    if (--shift < 0) { shift = 7;
                        b++; }
                    int color = (current >> shift) & 1;
                    switch (cmd.Type)
                    {
                        case SynchronizedLyricsCommandType.SetSprite2Colors: m_Buffer[bufferOffset + x] = colors[color]; break;
                        case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: m_Buffer[bufferOffset + x] ^= colors[color]; break;
                    }
                }
                bufferOffset += BufferWidth;
                b++;
            }
            Invalidate();
        }

        private void SetTransparentColor(SlcWithColorIndex cmd)
        {
            m_TransparentColor = cmd.ColorIndex;
            Invalidate();
        }

        private void ReplacePaletteColor(SlcReplacePaletteColor cmd)
        {
            m_Palette[cmd.ColorIndex] = cmd.ColorValue;
            Invalidate();
        }

        private void ClearScreen(SlcWithColorIndex cmd)
        {
            m_ClearColor = cmd.ColorIndex;
            if (TransparentColorOverride)
            {
                for (int i = 0; i < BufferSize; i++)
                {
                    m_Buffer[i] = m_TransparentColor;
                }
            }
            else
            {
                for (int i = 0; i < BufferSize; i++)
                {
                    m_Buffer[i] = m_ClearColor;
                }
            }
            Invalidate();
        }

        /// <summary>Gets a value indicating whether this <see cref="ISynchronizedLyricsBackbuffer" /> was updated by a play command.</summary>
        /// <value><c>true</c> if updated; otherwise, <c>false</c>.</value>
        public bool Updated { get; private set; }

        /// <summary>Invalidates this instance.</summary>
        public void Invalidate()
        {
            Updated = true;
#if SKIA && (NETSTANDARD20 || NET45 || NET46 || NET47)
            skBitmap?.Dispose();
            skBitmap = null;
#elif NET20 || NET35 || NET40 || !SKIA
#else
#error No code defined for the current framework or NETXX version define missing!
#endif

#if NET20 || NET35 || NET40 || NET45 || NET46 || NET47
            bitmap?.Dispose();
            bitmap = null;
#elif NETSTANDARD20
#else
#error No code defined for the current framework or NETXX version define missing!
#endif
        }

        ARGBImageData ToImage()
        {
            int w = BufferWidth - 12;
            int h = BufferHeight - 24;
            ARGBImageData data = new ARGBImageData(w, h);
            int targetOffset = 0;
            int sourceOffset = (BufferWidth * (12 + m_OffsetVertical)) + m_OffsetHorizontal + 6;
            for (int y = 0; y < h; y++)
            {
                for (int x = 0; x < w; x++)
                {
                    byte colorIndex = m_Buffer[sourceOffset + x];
                    ARGB color;
                    if (TransparentColorOverride && ((colorIndex == m_TransparentColor) || (colorIndex == m_ClearColor)))
                    {
                        color = TransparentColorValue;
                    }
                    else
                    {
                        color = m_Palette[colorIndex];
                        color.Alpha = GlobalAlpha;
                    }
                    data[targetOffset++] = color;
                    sourceOffset += BufferWidth;
                }
            }
            return data;
        }

#if SKIA && (NETSTANDARD20 || NET45 || NET46 || NET47)
        SkiaSharp.SKBitmap skBitmap;

        /// <summary>
        /// Copies the image to the specified bitmapdata instance
        /// </summary>
        public SkiaSharp.SKBitmap ToSKBitmap()
        {
            if (Updated || skBitmap == null)
            {
                skBitmap = ToImage().ToSKBitmap();
            }
            return skBitmap;
        }
#elif NET20 || NET35 || NET40 || !SKIA
#else
#error No code defined for the current framework or NETXX version define missing!
#endif

#if NET20 || NET35 || NET40 || NET45 || NET46 || NET47
        Bitmap bitmap;

        /// <summary>
        /// Copies the image to the specified bitmapdata instance
        /// </summary>
        public Bitmap ToBitmap()
        {
            if (Updated || bitmap == null)
            {
                bitmap = ToImage().ToGdiBitmap();
            }
            return bitmap;
        }
#elif NETSTANDARD20
#else
#error No code defined for the current framework or NETXX version define missing!
#endif
    }
}
