using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using System.Runtime.InteropServices;
using System.ComponentModel;



namespace Cybtans.Math
{

    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
   
    public struct Vector3:IEquatable<Vector3>
    {
        public static readonly Vector3 Zero = new Vector3();
        public static readonly Vector3 UnitX = new Vector3(1, 0 , 0);
        public static readonly Vector3 UnitY = new Vector3(0, 1, 0);
        public static readonly Vector3 UnitZ = new Vector3(0, 0, 1);
        public static readonly Vector3 One = new Vector3(1, 1, 1);

        public float X;
        public float Y;
        public float Z;

        #region Constructs

        public Vector3(float x, float y , float z)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
        }

        public Vector3(float x)
        {
            this.X = x;
            this.Y = x;
            this.Z = x;
        }

        public Vector3(Vector2 v, float z)
        {
            this.X = v.X;
            this.Y = v.Y;
            this.Z = z;
        }
       

        #endregion

        #region Converters

        public static explicit operator float(in Vector3 v)
        {
            return v.X;
        }

        public static explicit operator Vector2(in Vector3 v)
        {
            unsafe
            {
                return new Vector2(v.X, v.Y);
            }
        }

        public static explicit operator Vector4(in Vector3 v)
        {
            unsafe
            {
                return new Vector4(v.X, v.Y, v.Z, 1);
            }
        }    

      
        #endregion

        #region Operators

        public static Vector3 operator +(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            vector.Z = value1.Z + value2.Z;
            return vector;
        }

        public static Vector3 operator /(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            vector.Z = value1.Z / value2.Z;
            return vector;
        }
        public static Vector3 operator /(in Vector3 value, float divider)
        {
            Vector3 vector;
            float num = 1f / divider;
            vector.X = value.X * num;
            vector.Y = value.Y * num;
            vector.Z = value.Z * num;
            return vector;
        }

        public static Vector3 operator *(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            vector.Z = value1.Z * value2.Z;
            return vector;
        }
        public static Vector3 operator *(in Vector3 value, float scaleFactor)
        {
            Vector3 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            vector.Z = value.Z * scaleFactor;
            return vector;
        }
        public static Vector3 operator *(float scaleFactor, in Vector3 value)
        {
            Vector3 vector;
            vector.X = value.X * scaleFactor;
            vector.Y = value.Y * scaleFactor;
            vector.Z = value.Z * scaleFactor;
            return vector;
        }

        public static Vector3 operator -(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            vector.Z = value1.Z - value2.Z;
            return vector;
        }
        public static Vector3 operator -(in Vector3 value)
        {
            Vector3 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            vector.Z = -value.Z;
            return vector;
        }

        public static bool operator ==(in Vector3 value1, in Vector3 value2)
        {
            return (((value1.X == value2.X) && (value1.Y == value2.Y)) && (value1.Z == value2.Z));
        }

        public static bool operator !=(in Vector3 value1, in Vector3 value2)
        {
            return (((value1.X != value2.X) || (value1.Y != value2.Y)) || !(value1.Z == value2.Z));
        }

        public static implicit operator Vector3(ValueTuple<int, int, int> tuple)
        {
            return new Vector3(tuple.Item1, tuple.Item2, tuple.Item3);
        }

        public static implicit operator Vector3(ValueTuple<float, float, float> tuple)
        {
            return new Vector3(tuple.Item1, tuple.Item2,tuple.Item3);
        }

        #endregion

        #region Instance Methods

        public readonly float Length()
        {
            float num = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
            return (float)System.Math.Sqrt((double)num);
        }

        public readonly float LengthSquared()
        {
            return (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z));
        }

        public void Normalize()
        {
            float num = ((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            this.X *= num2;
            this.Y *= num2;
            this.Z *= num2;
        }

        public readonly bool Equals(Vector3 other)
        {
            return (((this.X == other.X) && (this.Y == other.Y)) && (this.Z == other.Z));
        }

        public readonly override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Vector3)
            {
                flag = this.Equals((Vector3)obj);
            }
            return flag;
        } 

        public readonly bool IsZero(float epsilon = Numerics.Epsilon)
        {
            return X.IsZero(epsilon) && Y.IsZero(epsilon) && Z.IsZero(epsilon);
        }        

        public readonly override string ToString()
        {
            return X + " ," + Y + " ," + Z;
        }     

        public readonly float[] ToArray()
        {
            return new float[] {X, Y, Z };
        }

        public readonly override int GetHashCode()
        {
            float x = this.X;
            float y = this.Y;
            float z = this.Z;
            int num = y.GetHashCode() + z.GetHashCode();
            return (x.GetHashCode() + num);
        }
        #endregion

        #region Utilities

        public static Vector3 Cross(in Vector3 vector1, in Vector3 vector2)
        {
            Vector3 vector;
            vector.X = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
            vector.Y = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
            vector.Z = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
            return vector;
        }

        public static void Cross(ref Vector3 vector1, ref Vector3 vector2, out Vector3 result)
        {
            float num = (vector1.Y * vector2.Z) - (vector1.Z * vector2.Y);
            float num2 = (vector1.Z * vector2.X) - (vector1.X * vector2.Z);
            float num3 = (vector1.X * vector2.Y) - (vector1.Y * vector2.X);
            result.X = num;
            result.Y = num2;
            result.Z = num3;
        }

        public static unsafe Vector3* Cross(Vector3* left, Vector3* right, Vector3 * result)
        {           
            result->X = (right->Z * left->Y) - (left->Z * right->Y);
            result->Y = (left->Z * right->X) - (right->Z * left->X);
            result->Z = (right->Y * left->X) - (left->Y * right->X);
            return result;
        }

        public static float Dot(in Vector3 vector1, in Vector3 vector2)
        {
            return (((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z));
        }

        public static void Dot(ref Vector3 vector1, ref Vector3 vector2, out float result)
        {
            result = ((vector1.X * vector2.X) + (vector1.Y * vector2.Y)) + (vector1.Z * vector2.Z);
        }

        public unsafe static float Dot(Vector3* a, Vector3* b)
        {
            return a->X * b->X + a->Y * b->Y + a->Z * b->Z;
        }

        public static float Length(in Vector3 a)
        {
            return a.Length();
        }

        public unsafe static float Length(Vector3* a)
        {
            return (float)System.Math.Sqrt(a->X * a->X + a->Y * a->Y + a->Z * a->Z);
        }

        public static float LengthSquared(in Vector3 a)
        {
            return a.LengthSquared();
        }

        public unsafe static float LengthSquared(Vector3* a)
        {
            return a->X * a->X + a->Y * a->Y + a->Z * a->Z;
        }

        public static float Distance(in Vector3 value1, in Vector3 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = ((num * num) + (num2 * num2)) + (num3 * num3);
            return (float)System.Math.Sqrt((double)num4);
        }

        public static void Distance(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            float num4 = ((num * num) + (num2 * num2)) + (num3 * num3);
            result = (float)System.Math.Sqrt((double)num4);
        }

        public static unsafe float Distance(Vector3 *a, Vector3 *b)
        {
            float x = a->X - b->X;
            float y = a->Y - b->Y;
            float z = a->Z - b->Z;
            return (float)System.Math.Sqrt(x * x + y * y + z * z);
        }

        public static float DistanceSquared(in Vector3 value1, in Vector3 value2)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            return (((num * num) + (num2 * num2)) + (num3 * num3));
        }

        public static void DistanceSquared(ref Vector3 value1, ref Vector3 value2, out float result)
        {
            float num = value1.X - value2.X;
            float num2 = value1.Y - value2.Y;
            float num3 = value1.Z - value2.Z;
            result = ((num * num) + (num2 * num2)) + (num3 * num3);
        }
        
        public static unsafe float DistanceSquared(in Vector3* a, in Vector3* b)
        {
            float x = a->X - b->X;
            float y = a->Y - b->Y;
            float z = a->Z - b->Z;
            return x * x + y * y + z * z;
        }

        public static Vector3 Normalize(in Vector3 value)
        {
            Vector3 vector;
            float num = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            vector.X = value.X * num2;
            vector.Y = value.Y * num2;
            vector.Z = value.Z * num2;
            return vector;
        }


        public static unsafe Vector3* Normalize(Vector3* v, Vector3* result)
        {
            *result = *v;
            result->Normalize();
            return result;
        }

        public static void Normalize(ref Vector3 value, out Vector3 result)
        {
            float num = ((value.X * value.X) + (value.Y * value.Y)) + (value.Z * value.Z);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            result.X = value.X * num2;
            result.Y = value.Y * num2;
            result.Z = value.Z * num2;
        }
        public static Vector3 Reflect(in Vector3 vector, in Vector3 normal)
        {
            Vector3 vector2;
            float num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            vector2.X = vector.X - ((2f * num) * normal.X);
            vector2.Y = vector.Y - ((2f * num) * normal.Y);
            vector2.Z = vector.Z - ((2f * num) * normal.Z);
            return vector2;
        }

        public static void Reflect(ref Vector3 vector, ref Vector3 normal, out Vector3 result)
        {
            float num = ((vector.X * normal.X) + (vector.Y * normal.Y)) + (vector.Z * normal.Z);
            result.X = vector.X - ((2f * num) * normal.X);
            result.Y = vector.Y - ((2f * num) * normal.Y);
            result.Z = vector.Z - ((2f * num) * normal.Z);
        }


        public static Vector3 Saturate(in Vector3 value)
        {
            Vector3 v;
            v.X = System.Math.Min(1, System.Math.Max(0, value.X));
            v.Y = System.Math.Min(1, System.Math.Max(0, value.Y));
            v.Z = System.Math.Min(1, System.Math.Max(0, value.Z));
            return v;
        }

        public static Vector3 Min(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = (value1.X < value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            vector.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
            return vector;
        }

        public static void Min(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = (value1.X < value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y < value2.Y) ? value1.Y : value2.Y;
            result.Z = (value1.Z < value2.Z) ? value1.Z : value2.Z;
        }

        public static Vector3 Max(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = (value1.X > value2.X) ? value1.X : value2.X;
            vector.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            vector.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
            return vector;
        }
        public static void Max(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = (value1.X > value2.X) ? value1.X : value2.X;
            result.Y = (value1.Y > value2.Y) ? value1.Y : value2.Y;
            result.Z = (value1.Z > value2.Z) ? value1.Z : value2.Z;
        }
        public static bool Equals(in Vector3 a, in Vector3 b, float epsilon = Numerics.Epsilon)
        {
            return (a - b).IsZero(epsilon);
        }
        public static Vector3 Clamp(in Vector3 value1, in Vector3 min, in Vector3 max)
        {
            Vector3 vector;
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            float z = value1.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;
            vector.X = x;
            vector.Y = y;
            vector.Z = z;
            return vector;
        }

        public static void Clamp(ref Vector3 value1, ref Vector3 min, ref Vector3 max, out Vector3 result)
        {
            float x = value1.X;
            x = (x > max.X) ? max.X : x;
            x = (x < min.X) ? min.X : x;
            float y = value1.Y;
            y = (y > max.Y) ? max.Y : y;
            y = (y < min.Y) ? min.Y : y;
            float z = value1.Z;
            z = (z > max.Z) ? max.Z : z;
            z = (z < min.Z) ? min.Z : z;
            result.X = x;
            result.Y = y;
            result.Z = z;
        }
        #endregion

        #region Unmanaged Operators

        public static unsafe Vector3* Add(Vector3 *a, Vector3 *b , Vector3 * result)
        {
            result->X = a->X + b->X;
            result->Y = a->Y + b->Y;
            result->Z = a->Z + b->Z;
            return result;
        }

        public static unsafe Vector3* Subtracts(Vector3* a, Vector3* b, Vector3* result)
        {
            result->X = a->X - b->X;
            result->Y = a->Y - b->Y;
            result->Z = a->Z - b->Z;
            return result;
        }

        public static unsafe Vector3* Mul(Vector3* a, Vector3* b, Vector3* result)
        {
            result->X = a->X * b->X;
            result->Y = a->Y * b->Y;
            result->Z = a->Z * b->Z;
            return result;
        }

        public static unsafe Vector3* Div(Vector3* a, Vector3* b, Vector3* result)
        {
            result->X = a->X / b->X;
            result->Y = a->Y / b->Y;
            result->Z = a->Z / b->Z;
            return result;
        }

        public static unsafe Vector3* Mul(Vector3* a, float b, Vector3* result)
        {
            result->X = a->X * b;
            result->Y = a->Y * b;
            result->Z = a->Z * b;
            return result;
        }

        #endregion

        #region Aritmetic Methods

        public static Vector3 Multiply(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X * value2.X;
            vector.Y = value1.Y * value2.Y;
            vector.Z = value1.Z * value2.Z;
            return vector;
        }
        public static Vector3 Multiply(in Vector3 value1, float scaleFactor)
        {
            Vector3 vector;
            vector.X = value1.X * scaleFactor;
            vector.Y = value1.Y * scaleFactor;
            vector.Z = value1.Z * scaleFactor;
            return vector;
        }
        public static void Multiply(ref Vector3 value1, float scaleFactor, out Vector3 result)
        {
            result.X = value1.X * scaleFactor;
            result.Y = value1.Y * scaleFactor;
            result.Z = value1.Z * scaleFactor;
        }
        public static void Multiply(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X * value2.X;
            result.Y = value1.Y * value2.Y;
            result.Z = value1.Z * value2.Z;
        }
        public static Vector3 Negate(in Vector3 value)
        {
            Vector3 vector;
            vector.X = -value.X;
            vector.Y = -value.Y;
            vector.Z = -value.Z;
            return vector;
        }

        public static void Negate(ref Vector3 value, out Vector3 result)
        {
            result.X = -value.X;
            result.Y = -value.Y;
            result.Z = -value.Z;
        }
        public static Vector3 Add(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X + value2.X;
            vector.Y = value1.Y + value2.Y;
            vector.Z = value1.Z + value2.Z;
            return vector;
        }
        public static void Add(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X + value2.X;
            result.Y = value1.Y + value2.Y;
            result.Z = value1.Z + value2.Z;
        }
        public static Vector3 Divide(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X / value2.X;
            vector.Y = value1.Y / value2.Y;
            vector.Z = value1.Z / value2.Z;
            return vector;
        }
        public static Vector3 Divide(in Vector3 value1, float value2)
        {
            Vector3 vector;
            float num = 1f / value2;
            vector.X = value1.X * num;
            vector.Y = value1.Y * num;
            vector.Z = value1.Z * num;
            return vector;
        }
        public static void Divide(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X / value2.X;
            result.Y = value1.Y / value2.Y;
            result.Z = value1.Z / value2.Z;
        }

        public static void Divide(ref Vector3 value1, float value2, out Vector3 result)
        {
            float num = 1f / value2;
            result.X = value1.X * num;
            result.Y = value1.Y * num;
            result.Z = value1.Z * num;
        }
        public static Vector3 Subtract(in Vector3 value1, in Vector3 value2)
        {
            Vector3 vector;
            vector.X = value1.X - value2.X;
            vector.Y = value1.Y - value2.Y;
            vector.Z = value1.Z - value2.Z;
            return vector;
        }
        public static void Subtract(ref Vector3 value1, ref Vector3 value2, out Vector3 result)
        {
            result.X = value1.X - value2.X;
            result.Y = value1.Y - value2.Y;
            result.Z = value1.Z - value2.Z;
        }

        #endregion

        #region Interpolation Methods

        public static Vector3 Barycentric(in Vector3 value1, in Vector3 value2, in Vector3 value3, float amount1, float amount2)
        {
            Vector3 vector;
            vector.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            vector.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
            vector.Z = (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z));
            return vector;
        }
        public static void Barycentric(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, float amount1, float amount2, out Vector3 result)
        {
            result.X = (value1.X + (amount1 * (value2.X - value1.X))) + (amount2 * (value3.X - value1.X));
            result.Y = (value1.Y + (amount1 * (value2.Y - value1.Y))) + (amount2 * (value3.Y - value1.Y));
            result.Z = (value1.Z + (amount1 * (value2.Z - value1.Z))) + (amount2 * (value3.Z - value1.Z));
        }
        public static Vector3 CatmullRom(in Vector3 value1, in Vector3 value2, in Vector3 value3, in Vector3 value4, float amount)
        {
            Vector3 vector;
            float num = amount * amount;
            float num2 = amount * num;
            vector.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            vector.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
            vector.Z = 0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + (((((2f * value1.Z) - (5f * value2.Z)) + (4f * value3.Z)) - value4.Z) * num)) + ((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * num2));
            return vector;
        }
        public static void CatmullRom(ref Vector3 value1, ref Vector3 value2, ref Vector3 value3, ref Vector3 value4, float amount, out Vector3 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            result.X = 0.5f * ((((2f * value2.X) + ((-value1.X + value3.X) * amount)) + (((((2f * value1.X) - (5f * value2.X)) + (4f * value3.X)) - value4.X) * num)) + ((((-value1.X + (3f * value2.X)) - (3f * value3.X)) + value4.X) * num2));
            result.Y = 0.5f * ((((2f * value2.Y) + ((-value1.Y + value3.Y) * amount)) + (((((2f * value1.Y) - (5f * value2.Y)) + (4f * value3.Y)) - value4.Y) * num)) + ((((-value1.Y + (3f * value2.Y)) - (3f * value3.Y)) + value4.Y) * num2));
            result.Z = 0.5f * ((((2f * value2.Z) + ((-value1.Z + value3.Z) * amount)) + (((((2f * value1.Z) - (5f * value2.Z)) + (4f * value3.Z)) - value4.Z) * num)) + ((((-value1.Z + (3f * value2.Z)) - (3f * value3.Z)) + value4.Z) * num2));
        }

        public static Vector3 Hermite(in Vector3 value1, in Vector3 tangent1, in Vector3 value2, in Vector3 tangent2, float amount)
        {
            Vector3 vector;
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = ((2f * num2) - (3f * num)) + 1f;
            float num4 = (-2f * num2) + (3f * num);
            float num5 = (num2 - (2f * num)) + amount;
            float num6 = num2 - num;
            vector.X = (((value1.X * num3) + (value2.X * num4)) + (tangent1.X * num5)) + (tangent2.X * num6);
            vector.Y = (((value1.Y * num3) + (value2.Y * num4)) + (tangent1.Y * num5)) + (tangent2.Y * num6);
            vector.Z = (((value1.Z * num3) + (value2.Z * num4)) + (tangent1.Z * num5)) + (tangent2.Z * num6);
            return vector;
        }

        public static void Hermite(ref Vector3 value1, ref Vector3 tangent1, ref Vector3 value2, ref Vector3 tangent2, float amount, out Vector3 result)
        {
            float num = amount * amount;
            float num2 = amount * num;
            float num3 = ((2f * num2) - (3f * num)) + 1f;
            float num4 = (-2f * num2) + (3f * num);
            float num5 = (num2 - (2f * num)) + amount;
            float num6 = num2 - num;
            result.X = (((value1.X * num3) + (value2.X * num4)) + (tangent1.X * num5)) + (tangent2.X * num6);
            result.Y = (((value1.Y * num3) + (value2.Y * num4)) + (tangent1.Y * num5)) + (tangent2.Y * num6);
            result.Z = (((value1.Z * num3) + (value2.Z * num4)) + (tangent1.Z * num5)) + (tangent2.Z * num6);
        }

        public static Vector3 Lerp(in Vector3 value1, in Vector3 value2, float amount)
        {
            Vector3 vector;
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
            return vector;
        }
        public static void Lerp(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            result.Z = value1.Z + ((value2.Z - value1.Z) * amount);
        }
        public static Vector3 SmoothStep(in Vector3 value1, in Vector3 value2, float amount)
        {
            Vector3 vector;
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            vector.X = value1.X + ((value2.X - value1.X) * amount);
            vector.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            vector.Z = value1.Z + ((value2.Z - value1.Z) * amount);
            return vector;
        }
        public static void SmoothStep(ref Vector3 value1, ref Vector3 value2, float amount, out Vector3 result)
        {
            amount = (amount > 1f) ? 1f : ((amount < 0f) ? 0f : amount);
            amount = (amount * amount) * (3f - (2f * amount));
            result.X = value1.X + ((value2.X - value1.X) * amount);
            result.Y = value1.Y + ((value2.Y - value1.Y) * amount);
            result.Z = value1.Z + ((value2.Z - value1.Z) * amount);
        }

        #endregion

        #region Transforms

        public static Vector3 Transform(in Vector3 position, in Matrix matrix)
        {
            Vector3 vector;
            float num = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
            float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
            float num3 = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
            vector.X = num;
            vector.Y = num2;
            vector.Z = num3;
            return vector;
        }      

        public static Vector3 Transform(in Vector3 value, in Quaternion rotation)
        {
            Vector3 vector;
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
            float num13 = ((value.X * ((1f - num10) - num12)) + (value.Y * (num8 - num6))) + (value.Z * (num9 + num5));
            float num14 = ((value.X * (num8 + num6)) + (value.Y * ((1f - num7) - num12))) + (value.Z * (num11 - num4));
            float num15 = ((value.X * (num9 - num5)) + (value.Y * (num11 + num4))) + (value.Z * ((1f - num7) - num10));
            vector.X = num13;
            vector.Y = num14;
            vector.Z = num15;
            return vector;
        }

        public static void Transform(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
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
                float z = sourceArray[i].Z;
                destinationArray[i].X = (((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + matrix.M41;
                destinationArray[i].Y = (((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + matrix.M42;
                destinationArray[i].Z = (((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + matrix.M43;
            }
        }
        
        public static void Transform(Vector3[] sourceArray, ref Quaternion rotation, Vector3[] destinationArray)
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
            for (int i = 0; i < sourceArray.Length; i++)
            {
                float x = sourceArray[i].X;
                float y = sourceArray[i].Y;
                float z = sourceArray[i].Z;
                destinationArray[i].X = ((x * num13) + (y * num14)) + (z * num15);
                destinationArray[i].Y = ((x * num16) + (y * num17)) + (z * num18);
                destinationArray[i].Z = ((x * num19) + (y * num20)) + (z * num21);
            }
        }
       
        public static void Transform(ref Vector3 position, ref Matrix matrix, out Vector3 result)
        {
            float num = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
            float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
            float num3 = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
            result.X = num;
            result.Y = num2;
            result.Z = num3;
        }
        
        public static void Transform(ref Vector3 value, ref Quaternion rotation, out Vector3 result)
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
            float num13 = ((value.X * ((1f - num10) - num12)) + (value.Y * (num8 - num6))) + (value.Z * (num9 + num5));
            float num14 = ((value.X * (num8 + num6)) + (value.Y * ((1f - num7) - num12))) + (value.Z * (num11 - num4));
            float num15 = ((value.X * (num9 - num5)) + (value.Y * (num11 + num4))) + (value.Z * ((1f - num7) - num10));
            result.X = num13;
            result.Y = num14;
            result.Z = num15;
        }
       
        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
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
                float z = sourceArray[sourceIndex].Z;
                destinationArray[destinationIndex].X = (((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + matrix.M41;
                destinationArray[destinationIndex].Y = (((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + matrix.M42;
                destinationArray[destinationIndex].Z = (((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + matrix.M43;
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }
       
        public static void Transform(Vector3[] sourceArray, int sourceIndex, ref Quaternion rotation, Vector3[] destinationArray, int destinationIndex, int length)
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
            while (length > 0)
            {
                float x = sourceArray[sourceIndex].X;
                float y = sourceArray[sourceIndex].Y;
                float z = sourceArray[sourceIndex].Z;
                destinationArray[destinationIndex].X = ((x * num13) + (y * num14)) + (z * num15);
                destinationArray[destinationIndex].Y = ((x * num16) + (y * num17)) + (z * num18);
                destinationArray[destinationIndex].Z = ((x * num19) + (y * num20)) + (z * num21);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }
       
        public static unsafe void Transform(Vector3* vectors, Matrix* matrix, int count, Vector3* result)
        {
            for (int i = 0; i < count; i++)
            {
                Vector3.Transform(ref *(vectors + i), ref *matrix, out *(result + i));
            }
        }

        //public static unsafe Vector3* Transform(Vector3* v, Matrix* m, Vector3* r)
        //{
        //    var w = v->X * m->M14 + v->Y * m->M24 + v->Z * m->M34 + m->M44;
        //    r->X = (v->X * m->M11 + v->Y * m->M21 + v->Z * m->M31 + m->M41) / w;
        //    r->Y = (v->X * m->M12 + v->Y * m->M22 + v->Z * m->M32 + m->M42) / w;
        //    r->Z = (v->X * m->M13 + v->Y * m->M23 + v->Z * m->M33 + m->M43) / w;
        //    return r;
        //}

        public static Vector3 TransformNormal(in Vector3 normal, in Matrix matrix)
        {
            Vector3 vector;
            float num = ((normal.X * matrix.M11) + (normal.Y * matrix.M21)) + (normal.Z * matrix.M31);
            float num2 = ((normal.X * matrix.M12) + (normal.Y * matrix.M22)) + (normal.Z * matrix.M32);
            float num3 = ((normal.X * matrix.M13) + (normal.Y * matrix.M23)) + (normal.Z * matrix.M33);
            vector.X = num;
            vector.Y = num2;
            vector.Z = num3;
            return vector;
        }
       
        public static void TransformNormal(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
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
                float z = sourceArray[i].Z;
                destinationArray[i].X = ((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31);
                destinationArray[i].Y = ((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32);
                destinationArray[i].Z = ((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33);
            }
        }
      
        public static void TransformNormal(ref Vector3 normal, ref Matrix matrix, out Vector3 result)
        {
            float num = ((normal.X * matrix.M11) + (normal.Y * matrix.M21)) + (normal.Z * matrix.M31);
            float num2 = ((normal.X * matrix.M12) + (normal.Y * matrix.M22)) + (normal.Z * matrix.M32);
            float num3 = ((normal.X * matrix.M13) + (normal.Y * matrix.M23)) + (normal.Z * matrix.M33);
            result.X = num;
            result.Y = num2;
            result.Z = num3;
        }
     
        public static void TransformNormal(Vector3[] sourceArray, int sourceIndex, ref Matrix matrix, Vector3[] destinationArray, int destinationIndex, int length)
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
                float z = sourceArray[sourceIndex].Z;
                destinationArray[destinationIndex].X = ((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31);
                destinationArray[destinationIndex].Y = ((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32);
                destinationArray[destinationIndex].Z = ((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33);
                sourceIndex++;
                destinationIndex++;
                length--;
            }
        }

        public static IEnumerable<Vector3> Transform(IEnumerable<Vector3> stream, Matrix transform)
        {
            foreach (var v in stream)
            {
                yield return Vector3.Transform(v, transform);
            }
        }

        public static IEnumerable<Vector3> TransformNormal(IEnumerable<Vector3> stream, Matrix transform)
        {
            foreach (var v in stream)
            {
                yield return Vector3.TransformNormal(v, transform);
            }
        }

        public static Vector3 TransformCoordinates(in Vector3 position, in Matrix matrix)
        {
            Vector3 vector;
            float num = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
            float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
            float num3 = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
            float num4 = 1f / ((((position.X * matrix.M14) + (position.Y * matrix.M24)) + (position.Z * matrix.M34)) + matrix.M44);
            vector.X = num * num4;
            vector.Y = num2 * num4;
            vector.Z = num3 * num4;
            return vector;
        }

        public static void TransformCoordinates(ref Vector3 position, ref Matrix matrix, out Vector3 vector)
        {            
            float num = (((position.X * matrix.M11) + (position.Y * matrix.M21)) + (position.Z * matrix.M31)) + matrix.M41;
            float num2 = (((position.X * matrix.M12) + (position.Y * matrix.M22)) + (position.Z * matrix.M32)) + matrix.M42;
            float num3 = (((position.X * matrix.M13) + (position.Y * matrix.M23)) + (position.Z * matrix.M33)) + matrix.M43;
            float num4 = 1f / ((((position.X * matrix.M14) + (position.Y * matrix.M24)) + (position.Z * matrix.M34)) + matrix.M44);
            vector.X = num * num4;
            vector.Y = num2 * num4;
            vector.Z = num3 * num4;            
        }

        public static void TransformCoordinates(Vector3[] sourceArray, ref Matrix matrix, Vector3[] destinationArray)
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
                float z = sourceArray[i].Z;

                float w = 1f / ((((x * matrix.M14) + (y * matrix.M24)) + (z * matrix.M34)) + matrix.M44);

                destinationArray[i].X = ((((x * matrix.M11) + (y * matrix.M21)) + (z * matrix.M31)) + matrix.M41) * w;
                destinationArray[i].Y = ((((x * matrix.M12) + (y * matrix.M22)) + (z * matrix.M32)) + matrix.M42) * w;
                destinationArray[i].Z = ((((x * matrix.M13) + (y * matrix.M23)) + (z * matrix.M33)) + matrix.M43) * w;
            }
        }

        #endregion
      
        //public static unsafe Vector3* TransformCoordinate(Vector3* v, Matrix* m, Vector3* r , int count)
        //{
        //    float w;
        //    for (int i = 0; i < count; i++, v++, r++)
        //    {
        //        w = v->X * m->M14 + v->Y * m->M24 + v->Z * m->M34 + m->M44;

        //        r->X = (v->X * m->M11 + v->Y * m->M21 + v->Z * m->M31 + m->M41) / w;
        //        r->Y = (v->X * m->M12 + v->Y * m->M22 + v->Z * m->M32 + m->M42) / w;
        //        r->Z = (v->X * m->M13 + v->Y * m->M23 + v->Z * m->M33 + m->M43) / w;
        //    }
            
        //    return r;
        //}

        //public static Vector3 TransformNormal(Vector3 v, Matrix m)
        //{
        //    Vector3 r;
        //    r.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
        //    r.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
        //    r.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;
        //    return r;
        //}

        //public static void TransformNormal(ref Vector3 v, ref Matrix m, out Vector3 r)
        //{
        //    r = new Vector3();
        //    r.X = v.X * m.M11 + v.Y * m.M21 + v.Z * m.M31;
        //    r.Y = v.X * m.M12 + v.Y * m.M22 + v.Z * m.M32;
        //    r.Z = v.X * m.M13 + v.Y * m.M23 + v.Z * m.M33;
        //}

        //public static unsafe Vector3* TransformNormal(Vector3* v, Matrix* m, Vector3* r)
        //{
        //    r->X = v->X * m->M11 + v->Y * m->M21 + v->Z * m->M31;
        //    r->Y = v->X * m->M12 + v->Y * m->M22 + v->Z * m->M32;
        //    r->Z = v->X * m->M13 + v->Y * m->M23 + v->Z * m->M33;
        //    return r;
        //}

        //public static unsafe Vector3* TransformNormal(Vector3* v, Matrix* m, Vector3* r , int count)
        //{
        //    for (int i = 0; i < count; i++, v++, r++)
        //    {
        //        r->X = v->X * m->M11 + v->Y * m->M21 + v->Z * m->M31;
        //        r->Y = v->X * m->M12 + v->Y * m->M22 + v->Z * m->M32;
        //        r->Z = v->X * m->M13 + v->Y * m->M23 + v->Z * m->M33;   
        //    }            
        //    return r;
        //}

        #region Coordinates

        public static Vector3 CylindricalToCartesian(float theta, float y, float radius)
        {
            return new Vector3(radius * (float)System.Math.Cos(theta), y, radius * (float)System.Math.Sin(theta));
        }

        public static Vector3 CylindricalToCartesian(in Vector3 p)
        {
            return new Vector3(p.Z * (float)System.Math.Cos(p.X), p.Y, p.Z * (float)System.Math.Sin(p.X));
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="phi">Rotation angle respect to the Xaxis </param>
        /// <param name="theta">Rotation Angle respect to the Yaxis</param>
        /// <param name="radius">lenght of the vector</param>
        /// <returns></returns>
        public static Vector3 SphericalToCartesian(float phi, float theta, float radius)
        {
            float b = radius * (float)System.Math.Sin(phi);
            return new Vector3(b * (float)System.Math.Sin(theta), radius * (float)System.Math.Cos(phi), b * (float)System.Math.Cos(theta));
        }

        public static Vector3 SphericalToCartesian(in Vector3 p)
        {
            float b = p.Z * (float)System.Math.Sin(p.X);
            return new Vector3(b * (float)System.Math.Sin(p.Y), p.Z * (float)System.Math.Cos(p.X), b * (float)System.Math.Cos(p.Y));
        }

        /// <summary>
        /// Converte a (x,y,z) = > (phi, theta ,radius)
        /// phi : Rotation angle respect to the Zaxis ,
        /// theta : Rotation Angle respect to the Yaxis , 
        /// radius : lenght of the vector
        /// </summary>
        /// <param name="v">direction</param>
        /// <returns>phi - Rotation angle respect to the Zaxis , theta - Rotation Angle respect to the Yaxis , radius - lenght of the vector</returns>
        public static Vector3 CartesianToSpherical(in Vector3 p)
        {
            float radius = (float)System.Math.Sqrt(p.X * p.X + p.Y * p.Y + p.Z * p.Z);
            float phi = (float)System.Math.Acos(p.Y / radius);
            float theta = (float)System.Math.Asin(p.X / (radius * (float)System.Math.Sin(phi)));
            return new Vector3(phi, theta, radius);
        }
        /// <summary>
        /// Converte a (x,y,z) = > (phi, theta ,radius)
        /// </summary>
        /// <param name="v"></param>
        /// <returns>phi - angle in the XY plane starting 0 at top, theta - angle in the ZX plane starting 0 at front radius - lenght of the vector</returns>
        public static Vector3 CartesianToCylindrical(float x, float y, float z)
        {
            float radius = (float)System.Math.Sqrt(x * x + z * z);
            if (radius != 0)
            {
                float theta = (float)System.Math.Acos(x / radius);
                return new Vector3(theta, y, radius);
            }
            return new Vector3(0, y, 0);
        }

        public static Vector3 CartesianToCylindrical(in Vector3 v)
        {
            float radius = (float)System.Math.Sqrt(v.X * v.X + v.Z * v.Z);
            if (radius != 0)
            {
                float theta = (float)System.Math.Acos(v.X / radius);
                return new Vector3(theta, v.Y, radius);
            }
            return new Vector3(0, v.Y, 0);
        }

        #endregion      

        public List<float> ToList() => new List<float> { X, Y, Z};

    }
  
}
