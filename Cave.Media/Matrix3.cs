using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Cave.Media
{
    /// <summary>
    /// Provides a simple 3d matrix with float values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 3 * 3 * 4)]
    public struct Matrix3
    {
        #region static constructors

        /// <summary>
        /// Parses a <see cref="ToString()"/> output.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", MessageId = "Cave.Vector3.Parse(System.String)")]
        public static Matrix3 Parse(string text)
        {
            if (text == null)
            {
                throw new ArgumentNullException("text");
            }

            if (!text.StartsWith("[(") || !text.EndsWith(")]"))
            {
                throw new FormatException();
            }

            string[] vectors = text.Substring(2, text.Length - 4).Split(new string[] { "), (" }, StringSplitOptions.None);
            if (vectors.Length != 3)
            {
                throw new FormatException();
            }

            var v1 = Vector3.Parse("(" + vectors[0] + ")");
            var v2 = Vector3.Parse("(" + vectors[1] + ")");
            var v3 = Vector3.Parse("(" + vectors[2] + ")");
            return FromColumns(v1, v2, v3);
        }

        /// <summary>
        /// Obtains an empty <see cref="Matrix3"/>.
        /// </summary>
        /// <returns>Returns a new empty <see cref="Matrix3"/> instance (all values set to 0).</returns>
        public static Matrix3 Empty => default;

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified rows.
        /// </summary>
        /// <param name="vectors">The row vectors.</param>
        /// <returns></returns>
        public static Matrix3 FromRows(Vector3[] vectors)
        {
            if (vectors == null)
            {
                throw new ArgumentNullException("vectors");
            }

            if (vectors.Length != 3)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            return FromRows(vectors[0], vectors[1], vectors[2]);
        }

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified rows.
        /// </summary>
        /// <param name="v1">Defines the first vector.</param>
        /// <param name="v2">Defines the second vector.</param>
        /// <param name="v3">Defines the third vector.</param>
        /// <returns></returns>
        public static Matrix3 FromRows(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var result = default(Matrix3);
            result.v11 = v1.X;
            result.v21 = v1.Y;
            result.v31 = v1.Z;
            result.v12 = v2.X;
            result.v22 = v2.Y;
            result.v32 = v2.Z;
            result.v13 = v3.X;
            result.v23 = v3.Y;
            result.v33 = v3.Z;
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified columns.
        /// </summary>
        /// <param name="vectors">The column vectors.</param>
        /// <returns></returns>
        public static Matrix3 FromColumns(Vector3[] vectors)
        {
            if (vectors == null)
            {
                throw new ArgumentNullException("vectors");
            }

            if (vectors.Length != 3)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            return FromColumns(vectors[0], vectors[1], vectors[2]);
        }

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified columns.
        /// </summary>
        /// <param name="v1">Defines the first vector.</param>
        /// <param name="v2">Defines the second vector.</param>
        /// <param name="v3">Defines the third vector.</param>
        /// <returns></returns>
        public static Matrix3 FromColumns(Vector3 v1, Vector3 v2, Vector3 v3)
        {
            var result = default(Matrix3);
            result.v11 = v1.X;
            result.v12 = v1.Y;
            result.v13 = v1.Z;
            result.v21 = v2.X;
            result.v22 = v2.Y;
            result.v23 = v2.Z;
            result.v31 = v3.X;
            result.v32 = v3.Y;
            result.v33 = v3.Z;
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified values.
        /// </summary>
        /// <param name="p11">Sets the value at x1 y1.</param>
        /// <param name="p12">Sets the value at x2 y1.</param>
        /// <param name="p13">Sets the value at x3 y1.</param>
        /// <param name="p21">Sets the value at x1 y2.</param>
        /// <param name="p22">Sets the value at x2 y2.</param>
        /// <param name="p23">Sets the value at x3 y2.</param>
        /// <param name="p31">Sets the value at x1 y3.</param>
        /// <param name="p32">Sets the value at x2 y3.</param>
        /// <param name="p33">Sets the value at x3 y3.</param>
        /// <returns>Returns a new <see cref="Matrix3"/> instance with the specified values.</returns>
        public static Matrix3 Create(float p11, float p12, float p13, float p21, float p22, float p23, float p31, float p32, float p33)
        {
            var result = default(Matrix3);
            result.v11 = p11;
            result.v12 = p12;
            result.v13 = p13;
            result.v21 = p21;
            result.v22 = p22;
            result.v23 = p23;
            result.v31 = p31;
            result.v32 = p32;
            result.v33 = p33;
            return result;
        }

        /// <summary>
        /// Obtains a rotation matrix (rotation around x axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix3"/>.</returns>
        public static Matrix3 RotationX(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float l_Sin = (float)Math.Sin(radians);
            return Create(1, 0, 0, 0, cos, -l_Sin, 0, l_Sin, cos);
        }

        /// <summary>
        /// Obtains a rotation matrix (rotation around y axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix3"/>.</returns>
        public static Matrix3 RotationY(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float l_Sin = (float)Math.Sin(radians);
            return Create(cos, 0, -l_Sin, 0, 1, 0, l_Sin, 0, cos);
        }

        /// <summary>
        /// Obtains a rotation matrix (rotation around z axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix3"/>.</returns>
        public static Matrix3 RotationZ(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float l_Sin = (float)Math.Sin(radians);
            return Create(cos, l_Sin, 0, -l_Sin, cos, 0, 0, 0, 1);
        }

        /// <summary>
        /// Obtains a scaling matrix.
        /// </summary>
        /// <param name="X">the x scaling factor.</param>
        /// <param name="Y">the y scaling factor.</param>
        /// <param name="Z">the z scaling factor.</param>
        /// <returns>Returns a scaling <see cref="Matrix3"/>.</returns>
        public static Matrix3 Scaling(float X, float Y, float Z)
        {
            return Create(X, 0, 0, 0, Y, 0, 0, 0, Z);
        }

        /// <summary>
        /// Provides the Identity(Einheits-)matrix.
        /// </summary>
        public static Matrix3 Identity
        {
            get
            {
                return Create(1f, 0f, 0f, 0f, 1f, 0f, 0f, 0f, 1f);
            }
        }
        #endregion

        #region operators

        /// <summary>
        /// Checks two <see cref="Matrix3"/> instances for equality.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix3 first, Matrix3 second)
        {
            return Equals(null, first) ? Equals(null, second) : first.Equals(second);
        }

        /// <summary>
        /// Checks two <see cref="Matrix3"/> instances for inequality.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix3 first, Matrix3 second)
        {
            return Equals(null, first) ? !Equals(null, second) : !first.Equals(second);
        }

        /// <summary>
        /// Calculates the sum of two <see cref="Matrix3"/> structs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix3 operator +(Matrix3 first, Matrix3 second)
        {
            return first.Add(second);
        }

        /// <summary>
        /// Calculates the product of two <see cref="Matrix3"/> structs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix3 operator *(Matrix3 first, Matrix3 second)
        {
            return first.Multiply(second);
        }

        /// <summary>
        /// Calculates the product of a <see cref="Matrix3"/> struct and a <see cref="Vector3"/>.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Vector3 operator *(Matrix3 first, Vector3 second)
        {
            return first.Multiply(second);
        }

        /// <summary>
        /// Calculates the product of a <see cref="Matrix3"/> struct and a scalar.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix3 operator *(Matrix3 first, float second)
        {
            return first.Multiply(second);
        }
        #endregion

        #region value access

        /// <summary>
        /// Provides the v11 value of the matrix = Values[0].
        /// </summary>
        [FieldOffset(0)]
        public float v11;

        /// <summary>
        /// Provides the v12 value of the matrix = Values[1].
        /// </summary>
        [FieldOffset(4)]
        public float v12;

        /// <summary>
        /// Provides the v13 value of the matrix = Values[2].
        /// </summary>
        [FieldOffset(8)]
        public float v13;

        /// <summary>
        /// Provides the v21 value of the matrix = Values[3].
        /// </summary>
        [FieldOffset(12)]
        public float v21;

        /// <summary>
        /// Provides the v22 value of the matrix = Values[4].
        /// </summary>
        [FieldOffset(16)]
        public float v22;

        /// <summary>
        /// Provides the v23 value of the matrix = Values[5].
        /// </summary>
        [FieldOffset(20)]
        public float v23;

        /// <summary>
        /// Provides the v31 value of the matrix = Values[6].
        /// </summary>
        [FieldOffset(24)]
        public float v31;

        /// <summary>
        /// Provides the v32 value of the matrix = Values[7].
        /// </summary>
        [FieldOffset(28)]
        public float v32;

        /// <summary>
        /// Provides the v33 value of the matrix = Values[8].
        /// </summary>
        [FieldOffset(32)]
        public float v33;

        /// <summary>
        /// Provides direct linear access to all matrix values (0..8).
        /// </summary>
        /// <param name="index">The linear index (0..8).</param>
        /// <returns>Returns the value at the specified matrix index.</returns>
        public unsafe float this[int index]
        {
            get
            {
#if DEBUG
                if ((index < 0) || (index > 8)) throw new ArgumentOutOfRangeException(nameof(index));
#endif
                fixed (float* p = &v11)
                {
                    return p[index];
                }
            }
            set
            {
#if DEBUG
                if ((index < 0) || (index > 8)) throw new ArgumentOutOfRangeException(nameof(index));
#endif
                fixed (float* p = &v11)
                {
                    p[index] = value;
                }
            }
        }

        /// <summary>
        /// Provides direct (slow) access to all matrix values. x, y = [0..2].
        /// </summary>
        /// <param name="x">The column [0..2].</param>
        /// <param name="y">The row [0..2].</param>
        /// <returns>Returns the value at the specified matrix index.</returns>
        public float this[int x, int y]
        {
            get
            {
#if DEBUG
                if ((x < 0) || (x > 2)) throw new ArgumentOutOfRangeException(nameof(x));
                if ((y < 0) || (y > 2)) throw new ArgumentOutOfRangeException(nameof(y));
#endif

                return this[(y * 3) + x];
            }
            set
            {
#if DEBUG
                if ((x < 0) || (x > 2)) throw new ArgumentOutOfRangeException(nameof(x));
                if ((y < 0) || (y > 2)) throw new ArgumentOutOfRangeException(nameof(y));
#endif
                this[(y * 3) + x] = value;
            }
        }

        /// <summary>
        /// Obtains the column vectors.
        /// </summary>
        /// <returns></returns>
        public Vector3[] ToColumns()
        {
            return new Vector3[]
            {
                Vector3.Create(v11, v12, v13),
                Vector3.Create(v21, v22, v23),
                Vector3.Create(v31, v32, v33),
            };
        }

        /// <summary>
        /// Obtains the row vectors.
        /// </summary>
        /// <returns></returns>
        public Vector3[] ToRows()
        {
            return new Vector3[]
            {
                Vector3.Create(v11, v21, v31),
                Vector3.Create(v12, v22, v32),
                Vector3.Create(v13, v23, v33),
            };
        }
        #endregion

        /// <summary>
        /// Sets all values of the matrix to a specified value.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        public void SetAll(float value)
        {
            for (int i = 0; i < 9; i++)
            {
                this[i] = value;
            }
        }

        /// <summary>
        /// Calculates the convolution of this matrix with a specified one.
        /// </summary>
        /// <param name="convolutionMatrix">The matrix to convolve with.</param>
        /// <returns>Returns the convolution sum.</returns>
        public float Convolve(Matrix3 convolutionMatrix)
        {
            float result = 0;
            for (int i = 0; i < 9; i++)
            {
                result += this[i] * convolutionMatrix[i];
            }
            return result;
        }

        /// <summary>
        /// Multiplies with a specified vector.
        /// </summary>
        /// <param name="vector">The <see cref="Vector3"/> to multiply with.</param>
        /// <returns>Returns the matrix-vector product.</returns>
        public Vector3 Multiply(Vector3 vector)
        {
            return Vector3.Create(
                (v11 * vector.X) + (v12 * vector.Y) + (v13 * vector.Z),
                (v21 * vector.X) + (v22 * vector.Y) + (v23 * vector.Z),
                (v31 * vector.X) + (v32 * vector.Y) + (v33 * vector.Z));
        }

        /// <summary>
        /// Multiplies with a specified value.
        /// </summary>
        /// <param name="value">The value to multiplay with.</param>
        /// <returns>Returns the matrix-scalar product.</returns>
        public Matrix3 Multiply(float value)
        {
            var result = (Matrix3)Clone();
            for (int i = 0; i < 9; i++)
            {
                result[i] *= value;
            }
            return result;
        }

        /// <summary>
        /// Multiplies with a specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix3"/> to multiply with.</param>
        /// <returns>Returns the matrix-matrix product.</returns>
        public Matrix3 Multiply(Matrix3 matrix)
        {
            return Create(
                (v11 * matrix.v11) + (v12 * matrix.v21) + (v13 * matrix.v31),
                (v11 * matrix.v12) + (v12 * matrix.v22) + (v13 * matrix.v32),
                (v11 * matrix.v13) + (v12 * matrix.v23) + (v13 * matrix.v33),
                (v21 * matrix.v11) + (v22 * matrix.v21) + (v23 * matrix.v31),
                (v21 * matrix.v12) + (v22 * matrix.v22) + (v23 * matrix.v32),
                (v21 * matrix.v13) + (v22 * matrix.v23) + (v23 * matrix.v33),
                (v31 * matrix.v11) + (v32 * matrix.v21) + (v33 * matrix.v31),
                (v31 * matrix.v12) + (v32 * matrix.v22) + (v33 * matrix.v32),
                (v31 * matrix.v13) + (v32 * matrix.v23) + (v33 * matrix.v33));
        }

        /// <summary>
        /// Adds a specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix3"/> to add.</param>
        /// <returns>Returns the matrix-sum.</returns>
        public Matrix3 Add(Matrix3 matrix)
        {
            var result = (Matrix3)Clone();
            for (int i = 0; i < 9; i++)
            {
                result[i] += matrix[i];
            }
            return result;
        }

        /// <summary>
        /// Checks another <see cref="Matrix3"/> for equality with this one.
        /// </summary>
        /// <param name="obj">The other <see cref="Matrix3"/> instance.</param>
        /// <returns>Returns true if the two <see cref="Matrix3"/> instances equal each other.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix3))
            {
                return false;
            }

            var other = (Matrix3)obj;
            return
                (other.v11 == v11) && (other.v12 == v12) && (other.v13 == v13) &&
                (other.v21 == v21) && (other.v22 == v22) && (other.v23 == v23) &&
                (other.v31 == v31) && (other.v32 == v32) && (other.v33 == v33);
        }

        /// <summary>
        /// Returns the matrix values as string of the form [(v11, v12, v13), (v21, v22, v23), (v31, v32, v33)]. All values are encoded with pre decimal DOT decimal place.
        /// This encoding is culture invariant!.
        /// </summary>
        /// <returns>
        /// Returns the matrix values as string of the form [(v11, v12, v13), (v21, v22, v23), (v31, v32, v33)]. All values are encoded with pre decimal DOT decimal place.
        /// This encoding is culture invariant!.
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("[");
            for (int y = 0; y < 3; y++)
            {
                if (y > 0)
                {
                    result.Append(", ");
                }

                result.Append("(");
                for (int x = 0; x < 3; x++)
                {
                    if (x > 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(this[(y * 3) + x].ToString("0.0", CultureInfo.InvariantCulture));
                }
                result.Append(")");
            }
            result.Append("]");
            return result.ToString();
        }

        /// <summary>
        /// Obtains a hash code for this instance.
        /// </summary>
        /// <returns>Returns a hash code for this object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region ICloneable Member

        /// <summary>
        /// Obtains a copy of this object.
        /// </summary>
        /// <returns>Returns a copy of this object.</returns>
        public Matrix3 Clone()
        {
            return Create(v11, v12, v13, v21, v22, v23, v31, v32, v33);
        }

        #endregion
    }
}
