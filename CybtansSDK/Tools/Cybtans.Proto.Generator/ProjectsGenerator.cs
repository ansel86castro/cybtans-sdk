using Cybtans.Proto.Generators;
using System;
using System.Diagnostics;
using System.IO;
using System.Linq;

using static Cybtans.Proto.Generator.TemplateManager;

namespace Cybtans.Proto.Generator
{

    public class ProjectsGenerator : IGenerator
    {
        public class Options
        {
            public string Name { get; set; }

            public string Output { get; set; }

            public string Solution { get; set; }
        }
       
        public bool Generate(string[] args)
        {
            if (args == null || args.Length == 0 || !CanGenerate(args[0]))
                return false;

            GenerateMicroservice(args);
            return true;
        }

        public bool Generate(CybtansConfig config, GenerationStep step)
        {
            return false;
        }

        public bool CanGenerate(string value)
        {
            return value == "service" || value == "s";
        }

        public void PrintHelp()
        {
            Console.WriteLine("Microsevice Generator options are:");
            Console.WriteLine("s|service : Generates service project structure");
            Console.WriteLine("-n : The service Name");
            Console.WriteLine("-o : The output folder");
            Console.WriteLine("-sln :The solution file to attach");
            Console.WriteLine("Example: cybtans-cli s -n Service1 -o Services/Service1 -sln Services.sln");
        }

        private void GenerateMicroservice(string[] args)
        {
            Options options = new Options();
            for (int i = 1; i < args.Length; i++)
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
                    PrintHelp();
                    return;
                }
            }


            Directory.CreateDirectory(options.Output);
            //Generate Projects              
            Console.WriteLine("Generating projects...");

            GenerateProject("ModelsProject.tpl", options.Output, $"{ options.Name }.Models", null);
            GenerateProject("ClientsProject.tlp", options.Output, $"{ options.Name }.Clients", new[] { $"{ options.Name }.Models" });
            GenerateProject("ServicesProject.tpl", options.Output, $"{ options.Name }.Services", new[] { $"{ options.Name }.Models" });
            GenerateProject("TestProject.tpl", options.Output, $"{ options.Name }.Services.Tests", new[] { $"{ options.Name }.Models", $"{ options.Name }.Services" });
            GenerateWebApi(options);

            //Generate Proto 
            Directory.CreateDirectory($"{options.Output}/Proto");
            File.WriteAllText($"{options.Output}/Proto/{options.Name}.proto", GetTemplate("Proto.tpl", new
            {
                SERVICE = options.Name
            }));

            File.WriteAllText($"{options.Output}/cybtans.json", GetTemplate("cybtans.tpl", new
            {              
                SERVICE = options.Name
            }));            

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

        private static void GenerateProject(string template, string output, string project, string[] references)
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
                NAMESPACE = $"{options.Name}.RestApi",
                SERVICE = options.Name
            }));
            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/Startup.cs", GetTemplate("WebAPI.Startup.tpl", new
            {
                NAMESPACE = $"{options.Name}.RestApi",
                SERVICE = options.Name
        }));
        }
     
    }
}
