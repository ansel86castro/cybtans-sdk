using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Matrix:IEquatable<Matrix>
    {
        public static readonly Matrix Identity = new Matrix(1, 0, 0, 0,
                                                            0, 1, 0, 0,
                                                            0, 0, 1, 0,
                                                            0, 0, 0, 1);

        public struct Composition
        {
            public Matrix Rotation;
            public Vector3 Translation;
            public Vector3 Scaling;
        }

        public float M11;
        public float M12;
        public float M13;
        public float M14;
        public float M21;
        public float M22;
        public float M23;
        public float M24;
        public float M31;
        public float M32;
        public float M33;
        public float M34;
        public float M41;
        public float M42;
        public float M43;
        public float M44;

        #region Constructors

        public Matrix(float m11, float m12, float m13, float m14,
                      float m21, float m22, float m23, float m24,
                      float m31, float m32, float m33, float m34,
                      float m41, float m42, float m43, float m44)
        {
            this.M11 = m11;
            this.M12 = m12;
            this.M13 = m13;
            this.M14 = m14;

            this.M21 = m21;
            this.M22 = m22;
            this.M23 = m23;
            this.M24 = m24;

            this.M31 = m31;
            this.M32 = m32;
            this.M33 = m33;
            this.M34 = m34;

            this.M41 = m41;
            this.M42 = m42;
            this.M43 = m43;
            this.M44 = m44;
        }

        public Matrix(in Vector4 x, in Vector4 y, in Vector4 z, in Vector4 t)
        {
            M11 = x.X; M12 = x.Y; M13 = x.Z; M14 = x.W;
            M21 = y.X; M22 = y.Y; M23 = y.Z; M24 = y.W;
            M31 = z.X; M32 = z.Y; M33 = z.Z; M34 = z.W;
            M41 = t.X; M42 = t.Y; M43 = t.Z; M44 = t.W;
        }

        public Matrix(in Vector3 x, in Vector3 y, in Vector3 z, in Vector3 t)
        {
            M11 = x.X; 
            M12 = x.Y; 
            M13 = x.Z;
            M14 = 0;

            M21 = y.X; 
            M22 = y.Y; 
            M23 = y.Z; 
            M24 = 0;

            M31 = z.X; 
            M32 = z.Y; 
            M33 = z.Z; 
            M34 = 0;

            M41 = t.X;
            M42 = t.Y; 
            M43 = t.Z;
            M44 = 1;
        }

        #endregion;

        #region Properties

        //public static Matrix Identity
        //{
        //    get
        //    {
        //        Matrix m = new Matrix();
        //        m.M11 = m.M22 = m.M33 = m.M44 = 1;
        //        return m;
        //    }
        //}

        public Vector3 Translation
        {
            readonly get
            {
                Vector3 vector;
                vector.X = this.M41;
                vector.Y = this.M42;
                vector.Z = this.M43;
                return vector;
            }
            set
            {
                this.M41 = value.X;
                this.M42 = value.Y;
                this.M43 = value.Z;
            }
        }

        public Vector3 Right
        {
            readonly get 
            {
                Vector3 vector;
                vector.X = this.M11;
                vector.Y = this.M12;
                vector.Z = this.M13;
                return vector;
            }
            set
            {
                this.M11 = value.X;
                this.M12 = value.Y;
                this.M13 = value.Z;
            }
        }      
        
        public Vector3 Up
        {
            readonly get
            {
                Vector3 vector;
                vector.X = this.M21;
                vector.Y = this.M22;
                vector.Z = this.M23;
                return vector;
            }
            set
            {
                this.M21 = value.X;
                this.M22 = value.Y;
                this.M23 = value.Z;
            }
        }
        
        public Vector3 Front
        {
            readonly get
            {
                Vector3 vector;
                vector.X = this.M31;
                vector.Y = this.M32;
                vector.Z = this.M33;
                return vector;
            }
            set
            {
                this.M31 = value.X;
                this.M32 = value.Y;
                this.M33 = value.Z;
            }
        }
 
        #endregion

        #region Operators
        public static Matrix operator +(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 + matrix2.M11;
            matrix.M12 = matrix1.M12 + matrix2.M12;
            matrix.M13 = matrix1.M13 + matrix2.M13;
            matrix.M14 = matrix1.M14 + matrix2.M14;
            matrix.M21 = matrix1.M21 + matrix2.M21;
            matrix.M22 = matrix1.M22 + matrix2.M22;
            matrix.M23 = matrix1.M23 + matrix2.M23;
            matrix.M24 = matrix1.M24 + matrix2.M24;
            matrix.M31 = matrix1.M31 + matrix2.M31;
            matrix.M32 = matrix1.M32 + matrix2.M32;
            matrix.M33 = matrix1.M33 + matrix2.M33;
            matrix.M34 = matrix1.M34 + matrix2.M34;
            matrix.M41 = matrix1.M41 + matrix2.M41;
            matrix.M42 = matrix1.M42 + matrix2.M42;
            matrix.M43 = matrix1.M43 + matrix2.M43;
            matrix.M44 = matrix1.M44 + matrix2.M44;
            return matrix;
        }
        public static Matrix operator /(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 / matrix2.M11;
            matrix.M12 = matrix1.M12 / matrix2.M12;
            matrix.M13 = matrix1.M13 / matrix2.M13;
            matrix.M14 = matrix1.M14 / matrix2.M14;
            matrix.M21 = matrix1.M21 / matrix2.M21;
            matrix.M22 = matrix1.M22 / matrix2.M22;
            matrix.M23 = matrix1.M23 / matrix2.M23;
            matrix.M24 = matrix1.M24 / matrix2.M24;
            matrix.M31 = matrix1.M31 / matrix2.M31;
            matrix.M32 = matrix1.M32 / matrix2.M32;
            matrix.M33 = matrix1.M33 / matrix2.M33;
            matrix.M34 = matrix1.M34 / matrix2.M34;
            matrix.M41 = matrix1.M41 / matrix2.M41;
            matrix.M42 = matrix1.M42 / matrix2.M42;
            matrix.M43 = matrix1.M43 / matrix2.M43;
            matrix.M44 = matrix1.M44 / matrix2.M44;
            return matrix;
        }
        public static Matrix operator /(in Matrix matrix1, float divider)
        {
            Matrix matrix;
            float num = 1f / divider;
            matrix.M11 = matrix1.M11 * num;
            matrix.M12 = matrix1.M12 * num;
            matrix.M13 = matrix1.M13 * num;
            matrix.M14 = matrix1.M14 * num;
            matrix.M21 = matrix1.M21 * num;
            matrix.M22 = matrix1.M22 * num;
            matrix.M23 = matrix1.M23 * num;
            matrix.M24 = matrix1.M24 * num;
            matrix.M31 = matrix1.M31 * num;
            matrix.M32 = matrix1.M32 * num;
            matrix.M33 = matrix1.M33 * num;
            matrix.M34 = matrix1.M34 * num;
            matrix.M41 = matrix1.M41 * num;
            matrix.M42 = matrix1.M42 * num;
            matrix.M43 = matrix1.M43 * num;
            matrix.M44 = matrix1.M44 * num;
            return matrix;
        }
        public static bool operator ==(in Matrix matrix1, in Matrix matrix2)
        {
            return ((((((matrix1.M11 == matrix2.M11) && (matrix1.M22 == matrix2.M22)) && ((matrix1.M33 == matrix2.M33) && (matrix1.M44 == matrix2.M44))) && (((matrix1.M12 == matrix2.M12) && (matrix1.M13 == matrix2.M13)) && ((matrix1.M14 == matrix2.M14) && (matrix1.M21 == matrix2.M21)))) && ((((matrix1.M23 == matrix2.M23) && (matrix1.M24 == matrix2.M24)) && ((matrix1.M31 == matrix2.M31) && (matrix1.M32 == matrix2.M32))) && (((matrix1.M34 == matrix2.M34) && (matrix1.M41 == matrix2.M41)) && (matrix1.M42 == matrix2.M42)))) && (matrix1.M43 == matrix2.M43));
        }

        public static bool operator !=(in Matrix matrix1, in Matrix matrix2)
        {
            return ((((((matrix1.M11 != matrix2.M11) || (matrix1.M12 != matrix2.M12)) || ((matrix1.M13 != matrix2.M13) || (matrix1.M14 != matrix2.M14))) || (((matrix1.M21 != matrix2.M21) || (matrix1.M22 != matrix2.M22)) || ((matrix1.M23 != matrix2.M23) || (matrix1.M24 != matrix2.M24)))) || ((((matrix1.M31 != matrix2.M31) || (matrix1.M32 != matrix2.M32)) || ((matrix1.M33 != matrix2.M33) || (matrix1.M34 != matrix2.M34))) || (((matrix1.M41 != matrix2.M41) || (matrix1.M42 != matrix2.M42)) || (matrix1.M43 != matrix2.M43)))) || !(matrix1.M44 == matrix2.M44));
        }
        public static Matrix operator *(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            matrix.M12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            matrix.M13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            matrix.M14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            matrix.M21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            matrix.M22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            matrix.M23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            matrix.M24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            matrix.M31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            matrix.M32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            matrix.M33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            matrix.M34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            matrix.M41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            matrix.M42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            matrix.M43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
            matrix.M44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            return matrix;
        }

        public static Matrix operator *(in Matrix matrix, float scaleFactor)
        {
            Matrix matrix2;
            float num = scaleFactor;
            matrix2.M11 = matrix.M11 * num;
            matrix2.M12 = matrix.M12 * num;
            matrix2.M13 = matrix.M13 * num;
            matrix2.M14 = matrix.M14 * num;
            matrix2.M21 = matrix.M21 * num;
            matrix2.M22 = matrix.M22 * num;
            matrix2.M23 = matrix.M23 * num;
            matrix2.M24 = matrix.M24 * num;
            matrix2.M31 = matrix.M31 * num;
            matrix2.M32 = matrix.M32 * num;
            matrix2.M33 = matrix.M33 * num;
            matrix2.M34 = matrix.M34 * num;
            matrix2.M41 = matrix.M41 * num;
            matrix2.M42 = matrix.M42 * num;
            matrix2.M43 = matrix.M43 * num;
            matrix2.M44 = matrix.M44 * num;
            return matrix2;
        }
        public static Matrix operator *(float scaleFactor, in Matrix matrix)
        {
            Matrix matrix2;
            float num = scaleFactor;
            matrix2.M11 = matrix.M11 * num;
            matrix2.M12 = matrix.M12 * num;
            matrix2.M13 = matrix.M13 * num;
            matrix2.M14 = matrix.M14 * num;
            matrix2.M21 = matrix.M21 * num;
            matrix2.M22 = matrix.M22 * num;
            matrix2.M23 = matrix.M23 * num;
            matrix2.M24 = matrix.M24 * num;
            matrix2.M31 = matrix.M31 * num;
            matrix2.M32 = matrix.M32 * num;
            matrix2.M33 = matrix.M33 * num;
            matrix2.M34 = matrix.M34 * num;
            matrix2.M41 = matrix.M41 * num;
            matrix2.M42 = matrix.M42 * num;
            matrix2.M43 = matrix.M43 * num;
            matrix2.M44 = matrix.M44 * num;
            return matrix2;
        }
        public static Matrix operator -(in Matrix matrix1,in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 - matrix2.M11;
            matrix.M12 = matrix1.M12 - matrix2.M12;
            matrix.M13 = matrix1.M13 - matrix2.M13;
            matrix.M14 = matrix1.M14 - matrix2.M14;
            matrix.M21 = matrix1.M21 - matrix2.M21;
            matrix.M22 = matrix1.M22 - matrix2.M22;
            matrix.M23 = matrix1.M23 - matrix2.M23;
            matrix.M24 = matrix1.M24 - matrix2.M24;
            matrix.M31 = matrix1.M31 - matrix2.M31;
            matrix.M32 = matrix1.M32 - matrix2.M32;
            matrix.M33 = matrix1.M33 - matrix2.M33;
            matrix.M34 = matrix1.M34 - matrix2.M34;
            matrix.M41 = matrix1.M41 - matrix2.M41;
            matrix.M42 = matrix1.M42 - matrix2.M42;
            matrix.M43 = matrix1.M43 - matrix2.M43;
            matrix.M44 = matrix1.M44 - matrix2.M44;
            return matrix;
        }
        public static Matrix operator -(in Matrix matrix1)
        {
            Matrix matrix;
            matrix.M11 = -matrix1.M11;
            matrix.M12 = -matrix1.M12;
            matrix.M13 = -matrix1.M13;
            matrix.M14 = -matrix1.M14;
            matrix.M21 = -matrix1.M21;
            matrix.M22 = -matrix1.M22;
            matrix.M23 = -matrix1.M23;
            matrix.M24 = -matrix1.M24;
            matrix.M31 = -matrix1.M31;
            matrix.M32 = -matrix1.M32;
            matrix.M33 = -matrix1.M33;
            matrix.M34 = -matrix1.M34;
            matrix.M41 = -matrix1.M41;
            matrix.M42 = -matrix1.M42;
            matrix.M43 = -matrix1.M43;
            matrix.M44 = -matrix1.M44;
            return matrix;
        }

        #endregion

        #region Operator Methods
        public static Matrix Add(in Matrix matrix1,in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 + matrix2.M11;
            matrix.M12 = matrix1.M12 + matrix2.M12;
            matrix.M13 = matrix1.M13 + matrix2.M13;
            matrix.M14 = matrix1.M14 + matrix2.M14;
            matrix.M21 = matrix1.M21 + matrix2.M21;
            matrix.M22 = matrix1.M22 + matrix2.M22;
            matrix.M23 = matrix1.M23 + matrix2.M23;
            matrix.M24 = matrix1.M24 + matrix2.M24;
            matrix.M31 = matrix1.M31 + matrix2.M31;
            matrix.M32 = matrix1.M32 + matrix2.M32;
            matrix.M33 = matrix1.M33 + matrix2.M33;
            matrix.M34 = matrix1.M34 + matrix2.M34;
            matrix.M41 = matrix1.M41 + matrix2.M41;
            matrix.M42 = matrix1.M42 + matrix2.M42;
            matrix.M43 = matrix1.M43 + matrix2.M43;
            matrix.M44 = matrix1.M44 + matrix2.M44;
            return matrix;
        }
        public static void Add(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.M11 = matrix1.M11 + matrix2.M11;
            result.M12 = matrix1.M12 + matrix2.M12;
            result.M13 = matrix1.M13 + matrix2.M13;
            result.M14 = matrix1.M14 + matrix2.M14;
            result.M21 = matrix1.M21 + matrix2.M21;
            result.M22 = matrix1.M22 + matrix2.M22;
            result.M23 = matrix1.M23 + matrix2.M23;
            result.M24 = matrix1.M24 + matrix2.M24;
            result.M31 = matrix1.M31 + matrix2.M31;
            result.M32 = matrix1.M32 + matrix2.M32;
            result.M33 = matrix1.M33 + matrix2.M33;
            result.M34 = matrix1.M34 + matrix2.M34;
            result.M41 = matrix1.M41 + matrix2.M41;
            result.M42 = matrix1.M42 + matrix2.M42;
            result.M43 = matrix1.M43 + matrix2.M43;
            result.M44 = matrix1.M44 + matrix2.M44;
        }
        public static Matrix Divide(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 / matrix2.M11;
            matrix.M12 = matrix1.M12 / matrix2.M12;
            matrix.M13 = matrix1.M13 / matrix2.M13;
            matrix.M14 = matrix1.M14 / matrix2.M14;
            matrix.M21 = matrix1.M21 / matrix2.M21;
            matrix.M22 = matrix1.M22 / matrix2.M22;
            matrix.M23 = matrix1.M23 / matrix2.M23;
            matrix.M24 = matrix1.M24 / matrix2.M24;
            matrix.M31 = matrix1.M31 / matrix2.M31;
            matrix.M32 = matrix1.M32 / matrix2.M32;
            matrix.M33 = matrix1.M33 / matrix2.M33;
            matrix.M34 = matrix1.M34 / matrix2.M34;
            matrix.M41 = matrix1.M41 / matrix2.M41;
            matrix.M42 = matrix1.M42 / matrix2.M42;
            matrix.M43 = matrix1.M43 / matrix2.M43;
            matrix.M44 = matrix1.M44 / matrix2.M44;
            return matrix;
        }
        public static Matrix Divide(in Matrix matrix1, float divider)
        {
            Matrix matrix;
            float num = 1f / divider;
            matrix.M11 = matrix1.M11 * num;
            matrix.M12 = matrix1.M12 * num;
            matrix.M13 = matrix1.M13 * num;
            matrix.M14 = matrix1.M14 * num;
            matrix.M21 = matrix1.M21 * num;
            matrix.M22 = matrix1.M22 * num;
            matrix.M23 = matrix1.M23 * num;
            matrix.M24 = matrix1.M24 * num;
            matrix.M31 = matrix1.M31 * num;
            matrix.M32 = matrix1.M32 * num;
            matrix.M33 = matrix1.M33 * num;
            matrix.M34 = matrix1.M34 * num;
            matrix.M41 = matrix1.M41 * num;
            matrix.M42 = matrix1.M42 * num;
            matrix.M43 = matrix1.M43 * num;
            matrix.M44 = matrix1.M44 * num;
            return matrix;
        }

        public static void Divide(ref Matrix matrix1, float divider, out Matrix result)
        {
            float num = 1f / divider;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M14 = matrix1.M14 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M24 = matrix1.M24 * num;
            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
            result.M34 = matrix1.M34 * num;
            result.M41 = matrix1.M41 * num;
            result.M42 = matrix1.M42 * num;
            result.M43 = matrix1.M43 * num;
            result.M44 = matrix1.M44 * num;
        }
        public static void Divide(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.M11 = matrix1.M11 / matrix2.M11;
            result.M12 = matrix1.M12 / matrix2.M12;
            result.M13 = matrix1.M13 / matrix2.M13;
            result.M14 = matrix1.M14 / matrix2.M14;
            result.M21 = matrix1.M21 / matrix2.M21;
            result.M22 = matrix1.M22 / matrix2.M22;
            result.M23 = matrix1.M23 / matrix2.M23;
            result.M24 = matrix1.M24 / matrix2.M24;
            result.M31 = matrix1.M31 / matrix2.M31;
            result.M32 = matrix1.M32 / matrix2.M32;
            result.M33 = matrix1.M33 / matrix2.M33;
            result.M34 = matrix1.M34 / matrix2.M34;
            result.M41 = matrix1.M41 / matrix2.M41;
            result.M42 = matrix1.M42 / matrix2.M42;
            result.M43 = matrix1.M43 / matrix2.M43;
            result.M44 = matrix1.M44 / matrix2.M44;
        }
        public static Matrix Multiply(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            matrix.M12 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            matrix.M13 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            matrix.M14 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            matrix.M21 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            matrix.M22 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            matrix.M23 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            matrix.M24 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            matrix.M31 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            matrix.M32 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            matrix.M33 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            matrix.M34 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            matrix.M41 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            matrix.M42 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            matrix.M43 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
            matrix.M44 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            return matrix;
        }
        public static Matrix Multiply(in Matrix matrix1, float scaleFactor)
        {
            Matrix matrix;
            float num = scaleFactor;
            matrix.M11 = matrix1.M11 * num;
            matrix.M12 = matrix1.M12 * num;
            matrix.M13 = matrix1.M13 * num;
            matrix.M14 = matrix1.M14 * num;
            matrix.M21 = matrix1.M21 * num;
            matrix.M22 = matrix1.M22 * num;
            matrix.M23 = matrix1.M23 * num;
            matrix.M24 = matrix1.M24 * num;
            matrix.M31 = matrix1.M31 * num;
            matrix.M32 = matrix1.M32 * num;
            matrix.M33 = matrix1.M33 * num;
            matrix.M34 = matrix1.M34 * num;
            matrix.M41 = matrix1.M41 * num;
            matrix.M42 = matrix1.M42 * num;
            matrix.M43 = matrix1.M43 * num;
            matrix.M44 = matrix1.M44 * num;
            return matrix;
        }
        public static void Multiply(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            float num = (((matrix1.M11 * matrix2.M11) + (matrix1.M12 * matrix2.M21)) + (matrix1.M13 * matrix2.M31)) + (matrix1.M14 * matrix2.M41);
            float num2 = (((matrix1.M11 * matrix2.M12) + (matrix1.M12 * matrix2.M22)) + (matrix1.M13 * matrix2.M32)) + (matrix1.M14 * matrix2.M42);
            float num3 = (((matrix1.M11 * matrix2.M13) + (matrix1.M12 * matrix2.M23)) + (matrix1.M13 * matrix2.M33)) + (matrix1.M14 * matrix2.M43);
            float num4 = (((matrix1.M11 * matrix2.M14) + (matrix1.M12 * matrix2.M24)) + (matrix1.M13 * matrix2.M34)) + (matrix1.M14 * matrix2.M44);
            float num5 = (((matrix1.M21 * matrix2.M11) + (matrix1.M22 * matrix2.M21)) + (matrix1.M23 * matrix2.M31)) + (matrix1.M24 * matrix2.M41);
            float num6 = (((matrix1.M21 * matrix2.M12) + (matrix1.M22 * matrix2.M22)) + (matrix1.M23 * matrix2.M32)) + (matrix1.M24 * matrix2.M42);
            float num7 = (((matrix1.M21 * matrix2.M13) + (matrix1.M22 * matrix2.M23)) + (matrix1.M23 * matrix2.M33)) + (matrix1.M24 * matrix2.M43);
            float num8 = (((matrix1.M21 * matrix2.M14) + (matrix1.M22 * matrix2.M24)) + (matrix1.M23 * matrix2.M34)) + (matrix1.M24 * matrix2.M44);
            float num9 = (((matrix1.M31 * matrix2.M11) + (matrix1.M32 * matrix2.M21)) + (matrix1.M33 * matrix2.M31)) + (matrix1.M34 * matrix2.M41);
            float num10 = (((matrix1.M31 * matrix2.M12) + (matrix1.M32 * matrix2.M22)) + (matrix1.M33 * matrix2.M32)) + (matrix1.M34 * matrix2.M42);
            float num11 = (((matrix1.M31 * matrix2.M13) + (matrix1.M32 * matrix2.M23)) + (matrix1.M33 * matrix2.M33)) + (matrix1.M34 * matrix2.M43);
            float num12 = (((matrix1.M31 * matrix2.M14) + (matrix1.M32 * matrix2.M24)) + (matrix1.M33 * matrix2.M34)) + (matrix1.M34 * matrix2.M44);
            float num13 = (((matrix1.M41 * matrix2.M11) + (matrix1.M42 * matrix2.M21)) + (matrix1.M43 * matrix2.M31)) + (matrix1.M44 * matrix2.M41);
            float num14 = (((matrix1.M41 * matrix2.M12) + (matrix1.M42 * matrix2.M22)) + (matrix1.M43 * matrix2.M32)) + (matrix1.M44 * matrix2.M42);
            float num15 = (((matrix1.M41 * matrix2.M13) + (matrix1.M42 * matrix2.M23)) + (matrix1.M43 * matrix2.M33)) + (matrix1.M44 * matrix2.M43);
            float num16 = (((matrix1.M41 * matrix2.M14) + (matrix1.M42 * matrix2.M24)) + (matrix1.M43 * matrix2.M34)) + (matrix1.M44 * matrix2.M44);
            result.M11 = num;
            result.M12 = num2;
            result.M13 = num3;
            result.M14 = num4;
            result.M21 = num5;
            result.M22 = num6;
            result.M23 = num7;
            result.M24 = num8;
            result.M31 = num9;
            result.M32 = num10;
            result.M33 = num11;
            result.M34 = num12;
            result.M41 = num13;
            result.M42 = num14;
            result.M43 = num15;
            result.M44 = num16;
        }
        public static void Multiply(ref Matrix matrix1, float scaleFactor, out Matrix result)
        {
            float num = scaleFactor;
            result.M11 = matrix1.M11 * num;
            result.M12 = matrix1.M12 * num;
            result.M13 = matrix1.M13 * num;
            result.M14 = matrix1.M14 * num;
            result.M21 = matrix1.M21 * num;
            result.M22 = matrix1.M22 * num;
            result.M23 = matrix1.M23 * num;
            result.M24 = matrix1.M24 * num;
            result.M31 = matrix1.M31 * num;
            result.M32 = matrix1.M32 * num;
            result.M33 = matrix1.M33 * num;
            result.M34 = matrix1.M34 * num;
            result.M41 = matrix1.M41 * num;
            result.M42 = matrix1.M42 * num;
            result.M43 = matrix1.M43 * num;
            result.M44 = matrix1.M44 * num;
        }
        public static Matrix Negate(in Matrix matrix)
        {
            Matrix matrix2;
            matrix2.M11 = -matrix.M11;
            matrix2.M12 = -matrix.M12;
            matrix2.M13 = -matrix.M13;
            matrix2.M14 = -matrix.M14;
            matrix2.M21 = -matrix.M21;
            matrix2.M22 = -matrix.M22;
            matrix2.M23 = -matrix.M23;
            matrix2.M24 = -matrix.M24;
            matrix2.M31 = -matrix.M31;
            matrix2.M32 = -matrix.M32;
            matrix2.M33 = -matrix.M33;
            matrix2.M34 = -matrix.M34;
            matrix2.M41 = -matrix.M41;
            matrix2.M42 = -matrix.M42;
            matrix2.M43 = -matrix.M43;
            matrix2.M44 = -matrix.M44;
            return matrix2;
        }
        public static void Negate(ref Matrix matrix, out Matrix result)
        {
            result.M11 = -matrix.M11;
            result.M12 = -matrix.M12;
            result.M13 = -matrix.M13;
            result.M14 = -matrix.M14;
            result.M21 = -matrix.M21;
            result.M22 = -matrix.M22;
            result.M23 = -matrix.M23;
            result.M24 = -matrix.M24;
            result.M31 = -matrix.M31;
            result.M32 = -matrix.M32;
            result.M33 = -matrix.M33;
            result.M34 = -matrix.M34;
            result.M41 = -matrix.M41;
            result.M42 = -matrix.M42;
            result.M43 = -matrix.M43;
            result.M44 = -matrix.M44;
        }
        public static Matrix Subtract(in Matrix matrix1, in Matrix matrix2)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 - matrix2.M11;
            matrix.M12 = matrix1.M12 - matrix2.M12;
            matrix.M13 = matrix1.M13 - matrix2.M13;
            matrix.M14 = matrix1.M14 - matrix2.M14;
            matrix.M21 = matrix1.M21 - matrix2.M21;
            matrix.M22 = matrix1.M22 - matrix2.M22;
            matrix.M23 = matrix1.M23 - matrix2.M23;
            matrix.M24 = matrix1.M24 - matrix2.M24;
            matrix.M31 = matrix1.M31 - matrix2.M31;
            matrix.M32 = matrix1.M32 - matrix2.M32;
            matrix.M33 = matrix1.M33 - matrix2.M33;
            matrix.M34 = matrix1.M34 - matrix2.M34;
            matrix.M41 = matrix1.M41 - matrix2.M41;
            matrix.M42 = matrix1.M42 - matrix2.M42;
            matrix.M43 = matrix1.M43 - matrix2.M43;
            matrix.M44 = matrix1.M44 - matrix2.M44;
            return matrix;
        }
        public static void Subtract(ref Matrix matrix1, ref Matrix matrix2, out Matrix result)
        {
            result.M11 = matrix1.M11 - matrix2.M11;
            result.M12 = matrix1.M12 - matrix2.M12;
            result.M13 = matrix1.M13 - matrix2.M13;
            result.M14 = matrix1.M14 - matrix2.M14;
            result.M21 = matrix1.M21 - matrix2.M21;
            result.M22 = matrix1.M22 - matrix2.M22;
            result.M23 = matrix1.M23 - matrix2.M23;
            result.M24 = matrix1.M24 - matrix2.M24;
            result.M31 = matrix1.M31 - matrix2.M31;
            result.M32 = matrix1.M32 - matrix2.M32;
            result.M33 = matrix1.M33 - matrix2.M33;
            result.M34 = matrix1.M34 - matrix2.M34;
            result.M41 = matrix1.M41 - matrix2.M41;
            result.M42 = matrix1.M42 - matrix2.M42;
            result.M43 = matrix1.M43 - matrix2.M43;
            result.M44 = matrix1.M44 - matrix2.M44;
        }

        #endregion

        #region Instance
        
        public float Determinant()
        {
            float num = this.M11;
            float num2 = this.M12;
            float num3 = this.M13;
            float num4 = this.M14;
            float num5 = this.M21;
            float num6 = this.M22;
            float num7 = this.M23;
            float num8 = this.M24;
            float num9 = this.M31;
            float num10 = this.M32;
            float num11 = this.M33;
            float num12 = this.M34;
            float num13 = this.M41;
            float num14 = this.M42;
            float num15 = this.M43;
            float num16 = this.M44;
            float num17 = (num11 * num16) - (num12 * num15);
            float num18 = (num10 * num16) - (num12 * num14);
            float num19 = (num10 * num15) - (num11 * num14);
            float num20 = (num9 * num16) - (num12 * num13);
            float num21 = (num9 * num15) - (num11 * num13);
            float num22 = (num9 * num14) - (num10 * num13);
            return ((((num * (((num6 * num17) - (num7 * num18)) + (num8 * num19))) - (num2 * (((num5 * num17) - (num7 * num20)) + (num8 * num21)))) + (num3 * (((num5 * num18) - (num6 * num20)) + (num8 * num22)))) - (num4 * (((num5 * num19) - (num6 * num21)) + (num7 * num22))));
        }
        
        public bool Equals(Matrix other)
        {
            return ((((((this.M11 == other.M11) && (this.M22 == other.M22)) && ((this.M33 == other.M33) && (this.M44 == other.M44))) && (((this.M12 == other.M12) && (this.M13 == other.M13)) && ((this.M14 == other.M14) && (this.M21 == other.M21)))) && ((((this.M23 == other.M23) && (this.M24 == other.M24)) && ((this.M31 == other.M31) && (this.M32 == other.M32))) && (((this.M34 == other.M34) && (this.M41 == other.M41)) && (this.M42 == other.M42)))) && (this.M43 == other.M43));
        }
        
        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Matrix)
            {
                flag = this.Equals((Matrix)obj);
            }
            return flag;
        }
        
        public override int GetHashCode()
        {
            return (((((((((((((((this.M11.GetHashCode() + this.M12.GetHashCode()) + this.M13.GetHashCode()) + this.M14.GetHashCode()) + this.M21.GetHashCode()) + this.M22.GetHashCode()) + this.M23.GetHashCode()) + this.M24.GetHashCode()) + this.M31.GetHashCode()) + this.M32.GetHashCode()) + this.M33.GetHashCode()) + this.M34.GetHashCode()) + this.M41.GetHashCode()) + this.M42.GetHashCode()) + this.M43.GetHashCode()) + this.M44.GetHashCode());
        }
        
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return ("{ " + string.Format(currentCulture, "{{M11:{0} M12:{1} M13:{2} M14:{3}}} ", new object[] { this.M11.ToString(currentCulture), this.M12.ToString(currentCulture), this.M13.ToString(currentCulture), this.M14.ToString(currentCulture) }) + string.Format(currentCulture, "{{M21:{0} M22:{1} M23:{2} M24:{3}}} ", new object[] { this.M21.ToString(currentCulture), this.M22.ToString(currentCulture), this.M23.ToString(currentCulture), this.M24.ToString(currentCulture) }) + string.Format(currentCulture, "{{M31:{0} M32:{1} M33:{2} M34:{3}}} ", new object[] { this.M31.ToString(currentCulture), this.M32.ToString(currentCulture), this.M33.ToString(currentCulture), this.M34.ToString(currentCulture) }) + string.Format(currentCulture, "{{M41:{0} M42:{1} M43:{2} M44:{3}}} ", new object[] { this.M41.ToString(currentCulture), this.M42.ToString(currentCulture), this.M43.ToString(currentCulture), this.M44.ToString(currentCulture) }) + "}");
        }

        public Matrix Adjoint()
        {
            float val0 = M22 * (M33 * M44 - M43 * M34) - M23 * (M32 * M44 - M42 * M34) + M24 * (M32 * M43 - M42 * M33);
            float val1 = -(M12 * (M33 * M44 - M43 * M34) - M13 * (M32 * M44 - M42 * M34) + M14 * (M32 * M43 - M42 * M33));
            float val2 = M12 * (M23 * M44 - M43 * M24) - M13 * (M22 * M44 - M42 * M24) + M14 * (M22 * M43 - M42 * M23);
            float val3 = -(M12 * (M23 * M34 - M33 * M24) - M13 * (M22 * M34 - M32 * M24) + M14 * (M22 * M33 - M32 * M23));
            float val4 = -(M21 * (M33 * M44 - M43 * M34) - M23 * (M31 * M44 - M41 * M34) + M24 * (M31 * M43 - M41 * M33));
            float val5 = M11 * (M33 * M44 - M43 * M34) - M13 * (M31 * M44 - M41 * M34) + M14 * (M31 * M43 - M41 * M33);
            float val6 = -(M11 * (M23 * M44 - M43 * M24) - M13 * (M21 * M44 - M41 * M24) + M14 * (M21 * M43 - M41 * M23));
            float val7 = M11 * (M23 * M34 - M33 * M24) - M13 * (M21 * M34 - M31 * M24) + M14 * (M21 * M33 - M31 * M23);
            float val8 = M21 * (M32 * M44 - M42 * M34) - M22 * (M31 * M44 - M41 * M34) + M24 * (M31 * M42 - M41 * M32);
            float val9 = -(M11 * (M32 * M44 - M42 * M34) - M12 * (M31 * M44 - M41 * M34) + M14 * (M31 * M42 - M41 * M32));
            float val10 = M11 * (M22 * M44 - M42 * M24) - M12 * (M21 * M44 - M41 * M24) + M14 * (M21 * M42 - M41 * M22);
            float val11 = -(M11 * (M22 * M34 - M32 * M24) - M12 * (M21 * M34 - M31 * M24) + M14 * (M21 * M32 - M31 * M22));
            float val12 = -(M21 * (M32 * M43 - M42 * M33) - M22 * (M31 * M43 - M41 * M33) + M23 * (M31 * M42 - M41 * M32));
            float val13 = M11 * (M32 * M43 - M42 * M33) - M12 * (M31 * M43 - M41 * M33) + M13 * (M31 * M42 - M41 * M32);
            float val14 = -(M11 * (M22 * M43 - M42 * M23) - M12 * (M21 * M43 - M41 * M23) + M13 * (M21 * M42 - M41 * M22));
            float val15 = M11 * (M22 * M33 - M32 * M23) - M12 * (M21 * M33 - M31 * M23) + M13 * (M21 * M32 - M31 * M22);

            return new Matrix(val0, val1, val2, val3, val4, val5, val6, val7, val8, val9, val10, val11, val12, val13, val14, val15);
        }

        public bool IsAffine { get { return M41 == 0 && M42 == 0 && M43 == 0 && M44 == 1; } }

        public Vector3 GetRotationXYZ()
        {
            Vector3 angles;
            angles.Y = (float)System.Math.Asin(-M13);
            float cosY = (float)System.Math.Cos(angles.Y);
            angles.X = (float)System.Math.Acos(M33 / cosY);
            angles.Z = (float)System.Math.Acos(M11 / cosY);
            return angles;
        }

        #endregion

        #region Static Methods           

        public static Matrix Invert(in Matrix matrix)
        {
            Matrix matrix2;
            float num = matrix.M11;
            float num2 = matrix.M12;
            float num3 = matrix.M13;
            float num4 = matrix.M14;
            float num5 = matrix.M21;
            float num6 = matrix.M22;
            float num7 = matrix.M23;
            float num8 = matrix.M24;
            float num9 = matrix.M31;
            float num10 = matrix.M32;
            float num11 = matrix.M33;
            float num12 = matrix.M34;
            float num13 = matrix.M41;
            float num14 = matrix.M42;
            float num15 = matrix.M43;
            float num16 = matrix.M44;
            float num17 = (num11 * num16) - (num12 * num15);
            float num18 = (num10 * num16) - (num12 * num14);
            float num19 = (num10 * num15) - (num11 * num14);
            float num20 = (num9 * num16) - (num12 * num13);
            float num21 = (num9 * num15) - (num11 * num13);
            float num22 = (num9 * num14) - (num10 * num13);
            float num23 = ((num6 * num17) - (num7 * num18)) + (num8 * num19);
            float num24 = -(((num5 * num17) - (num7 * num20)) + (num8 * num21));
            float num25 = ((num5 * num18) - (num6 * num20)) + (num8 * num22);
            float num26 = -(((num5 * num19) - (num6 * num21)) + (num7 * num22));
            float num27 = 1f / ((((num * num23) + (num2 * num24)) + (num3 * num25)) + (num4 * num26));
            matrix2.M11 = num23 * num27;
            matrix2.M21 = num24 * num27;
            matrix2.M31 = num25 * num27;
            matrix2.M41 = num26 * num27;
            matrix2.M12 = -(((num2 * num17) - (num3 * num18)) + (num4 * num19)) * num27;
            matrix2.M22 = (((num * num17) - (num3 * num20)) + (num4 * num21)) * num27;
            matrix2.M32 = -(((num * num18) - (num2 * num20)) + (num4 * num22)) * num27;
            matrix2.M42 = (((num * num19) - (num2 * num21)) + (num3 * num22)) * num27;
            float num28 = (num7 * num16) - (num8 * num15);
            float num29 = (num6 * num16) - (num8 * num14);
            float num30 = (num6 * num15) - (num7 * num14);
            float num31 = (num5 * num16) - (num8 * num13);
            float num32 = (num5 * num15) - (num7 * num13);
            float num33 = (num5 * num14) - (num6 * num13);
            matrix2.M13 = (((num2 * num28) - (num3 * num29)) + (num4 * num30)) * num27;
            matrix2.M23 = -(((num * num28) - (num3 * num31)) + (num4 * num32)) * num27;
            matrix2.M33 = (((num * num29) - (num2 * num31)) + (num4 * num33)) * num27;
            matrix2.M43 = -(((num * num30) - (num2 * num32)) + (num3 * num33)) * num27;
            float num34 = (num7 * num12) - (num8 * num11);
            float num35 = (num6 * num12) - (num8 * num10);
            float num36 = (num6 * num11) - (num7 * num10);
            float num37 = (num5 * num12) - (num8 * num9);
            float num38 = (num5 * num11) - (num7 * num9);
            float num39 = (num5 * num10) - (num6 * num9);
            matrix2.M14 = -(((num2 * num34) - (num3 * num35)) + (num4 * num36)) * num27;
            matrix2.M24 = (((num * num34) - (num3 * num37)) + (num4 * num38)) * num27;
            matrix2.M34 = -(((num * num35) - (num2 * num37)) + (num4 * num39)) * num27;
            matrix2.M44 = (((num * num36) - (num2 * num38)) + (num3 * num39)) * num27;
            return matrix2;
        }

        public static void Invert(ref Matrix matrix, out Matrix result)
        {
            float num = matrix.M11;
            float num2 = matrix.M12;
            float num3 = matrix.M13;
            float num4 = matrix.M14;
            float num5 = matrix.M21;
            float num6 = matrix.M22;
            float num7 = matrix.M23;
            float num8 = matrix.M24;
            float num9 = matrix.M31;
            float num10 = matrix.M32;
            float num11 = matrix.M33;
            float num12 = matrix.M34;
            float num13 = matrix.M41;
            float num14 = matrix.M42;
            float num15 = matrix.M43;
            float num16 = matrix.M44;
            float num17 = (num11 * num16) - (num12 * num15);
            float num18 = (num10 * num16) - (num12 * num14);
            float num19 = (num10 * num15) - (num11 * num14);
            float num20 = (num9 * num16) - (num12 * num13);
            float num21 = (num9 * num15) - (num11 * num13);
            float num22 = (num9 * num14) - (num10 * num13);
            float num23 = ((num6 * num17) - (num7 * num18)) + (num8 * num19);
            float num24 = -(((num5 * num17) - (num7 * num20)) + (num8 * num21));
            float num25 = ((num5 * num18) - (num6 * num20)) + (num8 * num22);
            float num26 = -(((num5 * num19) - (num6 * num21)) + (num7 * num22));
            float num27 = 1f / ((((num * num23) + (num2 * num24)) + (num3 * num25)) + (num4 * num26));
            result.M11 = num23 * num27;
            result.M21 = num24 * num27;
            result.M31 = num25 * num27;
            result.M41 = num26 * num27;
            result.M12 = -(((num2 * num17) - (num3 * num18)) + (num4 * num19)) * num27;
            result.M22 = (((num * num17) - (num3 * num20)) + (num4 * num21)) * num27;
            result.M32 = -(((num * num18) - (num2 * num20)) + (num4 * num22)) * num27;
            result.M42 = (((num * num19) - (num2 * num21)) + (num3 * num22)) * num27;
            float num28 = (num7 * num16) - (num8 * num15);
            float num29 = (num6 * num16) - (num8 * num14);
            float num30 = (num6 * num15) - (num7 * num14);
            float num31 = (num5 * num16) - (num8 * num13);
            float num32 = (num5 * num15) - (num7 * num13);
            float num33 = (num5 * num14) - (num6 * num13);
            result.M13 = (((num2 * num28) - (num3 * num29)) + (num4 * num30)) * num27;
            result.M23 = -(((num * num28) - (num3 * num31)) + (num4 * num32)) * num27;
            result.M33 = (((num * num29) - (num2 * num31)) + (num4 * num33)) * num27;
            result.M43 = -(((num * num30) - (num2 * num32)) + (num3 * num33)) * num27;
            float num34 = (num7 * num12) - (num8 * num11);
            float num35 = (num6 * num12) - (num8 * num10);
            float num36 = (num6 * num11) - (num7 * num10);
            float num37 = (num5 * num12) - (num8 * num9);
            float num38 = (num5 * num11) - (num7 * num9);
            float num39 = (num5 * num10) - (num6 * num9);
            result.M14 = -(((num2 * num34) - (num3 * num35)) + (num4 * num36)) * num27;
            result.M24 = (((num * num34) - (num3 * num37)) + (num4 * num38)) * num27;
            result.M34 = -(((num * num35) - (num2 * num37)) + (num4 * num39)) * num27;
            result.M44 = (((num * num36) - (num2 * num38)) + (num3 * num39)) * num27;
        }
      
        public static Matrix Lerp(in Matrix matrix1, in Matrix matrix2, float amount)
        {
            Matrix matrix;
            matrix.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
            matrix.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
            matrix.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
            matrix.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
            matrix.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
            matrix.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
            matrix.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
            matrix.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
            matrix.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
            matrix.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
            matrix.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
            matrix.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
            matrix.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
            matrix.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
            matrix.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
            matrix.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
            return matrix;
        }

        public static void Lerp(ref Matrix matrix1, ref Matrix matrix2, float amount, out Matrix result)
        {
            result.M11 = matrix1.M11 + ((matrix2.M11 - matrix1.M11) * amount);
            result.M12 = matrix1.M12 + ((matrix2.M12 - matrix1.M12) * amount);
            result.M13 = matrix1.M13 + ((matrix2.M13 - matrix1.M13) * amount);
            result.M14 = matrix1.M14 + ((matrix2.M14 - matrix1.M14) * amount);
            result.M21 = matrix1.M21 + ((matrix2.M21 - matrix1.M21) * amount);
            result.M22 = matrix1.M22 + ((matrix2.M22 - matrix1.M22) * amount);
            result.M23 = matrix1.M23 + ((matrix2.M23 - matrix1.M23) * amount);
            result.M24 = matrix1.M24 + ((matrix2.M24 - matrix1.M24) * amount);
            result.M31 = matrix1.M31 + ((matrix2.M31 - matrix1.M31) * amount);
            result.M32 = matrix1.M32 + ((matrix2.M32 - matrix1.M32) * amount);
            result.M33 = matrix1.M33 + ((matrix2.M33 - matrix1.M33) * amount);
            result.M34 = matrix1.M34 + ((matrix2.M34 - matrix1.M34) * amount);
            result.M41 = matrix1.M41 + ((matrix2.M41 - matrix1.M41) * amount);
            result.M42 = matrix1.M42 + ((matrix2.M42 - matrix1.M42) * amount);
            result.M43 = matrix1.M43 + ((matrix2.M43 - matrix1.M43) * amount);
            result.M44 = matrix1.M44 + ((matrix2.M44 - matrix1.M44) * amount);
        }
        
        public static Matrix Transform(in Matrix value, in Quaternion rotation)
        {
            Matrix matrix;
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num;
            float num5 = rotation.W * num2;
            float num6 = rotation.W * num3;
            float num7 = rotation.X * num;
            float num8 = rotation.X * num2;
            float num9 = rotation.X * num3;
            float num10 = rotation.Y * num2;
            float num11 = rotation.Y * num3;
            float num12 = rotation.Z * num3;
            float num13 = (1f - num10) - num12;
            float num14 = num8 - num6;
            float num15 = num9 + num5;
            float num16 = num8 + num6;
            float num17 = (1f - num7) - num12;
            float num18 = num11 - num4;
            float num19 = num9 - num5;
            float num20 = num11 + num4;
            float num21 = (1f - num7) - num10;
            matrix.M11 = ((value.M11 * num13) + (value.M12 * num14)) + (value.M13 * num15);
            matrix.M12 = ((value.M11 * num16) + (value.M12 * num17)) + (value.M13 * num18);
            matrix.M13 = ((value.M11 * num19) + (value.M12 * num20)) + (value.M13 * num21);
            matrix.M14 = value.M14;
            matrix.M21 = ((value.M21 * num13) + (value.M22 * num14)) + (value.M23 * num15);
            matrix.M22 = ((value.M21 * num16) + (value.M22 * num17)) + (value.M23 * num18);
            matrix.M23 = ((value.M21 * num19) + (value.M22 * num20)) + (value.M23 * num21);
            matrix.M24 = value.M24;
            matrix.M31 = ((value.M31 * num13) + (value.M32 * num14)) + (value.M33 * num15);
            matrix.M32 = ((value.M31 * num16) + (value.M32 * num17)) + (value.M33 * num18);
            matrix.M33 = ((value.M31 * num19) + (value.M32 * num20)) + (value.M33 * num21);
            matrix.M34 = value.M34;
            matrix.M41 = ((value.M41 * num13) + (value.M42 * num14)) + (value.M43 * num15);
            matrix.M42 = ((value.M41 * num16) + (value.M42 * num17)) + (value.M43 * num18);
            matrix.M43 = ((value.M41 * num19) + (value.M42 * num20)) + (value.M43 * num21);
            matrix.M44 = value.M44;
            return matrix;
        }
       
        public static void Transform(ref Matrix value, ref Quaternion rotation, out Matrix result)
        {
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num;
            float num5 = rotation.W * num2;
            float num6 = rotation.W * num3;
            float num7 = rotation.X * num;
            float num8 = rotation.X * num2;
            float num9 = rotation.X * num3;
            float num10 = rotation.Y * num2;
            float num11 = rotation.Y * num3;
            float num12 = rotation.Z * num3;
            float num13 = (1f - num10) - num12;
            float num14 = num8 - num6;
            float num15 = num9 + num5;
            float num16 = num8 + num6;
            float num17 = (1f - num7) - num12;
            float num18 = num11 - num4;
            float num19 = num9 - num5;
            float num20 = num11 + num4;
            float num21 = (1f - num7) - num10;
            float num22 = ((value.M11 * num13) + (value.M12 * num14)) + (value.M13 * num15);
            float num23 = ((value.M11 * num16) + (value.M12 * num17)) + (value.M13 * num18);
            float num24 = ((value.M11 * num19) + (value.M12 * num20)) + (value.M13 * num21);
            float num25 = value.M14;
            float num26 = ((value.M21 * num13) + (value.M22 * num14)) + (value.M23 * num15);
            float num27 = ((value.M21 * num16) + (value.M22 * num17)) + (value.M23 * num18);
            float num28 = ((value.M21 * num19) + (value.M22 * num20)) + (value.M23 * num21);
            float num29 = value.M24;
            float num30 = ((value.M31 * num13) + (value.M32 * num14)) + (value.M33 * num15);
            float num31 = ((value.M31 * num16) + (value.M32 * num17)) + (value.M33 * num18);
            float num32 = ((value.M31 * num19) + (value.M32 * num20)) + (value.M33 * num21);
            float num33 = value.M34;
            float num34 = ((value.M41 * num13) + (value.M42 * num14)) + (value.M43 * num15);
            float num35 = ((value.M41 * num16) + (value.M42 * num17)) + (value.M43 * num18);
            float num36 = ((value.M41 * num19) + (value.M42 * num20)) + (value.M43 * num21);
            float num37 = value.M44;
            result.M11 = num22;
            result.M12 = num23;
            result.M13 = num24;
            result.M14 = num25;
            result.M21 = num26;
            result.M22 = num27;
            result.M23 = num28;
            result.M24 = num29;
            result.M31 = num30;
            result.M32 = num31;
            result.M33 = num32;
            result.M34 = num33;
            result.M41 = num34;
            result.M42 = num35;
            result.M43 = num36;
            result.M44 = num37;
        }
   
        public static Matrix Transpose(in Matrix matrix)
        {
            Matrix matrix2;
            matrix2.M11 = matrix.M11;
            matrix2.M12 = matrix.M21;
            matrix2.M13 = matrix.M31;
            matrix2.M14 = matrix.M41;
            matrix2.M21 = matrix.M12;
            matrix2.M22 = matrix.M22;
            matrix2.M23 = matrix.M32;
            matrix2.M24 = matrix.M42;
            matrix2.M31 = matrix.M13;
            matrix2.M32 = matrix.M23;
            matrix2.M33 = matrix.M33;
            matrix2.M34 = matrix.M43;
            matrix2.M41 = matrix.M14;
            matrix2.M42 = matrix.M24;
            matrix2.M43 = matrix.M34;
            matrix2.M44 = matrix.M44;
            return matrix2;
        }

        public static void Transpose(ref Matrix matrix, out Matrix result)
        {
            float num = matrix.M11;
            float num2 = matrix.M12;
            float num3 = matrix.M13;
            float num4 = matrix.M14;
            float num5 = matrix.M21;
            float num6 = matrix.M22;
            float num7 = matrix.M23;
            float num8 = matrix.M24;
            float num9 = matrix.M31;
            float num10 = matrix.M32;
            float num11 = matrix.M33;
            float num12 = matrix.M34;
            float num13 = matrix.M41;
            float num14 = matrix.M42;
            float num15 = matrix.M43;
            float num16 = matrix.M44;
            result.M11 = num;
            result.M12 = num5;
            result.M13 = num9;
            result.M14 = num13;
            result.M21 = num2;
            result.M22 = num6;
            result.M23 = num10;
            result.M24 = num14;
            result.M31 = num3;
            result.M32 = num7;
            result.M33 = num11;
            result.M34 = num15;
            result.M41 = num4;
            result.M42 = num8;
            result.M43 = num12;
            result.M44 = num16;
        }

        public readonly Vector4 GetRow(int i)
        {
            switch (i)
            {
                case 0: return new Vector4(M11, M12, M13, M14);
                case 1: return new Vector4(M21, M22, M23, M24);
                case 2: return new Vector4(M31, M32, M33, M34);
                case 3: return new Vector4(M41, M42, M43, M44);
                default: throw new IndexOutOfRangeException();
            }
        }

        public void SetRow(int i, in Vector4 row)
        {
            switch (i)
            {
                case 0: M11 = row.X; M12 = row.Y; M13 = row.Z; M14 = row.W; break;
                case 1: M21 = row.X; M22 = row.Y; M23 = row.Z; M24 = row.W; break;
                case 2: M31 = row.X; M32 = row.Y; M33 = row.Z; M34 = row.W; break;
                case 3: M41 = row.X; M42 = row.Y; M43 = row.Z; M44 = row.W; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public readonly Vector4 GetColumn(int i)
        {
            switch (i)
            {
                case 0: return new Vector4(M11, M21, M31, M41);
                case 1: return new Vector4(M12, M22, M32, M42);
                case 2: return new Vector4(M13, M23, M33, M43);
                case 3: return new Vector4(M14, M24, M34, M44);
                default: throw new IndexOutOfRangeException();
            }
        }

        public void SetColumn(int i, in Vector4 column)
        {
            switch (i)
            {
                case 0: M11 = column.X; M21 = column.Y; M31 = column.Z; M41 = column.W; break;
                case 1: M12 = column.X; M22 = column.Y; M32 = column.Z; M42 = column.W; break;
                case 2: M13 = column.X; M23 = column.Y; M33 = column.Z; M43 = column.W; break;
                case 3: M14 = column.X; M24 = column.Y; M34 = column.Z; M44 = column.W; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public readonly Vector3 GetAxis(int i)
        {
            switch (i)
            {
                case 0: return new Vector3(M11, M12, M13);
                case 1: return new Vector3(M21, M22, M23);
                case 2: return new Vector3(M31, M32, M33);
                case 3: return new Vector3(M41, M42, M43);
                default: throw new IndexOutOfRangeException();
            }
        }

        public void SetAxis(int i, Vector3 row)
        {
            switch (i)
            {
                case 0: M11 = row.X; M12 = row.Y; M13 = row.Z; break;
                case 1: M21 = row.X; M22 = row.Y; M23 = row.Z; break;
                case 2: M31 = row.X; M32 = row.Y; M33 = row.Z; break;
                case 3: M41 = row.X; M42 = row.Y; M43 = row.Z; break;
                default: throw new IndexOutOfRangeException();
            }
        }

        public static Matrix ColumnMajor(in Vector4 x, in Vector4 y, in Vector4 z, in Vector4 t)
        {
            var mat = new Matrix(x, y, z, t);
            return Matrix.Transpose(mat);
        }

        public static Matrix RotationPivot(in Vector3 pivot, in Euler orientation)
        {
            var transformMatrix = Matrix.Translate(-pivot);
            transformMatrix *= orientation.ToMatrix();
            transformMatrix *= Matrix.Translate(pivot);
            return transformMatrix;
        }

        public static Matrix ScalePivot(in Vector3 pivot, in Vector3 scale)
        {
            var transformMatrix = Matrix.Translate(-pivot);
            transformMatrix *= Matrix.Scale(scale);
            transformMatrix *= Matrix.Translate(pivot);
            return transformMatrix;
        }
       
        public static void DecomposeAffineTranform(in Matrix matrix, out Vector3 scaling, out Euler orientation, out Vector3 translation)
        {
            orientation = Euler.FromMatrix(matrix);
            scaling = new Vector3(matrix.Right.Length(), matrix.Up.Length(), matrix.Front.Length());
            translation = matrix.Translation;
        }

        public static void DecomposeTranformationMatrix(in Matrix matrix, out Vector3 scaling, out Matrix orientation, out Vector3 translation)
        {
            unsafe
            {
                orientation = new Matrix(Vector3.Normalize(matrix.Right),
                                        Vector3.Normalize(matrix.Up),
                                        Vector3.Normalize(matrix.Front),
                                        new Vector3(0, 0, 0));

                scaling = new Vector3(matrix.Right.Length(),
                                      matrix.Up.Length(),
                                      matrix.Front.Length());

                translation = matrix.Translation;
            }
        }

        public readonly Composition GetComposition()
        {
            Composition c;
            c.Rotation = new Matrix(Vector3.Normalize(Right),
                                    Vector3.Normalize(Up),
                                    Vector3.Normalize(Front),
                                    new Vector3(0, 0, 0));

            c.Scaling = new Vector3(Right.Length(),
                                    Up.Length(),
                                    Front.Length());

            c.Translation = Translation;
            return c;
        }

        public static Matrix TransformationMatrix(Vector3 pivot, Vector3 scaling, Euler orientation, Vector3 translation)
        {
            //world = scaling * rotation * translation
            var transformMatrix = Matrix.Translate(-pivot);

            if (!(scaling.X == 1 && scaling.Y == 1 && scaling.Z == 1))
                transformMatrix *= Matrix.Scale(scaling);

            transformMatrix *= orientation.ToMatrix();

            transformMatrix *= Matrix.Translate(pivot + translation);

            return transformMatrix;
        }

        public static Vector3[] ComputeEigenVectors(Matrix m, float epsilon, out Vector3 eigenValues)
        {
            eigenValues = new Vector3();
            var eigenVectors = new Vector3[] { new Vector3(1, 0, 0), new Vector3(0, 1, 0), new Vector3(0, 0, 1) };
            float[,] r = new float[3, 3] { { 1f, 0f, 0f }, { 0f, 1f, 0f }, { 0f, 0f, 1f } };
            float u, u2, u2P1, t, c, s, temp;

            for (int i = 0; i < 32; i++)
            {
                if (System.Math.Abs(m.M12) < epsilon && System.Math.Abs(m.M13) < epsilon && System.Math.Abs(m.M23) < epsilon)
                    break;

                //eliminate [1,2] entry
                if (m.M12 != 0)
                {
                    u = (m.M22 - m.M11) * 0.5f / m.M12;
                    u2 = u * u;
                    u2P1 = u2 + 1f;
                    t = (u2 != u2P1) ? ((u < 0 ? -1f : 1f) * (float)System.Math.Sqrt(u2P1) - System.Math.Abs(u)) : 0.5f / u;
                    c = 1f / (float)System.Math.Sqrt(t * t + 1f);
                    s = c * t;

                    m.M11 -= t * m.M12;
                    m.M22 += t * m.M12;
                    m.M12 = 0;

                    temp = c * m.M13 - s * m.M23;
                    m.M23 = s * m.M13 + c * m.M23;
                    m.M13 = temp;

                    for (int j = 0; j < 3; j++)
                    {
                        temp = c * r[j, 0] - s * r[j, 1];
                        r[j, 1] = s * r[j, 0] + c * r[j, 1];
                        r[j, 0] = temp;
                    }
                }
                //eliminate [1,3] entry
                if (m.M13 != 0f)
                {
                    u = (m.M33 - m.M11) * 0.5f / m.M13;
                    u2 = u * u;
                    u2P1 = u2 + 1f;
                    t = (u2 != u2P1) ? ((u < 0f ? -1f : 1f) * (float)System.Math.Sqrt(u2P1) - System.Math.Abs(u)) : 0.5f / u;
                    c = 1f / (float)System.Math.Sqrt(t * t + 1f);
                    s = c * t;

                    m.M11 -= t * m.M13;
                    m.M33 += t * m.M13;
                    m.M13 = 0;

                    temp = c * m.M12 - s * m.M23;
                    m.M23 = s * m.M12 + c * m.M23;
                    m.M12 = temp;

                    for (int j = 0; j < 3; j++)
                    {
                        temp = c * r[j, 0] - s * r[j, 2];
                        r[j, 2] = s * r[j, 0] + c * r[j, 2];
                        r[j, 0] = temp;
                    }
                }

                //annihilate (2,3) entry
                if (m.M23 != 0f)
                {
                    u = (m.M33 - m.M22) * 0.5f / m.M23;
                    u2 = u * u;
                    u2P1 = u2 + 1f;
                    t = (u2 != u2P1) ? ((u < 0f ? -1f : 1f) * (float)System.Math.Sqrt(u2P1) - System.Math.Abs(u)) : 0.5f / u;
                    c = 1f / (float)System.Math.Sqrt(t * t + 1f);
                    s = c * t;

                    m.M22 -= t * m.M23;
                    m.M33 += t * m.M23;
                    m.M23 = 0;

                    temp = c * m.M12 - s * m.M13;
                    m.M13 = s * m.M12 + c * m.M13;
                    m.M12 = temp;

                    for (int j = 0; j < 3; j++)
                    {
                        temp = c * r[j, 1] - s * r[j, 2];
                        r[j, 2] = s * r[j, 1] + c * r[j, 2];
                        r[j, 1] = temp;
                    }
                }
            }

            eigenValues.X = m.M11;
            eigenValues.Y = m.M22;
            eigenValues.Z = m.M33;

            eigenVectors[0] = new Vector3(r[0, 0], r[1, 0], r[2, 0]);
            eigenVectors[1] = new Vector3(r[0, 1], r[1, 1], r[2, 1]);
            eigenVectors[2] = new Vector3(r[0, 2], r[1, 2], r[2, 2]);

            return eigenVectors;
        }

        public static unsafe Matrix CorrelationMatrix(byte* vertexes, int vertexCount, int stride)
        {
            Vector3 mean = Vector3.Zero;
            float n = 1.0f / (float)vertexCount;

            Matrix c = new Matrix();
            c.M44 = 1;
            Vector3* pter;

            //Compute Mean
            for (int i = 0; i < vertexCount; i++)
            {
                pter = (Vector3*)(vertexes + i * stride);
                mean += *pter;
            }
            mean *= n;

            for (int i = 0; i < vertexCount; i++)
            {
                pter = (Vector3*)(vertexes + i * stride);

                c.M11 += n * (pter->X - mean.X) * (pter->X - mean.X);
                c.M12 += n * (pter->X - mean.X) * (pter->Y - mean.Y);
                c.M13 += n * (pter->X - mean.X) * (pter->Z - mean.Z);
                c.M22 += n * (pter->Y - mean.Y) * (pter->Y - mean.Y);
                c.M23 += n * (pter->Y - mean.Y) * (pter->Z - mean.Z);
                c.M33 += n * (pter->Z - mean.Z) * (pter->Z - mean.Z);
            }
            c.M21 = c.M12;
            c.M31 = c.M13;
            c.M32 = c.M23;
            return c;
        }

        public static Matrix CorrelationMatrix(Vector3[] positions)
        {
            Vector3 mean = Vector3.Zero;
            float n = 1.0f / (float)positions.Length;
            //Compute Mean
            for (int i = 0; i < positions.Length; i++)
                mean += positions[i];
            mean *= n;

            Matrix c = new Matrix();
            for (int i = 0; i < positions.Length; i++)
            {
                c.M11 += n * (positions[i].X - mean.X) * (positions[i].X - mean.X);
                c.M12 += n * (positions[i].X - mean.X) * (positions[i].Y - mean.Y);
                c.M13 += n * (positions[i].X - mean.X) * (positions[i].Z - mean.Z);
                c.M22 += n * (positions[i].Y - mean.Y) * (positions[i].Y - mean.Y);
                c.M23 += n * (positions[i].Y - mean.Y) * (positions[i].Z - mean.Z);
                c.M33 += n * (positions[i].Z - mean.Z) * (positions[i].Z - mean.Z);
            }
            c.M21 = c.M12;
            c.M31 = c.M13;
            c.M32 = c.M23;

            return c;
        }

        public static Matrix CorrelationMatrix(IEnumerable<Vector3> positions)
        {
            Vector3 mean = Vector3.Zero;
            int count = 0;
            //Compute Mean
            foreach (var p in positions)
            {
                mean += p;
                count++;
            }
            float n = 1.0f / (float)count;
            mean *= n;

            Matrix c = new Matrix();
            foreach (var p in positions)
            {
                c.M11 += n * (p.X - mean.X) * (p.X - mean.X);
                c.M12 += n * (p.X - mean.X) * (p.Y - mean.Y);
                c.M13 += n * (p.X - mean.X) * (p.Z - mean.Z);
                c.M22 += n * (p.Y - mean.Y) * (p.Y - mean.Y);
                c.M23 += n * (p.Y - mean.Y) * (p.Z - mean.Z);
                c.M33 += n * (p.Z - mean.Z) * (p.Z - mean.Z);
            }
            c.M21 = c.M12;
            c.M31 = c.M13;
            c.M32 = c.M23;

            return c;
        }

        public static Vector3 GetRotationXYZ(in Matrix rotation)
        {
            Vector3 angles;
            angles.Y = (float)System.Math.Asin(-rotation.M13);
            float cosY = (float)System.Math.Cos(angles.Y);
            angles.X = (float)System.Math.Acos(rotation.M33 / cosY);
            angles.Z = (float)System.Math.Acos(rotation.M11 / cosY);
            return angles;
        }

        public static Matrix RotateXyz(in Vector3 angles)
        {
            Matrix m = Matrix.RotationX(angles.X);
            m *= Matrix.RotationY(angles.Y);
            m *= Matrix.RotationZ(angles.Z);
            return m;
        }
        #endregion

        #region CreationMatrix

        public static Matrix PerspectiveLh(float w, float h, float zn, float zf)
        {
            Matrix r = new Matrix();
            float a = 2 * zn;

            r.M11 = a / w;
            r.M22 = a / h;
            r.M33 = zf / (zf - zn);
            r.M43 = zn * zf / (zn - zf);
            r.M34 = 1;

            return r;
        }

        public static Matrix PerspectiveRh(float w, float h, float zn, float zf)
        {
            Matrix r = new Matrix();
            float a = 2 * zn;

            r.M11 = a / w;
            r.M22 = a / h;
            r.M33 = zf / (zn - zf);
            r.M43 = zn * zf / (zn - zf);
            r.M34 = -1;

            return r;
        }

        public static Matrix PerspectiveFovLh(float aspect, float fovY, float zn, float zf)
        {
            Matrix r = new Matrix();
            float yScale = 1.0f / (float)System.Math.Tan(fovY * 0.5f);
            float xScale = yScale / aspect;


            r.M11 = xScale;
            r.M22 = yScale;
            r.M33 = zf / (zf - zn);
            r.M43 = -zn * zf / (zf - zn);
            r.M34 = 1;

            return r;
        }

        public static Matrix PerspectiveFovRh(float aspect, float fovY, float zn, float zf)
        {
            Matrix r = new Matrix();
            float yScale = 1f / (float)System.Math.Tan(fovY * 0.5f);
            float xScale = yScale / aspect;


            r.M11 = xScale;
            r.M22 = yScale;
            r.M33 = zf / (zn - zf);
            r.M43 = zn * zf / (zn - zf);
            r.M34 = -1.0f;

            return r;
        }

        public static Matrix OrthoLh(float w, float h, float zn, float zf)
        {
            Matrix r = new Matrix();
            r.M11 = 2f / w;
            r.M22 = 2f / h;
            r.M33 = 1f / (zf - zn);
            r.M43 = -zn / (zf - zn);
            r.M44 = 1f;

            return r;
        }

        public static Matrix OrthoRh(float w, float h, float zn, float zf)
        {
            Matrix r = new Matrix();
            r.M11 = 2f / w;
            r.M22 = 2f / h;
            r.M33 = 1f / (zn - zf);
            r.M43 = zn / (zn - zf);
            r.M44 = 1f;

            return r;
        }

        /// <summary>
        ///  Builds a customized, left-handed orthographic projection matrix.
        /// </summary>
        /// <param name="l">Minimum x-value of view volume</param>
        /// <param name="r">Maximum x-value of view volume</param>
        /// <param name="b">Minimum y-value of view volume</param>
        /// <param name="t">Maximum y-value of view volume</param>
        /// <param name="zn">Minimum z-value of the view volume</param>
        /// <param name="zf">Maximum z-value of the view volume</param>
        /// <returns></returns>
        /// <remarks>
        /// The D3DXMatrixOrthoLH function is a special case of the D3DXMatrixOrthoOffCenterLH function. 
        /// To create the same projection using D3DXMatrixOrthoOffCenterLH, use the following values: l = -w/2, r = w/2, b = -h/2, and t = h/2.
        /// </remarks>
        public static Matrix OrthoOffCenterLh(float l, float r, float b, float t, float zn, float zf)
        {
            Matrix m = new Matrix();
            m.M11 = 2f / (r - 1);
            m.M22 = 2f / (t - b);
            m.M33 = 1f / (zf - zn);

            m.M41 = (l + r) / (l - r);
            m.M42 = (t + b) / (b - t);
            m.M43 = zn / (zn - zf);
            m.M44 = 1f;

            return m;
        }

        public static Matrix OrthoOffCenterRh(float l, float r, float b, float t, float zn, float zf)
        {
            Matrix m = new Matrix();
            m.M11 = 2f / (r - 1);
            m.M22 = 2f / (t - b);
            m.M33 = 1f / (zn - zf);

            m.M41 = (l + r) / (l - r);
            m.M42 = (t + b) / (b - t);
            m.M43 = zn / (zn - zf);
            m.M44 = 1f;

            return m;
        }

        public static Matrix Reflection(in Plane plane)
        {
            Matrix result = new Matrix();
            plane.Normalize();
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float x2 = x * -2f;
            float y2 = y * -2f;
            float z2 = z * -2f;
            result.M11 = (x2 * x) + ((float)1.0);
            result.M12 = y2 * x;
            result.M13 = z2 * x;
            result.M14 = 0f;
            result.M21 = x2 * y;
            result.M22 = (y2 * y) + ((float)1.0);
            result.M23 = z2 * y;
            result.M24 = 0f;
            result.M31 = x2 * z;
            result.M32 = y2 * z;
            result.M33 = (z2 * z) + ((float)1.0);
            result.M34 = 0f;
            result.M41 = plane.D * x2;
            result.M42 = plane.D * y2;
            result.M43 = plane.D * z2;
            result.M44 = 1f;
            return result;
        }

        public static void Reflection(ref Plane plane, out Matrix result)
        {
            plane.Normalize();
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float x2 = x * -2f;
            float y2 = y * -2f;
            float z2 = z * -2f;
            result.M11 = (x2 * x) + ((float)1.0);
            result.M12 = y2 * x;
            result.M13 = z2 * x;
            result.M14 = 0f;
            result.M21 = x2 * y;
            result.M22 = (y2 * y) + ((float)1.0);
            result.M23 = z2 * y;
            result.M24 = 0f;
            result.M31 = x2 * z;
            result.M32 = y2 * z;
            result.M33 = (z2 * z) + ((float)1.0);
            result.M34 = 0f;
            result.M41 = plane.D * x2;
            result.M42 = plane.D * y2;
            result.M43 = plane.D * z2;
            result.M44 = 1f;
        }

        public static Matrix RotationX(float angle)
        {
            Matrix result = new Matrix();
            float cos = (float)System.Math.Cos((double)angle);
            float sin = (float)System.Math.Sin((double)angle);
            result.M11 = 1f;
            result.M12 = 0f;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = cos;
            result.M23 = sin;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = -sin;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static Matrix RotationY(float angle)
        {
            Matrix result = new Matrix();
            float cos = (float)System.Math.Cos((double)angle);
            float sin = (float)System.Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = 0f;
            result.M13 = -sin;
            result.M14 = 0f;
            result.M21 = 0f;
            result.M22 = 1f;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = sin;
            result.M32 = 0f;
            result.M33 = cos;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static Matrix RotationZ(float angle)
        {
            Matrix result = new Matrix();
            float cos = (float)System.Math.Cos((double)angle);
            float sin = (float)System.Math.Sin((double)angle);
            result.M11 = cos;
            result.M12 = sin;
            result.M13 = 0f;
            result.M14 = 0f;
            result.M21 = -sin;
            result.M22 = cos;
            result.M23 = 0f;
            result.M24 = 0f;
            result.M31 = 0f;
            result.M32 = 0f;
            result.M33 = 1f;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static Matrix RotationAxis(in Vector3 axis, float angle)
        {
            if (axis.X == 0 && axis.Y == 0 && axis.Z == 0)
                return Matrix.Identity;

            if (axis.LengthSquared() != 1f)
            {
                axis.Normalize();
            }

            Matrix result = new Matrix();
            float x = axis.X;
            float y = axis.Y;
            float z = axis.Z;
            float cos = (float)System.Math.Cos((double)angle);
            float sin = (float)System.Math.Sin((double)angle);
            double num12 = x;
            float xx = (float)(num12 * num12);
            double num11 = y;
            float yy = (float)(num11 * num11);
            double num10 = z;
            float zz = (float)(num10 * num10);
            float xy = y * x;
            float xz = z * x;
            float yz = z * y;
            result.M11 = ((float)((1.0 - xx) * cos)) + xx;
            double num9 = xy;
            double num8 = num9 - (cos * num9);
            double num7 = sin * z;
            result.M12 = (float)(num7 + num8);
            double num6 = xz;
            double num5 = num6 - (cos * num6);
            double num4 = sin * y;
            result.M13 = (float)(num5 - num4);
            result.M14 = 0f;
            result.M21 = (float)(num8 - num7);
            result.M22 = ((float)((1.0 - yy) * cos)) + yy;
            double num3 = yz;
            double num2 = num3 - (cos * num3);
            double num = sin * x;
            result.M23 = (float)(num + num2);
            result.M24 = 0f;
            result.M31 = (float)(num4 + num5);
            result.M32 = (float)(num2 - num);
            result.M33 = ((float)((1.0 - zz) * cos)) + zz;
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static Matrix RotationQuaternion(in Quaternion rotation)
        {
            Matrix result = new Matrix();
            double x = rotation.X;
            float xx = (float)(x * x);
            double y = rotation.Y;
            float yy = (float)(y * y);
            double z = rotation.Z;
            float zz = (float)(z * z);
            float xy = rotation.Y * rotation.X;
            float zw = rotation.W * rotation.Z;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.W * rotation.Y;
            float yz = rotation.Z * rotation.Y;
            float xw = rotation.W * rotation.X;
            result.M11 = (float)(1.0 - ((zz + yy) * 2.0));
            result.M12 = (float)((zw + xy) * 2.0);
            result.M13 = (float)((zx - yw) * 2.0);
            result.M14 = 0f;
            result.M21 = (float)((xy - zw) * 2.0);
            result.M22 = (float)(1.0 - ((zz + xx) * 2.0));
            result.M23 = (float)((xw + yz) * 2.0);
            result.M24 = 0f;
            result.M31 = (float)((yw + zx) * 2.0);
            result.M32 = (float)((yz - xw) * 2.0);
            result.M33 = (float)(1.0 - ((yy + xx) * 2.0));
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
            return result;
        }

        public static void RotationQuaternion(ref Quaternion rotation, out Matrix result)
        {
            double x = rotation.X;
            float xx = (float)(x * x);
            double y = rotation.Y;
            float yy = (float)(y * y);
            double z = rotation.Z;
            float zz = (float)(z * z);
            float xy = rotation.Y * rotation.X;
            float zw = rotation.W * rotation.Z;
            float zx = rotation.Z * rotation.X;
            float yw = rotation.W * rotation.Y;
            float yz = rotation.Z * rotation.Y;
            float xw = rotation.W * rotation.X;
            result.M11 = (float)(1.0 - ((zz + yy) * 2.0));
            result.M12 = (float)((zw + xy) * 2.0);
            result.M13 = (float)((zx - yw) * 2.0);
            result.M14 = 0f;
            result.M21 = (float)((xy - zw) * 2.0);
            result.M22 = (float)(1.0 - ((zz + xx) * 2.0));
            result.M23 = (float)((xw + yz) * 2.0);
            result.M24 = 0f;
            result.M31 = (float)((yw + zx) * 2.0);
            result.M32 = (float)((yz - xw) * 2.0);
            result.M33 = (float)(1.0 - ((yy + xx) * 2.0));
            result.M34 = 0f;
            result.M41 = 0f;
            result.M42 = 0f;
            result.M43 = 0f;
            result.M44 = 1f;
        }

        public static Matrix RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            return Matrix.RotationQuaternion(Quaternion.RotationYawPitchRoll(yaw, pitch, roll));
        }

        public static Matrix Scale(in Vector3 scale)
        {
            return new Matrix { M11 = scale.X, M12 = 0f, M13 = 0f, M14 = 0f, M21 = 0f, M22 = scale.Y, M23 = 0f, M24 = 0f, M31 = 0f, M32 = 0f, M33 = scale.Z, M34 = 0f, M41 = 0f, M42 = 0f, M43 = 0f, M44 = 1f };
        }

        public static Matrix Scale(float x, float y, float z)
        {
            var m = new Matrix();
            m.M11 = x;
            m.M22 = y;
            m.M33 = z;
            m.M44 = 1f;
            return m;
        }

        public static Matrix Translate(in Vector3 t)
        {
            var m = Identity;
            m.M41 = t.X;
            m.M42 = t.Y;
            m.M43 = t.Z;
            m.M44 = 1f;
            return m;
        }

        public static Matrix Translate(float x, float y, float z)
        {
            var m = Identity;
            m.M41 = x;
            m.M42 = y;
            m.M43 = z;
            m.M44 = 1f;
            return m;
        }

        public static Matrix Pose(in Vector3 position, in  Vector3 forward, in Vector3 up)
        {
            Matrix matrix;
            Vector3 vector = Vector3.Normalize(forward);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            matrix.M11 = vector2.X;
            matrix.M12 = vector2.Y;
            matrix.M13 = vector2.Z;
            matrix.M14 = 0f;
            matrix.M21 = vector3.X;
            matrix.M22 = vector3.Y;
            matrix.M23 = vector3.Z;
            matrix.M24 = 0f;
            matrix.M31 = vector.X;
            matrix.M32 = vector.Y;
            matrix.M33 = vector.Z;
            matrix.M34 = 0f;
            matrix.M41 = position.X;
            matrix.M42 = position.Y;
            matrix.M43 = position.Z;
            matrix.M44 = 1f;
            return matrix;
        }

        public static void Pose(ref Vector3 position, ref Vector3 forward, ref Vector3 up, out Matrix result)
        {
            Vector3 vector = Vector3.Normalize(forward);
            Vector3 vector2 = Vector3.Normalize(Vector3.Cross(up, vector));
            Vector3 vector3 = Vector3.Cross(vector, vector2);
            result.M11 = vector2.X;
            result.M12 = vector2.Y;
            result.M13 = vector2.Z;
            result.M14 = 0f;
            result.M21 = vector3.X;
            result.M22 = vector3.Y;
            result.M23 = vector3.Z;
            result.M24 = 0f;
            result.M31 = vector.X;
            result.M32 = vector.Y;
            result.M33 = vector.Z;
            result.M34 = 0f;
            result.M41 = position.X;
            result.M42 = position.Y;
            result.M43 = position.Z;
            result.M44 = 1f;
        }

        public static Matrix Pose(in Vector3 scale, in Matrix rotation, in Vector3 translation)
        {
            Matrix localPose = new Matrix();

            //set translation
            localPose.M41 = translation.X;
            localPose.M42 = translation.Y;
            localPose.M43 = translation.Z;
            localPose.M41 = 1.0f;

            //set rotation and scale
            localPose.Right = rotation.Right * scale.X;
            localPose.Up = rotation.Up * scale.Y;
            localPose.Front = rotation.Front * scale.Z;

            return localPose;
        }

        public static Matrix View(Vector3 right, Vector3 up, Vector3 front, Vector3 position)
        {
            // Keep camera's axis orthogonal to each other and of unit length.
            front.Normalize();

            up = Vector3.Cross(front, right);
            up.Normalize();

            right = Vector3.Cross(up, front);
            right.Normalize();

            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(position, right);
            float y = -Vector3.Dot(position, up);
            float z = -Vector3.Dot(position, front);

            Matrix view = new Matrix();

            view.M11 = right.X;
            view.M21 = right.Y;
            view.M31 = right.Z;
            view.M41 = x;

            view.M12 = up.X;
            view.M22 = up.Y;
            view.M32 = up.Z;
            view.M42 = y;

            view.M13 = front.X;
            view.M23 = front.Y;
            view.M33 = front.Z;
            view.M43 = z;

            view.M14 = 0f;
            view.M24 = 0f;
            view.M34 = 0f;
            view.M44 = 1f;

            return view;
        }

        public static Matrix LookAt(Vector3 position, Vector3 lookAt, Vector3 up, out Vector3 right, out Vector3 front, out Vector3 vUp)
        {
            front = lookAt - position;
            front.Normalize();

            right = Vector3.Cross(up, front);
            right.Normalize();

            vUp = Vector3.Cross(front, right);
            vUp.Normalize();
            up = vUp;

            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(position, right);
            float y = -Vector3.Dot(position, up);
            float z = -Vector3.Dot(position, front);

            Matrix view = new Matrix();

            view.M11 = right.X;
            view.M21 = right.Y;
            view.M31 = right.Z;
            view.M41 = x;

            view.M12 = up.X;
            view.M22 = up.Y;
            view.M32 = up.Z;
            view.M42 = y;

            view.M13 = front.X;
            view.M23 = front.Y;
            view.M33 = front.Z;
            view.M43 = z;

            view.M14 = 0f;
            view.M24 = 0f;
            view.M34 = 0f;
            view.M44 = 1f;

            return view;
        }

        public static Matrix LookAt(Vector3 position, Vector3 lookAt, Vector3 up)
        {
            var front = lookAt - position;
            front.Normalize();

            var right = Vector3.Cross(up, front);
            right.Normalize();

            up = Vector3.Cross(front, right);
            up.Normalize();

            // Fill in the view matrix entries.            
            float x = -Vector3.Dot(position, right);
            float y = -Vector3.Dot(position, up);
            float z = -Vector3.Dot(position, front);

            Matrix view = new Matrix();

            view.M11 = right.X;
            view.M21 = right.Y;
            view.M31 = right.Z;
            view.M41 = x;

            view.M12 = up.X;
            view.M22 = up.Y;
            view.M32 = up.Z;
            view.M42 = y;

            view.M13 = front.X;
            view.M23 = front.Y;
            view.M33 = front.Z;
            view.M43 = z;

            view.M14 = 0f;
            view.M24 = 0f;
            view.M34 = 0f;
            view.M44 = 1f;

            return view;
        }


        #endregion

        public readonly bool IsZero
        {
            get
            {
                unsafe
                {
                    Matrix temp = this;
                    float* pFloats = (float*)(&temp);
                    for (int i = 0; i < 16; ++i)
                    {
                        if (pFloats[i] != 0)
                            return false;
                    }
                    return true;
                }
            }
        }
    }
}
