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
        public Texture(string filename, TextureType type)
        {
            Filename = filename;
            Type = type;
            Format = Path.GetExtension(filename);
            Id = Guid.NewGuid();
        }

        public string Filename { get; set; }     

        public TextureType Type { get; set; }

        public string Format { get; }

        public TextureDto ToDto()
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

    public enum TextureType
    {
        None,
        Texture2D,
        Texture3D,
        CubeMap
    }

}
