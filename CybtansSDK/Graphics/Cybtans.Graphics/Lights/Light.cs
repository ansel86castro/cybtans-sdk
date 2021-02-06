using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Lights
{
    public class Light
    {
        private Color3 diffuse = new Color3(1,1,1);
        private Color3 specular = new Color3(1,1,1);
        private Color3 ambient = new Color3(0.3f,0.3f,0.3f);
        private bool enable = true;
        private Vector3 attenuation = new Vector3(1, 0, 0);
        private float intensity = 1;
        private float spotPower = 16;
        private LightType type = LightType.Directional;
        private float range = 1000;

        public Guid Id { get; set; } = Guid.NewGuid();

        public ref Color3 Diffuse { get => ref diffuse; }

        public ref Color3 Specular { get => ref specular; }

        public ref Color3 Ambient { get => ref ambient; }

        public ref Vector3 Attenuation { get => ref attenuation; }

        public bool Enable { get => enable; set => enable = value; }

        public float Intensity { get => intensity; set => intensity = value; }

        public float SpotPower { get => spotPower; set => spotPower = value; }

        public LightType Type { get => type; set => type = value; }

        public float Range { get => range; set => range = value; }

        public LightDto ToDto()
        {
            return new LightDto
            {
                Id = Id,
                Ambient = Ambient.ToList(),
                Attenuation = Attenuation.ToList(),
                Diffuse = Diffuse.ToList(),
                Specular = Specular.ToList(),
                Enable = Enable,
                Intensity = Intensity,
                Range = range,
                SpotPower = spotPower,
                Type = (Models.LightType)Type
            };
        }
    }
}
