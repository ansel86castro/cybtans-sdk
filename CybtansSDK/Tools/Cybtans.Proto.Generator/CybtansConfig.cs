using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Proto.Generator
{
    public class CybtansConfig
    {
        public string Path { get; private set; }

        public string Service { get; set; }       

        public List<GenerationStep> Steps { get; set; }

        public static IEnumerable<CybtansConfig> SearchConfigs(string path)
        {
            return SearchConfigs(new DirectoryInfo(path));
        }

        public static IEnumerable<CybtansConfig> SearchConfigs(DirectoryInfo di)
        {           
            foreach (var file in di.EnumerateFiles())
            {
                if(file.Name == "cybtans.json")
                {
                    var json = File.ReadAllText(file.FullName);
                    var config = System.Text.Json.JsonSerializer.Deserialize<CybtansConfig>(json);
                    config.Path = di.FullName;

                    yield return config;
                }
            }

            foreach (var dir in di.EnumerateDirectories())
            {
                foreach (var item in SearchConfigs(dir))
                {
                    yield return item;
                }
            }
        }
    }

    public class GenerationStep
    {
        public string Type { get; set; }

        public string Output { get; set; }

        #region Proto Generator
        public string ProtoFile { get; set; }

        public string SearchPath { get; set; }

        public TypescriptGenerationOptions Typecript { get; set; }

        public string Gateway { get; set; }

        #endregion

        #region Message Generator
        public string AssemblyFile { get; set; }
        public string[] Imports { get;  set; }
        #endregion
    }

    public class TypescriptGenerationOptions
    {
        public string Output { get; set; }

        public string Framework { get; set; }
    }
    
}
