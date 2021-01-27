using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics
{
    public class FrameBounding
    {
        public Sphere LocalSphere { get; set; }
        public Sphere GlobalSphere { get; set; }
        public OrientedBox OrientedBox { get; set; }
        
        public bool IsInside(Frustum frustum)
        {
            return GlobalSphere.Radius == 0 || frustum.Contains(GlobalSphere);
        }

        public void Update(Matrix _worldTransform)
        {
            if (OrientedBox != null)
                OrientedBox.Update(_worldTransform);

            GlobalSphere = LocalSphere.GetTranformed(_worldTransform);
        }
    }
}
