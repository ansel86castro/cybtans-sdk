using Cybtans.Proto.Generators;
using Cybtans.Proto.Generators.CSharp;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

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
            if(args == null || args.Length == 0)
            {
                PrintHelp();
                return;
            }

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
                    Console.WriteLine("Microsevice Generator options are:");
                    Console.WriteLine("-n : Service Name");
                    Console.WriteLine("-o : Output Directory");
                    Console.WriteLine("-sln : Solution File");
                    Console.WriteLine("Example: Generator -n Service1 -o Services/Service1 -sln Services.sln");
                    return;
                }
            }
            

            Directory.CreateDirectory(options.Output);
            //Generate Projects              
            Console.WriteLine("Generating projects...");

            GenerateProject("ModelsProject.tpl", options.Output, $"{ options.Name }.Models", null);
            GenerateProject("ClientsProject.tlp", options.Output, $"{ options.Name }.Clients", new [] { $"{ options.Name }.Models" });
            GenerateProject("ServicesProject.tpl", options.Output, $"{ options.Name }.Services", new [] { $"{ options.Name }.Models" });
            GenerateProject("TestProject.tpl", options.Output, $"{ options.Name }.Services.Tests", new[] { $"{ options.Name }.Models", $"{ options.Name }.Services" });
            GenerateWebApi(options);                       

            //Generate Proto 
            Directory.CreateDirectory($"{options.Output}/Proto");
            File.WriteAllText($"{options.Output}/Proto/{options.Name}.proto", GetTemplate("Proto.tpl", new
            {
                SERVICE = options.Name
            }));

            File.WriteAllText($"{options.Output}/generate.bat", $"ServiceGenerator proto -n {options.Name} -o . -f ./Proto/{options.Name}.proto");
          
            //GenerateProto(new string[] { "proto", "-n", options.Name, "-o", options.Output, "-f", $"{options.Output}/Proto/{options.Name}.proto" });

            if (options.Solution != null)
            {
                Console.WriteLine("Adding projects to solution file");

                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Services/{ options.Name }.Services.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Services.Tests/{ options.Name }.Services.Tests.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Models/{ options.Name }.Models.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Clients/{ options.Name }.Clients.csproj").WaitForExit();
                Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.RestApi/{ options.Name }.RestApi.csproj").WaitForExit();
            }           

            Console.WriteLine("Generation Completed");           
        }        

        private static string References(params string[] references)
        {
            return string.Join(Environment.NewLine, references.Select(x => $"\t<ProjectReference Include=\"{x}\" />"));
        }      

        private static void GenerateProject(string template, string output ,string project, string[] references)
        {
            Directory.CreateDirectory($"{output}/{project}");
            
            var content = GetTemplate(template);
            if (references != null)
            {
                content = TemplateProcessor.Process(content, new
                {
                    FERERENCES = "<ItemGroup>\r\n" + References(references.Select(x => $"../{x}/{x}.csproj").ToArray()) + "\r\n</ItemGroup >"
                });
            }

            File.WriteAllText($"{output}/{project}/{project}.csproj", content);
        }

        private static string GetTemplate(string template, object args = null)
        {
            using var stream = typeof(Program).Assembly.GetManifestResourceStream($"Cybtans.Proto.Generator.Templates.{template}");
            var reader = new StreamReader(stream);
            var content = reader.ReadToEnd();
            if (args == null)
                return content;

            return TemplateProcessor.Process(content, args);
        }

        private static void GenerateWebApi(Options options)
        {
            GenerateProject("WebAPI.tpl", options.Output, $"{ options.Name }.RestApi", new[] { $"{ options.Name }.Models", $"{ options.Name }.Services" });

            Directory.CreateDirectory($"{options.Output}/{options.Name}.RestApi/Properties");
            Directory.CreateDirectory($"{options.Output}/{options.Name}.RestApi/Controllers");

            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/appsettings.Development.json", GetTemplate("WebAPI.appsettings.Development.tpl"));
            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/appsettings.json", GetTemplate("WebAPI.appsettings.tpl"));
            
            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/Properties/launchSettings.json", GetTemplate("WebAPI.launchSettings.tpl", new
            {
                PROJECT = $"{options.Name}.RestApi"
            }));

            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/Program.cs", GetTemplate("WebAPI.Program.tpl", new
            {
                NAMESPACE = $"{options.Name}.RestApi"
            }));
            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/Startup.cs", GetTemplate("WebAPI.Startup.tpl", new
            {
                NAMESPACE = $"{options.Name}.RestApi"
            }));
        }

        private static void GenerateProto(string[] args)
        {
            var options = new GenerationOptions()
            {
                ModelOptions = new TypeGeneratorOption(),
                ServiceOptions = new TypeGeneratorOption(),
                ControllerOptions = new TypeGeneratorOption(),
                ClientOptions = new TypeGeneratorOption()
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
                    case "-clients-o":
                        options.ClientOptions.OutputDirectory = value;
                        break;
                    case "-clients-ns":
                        options.ClientOptions.Namespace = value;
                        break;
                    case "-f":
                        protoFile = value;
                        break;
                    case "-search-path":
                        searchPath = value;
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

            if(name != null && output != null)
            {
                options.ModelOptions.OutputDirectory = $"{output}/{name}.Models";
                options.ServiceOptions.OutputDirectory = $"{output}/{name}.Services/Generated";
                options.ControllerOptions.OutputDirectory = $"{output}/{name}.RestApi/Controllers/Generated";
                options.ControllerOptions.Namespace = "RestApi.Controllers";
                options.ClientOptions.OutputDirectory = $"{output}/{name}.Clients";
            }

            MicroserviceGenerator microserviceGenerator = new MicroserviceGenerator(options);

            Console.WriteLine($"Generating code from {protoFile}");

            microserviceGenerator.GenerateCode(ast, scope);

            Console.WriteLine("Code generated succesfully");
        }

        private static void PrintHelp()
        {
            Console.WriteLine("Microsevice Generator options:");
            Console.WriteLine("Example: ServiceGenerator -n Service1 -o Services/Service1 -sln Services.sln");            
            Console.WriteLine("-n : Service Name");
            Console.WriteLine("-o : Output Directory");
            Console.WriteLine("-sln : Solution File");

            Console.WriteLine();
            PrintProtoHelp();
        }

        private static void PrintProtoHelp()
        {
            Console.WriteLine("Proto Generator options:");
            Console.WriteLine("Example: ServiceGenerator proto -n Service1 -o ./Services/Service1 -f ./Protos/Service1.proto");            
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
            Console.WriteLine("-clients-o : Clients output directory");
            Console.WriteLine("-clients-ns : Clients namespace");            
        }
    }
}
