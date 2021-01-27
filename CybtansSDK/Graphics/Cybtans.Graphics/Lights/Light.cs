using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Graphics.Lights
{
    public class Light
    {
        private Color3 diffuse;
        private Color3 specular;
        private Color3 ambient;
        private bool enable;
        private Vector3 attenuation;
        private float intensity;
        private float spotPower;
        private LightType type;
        private float range;

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
