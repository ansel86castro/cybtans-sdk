using Cybtans.Graphics.Meshes;
using Cybtans.Graphics.Models;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cybtans.Graphics.Shading
{
    public class Effect
    {
        public string Name { get; set; }

        public Dictionary<string, List<PredicateProgram>> Predicates { get; set; } = new Dictionary<string, List<PredicateProgram>>();

        public Dictionary<string, ShaderProgram> Programs { get; set; } = new Dictionary<string, ShaderProgram>();

        public static Dictionary<string, Type> GetTypes()
        {
            return new Dictionary<string, Type>
            {
                [nameof(Mesh)] = typeof(Mesh),
                [nameof(MeshSkin)] = typeof(MeshSkin)
            };
        }

        public static Effect LoadFromJson(string jsonFileName, EffectsManager manager)
        {
          
            var fi = new FileInfo(jsonFileName);
            if (!fi.Exists)
                throw new FileNotFoundException(jsonFileName);

            var json = File.ReadAllText(jsonFileName);
            var dto = JsonConvert.DeserializeObject<EffectDto>(json);

            var effect = new Effect
            {
                Name = dto.Name,
                Predicates = dto.Predicates.ToDictionary(x => x.Key, y => y.Value.Items.Select(x=>PredicateProgram.FromDto(x, manager)).ToList()),
                Programs = dto.Programs.ToDictionary(x=>x.Key, y=>  manager.Programs[y.Value])
            };

            return effect;
        }

        public static Task<Effect> LoadFromJsonAsync(string jsonFileName, EffectsManager manager) => Task.Run(() => LoadFromJson(jsonFileName, manager));

        public EffectDto ToDto()
        {
            return new EffectDto
            {
                Name = Name,
                Programs = Programs.ToDictionary(x => x.Key, y => y.Value.Name),
                Predicates = Predicates.ToDictionary(x => x.Key, y => new PredicateProgramList { Items = y.Value.Select(y => y.ToDto()).ToList() })
            };
        }
    }

    public class PredicateProgram
    {  
        public List<ParameterPredicate> AndConditions { get; set; }

        public List<ParameterPredicate> OrConditions { get; set; }

        public ParameterPredicate Condition { get; set; }

        public ShaderProgram Program { get; set; }

        public static PredicateProgram FromDto(PredicateProgramDto dto, EffectsManager manager)
        {
            var p = manager.Programs[dto.Program];            

            return new PredicateProgram
            {
                AndConditions = dto.AndConditions != null ? dto.AndConditions.Select(ParameterPredicate.FromDto).ToList() : null,
                OrConditions = dto.OrConditions != null ? dto.OrConditions.Select(ParameterPredicate.FromDto).ToList() : null,
                Condition = ParameterPredicate.FromDto(dto.Condition),
                Program = p               
            };
        }

        public PredicateProgramDto ToDto()
        {
            return new PredicateProgramDto
            {
                AndConditions = AndConditions != null ? AndConditions.Select(ParameterPredicate.ToDto).ToList() : null,
                OrConditions = OrConditions != null ? OrConditions.Select(ParameterPredicate.ToDto).ToList() : null,
                Condition = ParameterPredicate.ToDto(Condition),
                Program = Program.Name
            };
        }
    }



   
}
