using Cybtans.Math;
using System.Collections.Generic;

namespace Cybtans.Graphics
{
    public class OrientedBox
    {
        Vector3 center;
        Vector3 extends;
        Matrix rotation = Matrix.Identity;

        Matrix localPose = Matrix.Identity;
        Matrix globalPose = Matrix.Identity;

        public Vector3 Extends
        {
            get { return extends; }
            set { extends = value; }
        }

        public Matrix LocalPose { get { return localPose; } set { localPose = value; } }

        public Matrix GlobalPose { get { return globalPose; } set { globalPose = value; } }

        public Matrix LocalRotation { get { return rotation; } set { rotation = value; } }

        public Vector3 LocalPosition { get { return center; } set { center = value; } }

        public Vector3 GlobalTraslation { get { return globalPose.GetAxis(3); } }

        public OrientedBox() { }

        public OrientedBox(Vector3 center, Vector3 extends, Matrix rotation)
        {
            this.center = center;
            this.extends = extends;
            this.rotation = rotation;
        }

        public OrientedBox(Vector3 dimention, Matrix rotation)
        {
            extends = dimention;
            this.rotation = localPose;
        }

        public unsafe OrientedBox(byte* positions, int vertexCount, int stride)
        {
            GetOrientedBox(positions, vertexCount, stride);
        }

        public unsafe void GetOrientedBox(byte* positions, int vertexCount, int stride)
        {
            var minValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);


            var r = eigenVectors[0];
            var s = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                t = new Vector3(Vector3.Dot(*pter, r), Vector3.Dot(*pter, s), Vector3.Dot(*pter, T));
                minValues = Vector3.Min(minValues, t);
                maxValues = Vector3.Max(maxValues, t);
            }

            t = 0.5f * (minValues + maxValues);
            Extends = 0.5f * (maxValues - minValues);
            center = t.X * r + t.Y * s + t.Z * T;
            rotation = new Matrix(r, s, T, Vector3.Zero);
            Update(Matrix.Identity);

        }

        public OrientedBox(Vector3[] positions)
        {
            unsafe
            {
                fixed (Vector3* pPosition = positions)
                {
                    GetOrientedBox((byte*)pPosition, positions.Length, sizeof(Vector3));
                }
            }
        }

        public void Update(Matrix pose)
        {
            localPose = rotation;
            localPose.Translation = center;
            globalPose = localPose * pose;
        }

        public static OrientedBox Create(Vector3[] positions)
        {
            unsafe
            {
                OrientedBox box;
                fixed (Vector3* pPosition = positions)
                {
                    box = Create((byte*)pPosition, positions.Length, sizeof(Vector3));
                }

                return box;
            }
        }

        public unsafe static OrientedBox Create(byte* positions, int vertexCount, int stride)
        {
            OrientedBox box = new OrientedBox();

            var minValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions, vertexCount, stride);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);

            var r = eigenVectors[0];
            var s = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            for (int i = 0; i < vertexCount; i++)
            {
                Vector3* pter = (Vector3*)(positions + i * stride);
                t = new Vector3(Vector3.Dot(*pter, r), Vector3.Dot(*pter, s), Vector3.Dot(*pter, T));
                minValues = Vector3.Min(minValues, t);
                maxValues = Vector3.Max(maxValues, t);
            }

            t = 0.5f * (minValues + maxValues);
            box.extends = 0.5f * (maxValues - minValues);
            box.center = t.X * r + t.Y * s + t.Z * T;
            box.rotation = new Matrix(r, s, T, new Vector3());
            box.Update(Matrix.Identity);

            return box;
        }

        public static OrientedBox Create(IEnumerable<Vector3> positions)
        {
            OrientedBox box = new OrientedBox();

            var minValues = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);
            var maxValues = new Vector3(float.MinValue, float.MinValue, float.MinValue);

            //Compute Covariance Matrix
            Matrix c = Matrix.CorrelationMatrix(positions);
            Vector3[] eigenVectors;
            Vector3 eigenValues;
            eigenVectors = Matrix.ComputeEigenVectors(c, 1.0e-10f, out eigenValues);

            var r = eigenVectors[0];
            var s = eigenVectors[1];
            var T = eigenVectors[2];

            Vector3 t;

            foreach (var v in positions)
            {
                t = new Vector3(Vector3.Dot(v, r), Vector3.Dot(v, s), Vector3.Dot(v, T));
                minValues = Vector3.Min(minValues, t);
                maxValues = Vector3.Max(maxValues, t);
            }

            t = 0.5f * (minValues + maxValues);
            box.extends = 0.5f * (maxValues - minValues);
            box.center = t.X * r + t.Y * s + t.Z * T;
            box.rotation = new Matrix(r, s, T, new Vector3());
            box.Update(Matrix.Identity);

            return box;
        }

        public void CommitChanges()
        {
            localPose = rotation;
            localPose.SetAxis(3, center);
        }

        public OrientedBox Clone()
        {
            return (OrientedBox)MemberwiseClone();
        }

        public AABB GetAxisAlignedBoundingBox()
        {
            unsafe
            {
                Vector3* corners = stackalloc Vector3[8];
                corners[0] = new Vector3(-1.0f, -1.0f, 0.0f); // xyz 
                corners[1] = new Vector3(1.0f, -1.0f, 0.0f);  // Xyz 
                corners[2] = new Vector3(-1.0f, 1.0f, 0.0f);  // xYz 
                corners[3] = new Vector3(1.0f, 1.0f, 0.0f);   // XYz 
                corners[4] = new Vector3(-1.0f, -1.0f, 1.0f); // xyZ
                corners[5] = new Vector3(1.0f, -1.0f, 1.0f);  // XyZ 
                corners[6] = new Vector3(-1.0f, 1.0f, 1.0f);  // xYZ 
                corners[7] = new Vector3(1.0f, 1.0f, 1.0f);   // XYZ 

                var matrix = Matrix.Scale(extends) * rotation * Matrix.Translate(center);
                AABB aab = new AABB();

                aab.Maximum = new Vector3(float.MinValue, float.MinValue, float.MinValue);
                aab.Minimum = new Vector3(float.MaxValue, float.MaxValue, float.MaxValue);

                for (int i = 0; i < 8; i++)
                {
                    var pos = Vector3.Transform(corners[i], matrix);
                    aab.Minimum = Vector3.Min(pos, aab.Minimum);
                    aab.Maximum = Vector3.Max(pos, aab.Maximum);
                }

                return aab;
            }
        }

        public Box ToBox()
        {
            return new Box(center, extends, rotation);
        }

    }
}