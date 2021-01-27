using Cybtans.Graphics.Meshes;
using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Graphics.Components
{
    public class SkinMeshComponent : IFrameComponent
    {
        public SkinMeshComponent()
        {

        }

        public SkinMeshComponent(MeshSkin skin, IEnumerable<Material> materials)
        {
            Skin = skin;
            Materials = new List<Material>(materials);
        }

        public string Name => Mesh?.Name;

        public MeshSkin Skin { get; set; }

        public Mesh Mesh => Skin.Mesh;

        public Frame Frame { get; set; }

        public Boundings Bounding { get; }

        public List<Material> Materials { get; set; }

        public void Dispose()
        {
            Skin?.Dispose();
        }

        public void OnPoseUpdated()
        {
            
        }

        public FrameComponentDto ToDto()
        {
            return new FrameComponentDto
            {
                MeshSkin = new FrameMeshSkinDto
                {
                    MeshSkin = Skin.Id,
                    Materials = Materials.Select(x => x.Id).ToList()
                }
            };
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }
    }
}
