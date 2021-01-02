using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{

    /// <summary>
    /// Defines a four dimensional mathematical quaternion. 
    /// </summary>
    /// 
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Quaternion :IEquatable<Quaternion>
    {
        public float X;
        public float Y;
        public float Z;
        public float W;

        public Quaternion(Vector3 value, float w)
        {
            this.X = value.X;
            this.Y = value.Y;
            this.Z = value.Z;
            this.W = w;
        }

        public Quaternion(float x, float y, float z, float w)
        {
            this.X = x;
            this.Y = y;
            this.Z = z;
            this.W = w;
        }

        public static Quaternion Identity
        {
            get
            {
                return new Quaternion { X = 0f, Y = 0f, Z = 0f, W = 1f };
            }
        }      

        #region Operators

        public static explicit operator Vector4(Quaternion a)
        {
            unsafe
            {
                return (*(Vector4*)&a);
            }
        }

        public static Quaternion operator +(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X + quaternion2.X;
            quaternion.Y = quaternion1.Y + quaternion2.Y;
            quaternion.Z = quaternion1.Z + quaternion2.Z;
            quaternion.W = quaternion1.W + quaternion2.W;
            return quaternion;
        }

        public static Quaternion operator /(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            float x = quaternion1.X;
            float y = quaternion1.Y;
            float z = quaternion1.Z;
            float w = quaternion1.W;
            float num5 = (((quaternion2.X * quaternion2.X) + (quaternion2.Y * quaternion2.Y)) + (quaternion2.Z * quaternion2.Z)) + (quaternion2.W * quaternion2.W);
            float num6 = 1f / num5;
            float num7 = -quaternion2.X * num6;
            float num8 = -quaternion2.Y * num6;
            float num9 = -quaternion2.Z * num6;
            float num10 = quaternion2.W * num6;
            float num11 = (y * num9) - (z * num8);
            float num12 = (z * num7) - (x * num9);
            float num13 = (x * num8) - (y * num7);
            float num14 = ((x * num7) + (y * num8)) + (z * num9);
            quaternion.X = ((x * num10) + (num7 * w)) + num11;
            quaternion.Y = ((y * num10) + (num8 * w)) + num12;
            quaternion.Z = ((z * num10) + (num9 * w)) + num13;
            quaternion.W = (w * num10) - num14;
            return quaternion;
        }

        public static bool operator ==(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.X == quaternion2.X) && (quaternion1.Y == quaternion2.Y)) && (quaternion1.Z == quaternion2.Z)) && (quaternion1.W == quaternion2.W));
        }
        public static bool operator !=(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.X != quaternion2.X) || (quaternion1.Y != quaternion2.Y)) || (quaternion1.Z != quaternion2.Z)) || !(quaternion1.W == quaternion2.W));
        }
        //public static Quaternion operator *(Quaternion quaternion1, Quaternion quaternion2)
        //{
        //    Quaternion quaternion;
        //    float x = quaternion1.X;
        //    float y = quaternion1.Y;
        //    float z = quaternion1.Z;
        //    float w = quaternion1.W;
        //    float num5 = quaternion2.X;
        //    float num6 = quaternion2.Y;
        //    float num7 = quaternion2.Z;
        //    float num8 = quaternion2.W;
        //    float num9 = (y * num7) - (z * num6);
        //    float num10 = (z * num5) - (x * num7);
        //    float num11 = (x * num6) - (y * num5);
        //    float num12 = ((x * num5) + (y * num6)) + (z * num7);
        //    quaternion.X = ((x * num8) + (num5 * w)) + num9;
        //    quaternion.Y = ((y * num8) + (num6 * w)) + num10;
        //    quaternion.Z = ((z * num8) + (num7 * w)) + num11;
        //    quaternion.W = (w * num8) - num12;
        //    return quaternion;
        //}
        public static Quaternion operator *(Quaternion left, Quaternion right)
        {
            Quaternion quaternion = new Quaternion();
            float lx = left.X;
            float ly = left.Y;
            float lz = left.Z;
            float lw = left.W;
            float rx = right.X;
            float ry = right.Y;
            float rz = right.Z;
            float rw = right.W;
            quaternion.X = (((rx * lw) + (rw * lx)) + (ry * lz)) - (rz * ly);
            quaternion.Y = (((ry * lw) + (rw * ly)) + (rz * lx)) - (rx * lz);
            quaternion.Z = (((rz * lw) + (rw * lz)) + (rx * ly)) - (ry * lx);
            quaternion.W = (rw * lw) - (((ry * ly) + (rx * lx)) + (rz * lz));
            return quaternion;
        }



        public static Quaternion operator *(Quaternion quaternion1, float scaleFactor)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X * scaleFactor;
            quaternion.Y = quaternion1.Y * scaleFactor;
            quaternion.Z = quaternion1.Z * scaleFactor;
            quaternion.W = quaternion1.W * scaleFactor;
            return quaternion;
        }
        public static Quaternion operator -(Quaternion quaternion1, Quaternion quaternion2)
        {
            Quaternion quaternion;
            quaternion.X = quaternion1.X - quaternion2.X;
            quaternion.Y = quaternion1.Y - quaternion2.Y;
            quaternion.Z = quaternion1.Z - quaternion2.Z;
            quaternion.W = quaternion1.W - quaternion2.W;
            return quaternion;
        }
        public static Quaternion operator -(Quaternion quaternion)
        {
            Quaternion quaternion2;
            quaternion2.X = -quaternion.X;
            quaternion2.Y = -quaternion.Y;
            quaternion2.Z = -quaternion.Z;
            quaternion2.W = -quaternion.W;
            return quaternion2;
        }

        #endregion     

        public void Conjugate()
        {
            this.X = -this.X;
            this.Y = -this.Y;
            this.Z = -this.Z;
        }

        public float Length()
        {
            float num = (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)) + (this.W * this.W);
            return (float)System.Math.Sqrt((double)num);
        }

        public void Invert()
        {
            double y = this.Y;
            double x = this.X;
            double z = this.Z;
            double w = this.W;
            float lengthSq = (float)(1.0 / ((((x * x) + (y * y)) + (z * z)) + (w * w)));
            this.X = -this.X * lengthSq;
            this.Y = -this.Y * lengthSq;
            this.Z = -this.Z * lengthSq;
            this.W *= lengthSq;
        }

        public void Normalize()
        {
            float num = (((this.X * this.X) + (this.Y * this.Y)) + (this.Z * this.Z)) + (this.W * this.W);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            this.X *= num2;
            this.Y *= num2;
            this.Z *= num2;
            this.W *= num2;
        }

        //public static Quaternion RotationAxis(Vector3 axis, float angle)
        //{
        //    Quaternion quaternion;
        //    float num = angle * 0.5f;
        //    float num2 = (float)System.Math.Sin((double)num);
        //    float num3 = (float)System.Math.Cos((double)num);
        //    quaternion.X = axis.X * num2;
        //    quaternion.Y = axis.Y * num2;
        //    quaternion.Z = axis.Z * num2;
        //    quaternion.W = num3;
        //    return quaternion;
        //}
        public static Quaternion RotationAxis(Vector3 axis, float angle)
        {
            Quaternion result;          
            float half = angle * 0.5f;
            float sin = (float)System.Math.Sin((double)half);
            float cos = (float)System.Math.Cos((double)half);
            result.X = axis.X * sin;
            result.Y = axis.Y * sin;
            result.Z = axis.Z * sin;
            result.W = cos;
            return result;
        }

        public static void RotationAxis(ref Vector3 axis, float angle, out Quaternion result)
        {
            float num = angle * 0.5f;
            float num2 = (float)System.Math.Sin((double)num);
            float num3 = (float)System.Math.Cos((double)num);
            result.X = axis.X * num2;
            result.Y = axis.Y * num2;
            result.Z = axis.Z * num2;
            result.W = num3;
        }

        public static Quaternion Conjugate(Quaternion quaternion)
        {
            Quaternion q;
            q.X = -quaternion.X;
            q.Y = -quaternion.Y;
            q.Z = -quaternion.Z;
            q.W = quaternion.W;
            return q;
        }

        public static float Dot(Quaternion quaternion1, Quaternion quaternion2)
        {
            return ((((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W));
        }

        public static void Dot(ref Quaternion quaternion1, ref Quaternion quaternion2, out float result)
        {
            result = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
        }

        public unsafe static float Dot(Quaternion* a, Quaternion* b)
        {
            return a->X * b->X + a->Y * b->Y + a->Z * b->Z + a->W * b->W;
        }

        public static float Lenght(Quaternion a)
        {
            return (float)System.Math.Sqrt(a.X * a.X + a.Y * a.Y + a.Z * a.Z + a.W * a.W);
        }

        public unsafe static float Lenght(Quaternion* a)
        {
            return (float)System.Math.Sqrt(a->X * a->X + a->Y * a->Y + a->Z * a->Z + a->W * a->W);
        }

        public static float LenghtSquared(Quaternion a)
        {
            return a.X * a.X + a.Y * a.Y + a.Z * a.Z + a.W * a.W;
        }

        public unsafe static float LenghtSquared(Quaternion* a)
        {
            return a->X * a->X + a->Y * a->Y + a->Z * a->Z + a->W * a->W;
        }

        public static unsafe Quaternion* Add(Quaternion* a, Quaternion* b, Quaternion* result)
        {
            result->X = a->X + b->X;
            result->Y = a->Y + b->Y;
            result->Z = a->Z + b->Z;
            result->W = a->W + b->W;
            return result;
        }

        public static unsafe Quaternion* Subtracts(Quaternion* a, Quaternion* b, Quaternion* result)
        {
            result->X = a->X - b->X;
            result->Y = a->Y - b->Y;
            result->Z = a->Z - b->Z;
            result->W = a->W - b->W;
            return result;
        }

        public static unsafe Quaternion* Div(Quaternion* a, Quaternion* b, Quaternion* result)
        {
            result->X = a->X / b->X;
            result->Y = a->Y / b->Y;
            result->Z = a->Z / b->Z;
            result->W = a->W / b->W;
            return result;
        }

        public static Quaternion Normalize(Quaternion quaternion)
        {
            Quaternion quaternion2;
            float num = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            quaternion2.X = quaternion.X * num2;
            quaternion2.Y = quaternion.Y * num2;
            quaternion2.Z = quaternion.Z * num2;
            quaternion2.W = quaternion.W * num2;
            return quaternion2;
        }

        public static void Normalize(ref Quaternion quaternion, out Quaternion result)
        {
            float num = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            float num2 = 1f / ((float)System.Math.Sqrt((double)num));
            result.X = quaternion.X * num2;
            result.Y = quaternion.Y * num2;
            result.Z = quaternion.Z * num2;
            result.W = quaternion.W * num2;
        }

        public static unsafe Quaternion* Normalize(Quaternion* v, Quaternion* result)
        {
            *result = *v;
            float length = (float)System.Math.Sqrt(v->X * v->X + v->Y * v->Y + v->Z * v->Z + v->W * v->W);
            if (length != 0f)
            {
                float num = (1.0f / length);
                result->X *= num;
                result->Y *= num;
                result->Z *= num;
                result->W *= num;
            }
            return result;
        }

        public static Quaternion Inverse(Quaternion quaternion)
        {
            Quaternion quaternion2;
            float num = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            float num2 = 1f / num;
            quaternion2.X = -quaternion.X * num2;
            quaternion2.Y = -quaternion.Y * num2;
            quaternion2.Z = -quaternion.Z * num2;
            quaternion2.W = quaternion.W * num2;
            return quaternion2;
        }

        public static void Inverse(ref Quaternion quaternion, out Quaternion result)
        {
            float num = (((quaternion.X * quaternion.X) + (quaternion.Y * quaternion.Y)) + (quaternion.Z * quaternion.Z)) + (quaternion.W * quaternion.W);
            float num2 = 1f / num;
            result.X = -quaternion.X * num2;
            result.Y = -quaternion.Y * num2;
            result.Z = -quaternion.Z * num2;
            result.W = quaternion.W * num2;
        }

        public static Quaternion Negate(Quaternion a)
        {
            Quaternion r;
            r.X = -a.X;
            r.Y = -a.Y;
            r.Z = -a.Z;
            r.W = -a.W;
            return r;
        }

        public static unsafe Quaternion* Negate(Quaternion* a, Quaternion* result)
        {          
            result->X = -a->X;
            result->Y = -a->Y;
            result->Z = -a->Z;
            result->W = -a->W;
            return result;
        }

        //public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        //{
        //    Quaternion result = new Quaternion();

        //    float halfRoll = roll * 0.5f;
        //    float sinRoll = (float)System.Math.Sin((double)halfRoll);
        //    float cosRoll = (float)System.Math.Cos((double)halfRoll);

        //    float halfPitch = pitch * 0.5f;
        //    float sinPitch = (float)System.Math.Sin((double)halfPitch);
        //    float cosPitch = (float)System.Math.Cos((double)halfPitch);

        //    float halfYaw = yaw * 0.5f;
        //    float sinYaw = (float)System.Math.Sin((double)halfYaw);
        //    float cosYaw = (float)System.Math.Cos((double)halfYaw);

        //    float num4 = cosYaw * sinPitch;
        //    float num3 = sinYaw * cosPitch;
        //    float num2 = cosYaw * cosPitch;
        //    float num = sinYaw * sinPitch;

        //    result.X = ((sinRoll * num3) + (cosRoll * num4));
        //    result.Y = ((cosRoll * num3) - (sinRoll * num4));            
        //    result.Z = ((sinRoll * num2) - (cosRoll * num));
        //    result.W = ((sinRoll * num) + (cosRoll * num2));
        //    return result;
        //}
        public static Quaternion RotationYawPitchRoll(float yaw, float pitch, float roll)
        {
            Quaternion quaternion;
            float num7 = roll * 0.5f;
            float num = (float)System.Math.Sin((double)num7);
            float num2 = (float)System.Math.Cos((double)num7);
            float num8 = pitch * 0.5f;
            float num3 = (float)System.Math.Sin((double)num8);
            float num4 = (float)System.Math.Cos((double)num8);
            float num9 = yaw * 0.5f;
            float num5 = (float)System.Math.Sin((double)num9);
            float num6 = (float)System.Math.Cos((double)num9);
            quaternion.X = ((num6 * num3) * num2) + ((num5 * num4) * num);
            quaternion.Y = ((num5 * num4) * num2) - ((num6 * num3) * num);
            quaternion.Z = ((num6 * num4) * num) - ((num5 * num3) * num2);
            quaternion.W = ((num6 * num4) * num2) + ((num5 * num3) * num);
            return quaternion;
        }

        public static Quaternion RotationMatrix(Matrix matrix)
        {
            float num2;
            float num3;
            float num = (matrix.M11 + matrix.M22) + matrix.M33;
            Quaternion quaternion = new Quaternion();
            if (num > 0f)
            {
                num2 = (float)System.Math.Sqrt((double)(num + 1f));
                quaternion.W = num2 * 0.5f;
                num2 = 0.5f / num2;
                quaternion.X = (matrix.M23 - matrix.M32) * num2;
                quaternion.Y = (matrix.M31 - matrix.M13) * num2;
                quaternion.Z = (matrix.M12 - matrix.M21) * num2;
                return quaternion;
            }
            if ((matrix.M11 >= matrix.M22) && (matrix.M11 >= matrix.M33))
            {
                num2 = (float)System.Math.Sqrt((double)(((1f + matrix.M11) - matrix.M22) - matrix.M33));
                num3 = 0.5f / num2;
                quaternion.X = 0.5f * num2;
                quaternion.Y = (matrix.M12 + matrix.M21) * num3;
                quaternion.Z = (matrix.M13 + matrix.M31) * num3;
                quaternion.W = (matrix.M23 - matrix.M32) * num3;
                return quaternion;
            }
            if (matrix.M22 > matrix.M33)
            {
                num2 = (float)System.Math.Sqrt((double)(((1f + matrix.M22) - matrix.M11) - matrix.M33));
                num3 = 0.5f / num2;
                quaternion.X = (matrix.M21 + matrix.M12) * num3;
                quaternion.Y = 0.5f * num2;
                quaternion.Z = (matrix.M32 + matrix.M23) * num3;
                quaternion.W = (matrix.M31 - matrix.M13) * num3;
                return quaternion;
            }
            num2 = (float)System.Math.Sqrt((double)(((1f + matrix.M33) - matrix.M11) - matrix.M22));
            num3 = 0.5f / num2;
            quaternion.X = (matrix.M31 + matrix.M13) * num3;
            quaternion.Y = (matrix.M32 + matrix.M23) * num3;
            quaternion.Z = 0.5f * num2;
            quaternion.W = (matrix.M12 - matrix.M21) * num3;
            return quaternion;
        } 

        public static Quaternion Slerp(Quaternion quaternion1, Quaternion quaternion2, float amount)
        {
            float num3;
            float num4;
            Quaternion quaternion;
            float num = amount;
            float num2 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            bool flag = false;
            if (num2 < 0f)
            {
                flag = true;
                num2 = -num2;
            }
            if (num2 > 0.999999f)
            {
                num3 = 1f - num;
                num4 = flag ? -num : num;
            }
            else
            {
                float num5 = (float)System.Math.Acos((double)num2);
                float num6 = (float)(1.0 / System.Math.Sin((double)num5));
                num3 = ((float)System.Math.Sin((double)((1f - num) * num5))) * num6;
                num4 = flag ? (((float)-System.Math.Sin((double)(num * num5))) * num6) : (((float)System.Math.Sin((double)(num * num5))) * num6);
            }
            quaternion.X = (num3 * quaternion1.X) + (num4 * quaternion2.X);
            quaternion.Y = (num3 * quaternion1.Y) + (num4 * quaternion2.Y);
            quaternion.Z = (num3 * quaternion1.Z) + (num4 * quaternion2.Z);
            quaternion.W = (num3 * quaternion1.W) + (num4 * quaternion2.W);
            return quaternion;
        }

        public static void Slerp(ref Quaternion quaternion1, ref Quaternion quaternion2, float amount, out Quaternion result)
        {
            float num3;
            float num4;
            float num = amount;
            float num2 = (((quaternion1.X * quaternion2.X) + (quaternion1.Y * quaternion2.Y)) + (quaternion1.Z * quaternion2.Z)) + (quaternion1.W * quaternion2.W);
            bool flag = false;
            if (num2 < 0f)
            {
                flag = true;
                num2 = -num2;
            }
            if (num2 > 0.999999f)
            {
                num3 = 1f - num;
                num4 = flag ? -num : num;
            }
            else
            {
                float num5 = (float)System.Math.Acos((double)num2);
                float num6 = (float)(1.0 / System.Math.Sin((double)num5));
                num3 = ((float)System.Math.Sin((double)((1f - num) * num5))) * num6;
                num4 = flag ? (((float)-System.Math.Sin((double)(num * num5))) * num6) : (((float)System.Math.Sin((double)(num * num5))) * num6);
            }
            result.X = (num3 * quaternion1.X) + (num4 * quaternion2.X);
            result.Y = (num3 * quaternion1.Y) + (num4 * quaternion2.Y);
            result.Z = (num3 * quaternion1.Z) + (num4 * quaternion2.Z);
            result.W = (num3 * quaternion1.W) + (num4 * quaternion2.W);
        }

        public static unsafe Quaternion Logarithm(Quaternion quaternion)
        {
            Quaternion q;
            float a = (float)System.Math.Acos(quaternion.W);
            float invSin = 1.0f / (float)System.Math.Sin(a);

            q.W = 0;
            q.X = a * quaternion.X / invSin;
            q.Y = a * quaternion.Y / invSin;
            q.Z = a * quaternion.Z / invSin;

            return q;
        }
 
        public static Quaternion Exponential(Quaternion quaternion)
        {
            Vector3 n = new Vector3(quaternion.X, quaternion.Y, quaternion.Z);
            float a = n.Length();
            float sinA = (float)System.Math.Sin(a);

            Quaternion q;
            q.W = (float)System.Math.Cos(a);
            q.X = n.X * sinA;
            q.Y = n.Y * sinA;
            q.Z = n.Z * sinA;

            return q;
        }

        public static Quaternion Pow(Quaternion a, float exponent)
        {
            Quaternion q = a;
            if (System.Math.Abs(a.W) < .9999f)
            {
                // Extract the half angle alpha (alpha = theta/2)
                float alpha = (float)System.Math.Acos(a.W);
                // Compute new alpha value
                float newAlpha = alpha * exponent;
                // Compute new w value
                q.W = (float)System.Math.Acos(newAlpha);
                // Compute new xyz values
                float mult = (float)System.Math.Sin(newAlpha) / (float)System.Math.Sin(alpha);

                q.X *= mult;
                q.Y *= mult;
                q.Z *= mult;
            }
            return q;
        }

        public static Quaternion Squad(Quaternion pQ1, Quaternion pA, Quaternion pB, Quaternion pC, float t)
        {
            return Slerp(Slerp(pQ1, pC, t), Slerp(pA, pB, t), 2 * t * (1 - t));
        }

        //public static Vector3 InverseRotate(Quaternion q, Vector3 v)
        //{
        //    //NxReal msq = NxReal(1.0)/magnitudeSquared();	//assume unit quat!
        //    Quaternion myInverse;
        //    myInverse.X = -q.X;//*msq;
        //    myInverse.Y = -q.Y;//*msq;
        //    myInverse.Z = -q.Z;//*msq;
        //    myInverse.W = q.W;//*msq;

        //    //v = (myInverse * v) ^ (*this);
        //    Quaternion left;
        //    left.multiply(myInverse, v);
        //    v.x = left.W * x + w * left.x + left.y * z - y * left.z;
        //    v.y = left.w * y + w * left.y + left.z * x - z * left.x;
        //    v.z = left.w * z + w * left.z + left.x * y - x * left.y;
        //}

        public bool Equals(Quaternion other)
        {
            return ((((this.X == other.X) && (this.Y == other.Y)) && (this.Z == other.Z)) && (this.W == other.W));
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Quaternion)
            {
                flag = this.Equals((Quaternion)obj);
            }
            return flag;
        }

        public override int GetHashCode()
        {
            return (((this.X.GetHashCode() + this.Y.GetHashCode()) + this.Z.GetHashCode()) + this.W.GetHashCode());
        }

        public override string ToString()
        {
            System.Globalization.CultureInfo currentCulture = System.Globalization.CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{X:{0} Y:{1} Z:{2} W:{3}}}", new object[] { this.X.ToString(currentCulture), this.Y.ToString(currentCulture), this.Z.ToString(currentCulture), this.W.ToString(currentCulture) });
        }

 

 


    }

}
