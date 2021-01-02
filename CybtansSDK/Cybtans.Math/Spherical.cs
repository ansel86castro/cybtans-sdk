


using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    [Serializable]
    [System.Runtime.InteropServices.StructLayout(System.Runtime.InteropServices.LayoutKind.Sequential)]
   
    public struct Spherical
    {

        /// <summary>
        /// Angle respect to the X axis (Pitch)
        /// </summary>
        /// 
       
        public float Theta;

        /// <summary>
        /// Angle respect to the Y axis (Heading)
        /// </summary>
        /// 
       
        public float Phi;

        /// <summary>
        /// 
        /// </summary>
        /// <param name="theta">pitch</param>
        /// <param name="phi">heading</param>
        public Spherical(float theta, float phi)
        {
            this.Theta = theta;
            this.Phi = phi;
        }

        
        [Description("Angle respect to the X axis (Pitch)")]
        public float ThetaAngle
        {
            get
            {
                return Euler.ToAngle(Theta);
            }
            set
            {
                Theta = Euler.ToRadians(value);
            }
        }

        
        [Description("Angle respect to the Y axis (Heading)")]
        public float PhiAngle
        {
            get
            {
                return Euler.ToAngle(Phi);
            }
            set
            {
                Phi = Euler.ToRadians(value);
            }
        }

        public Vector3 ToCartesian()
        {
            float b = (float)System.Math.Sin(Theta);
            return new Vector3(b * (float)System.Math.Sin(Phi), (float)System.Math.Cos(Theta), b * (float)System.Math.Cos(Phi));
        }

        public static Spherical FromDirection(Vector3 direction)
        {
            Spherical s = new Spherical();
            s.Theta = (float)System.Math.Acos(direction.Y);
            if (direction.Z != 0)
                s.Phi = (float)System.Math.Atan(direction.X / direction.Z);
            //float a = (float)System.Math.Sin(s.Theta);
            //if (a > 0)
            //    s.Phi = (float)System.Math.Asin(direction.X / a);            
            return s;
        }

        public static Vector3 ToCartesian(float theta, float phi)
        {
            float b = (float)System.Math.Sin(theta);
            return new Vector3(b * (float)System.Math.Sin(phi), (float)System.Math.Cos(theta), b * (float)System.Math.Cos(phi));
        }

        public static Spherical FromGrades(float theta, float pi)
        {
            return new Spherical(Euler.ToRadians(theta), Euler.ToRadians(pi));
        }

        public override string ToString()
        {
            return ThetaAngle + " ," + PhiAngle;
        }
    }
}
