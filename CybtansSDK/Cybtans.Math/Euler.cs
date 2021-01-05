


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    /// <summary>
    /// Euler angles holder
    /// </summary>
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]      
    public struct Euler : IEquatable<Euler>
    {

        public const float Pi = (float)System.Math.PI;
        public const float TwoPi = (float)System.Math.PI * 2;
        public const float PIover2 = (float)System.Math.PI / 2;
        public const float PIover3 = (float)System.Math.PI / 3;
        public const float PIover4 = (float)System.Math.PI / 4;
        public const float PIover6 = (float)System.Math.PI / 2;

        public static Euler Empty = new Euler(0, 0, 0);

        /// <summary>
        /// Rotatio around the Y-iniertial axis or Heading in radians.
        /// </summary>
        /// 
       
        public float Heading;

        /// <summary>
        /// Rotation around the X- Object axis in radians.
        /// </summary>
        /// 
       
        public float Pitch;

        /// <summary>
        /// Rotation around the Z -Object axis in radians.
        /// </summary>
        /// 
       
        public float Roll;

        const float Epsilon = 0.000005f;

        public Euler(float heading, float pitch, float roll)
        {
            this.Heading = heading;
            this.Pitch = pitch;
            this.Roll = roll;
        }

        static float Frag(float x)
        {
            return x - (float)System.Math.Truncate(x);
        }

        public static float ToRadians(float angle)
        {
            return TwoPi * (angle / 360.0f);
        }

        public static float ToAngle(float radian)
        {
            return 360.0f * (radian / TwoPi);
        }

        public Vector3 ToDirection()
        {
            Vector3 d;
            if (Heading == 0)
            {
                d.X = 0;
                d.Y = -(float)System.Math.Sin(Pitch);
                d.Z = (float)System.Math.Cos(Pitch);
            }
            else
            {
                d.X = (float)System.Math.Sin(Heading);
                d.Y = -(float)System.Math.Sin(Pitch);
                d.Z = (float)System.Math.Cos(Heading);
            }
            return d;
        }

        public Matrix ToMatrix()
        {
            if (Heading == 0 && Pitch == 0 && Roll == 0)
                return Matrix.Identity;
            return Matrix.RotationYawPitchRoll(Heading, Pitch, Roll);
        }

        public Quaternion ToQuaternion()
        {
            return Quaternion.RotationYawPitchRoll(Heading, Pitch, Roll);
        }

        public static float NormalizeHeading(float value)
        {
            //if (value > Numerics.TwoPI)
            //    value = value - Numerics.TwoPI;
            //else if (value < -Numerics.TwoPI)
            //    value = Numerics.TwoPI + value;

            //if (value > Numerics.PI)
            //    value -= Numerics.TwoPI;
            //else if (value < -Numerics.PI)
            //    value += Numerics.TwoPI;
            float absValue = System.Math.Abs(value);
            float cicles = absValue / Numerics.TwoPI;
            int floorCicles = (int)cicles;
            float frag = cicles - floorCicles;
            
            if (floorCicles > 0)
            {                                
                value = (float)System.Math.Sign(value) * Numerics.TwoPI * frag;
            }
            return value;
        }

        public static float NormalizePitch(float value)
        {
            if (value > Numerics.PIover2)
                value = Numerics.PIover2;
            else if (value < -Numerics.PIover2)
                value = -Numerics.PIover2;

            return value;
        }

        public static float NormalizeRoll(float value)
        {
            //if (value > Numerics.TwoPI)
            //    value = value - Numerics.TwoPI;
            //else if (value < -Numerics.TwoPI)
            //    value = Numerics.TwoPI + value;

            //if (value > Numerics.PI)
            //    value -= Numerics.TwoPI;
            //else if (value < -Numerics.PI)
            //    value += Numerics.TwoPI;

            //return value;
            float cicles = value / Numerics.TwoPI;
            int floorCicles = (int)cicles;
            float frag = cicles - floorCicles;

            if (floorCicles != 0)
            {
                value = Numerics.TwoPI * frag;
            }
            return value;
        }
        

        public static Euler Normalize(Euler e)
        {            
            e.Heading = NormalizeHeading(e.Heading);
            e.Pitch = NormalizePitch(e.Pitch);
            e.Roll = NormalizeRoll(e.Roll);
            return e;
        }

        public void GetFrameFromOrientation(out Vector3 right, out Vector3 up, out Vector3 look)
        {
            Matrix rot = ToMatrix();

            right = new Vector3(rot.M11, rot.M12, rot.M13);
            up = new Vector3(rot.M21, rot.M22, rot.M23);
            look = new Vector3(rot.M31, rot.M32, rot.M33);
        }

        public static void GetFrame(Euler angles, out Vector3 right, out Vector3 up, out Vector3 look)
        {
            Matrix rot = angles.ToMatrix();

            right = new Vector3(rot.M11, rot.M12, rot.M13);
            up = new Vector3(rot.M21, rot.M22, rot.M23);
            look = new Vector3(rot.M31, rot.M32, rot.M33);
        }

        public static Euler FromMatrix(Matrix matrix)
        {            
            Vector3 right = Vector3.Normalize(new Vector3(matrix.M11, matrix.M12, matrix.M13));
            Vector3 up = Vector3.Normalize(new Vector3(matrix.M21, matrix.M22, matrix.M23));
            Vector3 front = Vector3.Normalize(new Vector3(matrix.M31, matrix.M32, matrix.M33));

            return FromFrame(right, up, front);
        }       

        public static Euler FromFrame(Vector3 right, Vector3 up, Vector3 front)
        {
            Euler e = new Euler();

            //check the case when we are looking up or down
            float a = Vector3.Dot(front, Vector3.UnitY);
            if (a < -0.9999f)
            {
                //looking down
                e.Roll = 0;
                e.Pitch = PIover2;
                e.Heading = (float)System.Math.Atan2(up.X, up.Z);
            }
            else if (a > 0.9999f)
            {
                e.Roll = 0;
                e.Pitch = -PIover2;
                //e.Heading = (float)System.Math.Atan2(up.Z, up.X);
                e.Heading = (float)System.Math.Atan2(up.X, up.Z);
            }
            else
            {
                //Projection of Front on the XZ Plane
                Vector3 f = Vector3.Normalize(new Vector3(front.X, 0, front.Z));
                //Projection of Right on the XZ Plane
                Vector3 r = Vector3.Normalize(Vector3.Cross(Vector3.UnitY, f));

                if (f.X < Epsilon && f.X > -Epsilon)
                    e.Heading = 0;
                else
                    e.Heading = (float)System.Math.Atan2(f.X, f.Z);

                e.Pitch = -(float)System.Math.Asin(front.Y);
                //// projection of up on f
                //a = Vector3.Dot(up, f);
                //if (a < GMaths.Epsilon && a > -GMaths.Epsilon)
                //    e.Pitch = 0;
                //else
                //    e.Pitch = (float)System.Math.Atan2(a, up.Y);

                Vector3 u = Vector3.Cross(front, r);
                e.Roll = (float)System.Math.Atan2(Vector3.Dot(right, u), Vector3.Dot(right, r));
                //projection of up on r
                //a = Vector3.Dot(up, r);
                //e.Roll = -(float)System.Math.Atan2(a, up.Y);
            }

            return e;
        }

        public static Euler FromQuaternion(Quaternion q)
        {
            float w = q.W, x = q.X, y = q.Y, z = q.Z;
            float h, p, b;

            // Extract sin(pitch)
            float sp = -2.0f * (y * z + w * x);

            // Check for Gimbal lock, giving slight tolerance for numerical imprecision
            if (System.Math.Abs(sp) > 0.9999f)
            {
                // Looking straight up or down
                p = PIover2 * sp;
                // Compute heading, slam bank to zero
                h = (float)System.Math.Atan2(-x * z - w * y, 0.5f - y * y - z * z);
                b = 0.0f;
            }
            else
            {
                //Compute Angles

                p = (float)System.Math.Asin(sp);
                h = (float)System.Math.Atan2(x * z - w * y, 0.5f - x * x - y * y);
                b = (float)System.Math.Atan2(x * y - w * z, 0.5f - x * x - z * z);
            }

            return new Euler(h, p, b);
        }

        public static Euler FromAxis(Vector3 right, Vector3 up, Vector3 front)
        {
            Euler a = new Euler();

            a.Pitch = (float)System.Math.Asin(-up.Z);
            float cosp = (float)System.Math.Cos(a.Pitch);
            if (cosp != 0)
            {
                a.Heading = (float)System.Math.Atan2(right.Z, front.Z);
                a.Roll = (float)System.Math.Atan2(up.X, up.Y);
            }
            else
            {
                a.Heading = (float)System.Math.Atan2(-front.X, right.X);
                a.Roll = 0;
            }

            return a;
        }

        /// <summary>
        /// Return the eulers for the direcction
        /// the euler angles (0,0,0) is asumed as the direction (0,0,1)
        /// </summary>
        /// <param name="direction"></param>
        /// <returns></returns>
        public static Euler FromDirection(Vector3 direction)
        {
            Euler att = new Euler();
            att.Pitch = (float)System.Math.Asin(direction.Y);

            if (direction.X < Epsilon && direction.X > -Epsilon && direction.Z < -Epsilon)
                att.Heading = Pi;
            else if (direction.Z < Epsilon && direction.Z > -Epsilon)
            {
                if (direction.X > Epsilon)
                    att.Heading = PIover2;
                else if (att.Heading < -Epsilon)
                    att.Heading = -PIover2;
            }
            else
                att.Heading = (float)System.Math.Atan2(direction.X, direction.Z);

            return att;
        }
        /// <summary>
        /// Heading in grades
        /// </summary>
        /// 
        //[Editor(typeof(UIAngleTypeEditor),typeof(UITypeEditor))]
        
        public float HeadingAngle
        {
            get { return 360 * (Heading / TwoPi); }
            set
            {
                Heading = TwoPi * (value / 360);
            }
        }

        /// <summary>
        /// Pitch in grades
        /// </summary>
        
        public float PitchAngle
        {
            get
            {
                return 360 * (Pitch / TwoPi);
            }
            set
            {
                Pitch = TwoPi * (value / 360);
            }
        }

        
        public float RollAngle
        {
            get
            {
                return 360 * (Roll / TwoPi);
            }
            set
            {
                Roll = TwoPi * (value / 360);
            }
        }

        public static Euler operator +(Euler a, Euler b)
        {
            return new Euler(a.Heading + b.Heading, a.Pitch + b.Pitch, a.Roll + b.Roll);            
        }
        public static Euler operator -(Euler a, Euler b)
        {
            return new Euler(a.Heading - b.Heading, a.Pitch - b.Pitch, a.Roll - b.Roll);            
        }
        public static Euler operator *(Euler a, float t)
        {
            return new Euler(a.Heading * t, a.Pitch * t, a.Roll * t);
        }
        public static Euler operator *(float t, Euler a)
        {
            return new Euler(a.Heading * t, a.Pitch * t, a.Roll * t);
        }

        public static bool operator ==(Euler a, Euler b)
        {
            return a.Heading == b.Heading && a.Pitch == b.Pitch && a.Roll == b.Roll;
        }
        //public static Euler operator +=(Euler a)
        //{
        //      Euler r = new Euler();
        //    r.Heading += a.Heading + b.Heading;
        //    r.Pitch = a.Pitch + b.Pitch;
        //    r.Roll = a.Roll + b.Roll;
        //    r = Normalize(r);
        //    return r;
        //}

        public static bool operator !=(Euler a, Euler b)
        {
            return !(a.Heading == b.Heading && a.Pitch == b.Pitch && a.Roll == b.Roll);
        }

        public override string ToString()
        {
            return "H:" + Heading + " P:" + Pitch + " R:" + Roll;
        }

        #region IEquatable<Euler> Members

        public bool Equals(Euler other)
        {
            return this == other;
        }

        public override bool Equals(object obj)
        {
            if (obj is Euler)
                return (Euler)obj == this;
            return false;
        }

        public override int GetHashCode()
        {
            return base.GetHashCode();
        }

        #endregion
    }
}
