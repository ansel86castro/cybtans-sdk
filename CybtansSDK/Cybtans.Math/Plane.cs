using System;
using System.Collections.Generic;
using System.Globalization;
using System.Runtime.InteropServices;
using System.Text;

namespace Cybtans.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential, Pack = 4)]
    public struct Plane:IEquatable<Plane>
    {
        public Vector3 Normal;
        public float D;

        #region Constructs

        public Plane(Vector3 point1, Vector3 point2, Vector3 point3)
        {
            float num = point2.X - point1.X;
            float num2 = point2.Y - point1.Y;
            float num3 = point2.Z - point1.Z;
            float num4 = point3.X - point1.X;
            float num5 = point3.Y - point1.Y;
            float num6 = point3.Z - point1.Z;
            float num7 = (num2 * num6) - (num3 * num5);
            float num8 = (num3 * num4) - (num * num6);
            float num9 = (num * num5) - (num2 * num4);
            float num10 = ((num7 * num7) + (num8 * num8)) + (num9 * num9);
            float num11 = 1f / ((float)System.Math.Sqrt((double)num10));
            this.Normal.X = num7 * num11;
            this.Normal.Y = num8 * num11;
            this.Normal.Z = num9 * num11;
            this.D = -(((this.Normal.X * point1.X) + (this.Normal.Y * point1.Y)) + (this.Normal.Z * point1.Z));
        }

        public Plane(Vector3 point, Vector3 normal)
        {
            this.Normal = normal;
            this.D = -Vector3.Dot(normal, point);
        }

        public Plane(float a, float b, float c, float d)
        {           
            this.Normal = new Vector3(a, b, c);
            this.D = d;
        }

        #endregion

        #region Instance Methods

        public void Normalize()
        {
            double y = this.Normal.Y;
            double x = this.Normal.X;
            double z = this.Normal.Z;
            float magnitude = (float)(1.0 / ((double)((float)System.Math.Sqrt(((x * x) + (y * y)) + (z * z)))));
            this.Normal.X *= magnitude;
            this.Normal.Y *= magnitude;
            this.Normal.Z *= magnitude;
            this.D *= magnitude;

        }

        public bool Equals(Plane other)
        {
            return Normal == other.Normal && D == other.D;
        }
        public override int GetHashCode()
        {
            float d = this.D;
            return (this.Normal.GetHashCode() + d.GetHashCode());
        }

        public override bool Equals(object obj)
        {
            bool flag = false;
            if (obj is Plane)
            {
                flag = this.Equals((Plane)obj);
            }
            return flag;
        }

        public float Dot(Vector4 value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W));
        }
        public void Dot(ref Vector4 value, out float result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + (this.D * value.W);
        }
        public float DotCoordinate(Vector3 value)
        {
            return ((((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D);
        }
        public void DotCoordinate(ref Vector3 value, out float result)
        {
            result = (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z)) + this.D;
        }
        public float DotNormal(Vector3 value)
        {
            return (((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z));
        }
        public void DotNormal(ref Vector3 value, out float result)
        {
            result = ((this.Normal.X * value.X) + (this.Normal.Y * value.Y)) + (this.Normal.Z * value.Z);
        }
        public override string ToString()
        {
            CultureInfo currentCulture = CultureInfo.CurrentCulture;
            return string.Format(currentCulture, "{{Normal:{0} D:{1}}}", new object[] { this.Normal.ToString(), this.D.ToString(currentCulture) });
        }

        #endregion

        #region Operators

        public static bool operator ==(Plane left, Plane right)
        {
            return left.Normal == right.Normal && left.D == right.D;
        }

        public static bool operator !=(Plane left, Plane right)
        {
            return !(left.Normal == right.Normal && left.D == right.D);
        }
       
        public static explicit operator Vector4(Plane p)
        {
            unsafe
            {
                return *(Vector4*)&p;
            }
        }

       
        #endregion

        public static float Dot(Plane plane, Vector4 point)
        {
            return ((((plane.Normal.Y * point.Y) + (plane.Normal.X * point.X)) + (plane.Normal.Z * point.Z)) + (point.W * plane.D));
        }

        public static float DotCoordinate(Plane plane, Vector3 value)
        {
            return ((((plane.Normal.X * value.X) + (plane.Normal.Y * value.Y)) + (plane.Normal.Z * value.Z)) + plane.D);
        }

        public static float DotNormal(Plane plane, Vector3 point)
        {
            return (((plane.Normal.Y * point.Y) + (plane.Normal.X * point.X)) + (plane.Normal.Z * point.Z));
        }

        public static bool Equals(Plane p0, Plane p1, float epsilon = Numerics.Epsilon)
        {
            return p0 == p1 || ((p0.Normal - p1.Normal).IsZero(epsilon) && (p0.D - p1.D).IsZero(epsilon));
        }     

        public static Plane Transform(Plane plane, Matrix matrix)
        {
            Matrix matrix2;
            Plane plane2;
            Matrix.Invert(ref matrix, out matrix2);
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            plane2.Normal.X = (((x * matrix2.M11) + (y * matrix2.M12)) + (z * matrix2.M13)) + (d * matrix2.M14);
            plane2.Normal.Y = (((x * matrix2.M21) + (y * matrix2.M22)) + (z * matrix2.M23)) + (d * matrix2.M24);
            plane2.Normal.Z = (((x * matrix2.M31) + (y * matrix2.M32)) + (z * matrix2.M33)) + (d * matrix2.M34);
            plane2.D = (((x * matrix2.M41) + (y * matrix2.M42)) + (z * matrix2.M43)) + (d * matrix2.M44);
            return plane2;
        }       
      
        public static Plane Transform(Plane plane, Quaternion rotation)
        {
            Plane plane2;
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
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            plane2.Normal.X = ((x * num13) + (y * num14)) + (z * num15);
            plane2.Normal.Y = ((x * num16) + (y * num17)) + (z * num18);
            plane2.Normal.Z = ((x * num19) + (y * num20)) + (z * num21);
            plane2.D = plane.D;
            return plane2;
        }

        public static void Transform(ref Plane plane, ref Matrix matrix, out Plane result)
        {
            Matrix matrix2;
            Matrix.Invert(ref matrix, out matrix2);
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            result.Normal.X = (((x * matrix2.M11) + (y * matrix2.M12)) + (z * matrix2.M13)) + (d * matrix2.M14);
            result.Normal.Y = (((x * matrix2.M21) + (y * matrix2.M22)) + (z * matrix2.M23)) + (d * matrix2.M24);
            result.Normal.Z = (((x * matrix2.M31) + (y * matrix2.M32)) + (z * matrix2.M33)) + (d * matrix2.M34);
            result.D = (((x * matrix2.M41) + (y * matrix2.M42)) + (z * matrix2.M43)) + (d * matrix2.M44);
        }

        public static void Transform(ref Plane plane, ref Quaternion rotation, out Plane result)
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
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            result.Normal.X = ((x * num13) + (y * num14)) + (z * num15);
            result.Normal.Y = ((x * num16) + (y * num17)) + (z * num18);
            result.Normal.Z = ((x * num19) + (y * num20)) + (z * num21);
            result.D = plane.D;
        }

        public static Plane TransformInverse(Plane plane, Matrix matrix)
        {
            Plane plane2;
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            plane2.Normal.X = (((x * matrix.M11) + (y * matrix.M12)) + (z * matrix.M13)) + (d * matrix.M14);
            plane2.Normal.Y = (((x * matrix.M21) + (y * matrix.M22)) + (z * matrix.M23)) + (d * matrix.M24);
            plane2.Normal.Z = (((x * matrix.M31) + (y * matrix.M32)) + (z * matrix.M33)) + (d * matrix.M34);
            plane2.D = (((x * matrix.M41) + (y * matrix.M42)) + (z * matrix.M43)) + (d * matrix.M44);
            return plane2;
        }

        public static void TransformInverse(ref Plane plane, ref Matrix matrix, out Plane result)
        {
            float x = plane.Normal.X;
            float y = plane.Normal.Y;
            float z = plane.Normal.Z;
            float d = plane.D;
            result.Normal.X = (((x * matrix.M11) + (y * matrix.M12)) + (z * matrix.M13)) + (d * matrix.M14);
            result.Normal.Y = (((x * matrix.M21) + (y * matrix.M22)) + (z * matrix.M23)) + (d * matrix.M24);
            result.Normal.Z = (((x * matrix.M31) + (y * matrix.M32)) + (z * matrix.M33)) + (d * matrix.M34);
            result.D = (((x * matrix.M41) + (y * matrix.M42)) + (z * matrix.M43)) + (d * matrix.M44);
        }
       

    }
   
}
