using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    public enum FrustumTest { Inside, Partial, Outside }

    public enum InsideTestResult
    {
        /// <summary>
        /// The testing object is inside the region
        /// </summary>
        Inside,
        /// <summary>
        /// The testing object is outside the region
        /// </summary>
        Outside,
        /// <summary>
        /// The testing object is partially inside the region
        /// </summary>
        PartialInside,
        /// <summary>
        /// The testing object contains the region or the region is inside the testing object
        /// </summary>
        Contained,

        /// <summary>
        /// The testing object partially contains the region or the region is partially inside the testing object
        /// </summary>
        PartialContained
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Sphere :IEquatable<Sphere>
    {        
        public Vector3 Center;
        public float Radius;

        public Sphere(Vector3 center, float radius)
        {
            Center = center;
            Radius = radius;
        }

        public Sphere(Vector3[] positions)
        {
            Center = new Vector3();
            Radius = 0;
            unsafe
            {
                fixed (Vector3* pPosition = positions)
                {
                    CreateBoundingSphere((byte*)pPosition, positions.Length, sizeof(Vector3));
                }
            }
        }     

        public unsafe Sphere(byte* positions, int vertexCount, int stride)
        {
            Center = new Vector3();
            Radius = 0;
            CreateBoundingSphere(positions, vertexCount, stride);
        }

        unsafe private void CreateBoundingSphere(byte* positions, int vertexCount, int stride)
        {
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);
            Vector3 r = eigenVectors[0];
            float min = float.MaxValue, max = float.MinValue;
            Vector3 minPoint = Vector3.Zero;
            Vector3 maxPoint = Vector3.Zero;
            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                float t = Vector3.Dot(*pter, r);
                if (t < min)
                {
                    min = t;
                    minPoint = *pter;
                }
                if (t > max)
                {
                    max = t;
                    maxPoint = *pter;
                }
            }

            Center = 0.5f * (maxPoint + minPoint);
            Radius = Vector3.Distance(maxPoint, Center);
            float r2 = Radius * Radius;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3 pos = *(Vector3*)(positions + i * stride);
                Vector3 d = pos - Center;
                if (d.LengthSquared() > r2)
                {
                    d.Normalize();
                    Vector3 g = Center - Radius * d;
                    Center = 0.5f * (g + pos);
                    Radius = Vector3.Distance(pos, Center);
                    r2 = Radius * Radius;
                }
            }
        }      

        public unsafe Sphere GetTranformed(Matrix matrix)
        {
            Sphere sphere = new Sphere();
            Vector3 center = Center;
            Vector3.TransformCoordinates(ref center, ref matrix, out sphere.Center);
            if (Radius > 0)
            {
                sphere.Radius = System.Math.Max(new Vector3(Radius * matrix.M11, Radius * matrix.M12, Radius * matrix.M13).Length(),
                                         System.Math.Max(new Vector3(Radius * matrix.M21, Radius * matrix.M22, Radius * matrix.M23).Length(),
                                                  new Vector3(Radius * matrix.M31, Radius * matrix.M32, Radius * matrix.M33).Length()));
            }
            return sphere;
        }

        public unsafe void GetTranformed(Matrix matrix, out Vector3 center, out float radius)
        {
            radius = 0;
            Vector3 _center = Center;
            Vector3.Transform(ref _center, ref matrix, out center);
            if (Radius > 0)
            {
                radius = System.Math.Max(new Vector3(Radius * matrix.M11, Radius * matrix.M12, Radius * matrix.M13).Length(),
                                         System.Math.Max(new Vector3(Radius * matrix.M21, Radius * matrix.M22, Radius * matrix.M23).Length(),
                                                  new Vector3(Radius * matrix.M31, Radius * matrix.M32, Radius * matrix.M33).Length()));
            }
        }

        //public SphereBuilder GetGeometry(int stack = 16, int slices = 16)
        //{
        //    SphereBuilder sphere = new SphereBuilder(stack, slices, Radius);
        //    for (int i = 0; i < sphere.Vertices.Length; i++)
        //    {
        //        var mat = Matrix.Translate(Center);
        //        sphere.Vertices[i].Position = Vector3.Transform(sphere.Vertices[i].Position, mat);
        //    }

        //    return sphere;
        //}

        public static FrustumTest GetCullTest(Sphere sphere, Plane[] planes)
        {
            if (sphere.Radius == 0) return FrustumTest.Inside;

            var globalPosition = sphere.Center;
            var radius = sphere.Radius;
            float distance;
            int count = 0;
            for (int i = 0; i < planes.Length; i++)
            {
                distance = Plane.DotCoordinate(planes[i], globalPosition);
                if (distance <= -radius)
                    return FrustumTest.Outside;
                if (distance >= radius)
                    count++;
            }
            return count == planes.Length ? FrustumTest.Inside : FrustumTest.Partial;
        }

        public static bool IsInsideFrustum(Sphere sphere, Plane[] planes)
        {
            var cullState = GetCullTest(sphere, planes);
            return cullState == FrustumTest.Inside || cullState == FrustumTest.Partial;
        }

        public static bool IntersectRect(Sphere sphere, RectangleF rect)
        {
            var globalPosition = sphere.Center;
            var radius = sphere.Radius;
            // Check to see if the bounding circle around the model 
            // intersects this rectangle. 
            float centerX = rect.X + rect.Width * 0.5f;
            float centerZ = rect.Y - rect.Height * 0.5f;

            float deltaX = centerX - globalPosition.X;
            float deltaZ = centerZ - globalPosition.Z;

            float distanceSquared = deltaX * deltaX + deltaZ * deltaZ;

            float combinedRadius = (radius * radius) + (rect.Width * rect.Width);

            return distanceSquared < combinedRadius;
        }

        public bool Intersect(RectangleF rect)
        {
           var radius = Radius;

            // Check to see if the bounding circle around the model 
            // intersects this rectangle. 
            float recCenterX = rect.X + rect.Width * 0.5f;
            float recCenterZ = rect.Y - rect.Height * 0.5f;

            float deltaX = recCenterX - Center.X;
            float deltaZ = recCenterZ - Center.Z;

            float distanceSquared = deltaX * deltaX + deltaZ * deltaZ;

            float combinedRadius = (radius * radius) + (rect.Width * rect.Width);

            return distanceSquared < combinedRadius;
        }        

        public bool Intersect(Plane[] planes)
        {
            var cullState = GetCullTest(new Sphere(Center, Radius), planes);
            return cullState == FrustumTest.Inside || cullState == FrustumTest.Partial;
        }

        public bool Equals(Sphere other)
        {
            return Center == other.Center && Radius == other.Radius;
        }

        public override bool Equals(object obj)
        {
            if (obj is Sphere)
                return Equals((Sphere)obj);
            return false;
        }
       
        public override int GetHashCode()
        {
            return Center.GetHashCode() + Radius.GetHashCode();
        }

        public override string ToString()
        {
            return "Center:" + Center + " Radius:" + Radius;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Box:IEquatable<Box>
    {
        public Vector3 Translation;
        public Vector3 Extends;
        public Matrix Rotation;

        public Box(Vector3 translation, Vector3 extends, Matrix rotation)
        {
            this.Translation = translation;
            this.Extends = extends;
            this.Rotation = rotation;
        }

        //public OrientedBox ToVolume()
        //{
        //    return new OrientedBox(Translation, Extends, Rotation);
        //}

        //public BoxBuilder GetGeometry()
        //{
        //    BoxBuilder box = new BoxBuilder(2, 2, 2);

        //    for (int i = 0; i < box.Vertices.Length; i++)
        //    {
        //        var mat = Matrix.Scale(Extends) * Rotation;
        //        box.Vertices[i].Position= Vector3.Transform(box.Vertices[i].Position, mat);
        //    }
        //    return box;
        //}

        public bool Equals(Box other)
        {
            return Translation == other.Translation && Extends == other.Extends && Rotation == other.Rotation;
        }

        public override string ToString()
        {
            return "T:" + Translation + ", E:" + Extends;
        }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Segment
    {
       public Vector3 P0;		//!< Start of segment
       public Vector3 P1;		//!< End of segment

       public override string ToString()
       {
           return "P0:" + P0 + " P1:" + P1;
       }
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct Capsule
    {
        public Segment Segment;
        public float Radius;
    }

    [Serializable]
    [StructLayout(LayoutKind.Sequential)]
    public struct AABB
    {
        public Vector3 Minimum;
        public Vector3 Maximum;

        public AABB(Vector3 minumun, Vector3 maximun)
        {
            this.Minimum = minumun;
            this.Maximum = maximun;
        }
        
        //public CullTestResult CullTest(Matrix viewProj)
        //{
        //     //transform the 8 points of the box to clip space and clip these point
        //     //if the test fail then transform the 6 faces of the box to clip space and clip these faces
        //    unsafe
        //    {
        //        Vector4* corners = stackalloc Vector4[8];
        //        corners[0]
        //    }
        //}

        public override string ToString()
        {
            return "Min:" + Minimum + " Max:" + Maximum;
        }
    }
}
