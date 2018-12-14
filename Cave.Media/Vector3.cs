using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Cave.Media
{
    /// <summary>
    /// Provides a simple 3d vector based on float values
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 3 * 4)]
    public struct Vector3
    {
        #region static constructors
        /// <summary>
        /// Parses a string and returns a new <see cref="Vector3"/>.
        /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
        /// Values have to be encoded with <see cref="CultureInfo.InvariantCulture"/>!
        /// </summary>
        /// <param name="text">String of the form bracket (optional) float separator float bracket (optional)</param>
        /// <returns>Returns a new <see cref="Vector3"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified string object is null</exception>
        /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount</exception>
        /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format</exception>
        /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/></exception>
        public static Vector3 Parse(string text)
        {
            return Parse(text, CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Parses a string and returns a new <see cref="Vector3"/>.
        /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
        /// </summary>
        /// <param name="text">String of the form bracket (optional) float separator float bracket (optional)</param>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> used to decode the float values</param>
        /// <returns>Returns a new <see cref="Vector3"/></returns>
        /// <exception cref="ArgumentNullException">Thrown if the specified string object is null</exception>
        /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount</exception>
        /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format</exception>
        /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/></exception>
        public static Vector3 Parse(string text, IFormatProvider cultureInfo)
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
        /// Creates a new <see cref="Vector3"/> object with the specified values
        /// </summary>
        /// <param name="x">x value</param>
        /// <param name="y">y value</param>
        /// <param name="z">z value</param>
        public static Vector3 Create(float x, float y, float z)
        {
            Vector3 l_Vector = new Vector3
            {
                X = x,
                Y = y,
                Z = z
            };
            return l_Vector;
        }

        /// <summary>
        /// Creates a new <see cref="Vector3"/> object with the specified values
        /// </summary>
        /// <param name="values">The values of the <see cref="Vector3"/></param>
        /// <exception cref="ArgumentException">Number of values do not match!</exception>
        public static Vector3 Create(float[] values)
        {
            if (values == null)
            {
                throw new ArgumentNullException("values");
            }

            if (values.Length != 3)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            Vector3 l_Vector = new Vector3
            {
                X = values[0],
                Y = values[1],
                Z = values[2]
            };
            return l_Vector;
        }

        /// <summary>
        /// Obtains an empty <see cref="Vector3"/> object
        /// </summary>
        public static Vector3 Empty => new Vector3();

        #endregion

        #region value access
        /// <summary>x</summary>
        [FieldOffset(0)]
        public float X;

        /// <summary>y</summary>
        [FieldOffset(4)]
        public float Y;

        /// <summary>z</summary>
        [FieldOffset(8)]
        public float Z;
        #endregion

        #region operators
        /// <summary>
        /// Checks two <see cref="Vector3"/> instances for equality
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool operator ==(Vector3 A, Vector3 B)
        {
            if (Equals(null, A))
            {
                return Equals(null, B);
            }

            return A.Equals(B);
        }

        /// <summary>
        /// Checks two <see cref="Vector3"/> instances for inequality
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static bool operator !=(Vector3 A, Vector3 B)
        {
            if (Equals(null, A))
            {
                return !Equals(null, B);
            }

            return !A.Equals(B);
        }

        /// <summary>
        /// provides addition
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 operator +(Vector3 A, Vector3 B)
        {
            return Create(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }

        /// <summary>
        /// provides addition
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Add(Vector3 A, Vector3 B)
        {
            return Create(A.X + B.X, A.Y + B.Y, A.Z + B.Z);
        }

        /// <summary>
        /// provides subtraction
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 operator -(Vector3 A, Vector3 B)
        {
            return Create(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }

        /// <summary>
        /// provides subtraction
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Subtract(Vector3 A, Vector3 B)
        {
            return Create(A.X - B.X, A.Y - B.Y, A.Z - B.Z);
        }

        /// <summary>
        /// provides multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 A, float B)
        {
            return Create(A.X * B, A.Y * B, A.Z * B);
        }

        /// <summary>
        /// provides multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Multiply(Vector3 A, float B)
        {
            return Create(A.X * B, A.Y * B, A.Z * B);
        }

        /// <summary>
        /// provides multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 operator *(Vector3 A, Vector3 B)
        {
            return Create(A.X * B.X, A.Y * B.Y, A.Z * B.Z);
        }

        /// <summary>
        /// provides multiplication
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Mulitply(Vector3 A, Vector3 B)
        {
            return Create(A.X * B.X, A.Y * B.Y, A.Z * B.Z);
        }

        /// <summary>
        /// provides division
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 operator /(Vector3 A, float B)
        {
            return Create(A.X / B, A.Y / B, A.Z / B);
        }

        /// <summary>
        /// provides division
        /// </summary>
        /// <param name="A"></param>
        /// <param name="B"></param>
        /// <returns></returns>
        public static Vector3 Divide(Vector3 A, float B)
        {
            return Create(A.X / B, A.Y / B, A.Z / B);
        }
        #endregion

        /// <summary>
        /// Calculates the length of the vector
        /// </summary>
        public float Length => (float)Math.Sqrt(X * X + Y * Y + Z * Z);

        /// <summary>
        /// Obtains a normalized version of the vector. (Length near 1.0f watch out for rounding errors!)
        /// </summary>
        /// <returns>Returns the normalized version of this vector</returns>
        public Vector3 Normalized => Create(X, Y, Z) / Length;

        /// <summary>
        /// Retrieves the vector values as array
        /// </summary>
        /// <returns>Returns the vector values as array</returns>
        public float[] ToArray()
        {
            return new float[] { X, Y, Z };
        }

        /// <summary>
        /// Obtains a string of the form '(x,y,z)'.
        /// The values are converted to strings using <see cref="CultureInfo.InvariantCulture"/>.
        /// </summary>
        /// <returns>Returns a string of the form '(x,y,z)'</returns>
        public override string ToString()
        {
            return ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Obtains a string of the form '(x,y,z)'.
        /// </summary>
        /// <param name="cultureInfo">The <see cref="CultureInfo"/> for encoding the float values</param>
        /// <returns>Returns a string of the form '(x,y,z)'</returns>
        public string ToString(IFormatProvider cultureInfo)
        {
            return string.Format("({0},{1},{2})", X.ToString(cultureInfo), Y.ToString(cultureInfo), Z.ToString(cultureInfo));
        }

        /// <summary>
        /// Checks another <see cref="Vector3"/> for equality
        /// </summary>
        /// <param name="obj">The <see cref="Vector3"/> instance to check for equality</param>
        /// <returns>Returns true if the specified object equals this one</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Vector3))
            {
                return false;
            }

            Vector3 other = (Vector3)obj;
            return (other.X == X) && (other.Y == Y) && (other.Z == Z);
        }

        /// <summary>
        /// Obtains the hash code for this instance
        /// </summary>
        /// <returns>Returns the hash code for this instance</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        /// <summary>
        /// Obtains a copy of this object
        /// </summary>
        /// <returns>Returns a copy of this object</returns>
        public Vector3 Clone()
        {
            return Create(X, Y, Z);
        }
    }
}
