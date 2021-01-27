using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Lights
{
    public class AmbientLight
    {
        public AmbientLight()
        {
            AmbientColor = new Color3(0.1f, 0.1f, 0.1f);
            SkyColor = new Color3(1, 1, 1);
            GroundColor = new Color3(0, 0, 0);
            NorthPole = Vector3.UnitY;
            Intensity = 1;
        }

        public Color3 AmbientColor { get; set; }

        public Color3 SkyColor { get; set; }

        public Color3 GroundColor { get; set; }

        public Vector3 NorthPole { get; set; }

        public float Intensity { get; set; }
    }
}
