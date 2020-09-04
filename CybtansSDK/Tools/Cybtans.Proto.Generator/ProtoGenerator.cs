using Cybtans.Proto.AST;
using Cybtans.Proto.Generators.CSharp;
using Cybtans.Proto.Generators.Typescript;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cybtans.Proto.Generator
{
    public class ProtoGenerator : IGenerator
    {
        public bool CanGenerate(string value)
        {
            return value == "proto" || value == "p";
        }

        public bool Generate(string[] args)
        {
            if (args == null || args.Length == 0 || !CanGenerate(args[0]))
                return false;
            
            GenerateProto(args);
            return true;
        }

        public bool Generate(CybtansConfig config, GenerationStep step)
        {
            if (!CanGenerate(step.Type))
                return false;

            var options = new GenerationOptions()
            {
                ModelOptions = new TypeGeneratorOption()
                {
                    OutputPath = Path.Combine(config.Path, $"{step.Output}/{config.Service}.Models")
                },
                ServiceOptions = new TypeGeneratorOption()
                {
                    OutputPath = Path.Combine(config.Path, $"{step.Output}/{config.Service}.Services/Generated")
                },
                ControllerOptions = new WebApiControllerGeneratorOption()
                {
                    OutputPath = Path.Combine(config.Path, $"{step.Output}/{config.Service}.RestApi/Controllers/Generated"),
                    Namespace = "RestApi.Controllers"
                },
                ClientOptions = new TypeGeneratorOption()
                {
                    OutputPath =Path.Combine(config.Path, $"{step.Output}/{config.Service}.Clients")
                }
            };

            if (!string.IsNullOrEmpty(step.Gateway))
            {
                options.ApiGatewayOptions = new ApiGateWayGeneratorOption
                {
                    OutputPath =Path.Combine(config.Path, step.Gateway),
                    Namespace = $"Gateway.Controllers.{config.Service}"
                };
            }

            var protoFile = Path.Combine(config.Path, step.ProtoFile);
            if (step.SearchPath == null)
            {
                step.SearchPath = Path.GetDirectoryName(protoFile);
            }

            if (string.IsNullOrEmpty(step.SearchPath))
            {
                step.SearchPath = Environment.CurrentDirectory;
            }

            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { step.SearchPath });

            Console.WriteLine($"Compiling {protoFile}");

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile(protoFile);

            MicroserviceGenerator microserviceGenerator = new MicroserviceGenerator(options);            

            microserviceGenerator.GenerateCode(ast, scope);            
            try
            {
                Console.ForegroundColor = ConsoleColor.Green;

                Console.WriteLine("CSharp generated succesfully");

                if (step.Typecript != null)
                {

                    GenerateTypecriptCode(ast, Path.Combine(config.Path, step.Typecript.Output), step.Typecript.Framework);

                    Console.WriteLine($"Typescript generated succesfully");
                }
            }

            finally
            {
                Console.ResetColor();
            }

            return true;
        }

        public void PrintHelp()
        {
            Console.WriteLine("Proto Generator options:");
            Console.WriteLine("Example: cybtans-cli proto -n Service1 -o ./Services/Service1 -f ./Protos/Service1.proto");
            Console.WriteLine("p|proto : Generate code from a proto file");
            Console.WriteLine("-n : Service Name");
            Console.WriteLine("-o : Output Directory");
            Console.WriteLine("-f : Proto filename");
            Console.WriteLine("-search-path : Search path for imports");
            Console.WriteLine("-models-o : Models output directory");
            Console.WriteLine("-models-ns : Models namespace");
            Console.WriteLine("-services-o : Services output directory");
            Console.WriteLine("-services-ns : Services namespace");
            Console.WriteLine("-controllers-o : Controllers output directory");
            Console.WriteLine("-controllers-ns : Controllers namespace");
            Console.WriteLine("-cs-clients-o : CSharp clients output directory");
            Console.WriteLine("-cs-clients-ns : CSharp clients namespace");
            Console.WriteLine("-ts-o : Typescript code output directory");
            Console.WriteLine("-ts-fr : Typescript framework, for axample -ts-fr angular");
            Console.WriteLine("-o-gateway : Generate Api Gateway");
        }

        private static void GenerateProto(string[] args)
        {
            var options = new GenerationOptions()
            {
                ModelOptions = new TypeGeneratorOption(),
                ServiceOptions = new TypeGeneratorOption(),
                ControllerOptions = new WebApiControllerGeneratorOption(),
                ClientOptions = new TypeGeneratorOption()
            };

            string protoFile = null;
            string searchPath = null;
            string name = null;
            string output = null;
            string tsOutput = null;
            string tsFramework = null;

            for (int i = 1; i < args.Length; i++)
            {
                var arg = args[i];
                var value = arg;
                if (arg.StartsWith("-"))
                {
                    i++;
                    if (i >= args.Length)
                    {
                        Console.WriteLine("Invalid options");
                        return;
                    }

                    value = args[i];
                }

                switch (arg)
                {
                    case "-n":
                        name = value;
                        break;
                    case "-o":
                        output = value;
                        break;
                    case "-models-o":
                        options.ModelOptions.OutputPath = value;
                        break;
                    case "-models-ns":
                        options.ModelOptions.Namespace = value;
                        break;
                    case "-services-o":
                        options.ServiceOptions.OutputPath = value;
                        break;
                    case "-services-ns":
                        options.ServiceOptions.Namespace = value;
                        break;
                    case "-controllers-o":
                        options.ControllerOptions.OutputPath = value;
                        break;
                    case "-controllers-ns":
                        options.ControllerOptions.Namespace = value;
                        break;
                    case "-cs-clients-o":
                        options.ClientOptions.OutputPath = value;
                        break;
                    case "-cs-clients-ns":
                        options.ClientOptions.Namespace = value;
                        break;
                    case "-ts-o":
                        tsOutput = value;
                        break;
                    case "-ts-fr":
                        tsFramework = value;
                        break;
                    case "-f":
                        protoFile = value;
                        break;
                    case "-search-path":
                        searchPath = value;
                        break;
                    case "-o-gateway":
                        options.ApiGatewayOptions = new ApiGateWayGeneratorOption { OutputPath = value };
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        break;
                }
            }

            if (searchPath == null)
            {
                searchPath = Path.GetDirectoryName(protoFile);
            }

            if (string.IsNullOrEmpty(searchPath))
            {
                searchPath = Environment.CurrentDirectory;
            }

            if (protoFile == null)
            {
                Console.WriteLine("Missing proto file");
                return;
            }

            var fileResolverFactory = new SearchPathFileResolverFactory(new string[] { searchPath });

            Console.WriteLine($"Compiling {protoFile}");

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile(protoFile);

            if (name != null && output != null)
            {
                options.ModelOptions.OutputPath = $"{output}/{name}.Models";
                options.ServiceOptions.OutputPath = $"{output}/{name}.Services/Generated";
                options.ControllerOptions.OutputPath = $"{output}/{name}.RestApi/Controllers/Generated";
                options.ControllerOptions.Namespace = "RestApi.Controllers";
                options.ClientOptions.OutputPath = $"{output}/{name}.Clients";

                if(options.ApiGatewayOptions != null)
                    options.ApiGatewayOptions.Namespace = $"Gateway.Controllers.{name.Pascal()}";
            }

            if (options.ModelOptions.OutputPath != null)
            {
                MicroserviceGenerator microserviceGenerator = new MicroserviceGenerator(options);

                Console.WriteLine($"Generating csharp code from {protoFile}");

                microserviceGenerator.GenerateCode(ast, scope);

                Console.WriteLine("Csharp code generated succesfully");
            }

            if(tsOutput != null)
            {
                Console.WriteLine($"Generating typescript code from {protoFile}");

                GenerateTypecriptCode(ast, tsOutput, tsFramework);

                Console.WriteLine($"Typescript code generated succesfully in {tsOutput}");
            }
        }

        private static void GenerateTypecriptCode(ProtoFile ast, string output, string framework)
        {
            TypescriptGenerator tsGenerator = new TypescriptGenerator(new TypescriptOptions
            {
                ModelOptions = new TsOutputOption
                {
                    OutputPath = output,
                },
                ClientOptions = new TsOutputOption
                {
                    OutputPath = output,
                    Framework = framework
                }
            });

            tsGenerator.GenerateCode(ast);
        }
      
    }
}
