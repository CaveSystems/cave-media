using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Runtime.InteropServices;
using Cave.IO;

namespace Cave.Media
{
    /// <summary>
    /// Provides overloaded access to a 32 bit int, uint, argb, rgb or color value.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 4)]
    public struct ARGB : IComparable, IComparable<int>, IComparable<ARGB>
    {
        #region static
        #region static operators

        /// <summary>
        /// Converts an ARGB structure to a color structure.
        /// </summary>
        /// <param name="value">color in argb format.</param>
        /// <returns>convertet color.</returns>
        public static implicit operator Color(ARGB value)
        {
            return Color.FromArgb(value.AsInt32);
        }

        /// <summary>
        /// Converts a Color structure to an ARGB structure.
        /// </summary>
        /// <param name="value">color in <see cref="Color"/>format.</param>
        /// <returns>convertet color.</returns>
        public static implicit operator ARGB(Color value)
        {
            return FromColor(value);
        }

        /// <summary>
        /// Converts a Color structure to an ARGB structure.
        /// </summary>
        /// <param name="value">color as <see cref="int"/> value.</param>
        /// <returns>converted color.</returns>
        public static implicit operator ARGB(int value)
        {
            return FromValue(value);
        }

        /// <summary>
        /// Converts a Color structure to an ARGB structure.
        /// </summary>
        /// <param name="value">color as <see cref="uint"/> value.</param>
        /// <returns>converted color.</returns>
        public static implicit operator ARGB(uint value)
        {
            return FromValue(value);
        }

        /// <summary>
        /// Compares the values of the two specified ARGB instances and returns true if the values are equal.
        /// </summary>
        /// <param name="v1">first color.</param>
        /// <param name="v2">second color.</param>
        /// <returns>true if the colors are equal.</returns>
        public static bool operator ==(ARGB v1, ARGB v2)
        {
            return v1.AsUInt32 == v2.AsUInt32;
        }

        /// <summary>
        /// Compares the values of the two specified ARGB instances and returns true if the values are inequal.
        /// </summary>
        /// <param name="v1">first color.</param>
        /// <param name="v2">second color.</param>
        /// <returns>true if the colors are not equal.</returns>
        public static bool operator !=(ARGB v1, ARGB v2)
        {
            return v1.AsUInt32 != v2.AsUInt32;
        }

        /// <summary>
        /// Adds two ARGB instances.
        /// </summary>
        /// <param name="v1">first color.</param>
        /// <param name="v2">second color.</param>
        /// <returns>sum of the colors.</returns>
        public static ARGB operator +(ARGB v1, ARGB v2)
        {
            ARGB result = new ARGB
            {
                Alpha = (byte)Math.Min(255, v1.Alpha + v2.Alpha),
                Red = (byte)Math.Min(255, v1.Red + v2.Red),
                Green = (byte)Math.Min(255, v1.Green + v2.Green),
                Blue = (byte)Math.Min(255, v1.Blue + v2.Blue),
            };
            return result;
        }

        /// <summary>
        /// Subtracts two ARGB instances.
        /// </summary>
        /// <param name="v1">first color.</param>
        /// <param name="v2">second color.</param>
        /// <returns>difference of the colors.</returns>
        public static ARGB operator -(ARGB v1, ARGB v2)
        {
            ARGB result = new ARGB
            {
                Alpha = (byte)Math.Max(0, v1.Alpha - v2.Alpha),
                Red = (byte)Math.Max(0, v1.Red - v2.Red),
                Green = (byte)Math.Max(0, v1.Green - v2.Green),
                Blue = (byte)Math.Max(0, v1.Blue - v2.Blue),
            };
            return result;
        }

        /// <summary>Implements the operator x &lt; y.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator <(ARGB x, ARGB y)
        {
            return x.AsUInt32 < y.AsUInt32;
        }

        /// <summary>Implements the operator x &gt; y.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static bool operator >(ARGB x, ARGB y)
        {
            return x.AsUInt32 > y.AsUInt32;
        }

        /// <summary>Implements the operator x | y.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static ARGB operator |(ARGB x, ARGB y)
        {
            return FromValue(x.AsUInt32 | y.AsUInt32);
        }

        /// <summary>Implements the operator x &amp; y.</summary>
        /// <param name="x">The x.</param>
        /// <param name="y">The y.</param>
        /// <returns>The result of the operator.</returns>
        public static ARGB operator &(ARGB x, ARGB y)
        {
            return FromValue(x.AsUInt32 & y.AsUInt32);
        }

        /// <summary>Implements the operator / division without alpha.</summary>
        /// <param name="c">The color.</param>
        /// <param name="v">The value to devide the rgb values with.</param>
        /// <returns>The result of the operator.</returns>
        public static ARGB operator /(ARGB c, float v)
        {
            return new ARGB()
            {
                Alpha = 255,
                RedFloat = c.RedFloat / v,
                GreenFloat = c.GreenFloat / v,
                BlueFloat = c.BlueFloat / v,
            };
        }

        /// <summary>Implements the operator * multiply without alpha.</summary>
        /// <param name="c">The color.</param>
        /// <param name="v">The value to multiply the rgb values with.</param>
        /// <returns>The result of the operator.</returns>
        public static ARGB operator *(ARGB c, float v)
        {
            return new ARGB()
            {
                Alpha = 255,
                RedFloat = c.RedFloat * v,
                GreenFloat = c.GreenFloat * v,
                BlueFloat = c.BlueFloat * v,
            };
        }

        #endregion

        #region struct operators

        /// <summary>Adds values of the specified other instance.</summary>
        /// <param name="other">The other instance.</param>
        public void Add(ARGB other)
        {
            Alpha = (byte)Math.Min(255, Alpha + other.Alpha);
            Red = (byte)Math.Min(255, Red + other.Red);
            Green = (byte)Math.Min(255, Green + other.Green);
            Blue = (byte)Math.Min(255, Blue + other.Blue);
        }

        /// <summary>
        /// Subtracts another ARGB instance.
        /// </summary>
        /// <param name="other">The other instance.</param>
        public void Substract(ARGB other)
        {
            Alpha = (byte)Math.Max(0, Alpha - other.Alpha);
            Red = (byte)Math.Max(0, Red - other.Red);
            Green = (byte)Math.Max(0, Green - other.Green);
            Blue = (byte)Math.Max(0, Blue - other.Blue);
        }
        #endregion

        #region static properties

        /// <summary>
        /// Gets a random color.
        /// </summary>
        public static ARGB Random
        {
            get
            {
                unchecked
                {
                    float hue = DefaultRNG.UInt32 % 36000 / 36000f;

                    const float _1_3 = 1f / 3f;
                    const float _60 = 3.141593f * 2f / 6f;

                    ARGB result = new ARGB
                    {
                        Alpha = 255,
                    };
                    if (hue < _1_3)
                    {
                        float rad = hue * (3.141593f * 2f);
                        float r = _1_3 * (1 + ((float)Math.Cos(rad) / (float)Math.Cos(_60 - rad)));
                        result.RedFloat = r;
                        result.GreenFloat = 1 - r;
                    }
                    else if (hue < 2f * _1_3)
                    {
                        float rad = (hue - _1_3) * (3.141593f * 2f);
                        float g = _1_3 * (1 + ((float)Math.Cos(rad) / (float)Math.Cos(_60 - rad)));
                        result.GreenFloat = g;
                        result.BlueFloat = 1 - g;
                    }
                    else
                    {
                        float rad = (hue - (2f * _1_3)) * (3.141593f * 2f);
                        float b = _1_3 * (1 + ((float)Math.Cos(rad) / (float)Math.Cos(_60 - rad)));
                        result.BlueFloat = b;
                        result.RedFloat = 1 - b;
                    }
                    return result;
                }
            }
        }

        /// <summary>
        /// Gets a transparent value.
        /// </summary>
        public static ARGB Transparent
        {
            get { return default(ARGB); }
        }
        #endregion

        #region static functions

        /// <summary>
        /// Returns a light color r,g,b = 128..255 for the specified index shaded with the specified number of steps.
        /// </summary>
        /// <param name="index">Index of the color.</param>
        /// <param name="steps">Steps to go from dark to light colors.</param>
        /// <returns>light color.</returns>
        public static ARGB GetLightColor(int index, int steps)
        {
            // 2 bits per color
            int r = 0;
            int b = 0;
            int g = 0;
            int v = index;
            while (v > 0)
            {
                r = (r << 1) | (v & 1);
                v >>= 1;
                g = (g << 1) | (v & 1);
                v >>= 1;
                b = (b << 1) | (v & 1);
                v >>= 1;
            }
            while (true)
            {
                if (r > steps) { g += r % steps; }
                if (g > steps) { b += g % steps; }
                if (b > steps) { r += b % steps; } else { break; }
            }
            ARGB result = new ARGB
            {
                Alpha = 0xFF,
                RedFloat = ((r % steps) / (3f * steps)) + 0.667f,
                GreenFloat = ((g % steps) / (3f * steps)) + 0.667f,
                BlueFloat = ((b % steps) / (3f * steps)) + 0.667f,
            };
            return result;
        }

        /// <summary>
        /// Returns a light color r,g,b = 0..127 for the specified index shaded with the specified number of steps.
        /// </summary>
        /// <param name="index">Index of the color.</param>
        /// <param name="steps">Steps to go from dark to light colors.</param>
        /// <returns>the light color.</returns>
        public static ARGB GetDarkColor(int index, int steps)
        {
            // 2 bits per color
            int r = 0;
            int b = 0;
            int g = 0;
            int v = index;
            while (v > 0)
            {
                r = (r << 1) | (v & 1);
                v >>= 1;
                g = (g << 1) | (v & 1);
                v >>= 1;
                b = (b << 1) | (v & 1);
                v >>= 1;
            }
            while (true)
            {
                if (r > steps) { g += r % steps; }
                if (g > steps) { b += g % steps; }
                if (b > steps) { r += b % steps; }
                else { break; }
            }
            ARGB result = new ARGB
            {
                Alpha = 0xFF,
                RedFloat = (r % steps) / (3f * steps),
                GreenFloat = (g % steps) / (3f * steps),
                BlueFloat = (b % steps) / (3f * steps),
            };
            return result;
        }

        /// <summary>Mixes the specified colors.</summary>
        /// <param name="colors">The colors to mix.</param>
        /// <returns>new mixed color.</returns>
        public static ARGB Mix(params ARGB[] colors)
        {
            return Mix((IEnumerable<ARGB>)colors);
        }

        /// <summary>Mixes the specified colors.</summary>
        /// <param name="colors">The colors to mix.</param>
        /// <returns>new mixed color.</returns>
        public static ARGB Mix(IEnumerable<ARGB> colors)
        {
            float red = 0, green = 0, blue = 0, alpha = 0;
            foreach (ARGB color in colors)
            {
                float alphaDiv = alpha + color.AlphaFloat;
                red = ((red * alpha) + (color.RedFloat * color.AlphaFloat)) / alphaDiv;
                green = ((green * alpha) + (color.GreenFloat * color.AlphaFloat)) / alphaDiv;
                blue = ((blue * alpha) + (color.BlueFloat * color.AlphaFloat)) / alphaDiv;
                alpha = 1;
            }
            return new ARGB() { AlphaFloat = alpha, RedFloat = red, BlueFloat = blue, GreenFloat = green };
        }

        /// <summary>
        /// Loads (copies) the struct from a specified pointer.
        /// </summary>
        /// <param name="value">pointer with color data.</param>
        /// <returns>argb color.</returns>
        public static unsafe ARGB FromPointer(void* value)
        {
            if (value == null) { throw new ArgumentNullException("Value"); }
            ARGB result = new ARGB
            {
                AsUInt32 = *((uint*)value),
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from a specified value.
        /// </summary>
        /// <param name="value">color value.</param>
        /// <returns>new argb color.</returns>
        public static ARGB FromValue(uint value)
        {
            ARGB result = new ARGB
            {
                AsUInt32 = value,
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from a specified value.
        /// </summary>
        /// <param name="value">color value.</param>
        /// <returns>new argb color.</returns>
        public static ARGB FromValue(int value)
        {
            ARGB result = new ARGB
            {
                AsInt32 = value,
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from a color.
        /// </summary>
        /// <param name="color">color value.</param>
        /// <returns>new argb color.</returns>
        public static ARGB FromColor(Color color)
        {
            ARGB result = new ARGB
            {
                AsInt32 = color.ToArgb(),
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from a specified string.
        /// The string may start with '#' (HtmlColor), '0x' (default hexadecimal), be a plain hexadecimal value or
        /// any known color name (e.g. black, white, ...)
        /// </summary>
        /// <param name="text">color as string.</param>
        /// <returns>color from string.</returns>
        public static ARGB FromString(string text)
        {
            if (text == null) { throw new ArgumentNullException("String"); }
            ARGB result = default(ARGB);
            if (text.StartsWith("#"))
            {
                result.AsUInt32 = Convert.ToUInt32(text.Substring(1), 16);
                return result;
            }

            if (text.StartsWith("0x"))
            {
                result.AsUInt32 = Convert.ToUInt32(text.Substring(2), 16);
                return result;
            }

            try
            {
                result.AsInt32 = Color.FromName(text).ToArgb();
                return result;
            }
            catch { }
            try
            {
                result.AsUInt32 = Convert.ToUInt32(text, 16);
                return result;
            }
            catch { }
            throw new Exception(string.Format("Could not parse string '{0}' with any known format!", text));
        }

        /// <summary>
        /// Loads (copies) the struct from specified gray value.
        /// </summary>
        /// <param name="gray">gray color value.</param>
        /// <returns>new argb color.</returns>
        public static ARGB FromGrayScale(byte gray)
        {
            ARGB result = new ARGB
            {
                Alpha = 255,
                Red = gray,
                Green = gray,
                Blue = gray,
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from specified red, green and blue values.
        /// </summary>
        /// <param name="red">Red.</param>
        /// <param name="green">Green.</param>
        /// <param name="blue">Blue.</param>
        /// <returns>new argb color.</returns>
        public static ARGB FromColor(byte red, byte green, byte blue)
        {
            ARGB result = new ARGB
            {
                Alpha = 255,
                Red = red,
                Green = green,
                Blue = blue,
            };
            return result;
        }

        /// <summary>
        /// Loads (copies) the struct from specified alpha, red, green and blue values.
        /// </summary>
        /// <param name="alpha">Alpha.</param>
        /// <param name="red">Red.</param>
        /// <param name="green">Green.</param>
        /// <param name="blue">Blue.</param>
        /// <returns>new argb color struct.</returns>
        public static ARGB FromColor(byte alpha, byte red, byte green, byte blue)
        {
            ARGB result = new ARGB
            {
                Alpha = alpha,
                Red = red,
                Green = green,
                Blue = blue,
            };
            return result;
        }

        /// <summary>
        /// Calculates a color from the specified heat.
        /// </summary>
        /// <param name="heat">Heat value 0..1.</param>
        /// <returns>Returns a new ARGB struct.</returns>
        public static ARGB FromHeat(float heat)
        {
            if (heat < 0)
            {
                throw new ArgumentOutOfRangeException(nameof(heat));
            }

            // scale to the 5 color ramps
            heat = heat * 5;
            byte col = (byte)(heat % 1f * 255f);
            switch ((int)heat)
            {
                // black..blue:      0..1 blue
                case 0: return FromColor(0, 0, col);

                // blue..cyan:       1 blue, 0..1 green
                case 1: return FromColor(0, col, 255);

                // cyan..yellow:     1 green, 1..0 blue, 0..1 red
                case 2: return FromColor(col, 255, (byte)(255 - col));

                // yellow..red:      1 red, 1..0 green
                case 3: return FromColor(255, (byte)(255 - col), 0);

                // red..white:       1 red, 0..1 green+blue
                case 4:
                default: return FromColor(255, col, col);
            }
        }

        /// <summary>
        /// Calculates a color from hue [0..360], saturation [0..1] and intensity [0..1].
        /// </summary>
        /// <param name="hue">hue (0..360).</param>
        /// <param name="saturation">saturation (0..1).</param>
        /// <param name="intensity">intensity (0..1).</param>
        /// <returns>Returns a new ARGB struct.</returns>
        public static ARGB FromHSI(float hue, float saturation, float intensity)
        {
            float f = hue / 60f;
            int hi = (int)f % 6;
            f = f % 1;

            intensity = intensity * 255;
            byte v = (byte)intensity;
            byte p = (byte)(intensity * (1 - saturation));
            byte q = (byte)(intensity * (1 - (f * saturation)));
            byte t = (byte)(intensity * (1 - ((1 - f) * saturation)));

            if (hi == 0) { return FromColor(255, v, t, p); }
            else if (hi == 1) { return FromColor(255, q, v, p); }
            else if (hi == 2) { return FromColor(255, p, v, t); }
            else if (hi == 3) { return FromColor(255, p, q, v); }
            else if (hi == 4) { return FromColor(255, t, p, v); }
            else { return FromColor(255, v, p, q); }
        }

        #endregion
        #endregion

        #region struct

        #region struct fields

        /// <summary>
        /// Obtains the blue value of the integer.
        /// </summary>
        [FieldOffset(0)]
        public byte Blue;

        /// <summary>
        /// Obtains the green value of the integer.
        /// </summary>
        [FieldOffset(1)]
        public byte Green;

        /// <summary>
        /// Obtains the red value of the integer.
        /// </summary>
        [FieldOffset(2)]
        public byte Red;

        /// <summary>
        /// Obtains the alpha value of the integer.
        /// </summary>
        [FieldOffset(3)]
        public byte Alpha;

        /// <summary>
        /// Obtains the unsigned integer value.
        /// </summary>
        [FieldOffset(0)]
        public uint AsUInt32;

        /// <summary>
        /// Obtains the integer value.
        /// </summary>
        [FieldOffset(0)]
        public int AsInt32;

        /// <summary>
        /// Obtains the high word value.
        /// </summary>
        [FieldOffset(0)]
        public ushort HiWord;

        /// <summary>
        /// Obtains the low word value.
        /// </summary>
        [FieldOffset(2)]
        public ushort LoWord;

        /// <summary>
        /// Obtains the high word as signed short value.
        /// </summary>
        [FieldOffset(0)]
        public short HiShort;

        /// <summary>
        /// Obtains the low word as signed short value.
        /// </summary>
        [FieldOffset(2)]
        public short LoShort;

        #endregion

        #region struct properties

        /// <summary>
        /// Gets the color at the hue color table.
        /// </summary>
        public HueColor HueColor
        {
            get
            {
                ToHSL(out float hue, out float saturation, out float luminance);
                if (luminance == 0) { return HueColor.Dark; }
                if (saturation < 0.1f)
                {
                    // gray
                    if (luminance < 0.5f) { return HueColor.Dark; }
                    else { return HueColor.Light; }
                }
                if (luminance > 0.9f) { return HueColor.Light; }

                // Hue 0..1 = 0..360° -> -30°..30° = index 1
                // index = (Hue * 360 - 30) / 60 + 1
                int col = 1 + (int)(((hue * 360) + 30) / 60);
                if (col == 0) { col = 1; }
                else if (col > 6) { col = 1; }
                return (HueColor)col;
            }
        }

        /// <summary>Gets a value indicating whether the color looks gray.</summary>
        /// <value><c>true</c> if this instance looks gray; otherwise, <c>false</c>.</value>
        public bool IsGray
        {
            get
            {
                int redDistance = Math.Abs(Red - Blue) + Math.Abs(Red - Green);
                int blueDistance = Math.Abs(Blue - Red) + Math.Abs(Blue - Green);
                int greenDistance = Math.Abs(Green - Blue) + Math.Abs(Green - Red);
                return redDistance * 299 < 25500 && blueDistance * 114 < 25500 && greenDistance * 587 < 25500;
            }
        }

        /// <summary>
        /// Gets a value indicating whether the color looks red.
        /// </summary>
        public bool IsRed
        {
            get
            {
                if (IsGray) { return false; }
                return (Red > Blue + 25) && (Red > Green + 25);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the color looks blue.
        /// </summary>
        public bool IsBlue
        {
            get
            {
                if (IsGray) { return false; }
                return (Blue > Red + 25) && (Blue > Green + 25);
            }
        }

        /// <summary>
        /// Gets a value indicating whether the color looks green.
        /// </summary>
        public bool IsGreen
        {
            get
            {
                if (IsGray) { return false; }
                return (Green > Blue + 25) && (Green > Red + 25);
            }
        }

        #endregion

        #region struct functions

        /// <summary>
        /// Provides a html color string.
        /// </summary>
        /// <returns>color as hex string in html format.</returns>
        public string ToHtmlColor()
        {
            return string.Format("#{0:X}", AsUInt32 & 0xFFFFFF);
        }

        /// <summary>
        /// Provides a default hexadecimal string starting with 0x.
        /// </summary>
        /// <returns>color as hex string.</returns>
        public string ToHexString()
        {
            return string.Format("0x{0:X}", AsUInt32);
        }

        /// <summary>
        /// (Re-)sets a bit at the specified index.
        /// </summary>
        /// <param name="bitIndex">the bit index.</param>
        /// <param name="items">if true sets the bit, else unsets the bit.</param>
        public void SetBit(int bitIndex, bool items)
        {
            uint mask = (uint)1 << bitIndex;
            if (items)
            {
                AsUInt32 |= mask;
            }
            else
            {
                AsUInt32 &= ~mask;
            }
        }

        /// <summary>
        /// Sets a specific color value.
        /// </summary>
        /// <param name="color">color to set.</param>
        public void Set(Color color)
        {
            AsInt32 = color.ToArgb();
        }

        /// <summary>
        /// Gets if a bit was set at the specified index.
        /// </summary>
        /// <param name="bitIndex">the bit index.</param>
        /// <returns>true if bit is set.</returns>
        public bool GetBit(int bitIndex)
        {
            return ((AsUInt32 >> bitIndex) & 1) != 0;
        }

        /// <summary>
        /// Gets a color with the structs values.
        /// </summary>
        /// <returns>represented color.</returns>
        public Color ToColor()
        {
            return Color.FromArgb(AsInt32);
        }

        /// <summary>
        /// Provides the grayscale representation of the color.
        /// </summary>
        /// <returns>gray color.</returns>
        public ARGB ToGrayScale()
        {
            var col = GrayScaleByte;
            return new ARGB()
            {
                Alpha = 255,
                Red = col,
                Green = col,
                Blue = col,
            };
        }
        #endregion

        #region struct properties

        /// <summary>
        /// Gets a gray scaled byte value with the structs color values.
        /// </summary>
        public byte GrayScaleByte
        {
            get
            {
                int result = (((299 * Red) + (587 * Green) + (114 * Blue)) * Alpha) / 255000;
                return (byte)result;
            }
        }

        /// <summary>
        /// Gets a gray scaled percent value with the structs color values.
        /// </summary>
        public double GrayScaleDouble
        {
            get
            {
                return (((299 * Red) + (587 * Green) + (114 * Blue)) * Alpha) / (255000.0d * 255.0d);
            }
        }

        /// <summary>
        /// Gets a gray scaled percent value with the structs color values.
        /// </summary>
        public float GrayScaleFloat
        {
            get
            {
                return (((299 * Red) + (587 * Green) + (114 * Blue)) * Alpha) / (255000.0f * 255.0f);
            }
        }

        /// <summary>
        /// Gets or sets the red float (0..1).
        /// </summary>
        public float RedFloat
        {
            get
            {
                return Red / 255.0f;
            }
            set
            {
                float v = Math.Max(0, Math.Min(1, value));
                Red = (byte)(255.0f * v);
            }
        }

        /// <summary>
        /// Gets or sets the green float (0..1).
        /// </summary>
        public float GreenFloat
        {
            get
            {
                return Green / 255.0f;
            }
            set
            {
                float v = Math.Max(0, Math.Min(1, value));
                Green = (byte)(255.0f * v);
            }
        }

        /// <summary>
        /// Gets or sets the blue float (0..1).
        /// </summary>
        public float BlueFloat
        {
            get
            {
                return Blue / 255.0f;
            }
            set
            {
                float v = Math.Max(0, Math.Min(1, value));
                Blue = (byte)(255.0f * v);
            }
        }

        /// <summary>Swaps the red and the blue value.</summary>
        public void SwapRedBlue()
        {
            byte b = Blue;
            Blue = Red;
            Red = b;
        }

        /// <summary>Gets the inverse color.</summary>
        /// <value>The inverse color.</value>
        public ARGB Inverse
        {
            get
            {
                return FromColor(Alpha, (byte)(255 - Red), (byte)(255 - Green), (byte)(255 - Blue));
            }
        }

        /// <summary>
        /// Gets or sets the alpha percentage.
        /// </summary>
        public float AlphaFloat
        {
            get
            {
                return Alpha / 255.0f;
            }
            set
            {
                float v = Math.Max(0, Math.Min(1, value));
                Alpha = (byte)(255.0f * v);
            }
        }

        /// <summary>
        /// Gets or sets the brightness.
        /// </summary>
        public float Brightness
        {
            get
            {
                return (0.2126f * RedFloat) + (0.7152f * GreenFloat) + (0.0722f * BlueFloat);
            }
            set
            {
                float difference = value - Brightness;
                RedFloat = RedFloat + (difference * 0.2126f);
                GreenFloat = GreenFloat + (difference * 0.7152f);
                BlueFloat = BlueFloat + (difference * 0.0722f);
            }
        }

        #endregion

        #endregion

        /// <summary>
        /// Get hue, saturation and value for this color.
        /// </summary>
        /// <param name="hue">Hue (0..1).</param>
        /// <param name="saturation">Saturation (0..1).</param>
        /// <param name="value">Value (0..1).</param>
        public void ToHSV(out float hue, out float saturation, out float value)
        {
            float r = RedFloat;
            float g = GreenFloat;
            float b = BlueFloat;

            var rgb = new float[] { r, g, b };

            var min = rgb.Min();
            var max = rgb.Max();
            var deltaMax = max - min;
            value = deltaMax;

            if (deltaMax == 0)
            {
                hue = 0;
                saturation = 0;
            }
            else
            {
                saturation = deltaMax / max;

                var deltaR = (((max - r) / 6) + (deltaMax / 2)) / deltaMax;
                var deltaG = (((max - g) / 6) + (deltaMax / 2)) / deltaMax;
                var deltaB = (((max - b) / 6) + (deltaMax / 2)) / deltaMax;

                if (r == max)
                {
                    hue = deltaB - deltaG;
                }
                else if (g == max)
                {
                    hue = (1 / 3) + deltaR - deltaB;
                }
                else if (b == max)
                {
                    hue = (2 / 3) + deltaG - deltaR;
                }
                else
                {
                    throw new Exception("Calculation error!");
                }

                if (hue < 0)
                {
                    hue += 1;
                }

                if (hue > 1)
                {
                    hue -= 1;
                }
            }
        }

        /// <summary>
        /// Get hue, saturation and luminance for this color.
        /// </summary>
        /// <param name="hue">Hue (0..1).</param>
        /// <param name="saturation">Saturation (0..1).</param>
        /// <param name="luminance">Luminance (0..1).</param>
        public void ToHSL(out float hue, out float saturation, out float luminance)
        {
            float r = RedFloat;
            float g = GreenFloat;
            float b = BlueFloat;

            var rgb = new float[] { r, g, b };

            var min = rgb.Min();
            var max = rgb.Max();
            var deltaMax = max - min;

            luminance = (max + min) / 2;

            if (deltaMax == 0)
            {
                hue = 0;
                saturation = 0;
            }
            else
            {
                if (luminance < 0.5) { saturation = deltaMax / (max + min); }
                else { saturation = deltaMax / (2 - max - min); }

                var deltaR = (((max - r) / 6) + (deltaMax / 2)) / deltaMax;
                var deltaG = (((max - g) / 6) + (deltaMax / 2)) / deltaMax;
                var deltaB = (((max - b) / 6) + (deltaMax / 2)) / deltaMax;

                if (r == max) { hue = deltaB - deltaG; }
                else if (g == max) { hue = (1 / 3) + deltaR - deltaB; }
                else if (b == max) { hue = (2 / 3) + deltaG - deltaR; }
                else { throw new Exception("Calculation error!"); }

                if (hue < 0) { hue += 1; }
                if (hue > 1) { hue -= 1; }
            }
        }

        /// <summary>Gets the RGB distance.</summary>
        /// <param name="other">The other.</param>
        /// <returns>distance as <see cref="float"/>.</returns>
        public float GetDistance(ARGB other)
        {
            float r = (Red - other.Red) / 255f;
            float g = (Green - other.Green) / 255f;
            float b = (Blue - other.Blue) / 255f;
            return Math.Min(1, Math.Max(-1, r * g * b));
        }

        /// <summary>
        /// gets the contrast ratio ranging from 0 for no contrast to 1 for highest contrast.
        /// </summary>
        /// <param name="other">the other color.</param>
        /// <returns>contrast ratio of the two colors.</returns>
        /// <remarks>
        /// smaller than 0.143 -> fail,
        /// between 0.143 and 0.214 -> minimum,
        /// between 0.214 and 0.333 -> good,
        /// greater than 0.333 -> perfect.
        /// </remarks>
        public float GetContrastRatio(ARGB other)
        {
            float l1 = Brightness;
            float l2 = other.Brightness;
            if (l2 > l1)
            {
                float t = l1;
                l1 = l2;
                l2 = t;
            }
            return (l1 + 0.05f) / (l2 + 0.05f) / 21f;
        }

        #region interfaces
        #region object overrides

        /// <summary>
        /// Obtains a hex string for the color.
        /// </summary>
        /// <returns>color as hex string.</returns>
        public override string ToString()
        {
            return ToHexString();
        }

        /// <summary>
        /// Checks for equality with another object.
        /// </summary>
        /// <param name="obj">the other object.</param>
        /// <returns>true if objects are equal.</returns>
        public override bool Equals(object obj)
        {
            if (obj is ARGB other)
            {
                return other.AsUInt32 == AsUInt32;
            }
            return false;
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        /// <returns>hashcode as <see cref="int"/>.</returns>
        public override int GetHashCode()
        {
            return AsInt32;
        }

        #endregion

        #region IComparable Member

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with.</param>
        /// <returns>compare value.</returns>
        public int CompareTo(object other)
        {
            return other is ARGB ? CompareTo((ARGB)other) : -1;
        }

        #endregion

        #region IComparable<int> Member

        /// <summary>
        /// Compares the current instance with another object of the same type and returns an integer that indicates whether the current instance precedes, follows, or occurs in the same position in the sort order as the other object.
        /// </summary>
        /// <param name="other">The object to compare with.</param>
        /// <returns>compare value.</returns>
        public int CompareTo(int other)
        {
            return CompareTo(FromValue(other));
        }

        #endregion

        #region IComparable<ARGB> Member

        /// <summary>
        /// Compares two ARGB instances.
        /// </summary>
        /// <param name="other">other argb color.</param>
        /// <returns>compare value.</returns>
        public int CompareTo(ARGB other)
        {
            return (299 * (Red - other.Red)) + (587 * (Green - other.Green)) + (114 * (Blue - other.Blue));
        }

        #endregion
        #endregion
    }
}
