using System;
using System.Globalization;
using System.Runtime.InteropServices;

namespace Cave.Media;

/// <summary>
/// Provides a homogeneous 3d vector with scale (4th value) based on float values.
/// </summary>
[StructLayout(LayoutKind.Explicit, Size = 4 * 4)]
public struct Vector4
{
    #region static constructors

    /// <summary>
    /// Parses a string and returns a new <see cref="Vector4"/>.
    /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
    /// Values have to be encoded with <see cref="CultureInfo.InvariantCulture"/>!.
    /// </summary>
    /// <param name="text">String of the form bracket (optional) float separator float bracket (optional).</param>
    /// <returns>Returns a new <see cref="Vector4"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified string object is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount.</exception>
    /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format.</exception>
    /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/>.</exception>
    public static Vector4 Parse(string text) => Parse(text, CultureInfo.InvariantCulture);

    /// <summary>
    /// Parses a string and returns a new <see cref="Vector4"/>.
    /// The values at the string may be enclosed in brackets and the following separators are accepted: ',' ';' '\t' or ' '.
    /// </summary>
    /// <param name="text">String of the form bracket (optional) float separator float bracket (optional).</param>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> used to decode the float values.</param>
    /// <returns>Returns a new <see cref="Vector4"/>.</returns>
    /// <exception cref="ArgumentNullException">Thrown if the specified string object is null.</exception>
    /// <exception cref="ArgumentException">Thrown if the valuecount at the string does not match the needed valuecount.</exception>
    /// <exception cref="FormatException">Thrown if the specified string contains a value / values with an invalid format.</exception>
    /// <exception cref="OverflowException">Thrown if one of the values if smaller <see cref="float.MinValue"/> or greater <see cref="float.MaxValue"/>.</exception>
    public static Vector4 Parse(string text, IFormatProvider cultureInfo)
    {
        var strings = text.UnboxBrackets(true).Split(new char[] { ';', ',', '\t', ' ' });
        var values = new float[strings.Length];
        for (var i = 0; i < strings.Length; i++)
        {
            values[i] = float.Parse(strings[i], cultureInfo);
        }
        return Create(values);
    }

    /// <summary>
    /// Creates a new <see cref="Vector4"/> object with the specified values.
    /// </summary>
    /// <param name="x">x value.</param>
    /// <param name="y">y value.</param>
    /// <param name="z">z value.</param>
    /// <param name="w">w value.</param>
    public static Vector4 Create(float x, float y, float z, float w)
    {
        var vector = new Vector4
        {
            X = x,
            Y = y,
            Z = z,
            W = w,
        };
        return vector;
    }

    /// <summary>
    /// Creates a new <see cref="Vector4"/> object with the specified values.
    /// </summary>
    /// <param name="values">The values of the <see cref="Vector4"/>.</param>
    /// <exception cref="ArgumentException">Number of values do not match!.</exception>
    public static Vector4 Create(float[] values)
    {
        if (values == null)
        {
            throw new ArgumentNullException("values");
        }

        if (values.Length != 4)
        {
            throw new ArgumentException(string.Format("Number of values do not match!"));
        }

        var vector = new Vector4
        {
            X = values[0],
            Y = values[1],
            Z = values[2],
            W = values[3],
        };
        return vector;
    }

    /// <summary>
    /// Gets an empty <see cref="Vector3"/> object.
    /// </summary>
    public static Vector4 Empty => default;

    #endregion

    #region value access

    /// <summary>x.</summary>
    [FieldOffset(0)]
    public float X;

    /// <summary>y.</summary>
    [FieldOffset(4)]
    public float Y;

    /// <summary>z.</summary>
    [FieldOffset(8)]
    public float Z;

    /// <summary>w.</summary>
    [FieldOffset(12)]
    public float W;
    #endregion

    #region operators

    /// <summary>
    /// Checks two <see cref="Vector4"/> instances for equality.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator ==(Vector4 first, Vector4 second)
    {
        return Equals(null, first) ? Equals(null, second) : first.Equals(second);
    }

    /// <summary>
    /// Checks two <see cref="Vector4"/> instances for inequality.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static bool operator !=(Vector4 first, Vector4 second)
    {
        return Equals(null, first) ? !Equals(null, second) : !first.Equals(second);
    }

    /// <summary>
    /// provides addition.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 operator +(Vector4 first, Vector4 second)
    {
        return Create(first.X + second.X, first.Y + second.Y, first.Z + second.Z, first.W + second.W);
    }

    /// <summary>
    /// provides addition.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 Add(Vector4 first, Vector4 second) => Create(first.X + second.X, first.Y + second.Y, first.Z + second.Z, first.W + second.W);

    /// <summary>
    /// provides subtraction.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 operator -(Vector4 first, Vector4 second)
    {
        return Create(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.W - second.W);
    }

    /// <summary>
    /// provides subtraction.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 Subtract(Vector4 first, Vector4 second) => Create(first.X - second.X, first.Y - second.Y, first.Z - second.Z, first.W - second.W);

    /// <summary>
    /// provides multiplication.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 operator *(Vector4 first, float second)
    {
        return Create(first.X * second, first.Y * second, first.Z * second, first.W * second);
    }

    /// <summary>
    /// provides multiplication.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 Muliply(Vector4 first, float second) => Create(first.X * second, first.Y * second, first.Z * second, first.W * second);

    /// <summary>
    /// provides multiplication.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static float operator *(Vector4 first, Vector4 second)
    {
        return (first.X * second.X) + (first.Y * second.Y) + (first.Z * second.Z) + (first.W * second.W);
    }

    /// <summary>
    /// provides multiplication.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static float Mulitly(Vector4 first, Vector4 second) => (first.X * second.X) + (first.Y * second.Y) + (first.Z * second.Z) + (first.W * second.W);

    /// <summary>
    /// provides division.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 operator /(Vector4 first, float second)
    {
        return Create(first.X / second, first.Y / second, first.Z / second, first.W / second);
    }

    /// <summary>
    /// provides division.
    /// </summary>
    /// <param name="first"></param>
    /// <param name="second"></param>
    /// <returns></returns>
    public static Vector4 Divide(Vector4 first, float second) => Create(first.X / second, first.Y / second, first.Z / second, first.W / second);
    #endregion

    /// <summary>
    /// Gets the length of the vector.
    /// </summary>
    public float Length => (float)Math.Sqrt((X * X) + (Y * Y) + (Z * Z) + (W * W));

    /// <summary>
    /// Gets a normalized version of the vector. (Length near 1.0f watch out for rounding errors!).
    /// </summary>
    /// <returns>Returns the normalized version of this vector.</returns>
    public Vector4 Normalized => W != 1.0f ? Create(X * W / Length, Y * W / Length, Z * W / Length, 1) : Create(X / Length, Y / Length, Z / Length, 1);

    /// <summary>
    /// Gets a <see cref="Vector3"/> instance.
    /// </summary>
    /// <returns>Returns a <see cref="Vector3"/> instance.</returns>
    public Vector3 ToVector3()
    {
        return W != 1.0f
            ? Vector3.Create(X * W / Length, Y * W / Length, Z * W / Length)
            : Vector3.Create(X / Length, Y / Length, Z / Length);
    }

    /// <summary>
    /// Retrieves the vector values as array.
    /// </summary>
    /// <returns>Returns the vector values as array.</returns>
    public float[] ToArray() => new float[] { X, Y, Z, W };

    /// <summary>
    /// Gets a string of the form '(x,y,z,w)'.
    /// The values are converted to strings using <see cref="CultureInfo.InvariantCulture"/>.
    /// </summary>
    /// <returns>Returns a string of the form '(x,y,z,w)'.</returns>
    public override string ToString() => ToString(CultureInfo.InvariantCulture);

    /// <summary>
    /// Gets a string of the form '(x,y,z,w)'.
    /// </summary>
    /// <param name="cultureInfo">The <see cref="CultureInfo"/> for encoding the float values.</param>
    /// <returns>Returns a string of the form '(x,y,z,w)'.</returns>
    public string ToString(IFormatProvider cultureInfo) => string.Format("({0},{1},{2},{3})", X.ToString(cultureInfo), Y.ToString(cultureInfo), Z.ToString(cultureInfo), W.ToString(cultureInfo));

    /// <summary>
    /// Checks another <see cref="Vector4"/> for equality.
    /// </summary>
    /// <param name="obj">The <see cref="Vector4"/> instance to check for equality.</param>
    /// <returns>Returns true if the specified object equals this one.</returns>
    public override bool Equals(object? obj)
    {
        if (!(obj is Vector4))
        {
            return false;
        }

        var other = ((Vector4)obj).ToVector3();
        return ToVector3().Equals(other);
    }

    /// <summary>
    /// Gets the hash code for this instance.
    /// </summary>
    /// <returns>Returns the hash code for this instance.</returns>
    public override int GetHashCode() => ToString().GetHashCode();

    /// <summary>
    /// Gets a copy of this object.
    /// </summary>
    /// <returns>Returns a copy of this object.</returns>
    public Vector4 Clone() => Create(X, Y, Z, W);
}
