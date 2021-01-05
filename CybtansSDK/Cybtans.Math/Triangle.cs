
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Triangle
    {
        public Vector3 P0, P1, P2;
        public Vector3 Normal;

        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2, Vector3 normal)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;
            this.Normal = normal;
        }

        public Triangle(Vector3 p0, Vector3 p1, Vector3 p2)
        {
            this.P0 = p0;
            this.P1 = p1;
            this.P2 = p2;

            Normal = Triangle.ComputeFaceNormal(p1, p1 ,p2);
        }

        public static Vector3 ComputeFaceNormal(Vector3 v0, Vector3 v1, Vector3 v2)
        {
            Vector3 _v0 = v1 - v0;
            Vector3 _v1 = v2 - v0;
            return Vector3.Normalize(Vector3.Cross(_v0, _v1));
        }

        public static Vector3 ComputeFaceTangent(Vector3 p0, Vector2 tex0, Vector3 p1, Vector2 tex1, Vector3 p2, Vector2 tex2)
        {
            Vector3 e0 = p1 - p0;
            Vector3 e1 = p2 - p0;

            // (u0 , v0)=(u1 - u0 ,v1 - v0)
            float u0 = tex1.X - tex0.X;
            float v0 = tex1.Y - tex0.Y;

            //(u1 , v1)= (u2 - u0, v2 - v0 )
            float u1 = tex2.X - tex0.X;
            float v1 = tex2.Y - tex0.Y;

            float a = 1.0f / (u0 * v1 - v0 * u1);

            Vector3 tanget;
            tanget.X = (v1 * e0.X - v0 * e1.X) * a;
            tanget.Y = (v1 * e0.Y - v0 * e1.Y) * a;
            tanget.Z = (v1 * e0.Z - v0 * e1.Z) * a;

            tanget.Normalize();

            return tanget;

        }             
    }   
}
