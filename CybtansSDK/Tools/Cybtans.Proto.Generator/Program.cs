using Cybtans.Proto;
using Cybtans.Proto.Generators;
using Cybtans.Proto.Generators.CSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

namespace Cybtans.Proto.Generator
{
    class Program
    {
        public class Options
        {
            public string Name { get; set; }

            public string Output { get; set; }

            public string Solution { get; set; }
        }

        static void Main(string[] args)
        {
            if (args[0] == "proto")
            {
                GenerateProto(args);
            }
            else
            {
                GenerateMicroservice(args);
            }
        }
      
        private static void GenerateMicroservice(string[] args)
        {
            Options options = new Options();
            for (int i = 0; i < args.Length; i++)
            {
                if (args[i] == "-n")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        Console.WriteLine("Missing Name");
                        return;
                    }
                    options.Name = args[i];
                }
                else if (args[i] == "-o")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        Console.WriteLine("Missing Output");
                        return;
                    }
                    options.Output = args[i];

                }
                else if (args[i] == "-sln")
                {
                    i++;
                    if (i >= args.Length)
                    {
                        Console.WriteLine("Missing Solution File");
                        return;
                    }
                    options.Solution = args[i];
                }
                else
                {
                    Console.WriteLine("Available options are:");
                    Console.WriteLine("-n : Service Name");
                    Console.WriteLine("-o : Output Directory");
                    Console.WriteLine("-sln : Solution File");
                    Console.WriteLine("Example: Generator -n Service1 -o Services/Service1 -sln Services.sln");
                    return;
                }
            }

            Directory.CreateDirectory(options.Output);
            //Generate Projects                      

            Process.Start("dotnet", $"new classlib -lang C# -f netcoreapp3.1 --no-restore --force -n { options.Name }.Services -o {options.Output}/{ options.Name }.Services").WaitForExit();
            Process.Start("dotnet", $"new xunit -lang C# -f netcoreapp3.1 --no-restore --force -n { options.Name }.Services.Tests -o {options.Output}/{ options.Name }.Services.Tests").WaitForExit();
            Process.Start("dotnet", $"new classlib -lang C# -f netstandard2.1 --no-restore --force -n { options.Name }.Models -o {options.Output}/{ options.Name }.Models").WaitForExit();
            Process.Start("dotnet", $"new classlib -lang C# -f netstandard2.1 --no-restore --force -n { options.Name }.Clients -o {options.Output}/{ options.Name }.Clients").WaitForExit();
            Process.Start("dotnet", $"new webapi -au None -lang C# -f netcoreapp3.1 --no-restore --force -n { options.Name }.RestApi -o {options.Output}/{ options.Name }.RestApi").WaitForExit();

            //Add Service references
            ReferenceProject(options, "NetCoreLib.tlp", $"Services/{ options.Name }.Services", new string[] { $"Models/{options.Name}.Models" });

            //Add Tests references
            ReferenceProject(options, "TestProject.tlp", $"Services.Tests/{ options.Name }.Services.Tests", new string[]
            {
                $"Services/{options.Name}.Services",
                $"Models/{options.Name}.Models"
            });

            // //Add Client references
            ReferenceProject(options, "NetStandardLib.tlp", $"Clients/{ options.Name }.Clients", new string[] { $"Models/{options.Name}.Models" });

            //Add WebAPi references
            ReferenceProject(options, "WebAPI.tlp", $"RestApi/{ options.Name }.RestApi", new string[]
           {
                 $"Services/{options.Name}.Services",
                 $"Models/{options.Name}.Models"
           });

            if (options.Solution != null)
            {
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Services/{ options.Name }.Services.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Services.Tests/{ options.Name }.Services.Tests.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Models/{ options.Name }.Models.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Clients/{ options.Name }.Clients.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.RestApi/{ options.Name }.RestApi.csproj").WaitForExit();
            }
        }

        private static string References(params string[] references)
        {
            return string.Join(Environment.NewLine, references.Select(x => $"\t<ProjectReference Include=\"{x}.csproj\" />"));
        }

        private static void ReferenceProject(Options options, string template, string project, string[] reference)
        {
            File.WriteAllText($"{options.Output}/{ options.Name }.{project}.csproj",
              TemplateProcessor.Process(File.ReadAllText(template), new
              {
                  FERERENCES = "<ItemGroup>\r\n"+ References(reference.Select(x=>$"../{options.Name}.{x}").ToArray()) + "\r\n</ItemGroup >"
              }));
        }

        private static void GenerateProto(string[] args)
        {
            var options = new GenerationOptions()
            {
                ModelOptions = new TypeGeneratorOption(),
                ServiceOptions = new TypeGeneratorOption(),
                ControllerOptions = new TypeGeneratorOption()
            };

            string protoFile = null;
            string searchPath = null;
            string name =null ;
            string output = null;

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
                        options.ModelOptions.OutputDirectory = value;
                        break;
                    case "-models-ns":
                        options.ModelOptions.Namespace = value;
                        break;
                    case "-services-o":
                        options.ServiceOptions.OutputDirectory = value;
                        break;
                    case "-services-ns":
                        options.ServiceOptions.Namespace = value;
                        break;
                    case "-controllers-o":
                        options.ControllerOptions.OutputDirectory = value;
                        break;
                    case "-controllers-ns":
                        options.ControllerOptions.Namespace = value;
                        break;
                    case "-f":
                        protoFile = value;
                        break;
                    case "-search-path":
                        searchPath = value;
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        Console.WriteLine("Valid options are:");
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

            Proto3Generator generator = new Proto3Generator(fileResolverFactory);
            var (ast, scope) = generator.LoadFromFile(protoFile);

            if(name != null && output != null)
            {
                options.ModelOptions.OutputDirectory = $"{output}/{name}.Models";
                options.ServiceOptions.OutputDirectory = $"{output}/{name}.Services/Generated";
                options.ControllerOptions.OutputDirectory = $"{output}/{name}.RestApi/Controllers/Generated";
                options.ControllerOptions.Namespace = "RestApi.Controllers";
            }

            MicroserviceGenerator microserviceGenerator = new MicroserviceGenerator(options);

            microserviceGenerator.GenerateCode(ast);
        }

    }
}
