
using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cybtans.Graphics
{

    public class Material
    {
        public const string DIFFUSE_MAP = "DIFFUSE_MAP";
        public const string SPECULAR_MAP = "SPECULAR_MAP";
        public const string NORMAL_MAP = "NORMAL_MAP";
        public const string HEIGHT_MAP = "HEIGHT_MAP";
        public const string GLOSS_MAP = "GLOSS_MAP";
        public const string ALPHA_MAP = "ALPHA_MAP";
        public const string EMISSIVE_MAP = "ALPHA_MAP";

        Dictionary<string, Texture> _textures = new Dictionary<string, Texture>();
        string _name;
        Color4 _diffuse = new Color4(1,1,1, 1);
        Color4 _specular = new Color4(1,1,1,1);
        Color4 _emissive = new Color4(0,0,0,0);

        public Material(string name):this()
        {
            _name = name;         
        }
         
        public Material()
        {
            _diffuse = new Color4(1, 1, 1, 1);
            _specular = new Color4(1, 1, 1, 1);
            _emissive = new Color4();            
            Reflectivity = 0;
            Refractitity = 0;
            SpecularPower = 4;          
        }

        public Guid Id { get; set; } = Guid.NewGuid();

        public string Name { get => _name; set => _name = value; }

        public ref Color4 Diffuse => ref _diffuse;

        public ref Color4 Specular => ref _specular;

        public ref Color4 Emissive => ref _emissive;

        public float Alpha { get => _diffuse.A; set => _diffuse.A = value; }                

        public float Reflectivity { get; set; }

        public float Refractitity { get; set; }

        public float SpecularPower { get; set; }

        public Texture DiffuseMap { get => GetTexture(DIFFUSE_MAP); set => SetTexture(DIFFUSE_MAP, value); }

        public Texture SpecularMap { get => GetTexture(SPECULAR_MAP); set => SetTexture(SPECULAR_MAP, value); }

        public Texture NormalMap { get => GetTexture(NORMAL_MAP); set => SetTexture(NORMAL_MAP, value); }

        public Texture GlossMap { get => GetTexture(GLOSS_MAP); set => SetTexture(GLOSS_MAP, value); }

        public Texture EmissiveMap { get => GetTexture(EMISSIVE_MAP); set => SetTexture(EMISSIVE_MAP, value); }

        public Texture AlphaMap { get => GetTexture(ALPHA_MAP); set => SetTexture(ALPHA_MAP, value); }

        public Texture HeightMap { get => GetTexture(HEIGHT_MAP); set => SetTexture(HEIGHT_MAP, value); }

        public Texture GetTexture(string name)
        {
            _textures.TryGetValue(name, out var t);
            return t;
        }

        public void SetTexture(string name, Texture texture)
        {
            _textures[name] = texture;
        }

        public MaterialDto ToDto()
        {
            return new MaterialDto
            {
                Id = Id,
                Name = Name,
                Diffuse = _diffuse.ToList(),
                Emissive = ((Color3)_emissive).ToList(),
                Specular =  ((Color3)_specular).ToList(),
                SpecularPower = Specular.A,
                Textures = _textures.ToDictionary(x => x.Key, x => x.Value.Id)
            };
        }

    }


}
