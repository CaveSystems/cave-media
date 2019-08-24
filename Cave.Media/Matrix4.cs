#pragma warning disable SA1307 // Accessible fields should begin with upper-case letter

using System;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Cave.Media
{
    /// <summary>
    /// Provides a simple 4d matrix with float values.
    /// </summary>
    [StructLayout(LayoutKind.Explicit, Size = 3 * 3 * 4)]
    public struct Matrix4
    {
        #region static constructors

        /// <summary>
        /// Parses a <see cref="ToString()"/> output.
        /// </summary>
        /// <param name="text"></param>
        /// <returns></returns>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Globalization", "CA1303:Literale nicht als lokalisierte Parameter übergeben", MessageId = "Cave.Vector4.Parse(System.String)")]
        public static Matrix4 Parse(string text)
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
            if (vectors.Length != 4)
            {
                throw new FormatException();
            }

            var v1 = Vector4.Parse("(" + vectors[0] + ")");
            var v2 = Vector4.Parse("(" + vectors[1] + ")");
            var v3 = Vector4.Parse("(" + vectors[2] + ")");
            var v4 = Vector4.Parse("(" + vectors[3] + ")");
            return FromColumns(v1, v2, v3, v4);
        }

        /// <summary>
        /// Gets a new empty <see cref="Matrix4"/>.
        /// </summary>
        public static Matrix4 Empty => default;

        /// <summary>
        /// Creates a new <see cref="Matrix4"/> with the specified columns.
        /// </summary>
        /// <param name="vectors">The column vectors.</param>
        /// <returns></returns>
        public static Matrix4 FromColumns(Vector4[] vectors)
        {
            if (vectors == null)
            {
                throw new ArgumentNullException("vectors");
            }

            if (vectors.Length != 4)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            return FromColumns(vectors[0], vectors[1], vectors[2], vectors[3]);
        }

        /// <summary>
        /// Creates a new <see cref="Matrix4"/> with the specified columns.
        /// </summary>
        /// <param name="v1">Defines the first vector.</param>
        /// <param name="v2">Defines the second vector.</param>
        /// <param name="v3">Defines the third vector.</param>
        /// <param name="v4">Defines the fourth vector.</param>
        /// <returns></returns>
        public static Matrix4 FromColumns(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 v4)
        {
            var result = new Matrix4
            {
                v11 = v1.X,
                v12 = v1.Y,
                v13 = v1.Z,
                v14 = v1.W,
                v21 = v2.X,
                v22 = v2.Y,
                v23 = v2.Z,
                v24 = v2.W,
                v31 = v3.X,
                v32 = v3.Y,
                v33 = v3.Z,
                v34 = v3.W,
                v41 = v4.X,
                v42 = v4.Y,
                v43 = v4.Z,
                v44 = v4.W
            };
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix3"/> with the specified columns.
        /// </summary>
        /// <param name="vectors">The row vectors.</param>
        /// <returns></returns>
        public static Matrix4 FromRows(Vector4[] vectors)
        {
            if (vectors == null)
            {
                throw new ArgumentNullException("vectors");
            }

            if (vectors.Length != 4)
            {
                throw new ArgumentException(string.Format("Number of values do not match!"));
            }

            return FromRows(vectors[0], vectors[1], vectors[2], vectors[3]);
        }

        /// <summary>
        /// Creates a new <see cref="Matrix4"/> with the specified rows.
        /// </summary>
        /// <param name="v1">Defines the first vector.</param>
        /// <param name="v2">Defines the second vector.</param>
        /// <param name="v3">Defines the third vector.</param>
        /// <param name="v4">Defines the fourth vector.</param>
        /// <returns></returns>
        public static Matrix4 FromRows(Vector4 v1, Vector4 v2, Vector4 v3, Vector4 v4)
        {
            var result = new Matrix4
            {
                v11 = v1.X,
                v21 = v1.Y,
                v31 = v1.Z,
                v41 = v1.W,
                v12 = v2.X,
                v22 = v2.Y,
                v32 = v2.Z,
                v42 = v2.W,
                v13 = v3.X,
                v23 = v3.Y,
                v33 = v3.Z,
                v43 = v3.W,
                v14 = v4.X,
                v24 = v4.Y,
                v34 = v4.Z,
                v44 = v4.W
            };
            return result;
        }

        /// <summary>
        /// Creates a new <see cref="Matrix4"/> with the specified values.
        /// </summary>
        /// <param name="p11">Sets the value at x1 y1.</param>
        /// <param name="p12">Sets the value at x2 y1.</param>
        /// <param name="p13">Sets the value at x3 y1.</param>
        /// <param name="p14">Sets the value at x4 y1.</param>
        /// <param name="p21">Sets the value at x1 y2.</param>
        /// <param name="p22">Sets the value at x2 y2.</param>
        /// <param name="p23">Sets the value at x3 y2.</param>
        /// <param name="p24">Sets the value at x4 y2.</param>
        /// <param name="p31">Sets the value at x1 y3.</param>
        /// <param name="p32">Sets the value at x2 y3.</param>
        /// <param name="p33">Sets the value at x3 y3.</param>
        /// <param name="p34">Sets the value at x4 y3.</param>
        /// <param name="p41">Sets the value at x1 y4.</param>
        /// <param name="p42">Sets the value at x2 y4.</param>
        /// <param name="p43">Sets the value at x3 y4.</param>
        /// <param name="p44">Sets the value at x4 y4.</param>
        /// <returns>Returns a new <see cref="Matrix4"/> instance with the specified values.</returns>
        public static Matrix4 Create(float p11, float p12, float p13, float p14, float p21, float p22, float p23, float p24, float p31, float p32, float p33, float p34, float p41, float p42, float p43, float p44)
        {
            var result = new Matrix4
            {
                v11 = p11,
                v12 = p12,
                v13 = p13,
                v14 = p14,
                v21 = p21,
                v22 = p22,
                v23 = p23,
                v24 = p24,
                v31 = p31,
                v32 = p32,
                v33 = p33,
                v34 = p34,
                v41 = p41,
                v42 = p42,
                v43 = p43,
                v44 = p44
            };
            return result;
        }

        /// <summary>
        /// Gets a rotation matrix (rotation around x axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix4"/>.</returns>
        public static Matrix4 RotationX(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            return Create(1, 0, 0, 0, 0, cos, -sin, 0, 0, sin, cos, 0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Gets a rotation matrix (rotation around y axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix4"/>.</returns>
        public static Matrix4 RotationY(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            return Create(cos, 0, -sin, 0, 0, 1, 0, 0, sin, 0, cos, 0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Gets a rotation matrix (rotation around z axsis).
        /// </summary>
        /// <param name="radians">angle 0..2pi.</param>
        /// <returns>Returns a rotation <see cref="Matrix4"/>.</returns>
        public static Matrix4 RotationZ(float radians)
        {
            float cos = (float)Math.Cos(radians);
            float sin = (float)Math.Sin(radians);
            return Create(cos, sin, 0, 0, -sin, cos, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Gets a scaling matrix.
        /// </summary>
        /// <param name="X">the x scaling factor.</param>
        /// <param name="Y">the y scaling factor.</param>
        /// <param name="Z">the z scaling factor.</param>
        /// <returns>Returns a scaling <see cref="Matrix4"/>.</returns>
        public static Matrix4 Scaling(float X, float Y, float Z)
        {
            return Create(X, 0, 0, 0, 0, Y, 0, 0, 0, 0, Z, 0, 0, 0, 0, 1);
        }

        /// <summary>
        /// Gets a translation matrix.
        /// </summary>
        /// <param name="X">the x translation.</param>
        /// <param name="Y">the y translation.</param>
        /// <param name="Z">the z translation.</param>
        /// <returns>Returns a translation <see cref="Matrix4"/>.</returns>
        public static Matrix4 Translation(float X, float Y, float Z)
        {
            return Create(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, X, Y, Z, 1);
        }

        /// <summary>
        /// Provides the Identity(Einheits-)matrix.
        /// </summary>
        public static Matrix4 Identity
        {
            get
            {
                return Create(1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1, 0, 0, 0, 0, 1);
            }
        }
        #endregion

        #region operators

        /// <summary>
        /// Checks two <see cref="Matrix4"/> instances for equality.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator ==(Matrix4 first, Matrix4 second)
        {
            return Equals(null, first) ? Equals(null, second) : first.Equals(second);
        }

        /// <summary>
        /// Checks two <see cref="Matrix4"/> instances for inequality.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static bool operator !=(Matrix4 first, Matrix4 second)
        {
            return Equals(null, first) ? !Equals(null, second) : !first.Equals(second);
        }

        /// <summary>
        /// Calculates the sum of two <see cref="Matrix4"/> structs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix4 operator +(Matrix4 first, Matrix4 second)
        {
            return first.Add(second);
        }

        /// <summary>
        /// Calculates the product of two <see cref="Matrix4"/> structs.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix4 operator *(Matrix4 first, Matrix4 second)
        {
            return first.Multiply(second);
        }

        /// <summary>
        /// Calculates the product of a <see cref="Matrix4"/> struct and a scalar.
        /// </summary>
        /// <param name="first"></param>
        /// <param name="second"></param>
        /// <returns></returns>
        public static Matrix4 operator *(Matrix4 first, float second)
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
        /// Provides the v14 value of the matrix = Values[3].
        /// </summary>
        [FieldOffset(12)]
        public float v14;

        /// <summary>
        /// Provides the v21 value of the matrix = Values[4].
        /// </summary>
        [FieldOffset(16)]
        public float v21;

        /// <summary>
        /// Provides the v22 value of the matrix = Values[5].
        /// </summary>
        [FieldOffset(20)]
        public float v22;

        /// <summary>
        /// Provides the v23 value of the matrix = Values[6].
        /// </summary>
        [FieldOffset(24)]
        public float v23;

        /// <summary>
        /// Provides the v24 value of the matrix = Values[7].
        /// </summary>
        [FieldOffset(28)]
        public float v24;

        /// <summary>
        /// Provides the v31 value of the matrix = Values[8].
        /// </summary>
        [FieldOffset(32)]
        public float v31;

        /// <summary>
        /// Provides the v32 value of the matrix = Values[9].
        /// </summary>
        [FieldOffset(36)]
        public float v32;

        /// <summary>
        /// Provides the v33 value of the matrix = Values[10].
        /// </summary>
        [FieldOffset(40)]
        public float v33;

        /// <summary>
        /// Provides the v34 value of the matrix = Values[11].
        /// </summary>
        [FieldOffset(44)]
        public float v34;

        /// <summary>
        /// Provides the v41 value of the matrix = Values[12].
        /// </summary>
        [FieldOffset(48)]
        public float v41;

        /// <summary>
        /// Provides the v42 value of the matrix = Values[13].
        /// </summary>
        [FieldOffset(52)]
        public float v42;

        /// <summary>
        /// Provides the v43 value of the matrix = Values[14].
        /// </summary>
        [FieldOffset(56)]
        public float v43;

        /// <summary>
        /// Provides the v44 value of the matrix = Values[15].
        /// </summary>
        [FieldOffset(60)]
        public float v44;

        /// <summary>
        /// Provides direct linear access to all matrix values (0..15).
        /// </summary>
        /// <param name="index">The linear index (0..15).</param>
        /// <returns>Returns the value at the specified matrix index.</returns>
        public unsafe float this[int index]
        {
            get
            {
#if DEBUG
                if ((index < 0) || (index > 15)) throw new ArgumentOutOfRangeException(nameof(index));
#endif
                fixed (float* p = &v11)
                {
                    return p[index];
                }
            }
            set
            {
#if DEBUG
                if ((index < 0) || (index > 15)) throw new ArgumentOutOfRangeException(nameof(index));
#endif
                fixed (float* p = &v11)
                {
                    p[index] = value;
                }
            }
        }

        /// <summary>
        /// Provides direct (slow) access to all matrix values. x, y = [0..3].
        /// </summary>
        /// <param name="x">The column [0..3].</param>
        /// <param name="y">The row [0..3].</param>
        /// <returns>Returns the value at the specified matrix index.</returns>
        public float this[int x, int y]
        {
            get
            {
#if DEBUG
                if ((x < 0) || (x > 3)) throw new ArgumentOutOfRangeException(nameof(x));
                if ((y < 0) || (y > 3)) throw new ArgumentOutOfRangeException(nameof(y));
#endif

                return this[(y * 4) + x];
            }
            set
            {
#if DEBUG
                if ((x < 0) || (x > 3)) throw new ArgumentOutOfRangeException(nameof(x));
                if ((y < 0) || (y > 3)) throw new ArgumentOutOfRangeException(nameof(y));
#endif
                this[(y * 4) + x] = value;
            }
        }

        /// <summary>
        /// Gets the column vectors.
        /// </summary>
        /// <returns></returns>
        public Vector4[] ToColumns()
        {
            return new Vector4[]
            {
                Vector4.Create(v11, v12, v13, v14),
                Vector4.Create(v21, v22, v23, v24),
                Vector4.Create(v31, v32, v33, v34),
                Vector4.Create(v41, v42, v43, v44),
            };
        }

        /// <summary>
        /// Gets the row vectors.
        /// </summary>
        /// <returns></returns>
        public Vector4[] ToRows()
        {
            return new Vector4[]
            {
                Vector4.Create(v11, v21, v31, v41),
                Vector4.Create(v12, v22, v32, v42),
                Vector4.Create(v13, v23, v33, v43),
                Vector4.Create(v14, v24, v34, v44),
            };
        }
        #endregion

        /// <summary>
        /// Sets all values of the matrix to a specified value.
        /// </summary>
        /// <param name="value">The value to be set.</param>
        public void SetAll(float value)
        {
            for (int i = 0; i < 4 * 4; i++)
            {
                this[i] = value;
            }

        }

        /// <summary>
        /// Multiplies with a specified value.
        /// </summary>
        /// <param name="value">The value to multiplay with.</param>
        /// <returns>Returns the matrix-scalar product.</returns>
        public Matrix4 Multiply(float value)
        {
            var result = (Matrix4)Clone();
            for (int i = 0; i < 16; i++)
            {
                this[i] *= value;
            }
            return result;
        }

        /// <summary>
        /// Multiplies with a specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4"/> to multiply with.</param>
        /// <returns>Returns the matrix-matrix product.</returns>
        public Matrix4 Multiply(Matrix4 matrix)
        {
            return Create(
                (v11 * matrix.v11) + (v12 * matrix.v21) + (v13 * matrix.v31) + (matrix.v14 * v41),
                (v11 * matrix.v12) + (v12 * matrix.v22) + (v13 * matrix.v32) + (matrix.v14 * v42),
                (v11 * matrix.v13) + (v12 * matrix.v23) + (v13 * matrix.v33) + (matrix.v14 * v43),
                (v11 * matrix.v14) + (v12 * matrix.v24) + (v13 * matrix.v34) + (matrix.v14 * v44),
                (v21 * matrix.v11) + (v22 * matrix.v21) + (v23 * matrix.v31) + (matrix.v24 * v41),
                (v21 * matrix.v12) + (v22 * matrix.v22) + (v23 * matrix.v32) + (matrix.v24 * v42),
                (v21 * matrix.v13) + (v22 * matrix.v23) + (v23 * matrix.v33) + (matrix.v24 * v43),
                (v21 * matrix.v14) + (v22 * matrix.v24) + (v23 * matrix.v34) + (matrix.v24 * v44),
                (v31 * matrix.v11) + (v32 * matrix.v21) + (v33 * matrix.v31) + (matrix.v34 * v41),
                (v31 * matrix.v12) + (v32 * matrix.v22) + (v33 * matrix.v32) + (matrix.v34 * v42),
                (v31 * matrix.v13) + (v32 * matrix.v23) + (v33 * matrix.v33) + (matrix.v34 * v43),
                (v31 * matrix.v14) + (v32 * matrix.v24) + (v33 * matrix.v34) + (matrix.v34 * v44),
                (v41 * matrix.v11) + (v42 * matrix.v21) + (v43 * matrix.v31) + (matrix.v44 * v41),
                (v41 * matrix.v12) + (v42 * matrix.v22) + (v43 * matrix.v32) + (matrix.v44 * v42),
                (v41 * matrix.v13) + (v42 * matrix.v23) + (v43 * matrix.v33) + (matrix.v44 * v43),
                (v41 * matrix.v14) + (v42 * matrix.v24) + (v43 * matrix.v34) + (matrix.v44 * v44));
        }

        /// <summary>
        /// Adds a specified matrix.
        /// </summary>
        /// <param name="matrix">The <see cref="Matrix4"/> to add.</param>
        /// <returns>Returns the matrix-sum.</returns>
        public Matrix4 Add(Matrix4 matrix)
        {
            var result = (Matrix4)Clone();
            for (int i = 0; i < 16; i++)
            {
                result[i] += matrix[i];
            }
            return result;
        }

        /// <summary>
        /// Provides very fast translation without multiplication.
        /// </summary>
        /// <param name="vector">The translation <see cref="Vector3"/>.</param>
        /// <returns>Returns a translated <see cref="Matrix4"/>.</returns>
        public Matrix4 Translate(Vector3 vector)
        {
            return Create(v11, v12, v13, 0, v21, v22, v23, 0, v31, v32, v33, 0, v41 + vector.X, v42 + vector.Y, v43 + vector.Z, v44);
        }

        /// <summary>
        /// Checks another <see cref="Matrix4"/> for equality with this one.
        /// </summary>
        /// <param name="obj">The other <see cref="Matrix4"/> instance.</param>
        /// <returns>Returns true if the two <see cref="Matrix4"/> instances equal each other.</returns>
        public override bool Equals(object obj)
        {
            if (!(obj is Matrix4))
            {
                return false;
            }

            var other = (Matrix4)obj;
            for (int i = 0; i < 16; i++)
            {
                if (other[i] != this[i])
                {
                    return false;
                }
            }
            return true;
        }

        /// <summary>
        /// Returns the matrix values as string of the form [(v11, v12, v13, v14), (v21, v22, v23, v24), (v31, v32, v33, v34), (v41, v42, v43, v44)].
        /// All values are encoded with pre decimal DOT decimal place. This encoding is culture invariant!.
        /// </summary>
        /// <returns>
        /// Returns the matrix values as string of the form [(v11, v12, v13, v14), (v21, v22, v23, v24), (v31, v32, v33, v34), (v41, v42, v43, v44)].
        /// All values are encoded with pre decimal DOT decimal place. This encoding is culture invariant!.
        /// </returns>
        public override string ToString()
        {
            var result = new StringBuilder();
            result.Append("[");
            for (int y = 0; y < 4; y++)
            {
                if (y > 0)
                {
                    result.Append(", ");
                }

                result.Append("(");
                for (int x = 0; x < 4; x++)
                {
                    if (x > 0)
                    {
                        result.Append(", ");
                    }

                    result.Append(this[(y * 4) + x].ToString("0.0", CultureInfo.InvariantCulture));
                }
                result.Append(")");
            }
            result.Append("]");
            return result.ToString();
        }

        /// <summary>
        /// Gets a hash code for this instance.
        /// </summary>
        /// <returns>Returns a hash code for this object.</returns>
        public override int GetHashCode()
        {
            return ToString().GetHashCode();
        }

        #region ICloneable Member

        /// <summary>
        /// Gets a copy of this object.
        /// </summary>
        /// <returns>Returns a copy of this object.</returns>
        public Matrix4 Clone()
        {
            return Create(v11, v12, v13, v14, v21, v22, v23, v24, v31, v32, v33, v34, v41, v42, v43, v44);
        }

        #endregion
    }
}

#pragma warning restore SA1307 // Accessible fields should begin with upper-case letter
