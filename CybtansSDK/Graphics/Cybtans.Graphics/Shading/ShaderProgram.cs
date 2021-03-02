using Cybtans.Graphics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Net.NetworkInformation;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Shading
{
    public class ShaderProgram
    {
        public string Name { get; set; }      
        public string BasePath { get; private set; }

        public string DefinitionFile { get; private set; }

        public Shader VertexShader { get; set; }

        public Shader FragmentShader { get; set; }

        public static ShaderProgram LoadFromJson(string jsonFileName)
        {
            var fi = new FileInfo(jsonFileName);
            if (!fi.Exists)
                throw new FileNotFoundException(jsonFileName);

            var json = File.ReadAllText(jsonFileName);
            ShaderProgram program = JsonConvert.DeserializeObject<ShaderProgram>(json);
            program.BasePath = fi.Directory.FullName;
            program.DefinitionFile = jsonFileName;
            return program;
        }

        public static Task<ShaderProgram> LoadFromJsonAsync(string jsonFileName) => Task.Run(() => LoadFromJson(jsonFileName));

        public ShaderProgramDto ToDto()
        {
            return new ShaderProgramDto
            {
                Name = Name,
                VertexShader = VertexShader?.ToDto(this),
                FragmentShader = FragmentShader?.ToDto(this)                
            };
        }
    }
}
