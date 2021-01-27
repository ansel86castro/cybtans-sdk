using Cybtans.Graphics.Common;
using Cybtans.Graphics.Components;
using Cybtans.Graphics.Lights;
using Cybtans.Graphics.Meshes;
using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Linq;

#nullable enable

namespace Cybtans.Graphics
{

    public class Scene : INameable
    {
        public Scene(string name = "default")
        {
            Root = new Frame("_root_");
            Name = name;
            AmbientLight = new AmbientLight();
            Id = Guid.NewGuid();
        }

        public Guid Id { get; set; }

        public string Name { get; set; }

        /// <summary>
        /// How many real-world meters in one distance unit as a floating-point number. 
        /// For example, 1.0 for the Type "Meter"; 1000 for the Type "Kilometer"; 0.3048 for the name "Foot
        /// </summary>
        public float Units { get; set; } = 1;

        public UOMType UnitOfMeasure { get; set; } = UOMType.Meters;

        public AmbientLight AmbientLight { get; set; }

        public Camera? CurrentCamera { get; set; }

        public List<Mesh> Meshes { get; set; } = new List<Mesh>();

        public List<MeshSkin> Skins { get; set; } = new List<MeshSkin>();

        public List<Material> Materials { get; set; } = new List<Material>();

        public List<Texture> Textures { get; set; } = new List<Texture>();

        public List<Light> Lights { get; set; } = new List<Light>();

        public List<Camera> Cameras { get; set; } = new List<Camera>();

        public Frame Root { get; set; }

        public Frame? FindNode(string name)
        {
            return Root?.FindNode(name);
        }

        public IEnumerable<Frame> Nodes => Root.Childrens;

        public override string ToString() => Name ?? base.ToString();

        public SceneDto ToDto()
        {
            return new SceneDto
            {
                Id = Id,
                Name = Name,
                UnitOfMeasure = (Models.Uomtype)UnitOfMeasure,
                Units = Units,
                Materials = Materials.Select(x => x.ToDto()).ToList(),
                Cameras = Cameras.Select(x => x.ToDto()).ToList(),
                CurrentCamera = CurrentCamera?.Id,
                Lights = Lights.Select(x => x.ToDto()).ToList(),
                Textures = Textures.Select(x => x.ToDto()).ToList(),
                Meshes = Meshes.Select(x => x.ToDto()).ToList(),
                Skins = Skins.Select(x => x.ToDto()).ToList(),
                Root = Root.ToDto()
            };
        }
    }
}
