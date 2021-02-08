using Cybtans.Graphics.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Shading
{
    public class EffectsManager
    {
        public Dictionary<string, ShaderProgram> Programs { get; } = new Dictionary<string, ShaderProgram>();

        public Dictionary<string, Effect> Effects { get; } = new Dictionary<string, Effect>();                

        public Task LoadShadersFromDirectory(string directoy) => LoadShadersFromDirectory(new DirectoryInfo(directoy));

        private async Task LoadShadersFromDirectory(DirectoryInfo di)
        {
            foreach (var file in di.EnumerateFiles("*.p.json"))
            {
                if(file.Extension == ".json")
                {
                    var shader = await ShaderProgram.LoadFromJsonAsync(file.FullName);
                    Programs.Add(shader.Name, shader);
                }                
            }

            foreach (var dir in di.EnumerateDirectories())
            {
                await LoadShadersFromDirectory(dir);
            }
        }

        public Task LoadEffectsFromDirectory(string directory) => LoadEffectsFromDirectory(new DirectoryInfo(directory));

        private async Task LoadEffectsFromDirectory(DirectoryInfo di)
        {
            if(Programs.Count == 0)
            {
                await LoadShadersFromDirectory(di);
            }

            foreach (var file in di.EnumerateFiles("*.e.json"))
            {
                if (file.Extension == ".json")
                {
                    var effect = await Effect.LoadFromJsonAsync(file.FullName, this);
                    Effects.Add(effect.Name, effect);
                }
            }

            foreach (var dir in di.EnumerateDirectories())
            {
                await LoadEffectsFromDirectory(dir);
            }
        }



        public EffectsManagerDto ToDto()
        {
            return new EffectsManagerDto
            {               
                Programs = Programs.ToDictionary(x => x.Key, y=> y.Value.ToDto()),
                Effects = Effects.ToDictionary(x=>x.Key, y=> y.Value.ToDto())
            };
        }
    }
}
