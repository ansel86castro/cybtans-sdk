using Cybtans.Graphics.Meshes;
using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Graphics.Components
{
    public class MeshComponent: IFrameComponent
    {
        public MeshComponent()
        {

        }

        public MeshComponent(Mesh mesh, IEnumerable<Material> materials)
        {
            Mesh = mesh;
            Materials = new List<Material>(materials);
        }

        public Mesh Mesh { get; set; }
        
        public Frame Frame { get; set; }

        public Boundings Bounding => Mesh?.Bounding;

        public List<Material> Materials { get; set; }

        public string Name => Mesh?.Name;

        public void Dispose()
        {
            Mesh?.Dispose();
        }

        public override string ToString()
        {
            return Name ?? base.ToString();
        }

        public void OnPoseUpdated()
        {
           
        }

        public FrameComponentDto ToDto()
        {
            return new FrameComponentDto
            {
                Mesh = new FrameMeshDto
                {
                    Mesh = Mesh.Id,
                    Materials = Materials.Select(x => x.Id).ToList()
                }
            };
        }
    }
}
