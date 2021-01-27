using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Shading
{
    public class ShaderProgramRepository
    {
        public List<ShaderProgram> Programs { get; } = new List<ShaderProgram>();

        public ShaderProgram Current { get; set; }

        public string Default { get; set; } = "basic";

        public Task LoadFromDirectory(string directoy) => LoadFromDirectory(new DirectoryInfo(directoy));

        public async Task LoadFromDirectory(DirectoryInfo di)
        {
            foreach (var file in di.EnumerateFiles("*.json"))
            {
                if(file.Extension == ".json")
                {
                    var shader = await ShaderProgram.LoadFromJsonAsync(file.FullName);
                    Programs.Add(shader);
                }                
            }

            foreach (var dir in di.EnumerateDirectories())
            {
                await LoadFromDirectory(dir);
            }
        }

        public ShaderProgramCollection ToDto()
        {
            return new ShaderProgramCollection
            {
                DefaultProgram = Default,
                Programs = Programs.Select(x => x.ToDto()).ToList()
            };
        }
    }
}
