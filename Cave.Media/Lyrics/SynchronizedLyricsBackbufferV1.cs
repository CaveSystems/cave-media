﻿using System;
using System.Drawing;

namespace Cave.Media.Lyrics;

/// <summary>Provides a SynchronizedLyrics version 1 backbuffer.</summary>
/// <seealso cref="ISynchronizedLyricsBackbuffer"/>
public class SynchronizedLyricsBackbufferV1 : ISynchronizedLyricsBackbuffer
{
    #region Private Fields

    const int BufferHeight = 216;
    const int BufferSize = BufferWidth * BufferHeight;
    const int BufferWidth = 300;
    const int ScreenHeight = BufferHeight - 24;
    const int ScreenWidth = BufferWidth - 12;
    IBitmap32? bitmap;
    byte[] buffer = new byte[BufferSize];
    byte clearColor;
    sbyte offsetHorizontal;
    sbyte offsetVertical;
    ARGB[] palette = new ARGB[256];
    byte transparentColor;

    #endregion Private Fields

    #region Private Methods

    private void ClearScreen(SlcWithColorIndex cmd)
    {
        clearColor = cmd.ColorIndex;
        if (TransparentColorOverride)
        {
            for (var i = 0; i < BufferSize; i++)
            {
                buffer[i] = transparentColor;
            }
        }
        else
        {
            for (var i = 0; i < BufferSize; i++)
            {
                buffer[i] = clearColor;
            }
        }
        Invalidate();
    }

    private void ReplacePaletteColor(SlcReplacePaletteColor cmd)
    {
        palette[cmd.ColorIndex] = cmd.ColorValue;
        Invalidate();
    }

    private void ReplacePaletteColors(SlcReplacePaletteColors cmd)
    {
        int i = cmd.ColorIndex;
        foreach (var color in cmd.PaletteUpdate)
        {
            palette[i++] = color;
        }
    }

    private void ScreenRoll(SlcScreenRoll cmd)
    {
        return;
        throw new NotImplementedException();
    }

    private void ScreenScroll(SlcScreenScroll cmd)
    {
        var targetOffset = 0;
        targetOffset += cmd.Horizontal;
        targetOffset += cmd.Vertical * BufferWidth;

        if (targetOffset == 0)
        {
            return;
        }

        if (targetOffset > 0)
        {
            // target offset > 0 : need to move from last to first pixel
            for (var target = BufferSize - 1; target >= 0; target--)
            {
                var source = target - targetOffset;
                if (source >= 0)
                {
                    buffer[target] = buffer[source];
                }
                else
                {
                    buffer[target] = cmd.ColorIndex;
                }
            }
        }
        else
        {
            // target offset < 0 : need to move from first to last pixel
            for (var target = 0; target < BufferSize; target++)
            {
                var source = target - targetOffset;
                if (source < BufferSize)
                {
                    buffer[target] = buffer[source];
                }
                else
                {
                    buffer[target] = cmd.ColorIndex;
                }
            }
        }
        Invalidate();
    }

    private void SetScreenOffset(SlcScreenOffset cmd)
    {
        offsetHorizontal = cmd.Horizontal;
        offsetVertical = cmd.Vertical;
        Invalidate();
    }

    private void SetSprite2Colors(SlcSetSprite2Colors cmd)
    {
        var colors = new byte[] { cmd.Color0, cmd.Color1 };

        var bufferOffset = cmd.X + (cmd.Y * BufferWidth);
        var b = 0;
        for (var y = 0; y < cmd.Height; y++)
        {
            // start at highest bit
            var shift = cmd.Width % 8;
            var current = cmd.BitArray[b];

            for (var x = 0; x < cmd.Width; x++)
            {
                if (--shift < 0)
                {
                    shift = 7;
                    b++;
                }
                var color = (current >> shift) & 1;
                switch (cmd.Type)
                {
                    case SynchronizedLyricsCommandType.SetSprite2Colors: buffer[bufferOffset + x] = colors[color]; break;
                    case SynchronizedLyricsCommandType.SetSprite2ColorsXOR: buffer[bufferOffset + x] ^= colors[color]; break;
                }
            }
            bufferOffset += BufferWidth;
            b++;
        }
        Invalidate();
    }

    private void SetTransparentColor(SlcWithColorIndex cmd)
    {
        transparentColor = cmd.ColorIndex;
        Invalidate();
    }

    ARGBImageData ToImage()
    {
        var w = BufferWidth - 12;
        var h = BufferHeight - 24;
        var data = new ARGBImageData(w, h);
        var targetOffset = 0;
        var sourceOffset = (BufferWidth * (12 + offsetVertical)) + offsetHorizontal + 6;
        for (var y = 0; y < h; y++)
        {
            for (var x = 0; x < w; x++)
            {
                var colorIndex = buffer[sourceOffset + x];
                ARGB color;
                if (TransparentColorOverride && ((colorIndex == transparentColor) || (colorIndex == clearColor)))
                {
                    color = TransparentColorValue;
                }
                else
                {
                    color = palette[colorIndex];
                    color.Alpha = GlobalAlpha;
                }
                data[targetOffset++] = color;
                sourceOffset += BufferWidth;
            }
        }
        return data;
    }

    #endregion Private Methods

    #region Public Constructors

    /// <summary>Initializes a new instance of the <see cref="SynchronizedLyricsBackbufferV1"/> class.</summary>
    public SynchronizedLyricsBackbufferV1()
    {
        GlobalAlpha = 255;
    }

    #endregion Public Constructors

    #region Public Properties

    /// <summary>Gets or sets the global alpha value (0 = transparent, 255 = opaque).</summary>
    /// <value>The alpha.</value>
    public byte GlobalAlpha { get; set; }

    /// <summary>Gets the size of the screen.</summary>
    /// <value>The size of the screen.</value>
    public Size ScreenSize => new Size(ScreenWidth, ScreenHeight);

    /// <summary>Gets or sets a value indicating whether [to use transparent color override].</summary>
    /// <value><c>true</c> if [transparent color override]; otherwise, <c>false</c>.</value>
    /// <remarks>Transparent color override replaces all background colors with the value specified by <see cref="TransparentColorValue"/>.</remarks>
    public bool TransparentColorOverride { get; set; }

    /// <summary>Gets or sets the transparent color.</summary>
    /// <value>The transparent color.</value>
    public ARGB TransparentColorValue { get; set; }

    /// <summary>Gets a value indicating whether this <see cref="ISynchronizedLyricsBackbuffer"/> was updated by a play command.</summary>
    /// <value><c>true</c> if updated; otherwise, <c>false</c>.</value>
    public bool Updated { get; private set; }

    #endregion Public Properties

    #region Public Methods

    /// <summary>Invalidates this instance.</summary>
    public void Invalidate()
    {
        bitmap?.Dispose();
        bitmap = null;
        Updated = true;
    }

    /// <summary>Plays the specified sl.</summary>
    /// <param name="sl">The sl.</param>
    public void Play(SynchronizedLyricsItem sl)
    {
        foreach (var cmd in sl.Commands)
        {
            Play(cmd);
        }
    }

    /// <summary>Plays the specified command.</summary>
    /// <param name="cmd">The command.</param>
    /// <exception cref="NotImplementedException"></exception>
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

    /// <summary>Copies the image to the specified bitmapdata instance.</summary>
    public IBitmap32 ToBitmap()
    {
        if (Updated || bitmap == null)
        {
            bitmap = ToImage().ToBitmap32();
        }
        return bitmap;
    }

    #endregion Public Methods
}
