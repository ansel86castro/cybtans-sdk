using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Shading
{
    public class Shader
    {
        public Dictionary<string, string> Inputs { get; set; }

        public Dictionary<string, ShaderParameter> Parameters { get; set; }

        public string Location { get; set; }

        public Task<string> LoadSourceStringAsync(string basePath)
        {
            return File.ReadAllTextAsync(Path.Combine(basePath, Location));
        }

        public string LoadSourceString(string basePath)
        {
            return File.ReadAllText(Path.Combine(basePath, Location));
        }

        public ShaderDto ToDto(ShaderProgram shaderProgram)
        {
            return new ShaderDto
            {
                Inputs = Inputs,             
                Parameters = Parameters.ToDictionary(x => x.Key, v => new ShaderParameterDto
                {
                    Property = v.Value.Property,
                    Target = v.Value.Target,
                    Type = v.Value.Type,
                    Path = v.Value.Path
                }),
                Source = LoadSourceString(shaderProgram.BasePath)
            };
        }
    }

    public class ShaderParameter
    {      

        public string Target { get; set; }        

        public string Property { get; set; }

        public string Type { get; set; }

        public string Path { get; set; }
    }

    

    public enum ShaderType
    {
        Float,
        Float2,
        Floa3,

        Int,
        Int2,
        Int3,

        Matrix
    }
}
