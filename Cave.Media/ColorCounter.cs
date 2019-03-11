using System;
using System.Collections.Generic;

namespace Cave.Media
{
    /// <summary>
    /// Provides color counting and reduction.
    /// </summary>
    /// <seealso cref="IComparable" />
    /// <seealso cref="IComparable{ColorCounter}" />
    public class ColorCounter : IComparable, IComparable<ColorCounter>
    {
        /// <summary>Reduces the specified color counters.</summary>
        /// <param name="colorCounters">The color counters.</param>
        /// <param name="distance">The distance.</param>
        /// <returns></returns>
        public static List<ColorCounter> Reduce(List<ColorCounter> colorCounters, ref uint distance)
        {
            uint nextDistance = uint.MaxValue;
            List<ColorCounter> result = new List<ColorCounter>();
            for (int x = 0; x < colorCounters.Count; x++)
            {
                ColorCounter c1 = colorCounters[x];
                for (int y = colorCounters.Count - 1; y > x; y--)
                {
                    ColorCounter c2 = colorCounters[y];
                    uint dist = (uint)Math.Abs(c1.Color.CompareTo(c2.Color));
                    if (dist <= distance)
                    {
                        c1.Count += c2.Count;
                        colorCounters.RemoveAt(y);
                        continue;
                    }
                    else
                    {
                        nextDistance = Math.Min(nextDistance, dist);
                    }
                }
                result.Add(c1);
            }
            distance = nextDistance;
            return result;
        }

        /// <summary>Initializes a new instance of the <see cref="ColorCounter"/> class.</summary>
        /// <param name="color">The color.</param>
        /// <param name="count">The count.</param>
        public ColorCounter(ARGB color, int count)
        {
            Color = color;
            Count = count;
        }

        /// <summary>The color.</summary>
        public ARGB Color;

        /// <summary>The count.</summary>
        public int Count;

        /// <summary>Compares to another instance.</summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public int CompareTo(object other)
        {
            if (other is ColorCounter) { return CompareTo((ColorCounter)other); }
            return other is ARGB ? CompareTo((ARGB)other) : -1;
        }

        /// <summary>Compares to another instance.</summary>
        /// <param name="other">The other instance.</param>
        /// <returns></returns>
        public int CompareTo(ColorCounter other)
        {
            int check = -Count.CompareTo(other.Count);
            return check != 0 ? check : Color.CompareTo(other.Color);
        }

        /// <summary>Returns a <see cref="System.String" /> that represents this instance.</summary>
        /// <returns>A <see cref="System.String" /> that represents this instance.</returns>
        public override string ToString()
        {
            return Color.ToHexString() + ", " + Count.ToString();
        }
    }
}
