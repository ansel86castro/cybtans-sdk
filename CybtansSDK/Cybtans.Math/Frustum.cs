using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Math
{
    public enum FrustumPlane { Front, Back, Left, Right, Top, Bottom }

    public class Frustum
    {
        const int TestPlanes = 4;

        /// <summary>
        /// Box's corners in Homogenius Projection Space that is between [(-1, -1 0) , (1, 1, 1)]
        /// </summary>
        public static Vector3[] HsBox = new Vector3[8];

        internal Vector3[] CornersWorld = new Vector3[8];
        internal Plane[] planes = new Plane[6];        

        public Frustum()
        {
            planes = new Plane[6];                   
            CornersWorld = new Vector3[8];          
        }

        static Frustum()
        {           
            HsBox[0] = new Vector3(-1.0f, -1.0f, 0.0f); // xyz 
            HsBox[1] = new Vector3(1.0f, -1.0f, 0.0f);  // Xyz 
            HsBox[2] = new Vector3(-1.0f, 1.0f, 0.0f);  // xYz 
            HsBox[3] = new Vector3(1.0f, 1.0f, 0.0f);   // XYz 
            HsBox[4] = new Vector3(-1.0f, -1.0f, 1.0f); // xyZ
            HsBox[5] = new Vector3(1.0f, -1.0f, 1.0f);  // XyZ 
            HsBox[6] = new Vector3(-1.0f, 1.0f, 1.0f);  // xYZ 
            HsBox[7] = new Vector3(1.0f, 1.0f, 1.0f);   // XYZ 
        }

        public Plane this[FrustumPlane plane]
        {
            get { return planes[IndexOfPlane(plane)]; }
        }

        public Plane[] Planes { get { return planes; } }

        public Vector3[] Corners { get { return CornersWorld; } }

        public int IndexOfPlane(FrustumPlane plane)
        {
            switch (plane)
            {
                case FrustumPlane.Front: return 3;
                case FrustumPlane.Back: return 2;
                case FrustumPlane.Left: return 1;
                case FrustumPlane.Right: return 0;
                case FrustumPlane.Top: return 4;
                case FrustumPlane.Bottom: return 5;
            }

            return -1;
        }

        public void Transform(Matrix invViewProj)
        {
            for (int i = 0; i < 8; i++)
                Vector3.TransformCoordinates(ref HsBox[i], ref invViewProj, out CornersWorld[i]);

            planes[0] = new Plane(CornersWorld[7], CornersWorld[3], CornersWorld[5]); // Right
            planes[1] = new Plane(CornersWorld[2], CornersWorld[6], CornersWorld[4]); // Left
            planes[2] = new Plane(CornersWorld[6], CornersWorld[7], CornersWorld[5]); // Far
            planes[3] = new Plane(CornersWorld[0], CornersWorld[1], CornersWorld[2]); // Near
            planes[4] = new Plane(CornersWorld[2], CornersWorld[3], CornersWorld[6]); // Top
            planes[5] = new Plane(CornersWorld[1], CornersWorld[0], CornersWorld[4]); // Bottom            
        }

        public static void CreatePlanes(Plane[] planes, Vector3[] corners)
        {
            planes[0] = new Plane(corners[7], corners[3], corners[5]); // Right
            planes[1] = new Plane(corners[2], corners[6], corners[4]); // Left
            planes[2] = new Plane(corners[6], corners[7], corners[5]); // Far
            planes[3] = new Plane(corners[0], corners[1], corners[2]); // Near
            planes[4] = new Plane(corners[2], corners[3], corners[6]); // Top
            planes[5] = new Plane(corners[1], corners[0], corners[4]); // Bottom
        }

        public FrustumTest TestFrustum(Sphere sphere)
        {
            return TestFrustum(sphere.Center, sphere.Radius);
        }

        public FrustumTest TestFrustum(Vector3 center, float radius)
        {            
            float distance;
            int count = 0;
            // Don’t check against top and bottom. 
            for (int i = 0; i < TestPlanes; i++)
            {
                //distancia del punto al plano , positiva si el punto esta en la direccion de la normal y negativa en otro caso
                distance = planes[i].DotCoordinate(center);

                //if (distance > radio) and (distance < 0) then the sphere is on the negative side of the plane
                if (distance <= -radius)
                    return FrustumTest.Outside;

                //if the sphere is on the posisive side of the plane 
                if (distance >= radius)
                    count++;
            }

            return count == TestPlanes ? FrustumTest.Inside : FrustumTest.Partial;
        }

        public static FrustumTest TestFrustum(Plane[] planes, Vector3 center, float radius)
        {          
            float distance;
            int count = 0;
            // Don’t check against top and bottom. 
            for (int i = 0; i < planes.Length; i++)
            {
                //distancia del punto al plano , positiva si el punto esta en la direccion de la normal y negativa en otro caso
                distance = planes[i].DotCoordinate(center);

                //if distance > radio and distance < 0 then the sphere is on the negative side of the plane
                if (distance <= -radius)
                    return FrustumTest.Outside;

                //if the sphere is on the posisive side of the plane 
                if (distance >= radius)
                    count++;
            }

            return count == planes.Length ? FrustumTest.Inside : FrustumTest.Partial;
        }

        public static FrustumTest TestFrustum(Plane[] planes, Sphere sphere)
        {
            return TestFrustum(planes, sphere.Center, sphere.Radius);
        }

        public bool Contains(Sphere sphere)
        {
            var cont = TestFrustum(sphere);
            return cont == FrustumTest.Inside || cont == FrustumTest.Partial;
        }

        public bool Contains(Vector3 center, float radius)
        {
            var cont = TestFrustum(center, radius);
            return cont == FrustumTest.Inside || cont == FrustumTest.Partial;
        }
    }
}
