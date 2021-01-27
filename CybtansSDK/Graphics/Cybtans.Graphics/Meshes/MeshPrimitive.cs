using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Meshes
{

    [Flags]
    public enum MeshPrimitive
    {
        PointList = 1,
        LineList = 2,
        LineStrip = 3,
        TriangleList = 4,
        TriangleStrip = 5,
        TriangleFan = 6
    }
}
