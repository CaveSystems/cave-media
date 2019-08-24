using System;
using System.Globalization;

namespace Cave.Media
{
    /// <summary>
    /// Provides a simple 2d vector based on float values.
    /// </summary>
    public struct Vector2
    {
        #region static constructors

        /// <summary>
        /// Parses a string and returns a new <see cref="Vector2"/>.
        /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
        /// Values have to be encoded with <see cref="CultureInfo.InvariantCulture"/>!.
        /// </summary>
        /// <param name="text">String of the form bracket (optional) float separator float bracket (optional).</param>
        /// <returns>Returns a new <see cref="Vector2"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified string object is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount.</exception>
        /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format.</exception>
        /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/>.</exception>
        public static Vector2 Parse(string text)
        {
            return Parse(text, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a string and returns a new <see cref="Vector2"/>.
        /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
        /// </summary>
        /// <param name="text">String of the form bracket (optional) float separator float bracket (optional).</param>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> used to decode the float values.</param>
        /// <returns>Returns a new <see cref="Vector2"/>.</returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified string object is null.</exception>
        /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount.</exception>
        /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format.</exception>
        /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/>.</exception>
        public static Vector2 Parse(string text, IFormatProvider cultureInfo)
        {
            string[] strings = text.UnboxBrackets(true).Split(new char[] { ';', ',', '\t', ' ' });
            float[] values = new float[strings.Length];
            for (int i = 0; i < strings.Length; i++)
            {
                values[i] = float.Parse(strings[i], cultureInfo);
            }
            return Create(values);
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> object.
        /// </summary>
        /// <param name="x">x value.</param>
        /// <param name="y">y value.</param>
        public static Vector2 Create(float x, float y)
        {
            var result = new Vector2
            {
                X = x,
                Y = y,
            };
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Vector2"/> object with the specified values.
        /// </summary>
        /// <param name="values">The values of the <see cref="Vector2"/>.</param>
        /// <exception cref="ArgumentException">Number of values do not match!.</exception>
        public static Vector2 Create(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length != 2)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            var result = new Vector2
            {
                X = values[0],
                Y = values[1],
            };
            return result;
        }

        /// <summary>
        /// Gets an empty <see cref="Vector2"/> object.
        /// </summary>
        public static Vector2 Empty => default;

        #endregion

        #region value access

        /// <summary>x.</summary>
        public float X;

        /// <summary>y.</summary>
        public float Y;
        #endregion

        #region operators

        /// <summary>
        /// Checks two <see cref="Vector2"/> instances for equality.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool operator ==(Vector2 A, Vector2 B)
        {
            return Equals(null, A) ? Equals(null, B) : A.Equals(B);
        }

        /// <summary>
        /// Checks two <see cref="Vector2"/> instances for inequality.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool operator !=(Vector2 A, Vector2 B)
        {
            return Equals(null, A) ? !Equals(null, B) : !A.Equals(B);
        }

        /// <summary>
        /// Provides addition.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 operator +(Vector2 A, Vector2 B)
        {
            return Create(A.X + B.X, A.Y + B.Y);
        }

        /// <summary>
        /// Provides addition.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Add(Vector2 A, Vector2 B)
        {
            return Create(A.X + B.X, A.Y + B.Y);
        }

        /// <summary>
        /// Provides subtraction.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 operator -(Vector2 A, Vector2 B)
        {
            return Create(A.X - B.X, A.Y - B.Y);
        }

        /// <summary>
        /// Provides subtraction.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Subtract(Vector2 A, Vector2 B)
        {
            return Create(A.X - B.X, A.Y - B.Y);
        }

        /// <summary>
        /// Provides multiplication.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 A, float B)
        {
            return Create(A.X * B, A.Y * B);
        }

        /// <summary>
        /// Provides multiplication.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Multiply(Vector2 A, float B)
        {
            return Create(A.X * B, A.Y * B);
        }

        /// <summary>
        /// Provides multiplication.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 operator *(Vector2 A, Vector2 B)
        {
            return Create(A.X * B.X, A.Y * B.Y);
        }

        /// <summary>
        /// Provides multiplication.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Multiply(Vector2 A, Vector2 B)
        {
            return Create(A.X * B.X, A.Y * B.Y);
        }

        /// <summary>
        /// Provides division.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 operator /(Vector2 A, float B)
        {
            return Create(A.X / B, A.Y / B);
        }

        /// <summary>
        /// Provides division.
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector2 Divide(Vector2 A, float B)
        {
            return Create(A.X / B, A.Y / B);
        }
        #endregion

        /// <summary>
        /// Calculates the length of the vector.
        /// </summary>
        public float Length => (float)Math.Sqrt((X * X) + (Y * Y));

        /// <summary>
        /// Gets a normalized version of the vector. (Length near 1.0f watch out for rounding errors!).
        /// </summary>
        /// <returns>Returns the normalized version of this vector.</returns>
        public Vector2 Normalized => Create(X, Y) / Length;

        /// <summary>
        /// Retrieves the vector values as array.
        /// </summary>
        /// <returns>Returns the vector values as array.</returns>
        public float[] ToArray()
        {
            return new float[] { X, Y };
        }

        /// <summary>
        /// Gets a string of the form '(x,y)'.
        /// The values are converted to strings using <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <returns>Returns a string of the form '(x,y)'.</returns>
        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Gets a string of the form '(x,y)'.
        /// </summary>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> for encoding the float values.</param>
        /// <returns>Returns a string of the form '(x,y)'.</returns>
        public string ToString(IFormatProvider cultureInfo)
        {
            return string.Format("({0},{1})", X.ToString(cultureInfo), Y.ToString(cultureInfo));
        }

        /// <summary>
        /// Checks another <see cref="Vector2"/> for equality.
        /// </summary>
        /// <param name="obj">The <see cref="Vector2"/> instance to check for equality.</param>
        /// <returns>Returns true if the specified object equals this one.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector2))
            {
                return false;
            }

            var other = (Vector2)obj;
            return (other.X == X) && (other.Y == Y);
        }

        /// <summary>
        /// Gets the hash code for this instance.
        /// </summary>
        /// <returns>Returns the hash code for this instance.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region ICloneable Member

        /// <summary>
        /// Gets a copy of this object.
        /// </summary>
        /// <returns>Returns a copy of this object.</returns>
        public object Clone()
        {
            return Create(X, Y);
        }

        #endregion
    }
}
