using Cybtans.Graphics.Buffers;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

#nullable enable

namespace Cybtans.Graphics
{
    public class Boundings
    {
        private Sphere sphere;
        private AABB axisAlignedBox;
        private OrientedBox orientedBox;

        public Boundings()
        {
            orientedBox = new OrientedBox();
        }

        public Sphere Sphere { get => sphere; set => sphere = value; }

        public OrientedBox OrientedBox { get => orientedBox; set => orientedBox = value; }

        public AABB AxisAlignedBox { get => axisAlignedBox; set => axisAlignedBox = value; }

        public unsafe void ComputeBoundingVolumenes(VertexBuffer vertexBuffer)
        {

            using var positions = vertexBuffer.GetVertexBufferView<Vector3>(VertexSemantic.Position, 0);
            OrientedBox = OrientedBox.Create(positions);
            Sphere = new Sphere(positions.BasePter, positions.Count, positions.Stride);
            AxisAlignedBox = new AABB(new Vector3(float.MaxValue, float.MaxValue, float.MaxValue),
                            new Vector3(float.MinValue, float.MinValue, float.MinValue));

            for (int i = 0; i < positions.Count; i++)
            {
                axisAlignedBox.Maximum = Vector3.Max(positions[i], axisAlignedBox.Maximum);
                axisAlignedBox.Minimum = Vector3.Min(positions[i], axisAlignedBox.Minimum);
            }
        }
    }
}
