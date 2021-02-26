using Cybtans.Entities;
using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Graphics
{
    public class Texture : DomainTenantEntity<Guid>
    {
        protected Texture(TextureType type) 
        {
            Id = Guid.NewGuid();
            Type = type;
        }

        public Texture(string filename, TextureType type)
        {
            Filename = filename;
            Type = type;
            Format = Path.GetExtension(filename);
            Id = Guid.NewGuid();
        }

        public string Filename { get; set; }     

        public TextureType Type { get; set; }

        public string Format { get; set; }

        public virtual TextureDto ToDto()
        {
            return new TextureDto
            {
                Filename = Filename,
                Format = Format,
                Id = Id,
                Type = (Models.TextureType)Type
            };
        }
    }


    public class CubeTexture : Texture
    {
        public string PositiveX { get; set; }

        public string NegativeX { get; set; }

        public string PositiveY { get; set; }

        public string NegativeY { get; set; }

        public string PositiveZ { get; set; }

        public string NegativeZ { get; set; }

        public CubeTexture(CubeMapDto faces) : base(TextureType.CubeMap)
        {
            PositiveX = faces.PositiveX;
            NegativeX = faces.NegativeX;
            PositiveY = faces.PositiveY;
            NegativeY = faces.NegativeY;
            PositiveZ = faces.PositiveZ;
            NegativeZ = faces.NegativeZ;

            Format = Path.GetExtension(PositiveX);
        }

        public override TextureDto ToDto()
        {
            return new TextureDto
            {
                CubeMap = new CubeMapDto
                {
                    PositiveX = PositiveX,
                    NegativeX = NegativeX,
                    PositiveY = PositiveY,
                    NegativeY = NegativeY,
                    PositiveZ = PositiveZ,
                    NegativeZ = NegativeZ
                },
                Format = Format,
                Id = Id,
                Type = (Models.TextureType)Type
            };
        }
    }


    public enum TextureType
    {
        None,
        Texture2D,
        Texture3D,
        CubeMap
    }

}
