using Cybtans.Proto.Generators.CSharp;
using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.InteropServices;
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

        public StepClientOptions Typecript { get; set; }

        public string Gateway { get; set; }

        public CSharpModelGenerationOption Models { get; set; }

        public CSharpServiceGenerationOption Services { get; set; }

        public CSharpStepOption CSharpClients { get; set; }

        public CSharpStepOption Controllers { get; set; }

        public CSharpStepOption GatewayOptions { get; set; }

        public List<StepClientOptions> Clients { get; set; } = new List<StepClientOptions>();

        #endregion

        #region Message Generator
        public string AssemblyFile { get; set; }

        public string[] Imports { get;  set; }

        public GrpcCompatibility? Grpc { get; set; } = new GrpcCompatibility();       
        
        #endregion
    }

    
    public class StepOption
    {
        public string Output { get; set; }       
        
        public bool ClearOutput { get; set; }
    }

    public class CSharpStepOption : StepOption
    {
        public string Namespace { get; set; }

        public bool Generate { get; set; }
    }

    public class CSharpModelGenerationOption : CSharpStepOption
    {
        public bool UseCytansSerialization { get; set; } = true;
    }

    public class CSharpServiceGenerationOption: CSharpStepOption
    {
        public string? NameTemplate { get; set; }

        public GrpcProxy Grpc { get; set; }
        
        public class GrpcProxy: CSharpStepOption
        {
            public bool AutoRegister { get; set; }
         
        }
    }

    public class StepClientOptions : StepOption
    {        
        public string Framework { get; set; }

        public Dictionary<string, string> Options { get; set; } = new Dictionary<string, string>();
    }

    public class GrpcCompatibility
    {
        public bool Enable { get; set; }

        public string MappingOutput { get; set; }

        public string MappingNamespace { get; set; }

        public string GrpcNamespace { get; set; }

    }


}
