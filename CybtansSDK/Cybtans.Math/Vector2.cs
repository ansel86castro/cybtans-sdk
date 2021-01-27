using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.Globalization;


namespace Cybtans.Math
{

    public struct Point2
    {
        public int X;
        public int Y;

        public List<float> ToList() => new List<float> { X, Y};
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Vector2:IEquatable<Vector2>
    {
        public static readonly Vector2 Zero = new Vector2();
        public static readonly Vector2 UnitX = new Vector2(1, 0);
        public static readonly Vector2 UnitY = new Vector2(0, 1);
        public static readonly Vector2 One = new Vector2(1, 1);

        public float X;
        public float Y;


        #region Constructors

        public Vector2(float x)
        {
            X = x;
            Y = x;
        }

        public Vector2(float x, float y)
        {
            this.X = x;
            this.Y = y;            
        }      

        #endregion

        #region operators

        public static Vector2 operator +(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }
        public static Vector2 operator /(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            return vector;
        }
        public static Vector2 operator /(in Vector2 value1, float divider)
        {
            Vector2 vector;
            float num = 1f / divider;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            return vector;
        }
        public static bool operator ==(in Vector2 value1, in Vector2 value2)
        {
            return ((value1.X == value2.X) && (value1.Y == value2.Y));
        }
        public static bool operator !=(in Vector2 value1, in Vector2 value2)
        {
            return ((value1.X != value2.X) || !(value1.Y == value2.Y));
        }
        public static Vector2 operator *(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            return vector;
        }
        public static Vector2 operator *(in Vector2 value, float scaleFactor)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }
        public static Vector2 operator *(float scaleFactor, in Vector2 value)
        {
            Vector2 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            return vector;
        }
        public static Vector2 operator -(Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }
        public static Vector2 operator -(Vector2 value)
        {
            Vector2 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }
 
        public static implicit operator Vector2(ValueTuple<int, int> tuple)
        {
            return new Vector2(tuple.Item1, tuple.Item2);
        }

        public static implicit operator Vector2(ValueTuple<float, float> tuple)
        {
            return new Vector2(tuple.Item1, tuple.Item2);
        }

        #endregion

        #region Aritmetic Methods

        public static Vector2 Add(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            return vector;
        }
        public static void Add(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
        }
        public static Vector2 Divide(Vector2 value1, Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            return vector;
        }
        public static Vector2 Divide(in Vector2 value1, float divider)
        {
            Vector2 vector;
            float num = 1f / divider;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            return vector;
        }
        public static void Divide(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
        }
        public static void Divide(ref Vector2 value1, float divider, out Vector2 result)
        {
            float num = 1f / divider;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
        }
        public static Vector2 Multiply(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            return vector;
        }
        public static Vector2 Multiply(in Vector2 value1, float scaleFactor)
        {
            Vector2 vector;
            vector.X = value1.X * scaleFactor;
            vector.Y = value1.Y * scaleFactor;
            return vector;
        }
        public static void Multiply(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
        }
        public static void Multiply(ref Vector2 value1, float scaleFactor, out Vector2 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
        }
        public static Vector2 Negate(in Vector2 value)
        {
            Vector2 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            return vector;
        }
        public static void Negate(ref Vector2 value, out Vector2 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
        }
        public static Vector2 Subtract(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            return vector;
        }
        public static void Subtract(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
        }

        #endregion

        #region Interpolation Methods
        public static Vector2 Barycentric(in Vector2 value1, in Vector2 value2, in Vector2 value3, float amount1, float amount2)
        {
            Vector2 vector;
            vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
            return vector;
        }

        public static void Barycentric(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, float amount1, float amount2, out Vector2 result)
        {
            result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
        }
        public static Vector2 CatmullRom(in Vector2 value1, in Vector2 value2, in Vector2 value3, in Vector2 value4, float amount)
        {
            Vector2 vector;
            float num = amount * amount;
            float num2 = amount * num;
            vector.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            vector.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
            return vector;
        }
        public static void CatmullRom(ref Vector2 value1, ref Vector2 value2, ref Vector2 value3, ref Vector2 value4, float amount, out Vector2 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            result.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            result.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
        }


        public static Vector2 Hermite(in Vector2 value1, in Vector2 tangent1, in Vector2 value2, in Vector2 tangent2, float amount)
        {
            Vector2 vector;
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = ((2f * num2) - (3f * num)) + 1f;
            float num4 = (-2f * num2) + (3f * num);
            float num5 = (num2 - (2f * num)) + amount;
            float num6 = num2 - num;
            vector.X = (((value1.X * num3) + (value2.X * num4)) + (tangent1.X * num5)) + (tangent2.X * num6);
            vector.Y = (((value1.Y * num3) + (value2.Y * num4)) + (tangent1.Y * num5)) + (tangent2.Y * num6);
            return vector;
        }
        public static void Hermite(ref Vector2 value1, ref Vector2 tangent1, ref Vector2 value2, ref Vector2 tangent2, float amount, out Vector2 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = ((2f * num2) - (3f * num)) + 1f;
            float num4 = (-2f * num2) + (3f * num);
            float num5 = (num2 - (2f * num)) + amount;
            float num6 = num2 - num;
            result.X = (((value1.X * num3) + (value2.X * num4)) + (tangent1.X * num5)) + (tangent2.X * num6);
            result.Y = (((value1.Y * num3) + (value2.Y * num4)) + (tangent1.Y * num5)) + (tangent2.Y * num6);
        }
        public static Vector2 Lerp(in Vector2 value1, in Vector2 value2, float amount)
        {
            Vector2 vector;
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            return vector;
        }

        public static void Lerp(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        }
        public static Vector2 SmoothStep(in Vector2 value1, in Vector2 value2, float amount)
        {
            Vector2 vector;
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            return vector;
        }

        public static void SmoothStep(ref Vector2 value1, ref Vector2 value2, float amount, out Vector2 result)
        {
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
        }

        #endregion

        #region Instance Methods
        public readonly bool Equals(Vector2 other)
        {
            return ((this.X == other.X) && (this.Y == other.Y));
        }

        public readonly override  bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector2)
            {
                flag = this.Equals((Vector2)obj);
            }
            return flag;
        }
        public readonly override int GetHashCode()
        {
            return (this.X.GetHashCode() + this.Y.GetHashCode());
        }

        public readonly float Length()
        {
            float num = (this.X * this.X) + (this.Y * this.Y);
            return (float)System.Math.Sqrt((double)num);
        }

        public readonly float LengthSquared()
        {
            return ((this.X * this.X) + (this.Y * this.Y));
        }

        public void Normalize()
        {
            float num = (this.X * this.X) + (this.Y * this.Y);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            this.X *= num2;
            this.Y *= num2;
        }

        public readonly override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture) });
        }

        public readonly bool IsZero(float epsilon = Numerics.Epsilon)
        {
            return X.IsZero(epsilon) && Y.IsZero(epsilon);
        }

        public float[] ToArray()
        {
            return new float[] { X, Y };
        }


        #endregion

        #region Utilities

        public static float Distance(in Vector2 value1, in Vector2 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = (num * num) + (num2 * num2);
            return (float)System.Math.Sqrt((double)num3);
        }
        public static void Distance(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = (num * num) + (num2 * num2);
            result = (float)System.Math.Sqrt((double)num3);
        }

        public static float DistanceSquared(in Vector2 value1, in Vector2 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            return ((num * num) + (num2 * num2));
        }

        public static void DistanceSquared(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            result = (num * num) + (num2 * num2);
        }
        public static float Dot(in Vector2 value1, in Vector2 value2)
        {
            return ((value1.X * value2.X) + (value1.Y * value2.Y));
        }
        public static void Dot(ref Vector2 value1, ref Vector2 value2, out float result)
        {
            result = (value1.X * value2.X) + (value1.Y * value2.Y);
        }

        public static Vector2 Max(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = (value1.X > value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            return vector;
        }
        public static void Max(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = (value1.X > value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
        }
        public static Vector2 Min(in Vector2 value1, in Vector2 value2)
        {
            Vector2 vector;
            vector.X = (value1.X < value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            return vector;
        }
        public static void Min(ref Vector2 value1, ref Vector2 value2, out Vector2 result)
        {
            result.X = (value1.X < value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
        }
        public static Vector2 Normalize(in Vector2 value)
        {
            Vector2 vector;
            float num = (value.X * value.X) + (value.Y * value.Y);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            vector.X = value.X * num2;
            vector.Y = value.Y * num2;
            return vector;
        }
        public static void Normalize(ref Vector2 value, out Vector2 result)
        {
            float num = (value.X * value.X) + (value.Y * value.Y);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            result.X = value.X * num2;
            result.Y = value.Y * num2;
        }
        public static Vector2 Reflect(in Vector2 vector, in Vector2 normal)
        {
            Vector2 vector2;
            float num = (vector.X * normal.X) + (vector.Y * normal.Y);
            vector2.X = vector.X - ((2f * num) * normal.X);
            vector2.Y = vector.Y - ((2f * num) * normal.Y);
            return vector2;
        }
        public static void Reflect(ref Vector2 vector, ref Vector2 normal, out Vector2 result)
        {
            float num = (vector.X * normal.X) + (vector.Y * normal.Y);
            result.X = vector.X - ((2f * num) * normal.X);
            result.Y = vector.Y - ((2f * num) * normal.Y);
        }

       
 
        #endregion            

        #region Transformation

        public static Vector2 Transform(in Vector2 position, in Matrix matrix)
        {
            Vector2 vector;
            float num = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
            float num2 = ((position.X * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
            vector.X = num;
            vector.Y = num2;
            return vector;
        }

        public static Vector2 Transform(in Vector2 value, in Quaternion rotation)
        {
            Vector2 vector;
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num3;
            float num5 = rotation.X * num;
            float num6 = rotation.X * num2;
            float num7 = rotation.Y * num2;
            float num8 = rotation.Z * num3;
            float num9 = (value.X * ((1f - num7) - num8)) + (value.Y * (num6 - num4));
            float num10 = (value.X * (num6 + num4)) + (value.Y * ((1f - num5) - num8));
            vector.X = num9;
            vector.Y = num10;
            return vector;
        }
       
        public static void Transform(ref Vector2 position, ref Matrix matrix, out Vector2 result)
        {
            float num = ((position.X * matrix.M11) + (position.Y * matrix.M21)) + matrix.M41;
            float num2 = ((position.X * matrix.M12) + (position.Y * matrix.M22)) + matrix.M42;
            result.X = num;
            result.Y = num2;
        }
       
        public static void Transform(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException("Different Sizes");
            }
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = ((x * matrix.M11) + (y * matrix.M21)) + matrix.M41;
                destinationArray[i].Y = ((x * matrix.M12) + (y * matrix.M22)) + matrix.M42;
            }
        }

        public static void Transform(Vector2[] sourceArray, ref Quaternion rotation, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException("Different Sizes");
            }
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num3;
            float num5 = rotation.X * num;
            float num6 = rotation.X * num2;
            float num7 = rotation.Y * num2;
            float num8 = rotation.Z * num3;
            float num9 = (1f - num7) - num8;
            float num10 = num6 - num4;
            float num11 = num6 + num4;
            float num12 = (1f - num5) - num8;
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = (x * num9) + (y * num10);
                destinationArray[i].Y = (x * num11) + (y * num12);
            }
        }

        public static void Transform(ref Vector2 value, ref Quaternion rotation, out Vector2 result)
        {
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num3;
            float num5 = rotation.X * num;
            float num6 = rotation.X * num2;
            float num7 = rotation.Y * num2;
            float num8 = rotation.Z * num3;
            float num9 = (value.X * ((1f - num7) - num8)) + (value.Y * (num6 - num4));
            float num10 = (value.X * (num6 + num4)) + (value.Y * ((1f - num5) - num8));
            result.X = num9;
            result.Y = num10;
        }

        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = ((x * matrix.M11) + (y * matrix.M21)) + matrix.M41;
                destinationArray[destinationIndex].Y = ((x * matrix.M12) + (y * matrix.M22)) + matrix.M42;
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static void Transform(Vector2[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            float num = rotation.X + rotation.X;
            float num2 = rotation.Y + rotation.Y;
            float num3 = rotation.Z + rotation.Z;
            float num4 = rotation.W * num3;
            float num5 = rotation.X * num;
            float num6 = rotation.X * num2;
            float num7 = rotation.Y * num2;
            float num8 = rotation.Z * num3;
            float num9 = (1f - num7) - num8;
            float num10 = num6 - num4;
            float num11 = num6 + num4;
            float num12 = (1f - num5) - num8;
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = (x * num9) + (y * num10);
                destinationArray[destinationIndex].Y = (x * num11) + (y * num12);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static Vector2 TransformNormal(in Vector2 normal, in Matrix matrix)
        {
            Vector2 vector;
            float num = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            float num2 = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            vector.X = num;
            vector.Y = num2;
            return vector;
        }

        public static void TransformNormal(ref Vector2 normal, ref Matrix matrix, out Vector2 result)
        {
            float num = (normal.X * matrix.M11) + (normal.Y * matrix.M21);
            float num2 = (normal.X * matrix.M12) + (normal.Y * matrix.M22);
            result.X = num;
            result.Y = num2;
        }

        public static void TransformNormal(Vector2[] sourceArray, ref Matrix matrix, Vector2[] destinationArray)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (destinationArray.Length < sourceArray.Length)
            {
                throw new ArgumentException("Different Sizes");
            }
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                destinationArray[i].X = (x * matrix.M11) + (y * matrix.M21);
                destinationArray[i].Y = (x * matrix.M12) + (y * matrix.M22);
            }
        }

        public static void TransformNormal(Vector2[] sourceArray, int sourceIndex, ref Matrix matrix, Vector2[] destinationArray, int destinationIndex, int length)
        {
            if (sourceArray == null)
            {
                throw new ArgumentNullException("sourceArray");
            }
            if (destinationArray == null)
            {
                throw new ArgumentNullException("destinationArray");
            }
            if (sourceArray.Length < (sourceIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            if (destinationArray.Length < (destinationIndex + length))
            {
                throw new ArgumentException("Different Sizes");
            }
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                destinationArray[destinationIndex].X = (x * matrix.M11) + (y * matrix.M21);
                destinationArray[destinationIndex].Y = (x * matrix.M12) + (y * matrix.M22);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        #endregion

        public List<float> ToList() => new List<float> { X, Y};
    }
  
}
