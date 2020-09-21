using Cybtans.Proto.Generators;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;

using static Cybtans.Proto.Generator.TemplateManager;

namespace Cybtans.Proto.Generator
{

    public class ProjectsGenerator : IGenerator
    {
        const string EntityFramework = "ef";
        const string SDK_VERSION = "1.2.1";

        public class Options
        {
            public string Name { get; set; }

            public string Output { get; set; }

            public string Solution { get; set; }

            public string Template { get; set; }
        }
       
        enum ProjectType { Models , Clients , Services, WebAPI, Domain, DomainEF }

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
            Console.WriteLine("-t : Template");
            Console.WriteLine("-sln :The solution file to attach");
            Console.WriteLine("Example: cybtans-cli s -n Service1 -o Services/Service1 -sln Services.sln");
        }

        private void GenerateMicroservice(string[] args)
        {
            Options options = new Options() { Template = EntityFramework };

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
                        options.Name = value;
                        break;
                    case "-o":
                        options.Output = value;
                        break;
                    case "-sln":
                        options.Solution = value;
                        break;
                    case "-t":
                        options.Template = value;
                        break;
                    default:
                        Console.WriteLine("Invalid Option");
                        PrintHelp();
                        break;
                }               
            }


            Directory.CreateDirectory(options.Output);
            //Generate Projects              
            Console.WriteLine("Generating projects...");

            GenerateProject("ModelsProject.tpl", options.Output, $"{ options.Name }.Models", null);
            GenerateProject("ClientsProject.tlp", options.Output, $"{ options.Name }.Clients", new[] { $"{ options.Name }.Models" });
            GenerateProject("ServicesProject.tpl", options.Output, $"{ options.Name }.Services", GetReferences(ProjectType.Services, options)  , GetPackages(ProjectType.Services, options));
            File.WriteAllText($"{options.Output}/{ options.Name }.Services/{ options.Name }Stub.cs", GetTemplate("Stub.tpl", new
            {
                SERVICE = options.Name
            }));

            GenerateProject("TestProject.tpl", options.Output, $"{ options.Name }.Services.Tests", new[] { $"{ options.Name }.Models", $"{ options.Name }.Services" });
            

            if(options.Template == EntityFramework)
            {
                GenerateProject("ServicesProject.tpl", options.Output, $"{ options.Name }.Domain", GetReferences(ProjectType.Domain, options), GetPackages(ProjectType.Domain, options));
                GenerateProject("ServicesProject.tpl", options.Output, $"{ options.Name }.Domain.EntityFramework", GetReferences(ProjectType.DomainEF, options), GetPackages(ProjectType.DomainEF, options));
                File.WriteAllText($"{options.Output}/{ options.Name }.Domain.EntityFramework/{ options.Name }Context.cs", GetTemplate("DbContext.tpl", new
                {
                    SERVICE = options.Name
                }));
            }

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

                if (options.Template == EntityFramework)
                {
                    Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Domain/{ options.Name }.Domain.csproj").WaitForExit();
                    Process.Start("dotnet", $"sln {options.Solution} add -s { options.Name } {options.Output}/{ options.Name }.Domain.EntityFramework/{ options.Name }.Domain.EntityFramework.csproj").WaitForExit();
                }
            }

            Console.WriteLine("Generation Completed");
        }

        private string[] GetReferences(ProjectType type, Options options)
        {
            List<string> references = new List<string>();

            switch (type)
            {
                case ProjectType.Services: 
                    references.Add( $"{ options.Name }.Models" );
                    if (options.Template == EntityFramework)
                    {
                        references.Add($"{ options.Name }.Domain");
                        references.Add($"{ options.Name }.Domain.EntityFramework");
                    }
                    break;
                case ProjectType.WebAPI:
                    references.AddRange(new[] { $"{ options.Name }.Models", $"{ options.Name }.Services" });
                    if (options.Template == EntityFramework)
                    {
                        references.Add($"{ options.Name }.Domain");
                        references.Add($"{ options.Name }.Domain.EntityFramework");
                    }
                    break;
                case ProjectType.DomainEF:
                    references.AddRange(new[] { $"{ options.Name }.Domain"});
                    break;
            }

            if (references.Any())
                return references.ToArray();
            return null;
        }        

        private string[] GetPackages(ProjectType type, Options options)
        {
            List<string> packages = new List<string>();


            switch (type)
            {
                case ProjectType.Services:
                    if (options.Template == EntityFramework)
                    {
                        packages.AddRange(new[]
                        {
                                $"<PackageReference Include=\"Cybtans.Entities\" Version=\"{SDK_VERSION}\" />",
                                $"<PackageReference Include=\"Cybtans.Services\" Version=\"{SDK_VERSION}\" />",
                                $"<PackageReference Include=\"Cybtans.Messaging\" Version=\"{SDK_VERSION}\" />",
                                $"<PackageReference Include=\"Cybtans.Entities.EventLog\" Version=\"{SDK_VERSION}\" />",
                                "<PackageReference Include=\"AutoMapper\" Version=\"10.0.0\" />",
                                "<PackageReference Include=\"Microsoft.Extensions.Logging.Abstractions\" Version=\"3.1.7\" />",
                                "<PackageReference Include=\"FluentValidation\" Version=\"9.2.0\" />"
                            });
                    }
                    break;
                case ProjectType.Domain:
                    packages.AddRange(new[]
                    {
                            $"<PackageReference Include=\"Cybtans.Entities\" Version=\"{SDK_VERSION}\" />",
                            $"<PackageReference Include=\"Cybtans.Entities.Proto\" Version=\"1.2.1\" />"
                        });
                    break;
                case ProjectType.DomainEF:
                    packages.AddRange(new[]
                    {
                            "<PackageReference Include=\"Microsoft.EntityFrameworkCore\" Version=\"3.1.7\" />",
                            $"<PackageReference Include=\"Cybtans.Entities.EntityFrameworkCore\" Version=\"{SDK_VERSION}\" />",
                        });
                    break;
                case ProjectType.WebAPI:
                    packages.AddRange(new[]
                    {                        
                        "<PackageReference Include=\"Swashbuckle.AspNetCore\" Version=\"5.5.1\" />",
                        "<PackageReference Include=\"Swashbuckle.AspNetCore.ReDoc\" Version=\"5.5.1\" />",
                        "<PackageReference Include=\"Microsoft.AspNetCore.Authentication.JwtBearer\" Version=\"3.1.7\" />",
                        "<PackageReference Include=\"Serilog.AspNetCore\" Version=\"3.4.0\" />",
                        $"<PackageReference Include=\"Cybtans.AspNetCore\" Version=\"{SDK_VERSION}\" />"
                    });

                    if (options.Template == EntityFramework)
                    {
                        packages.AddRange(new[]
                        {
                                $"<PackageReference Include=\"Cybtans.Entities.EntityFrameworkCore\" Version=\"{SDK_VERSION}\" />",
                                $"<PackageReference Include=\"Cybtans.Messaging.RabbitMQ\" Version=\"{SDK_VERSION}\" />",
                                $"<PackageReference Include=\"Cybtans.Services\" Version=\"{SDK_VERSION}\" /> ",
                                "<PackageReference Include=\"AutoMapper.Extensions.Microsoft.DependencyInjection\" Version=\"8.0.1\" />",
                                "<PackageReference Include=\"FluentValidation.AspNetCore\" Version=\"9.2.0\" />",
                                "<PackageReference Include=\"Microsoft.EntityFrameworkCore.SqlServer\" Version=\"3.1.7\" />"
                            });
                    }
                    break;
            }
            
            if(packages.Count == 0) return null;
            return packages.ToArray();
        }

        private static string References(params string[] references)
        {
            return string.Join(Environment.NewLine, references.Select(x => $"\t<ProjectReference Include=\"{x}\" />"));
        }

        private static void GenerateProject(string template, string output, string project, IEnumerable<string> references, IEnumerable<string> packages = null)
        {
            Directory.CreateDirectory($"{output}/{project}");

            var content = GetTemplate(template);
            content = TemplateProcessor.Process(content, new
            {
                FERERENCES = references != null ? "<ItemGroup>\r\n" + References(references.Select(x => $"../{x}/{x}.csproj").ToArray()) + "\r\n</ItemGroup >" : "",
                PACKAGES = packages != null ? "<ItemGroup>\r\n" + string.Join("\r\n", packages) + "\r\n</ItemGroup >" : "",
                SDK_VERSION
            });


            File.WriteAllText($"{output}/{project}/{project}.csproj", content);
        }

        private  void GenerateWebApi(Options options)
        {
            GenerateProject("WebAPI.tpl", options.Output, $"{ options.Name }.RestApi", GetReferences(ProjectType.WebAPI, options), GetPackages(ProjectType.WebAPI, options));

            Directory.CreateDirectory($"{options.Output}/{options.Name}.RestApi/Properties");
            Directory.CreateDirectory($"{options.Output}/{options.Name}.RestApi/Controllers");

            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/appsettings.Development.json", GetTemplate("WebAPI.appsettings.Development.tpl", new 
            {
                SERVICE = options.Name
            }));
            File.WriteAllText($"{options.Output}/{options.Name}.RestApi/appsettings.json", GetTemplate("WebAPI.appsettings.tpl", new 
            {
                SERVICE = options.Name
            }));

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
