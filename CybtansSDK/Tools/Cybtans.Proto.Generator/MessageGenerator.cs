using Cybtans.Entities;
using Cybtans.Proto.AST;
using Cybtans.Proto.Generators;
using Cybtans.Proto.Generators.CSharp;
using Cybtans.Proto.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using static Cybtans.Proto.Generator.TemplateManager;

namespace Cybtans.Proto.Generator
{
    public class MessageGenerator : IGenerator
    {
        public class GenerationOptions
        {
            public string ProtoOutputFilename { get; set; }

            public string AssemblyFilename { get; set; }            

            public bool GenerateAutoMapperProfile { get; set; }

            public bool GenerateCode => !string.IsNullOrEmpty(ServiceName) && !string.IsNullOrEmpty(ServiceDirectory);

            public string ServiceName { get; set; }

            public string ServiceDirectory { get; set; }

            public string[] Imports { get; set; }

            public bool IsValid()
            {
                return !string.IsNullOrEmpty(AssemblyFilename)
                    && !string.IsNullOrEmpty(ProtoOutputFilename);
            }

            public string GetMappingOutputPath()
            {
                var path =  Path.Combine(ServiceDirectory, $"{ServiceName}.Services", "Generated", "Mappings");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }

            public string GetServiceImplOutputPath()
            {
                var path = Path.Combine(ServiceDirectory, $"{ServiceName}.Services", "Generated");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }

            public string GetRestAPIOutputPath()
            {
                var path = Path.Combine(ServiceDirectory, $"{ServiceName}.RestApi");
                if (!Directory.Exists(path))
                    Directory.CreateDirectory(path);
                return path;
            }
        }

        public bool CanGenerate(string value)
        {
            return value == "messages" || value == "m";
        }

        public bool Generate(string[] args)
        {
            if (args == null || args.Length == 0 || !CanGenerate(args[0]))
                return false;


            var options = new GenerationOptions();

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
                        PrintHelp();

                        return false;
                    }

                    value = args[i];
                }

                switch (arg)
                {
                    case "-o":
                        options.ProtoOutputFilename = value;
                        break;
                    case "-assembly":
                        options.AssemblyFilename = value;
                        break;                                       
                    case "-service":
                        options.ServiceName = value;
                        break;
                    case "-service-o":
                        options.ServiceDirectory = value;
                        break;
                    case "-imports":
                        options.Imports = value.Split(",");
                        break;
                }
            }

            if (!options.IsValid())
            {
                PrintHelp();
                return false;
            }
               
            var loadAssemblyPath = Path.GetDirectoryName(options.AssemblyFilename);
            if (string.IsNullOrEmpty(loadAssemblyPath))
            {
                loadAssemblyPath = Environment.CurrentDirectory;
            }

            var assembly = Assembly.Load(File.ReadAllBytes(options.AssemblyFilename)); 
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                var i = args.Name.IndexOf(',');
                var name = args.Name.Substring(0, i);
                string assemplyPath = Path.Combine(loadAssemblyPath, name + ".dll");
                if (!File.Exists(assemplyPath))
                    return null; 

                return Assembly.Load(File.ReadAllBytes(assemplyPath));
            };

           var types = GenerateMessages(options, assembly.ExportedTypes);

            if (options.GenerateCode && options.ServiceName != null && options.ServiceDirectory != null)
            {
                ProtoGenerator protoGenerator = new ProtoGenerator();
                protoGenerator.Generate(new [] { 
                    "proto", 
                    "-n", options.ServiceName, 
                    "-o", options.ServiceDirectory, 
                    "-f",  options.ProtoOutputFilename });

                GenerateMappings(types, options);
                GenerateServicesImplementation(types, options);
                GenerateRestApiRegisterExtensor(types, options);
            }

            return true;
        }

        public void PrintHelp()
        {
            Console.WriteLine("Messages options are:");
            Console.WriteLine("m : Generates protobuff message definitions");            
            Console.WriteLine("-o : The output filename");
            Console.WriteLine("-assembly : The models assembly");
            Console.WriteLine("-models :The list of models root to generate");
            Console.WriteLine("-policies : one of (enabled | disabled) to generate service policies");
            Console.WriteLine("-service : Service Name");
            Console.WriteLine("-service-o : Service root directory");
            Console.WriteLine("-imports : Comma separated import paths for protobuff");
            Console.WriteLine("Example: ServiceGenerator m -o Service1/Protos/Models.proto -assembly Services1.Data.dll");
            Console.WriteLine("       : ServiceGenerator m -o Service1/Protos/Models.proto -assembly Services1.Data.dll -service Service1 -service-o Service1 -imports ./Common.proto");
        }

        #region Proto Generation

        private string GetTypeName(Type type)
        {
            if (type.IsEnum)
                return type.Name.Pascal();

            var primitive = PrimitiveType.GetPrimitiveType(type);
            if (primitive != null)
                return primitive.Name;

            var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
            return attr?.Name ?? (type.Name.Pascal() + "Dto");
        }

        private HashSet<Type> GenerateMessages(GenerationOptions options, IEnumerable<Type> types)
        {
            string outputFilename = options.ProtoOutputFilename;            

            CodeWriter codeWriter = new CodeWriter();
            codeWriter.Append("syntax = \"proto3\";").AppendLine(2);

            if (options.GenerateCode)
            {
                codeWriter.Append($"package {options.ServiceName};").AppendLine(2);
            }

            if(options.Imports!= null)
            {
                foreach (var import in options.Imports)
                {
                    codeWriter.Append($"import \"{import}\";").AppendLine();
                }

                codeWriter.AppendLine();
            }

            var generated = new HashSet<Type>();
            foreach (var type in types)
            {
                if (generated.Contains(type))                
                    continue;                

                var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
                if (attr == null)
                    continue;

                if (type.IsEnum)
                {
                    GenerateEnum(type, codeWriter, generated);
                }
                else if(type.IsClass && !type.IsAbstract)
                {                                        
                    GenerateMessage(type, codeWriter, generated, new HashSet<Type>());
                }
            }

            var path = Path.GetDirectoryName(outputFilename);
            if (!string.IsNullOrEmpty(path) && path != "." && path != "..")
            {
                Directory.CreateDirectory(path);
            }

            GenerateServices(codeWriter, generated, options);

            File.WriteAllText(outputFilename, codeWriter.ToString());
            return generated;
        }

        private void GenerateEnum(Type type, CodeWriter codeWriter, HashSet<Type> generated)
        {
            var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
            if (attr == null || generated.Contains(type))
                return;         

            generated.Add(type);

            codeWriter.Append($"enum { type.Name.Pascal() } {{");

            var members = Enum.GetNames(type);
            foreach (var item in members)
            {
                codeWriter.AppendLine();
                var value = Convert.ToInt32(Enum.Parse(type, item));
                codeWriter.Append('\t', 1).Append($"{item} = {value};");
            }


            codeWriter.AppendLine();
            codeWriter.Append("}");
            codeWriter.AppendLine(2);
        }

        private void GenerateMessage(Type type, CodeWriter codeWriter, HashSet<Type> generated, HashSet<Type> visited)
        {
            var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
            if (attr == null || generated.Contains(type))
                return;           

            generated.Add(type);
            visited.Add(type);

            codeWriter.Append($"message { GetTypeName(type) } {{");
            codeWriter.AppendLine();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var counter = 1;
            List<Type> types = new List<Type>();

            foreach (var p in props)
            {                
                if (p.GetCustomAttribute<MessageExcludedAttribute>() != null)
                    continue;

                Type propertyType = p.PropertyType;
                bool repeated = false;

                if (propertyType.IsArray && propertyType != typeof(byte[]))
                {
                    propertyType = propertyType.GetElementType();
                }
                else if (typeof(ICollection).IsAssignableFrom(propertyType))
                {
                    if (propertyType.IsGenericType)
                    {
                        propertyType = propertyType.GetGenericArguments()[0];
                    }
                    else
                    {
                        propertyType = typeof(object);
                    }
                }

                bool optional = propertyType.IsGenericType && propertyType.GetGenericTypeDefinition() == typeof(Nullable<>);
                if (optional)
                {
                    propertyType = propertyType.GetGenericArguments()[0];
                }

                var isPrimitive = PrimitiveType.GetPrimitiveType(propertyType) != null;
                var propertyTypeAttr = propertyType.GetCustomAttribute<GenerateMessageAttribute>(true);

                if (visited.Contains(propertyType) || (!isPrimitive && !propertyType.IsEnum && propertyTypeAttr == null))
                    continue;                                              

                codeWriter.Append('\t', 1);

                if (repeated)
                {
                    codeWriter.Append("repeated ");
                }

                codeWriter.Append($"{GetTypeName(propertyType)} {p.Name.Camel()} = {counter++}");
                if (optional)
                {
                    codeWriter.Append(" [optional = true]");
                }
                codeWriter.Append(";");
                codeWriter.AppendLine();

                if (!isPrimitive && !generated.Contains(propertyType) && propertyTypeAttr != null)
                {
                    types.Add(propertyType);
                }
            }

            codeWriter.Append("}");
            codeWriter.AppendLine(2);

            foreach (var t in types)
            {
                if (t.IsClass)
                {
                    GenerateMessage(t, codeWriter, generated, visited);
                }
                else if (t.IsEnum)
                {
                    GenerateEnum(t, codeWriter, generated);
                }
            }
        }

        private void GenerateServices(CodeWriter codeWriter, HashSet<Type> types, GenerationOptions options)
        {            
            codeWriter.AppendLine();

            var template = GetRawTemplate("ProtoServices.tpl");
            if (types.Any())
            {
                codeWriter.Append(GetAllTemplate);
            }

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
                if (attr == null || !attr.Service || !type.IsClass)
                    continue;

                PropertyInfo IdProp = GetKey(type);

                if (IdProp == null)
                    continue;                                

                codeWriter.AppendLine();
                codeWriter.Append(TemplateProcessor.Process(template, new
                {
                    ENTITY = type.Name.Pascal(),
                    ID_TYPE = GetTypeName(IdProp.PropertyType),
                    ID = IdProp.Name.Camel(),
                    ENTITYDTO = GetTypeName(type),
                    READ_POLICY = GetSecurity(attr.Security, attr.AllowedRead ?? $"{ type.Name.ToLowerInvariant()}.read"),
                    WRITE_POLICY = GetSecurity(attr.Security, attr.AllowedWrite ?? $"{ type.Name.ToLowerInvariant()}.write")
                }));

            }
        }

        private string GetSecurity(SecurityType securityType, string security)
        {            
            return securityType switch
            {
                SecurityType.None => "",
                SecurityType.Policy => $"option policy = \"{security}\";",
                SecurityType.Role => $"option roles = \"{security}\";",
                SecurityType.Authorized => $"option authorized = true;",
                SecurityType.AllowAnonymous => $"option anonymous = true;",               
                _ => throw new NotImplementedException()
            };
        }
      
        private static PropertyInfo GetKey(Type type)
        {
            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var IdProp = props.FirstOrDefault(x =>
            {
                var name = x.Name.ToLower();
                return name == "id" || name == $"{type.Name.ToLower()}id" || name == $"{type.Name.ToLower()}_id";
            });
            return IdProp;
        }

        string GetAllTemplate = @"
message GetAllRequest {
	string filter = 1;
	string sort = 2;
	int32 skip = 3;
	int32 take = 4;
}
";

        #endregion

        #region CSharp Generation

        private string GetNamespace(Type type)
        {
            var index = type.FullName.LastIndexOf(".");
            string ns = null;
            if (index > 0)
            {
                ns = type.FullName.Substring(0, index);
            }
            if (ns == null)
            {
                Console.WriteLine("Invalid Namespace Entity");              
            }
            return ns;
        }

        private void GenerateMappings(HashSet<Type> types, GenerationOptions options)
        {
            var writer = new CodeWriter();
            string ns = GetNamespace(types.First());
            if (ns == null)
            {                
                return;
            }

            foreach (var type in types)
            {
                if (!type.IsClass)
                    continue;

                writer.AppendLine();
                writer.Append($"CreateMap<{type.Name}, {GetTypeName(type)}>();").AppendLine();
                writer.Append($"CreateMap<{GetTypeName(type)},{type.Name}>();").AppendLine();
            }

            File.WriteAllText($"{options.GetMappingOutputPath()}/{options.ServiceName}AutoMapperProfile.cs", 
            TemplateProcessor.Process(MappingTemplate, new
            {
                ENTITIES_NAMESPACE = ns,
                SERVICE = options.ServiceName,
                MAPPINGS = writer.ToString()
            }));
        }

        private void GenerateServicesImplementation(HashSet<Type> types, GenerationOptions options)
        {          
            string ns = GetNamespace(types.First());
            if (ns == null)            
                return;            

            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);

                if (!type.IsClass || !attr.Service)
                    continue;

                PropertyInfo IdProp = GetKey(type);

                if (IdProp == null)
                    continue;

                var primitiveType = PrimitiveType.GetPrimitiveType(IdProp.PropertyType);
                if (primitiveType == null)
                    continue;

                File.WriteAllText($"{options.GetServiceImplOutputPath()}/{type.Name}Service.cs",
                TemplateProcessor.Process(ServiceImpTemplate, new
                {
                    ENTITIES_NAMESPACE = ns,
                    SERVICE = options.ServiceName,
                    ENTITY = type.Name,
                    TKEY = primitiveType.GetPrimitiveTypeName(),
                    TMESSAGE = GetTypeName(type)
                }));
            }
        }

        private void GenerateRestApiRegisterExtensor(HashSet<Type> types, GenerationOptions options)
        {
            CodeWriter writer = new CodeWriter();
            foreach (var type in types)
            {
                var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
                if (!type.IsClass || !attr.Service)
                    continue;

                writer.Append($"services.AddScoped<I{type.Name}Service, {type.Name}Service>();").AppendLine();                
            }

            File.WriteAllText($"{options.GetRestAPIOutputPath()}/Startup.ServiceRegisterExtension.cs",
               TemplateProcessor.Process(StartupRegisterTemplate, new
               {
                   SERVICE = options.ServiceName,
                   REGISTERS = writer.ToString()
               }));
        }

        const string MappingTemplate = @"
using System;
using AutoMapper;
using @{ ENTITIES_NAMESPACE };
using @{ SERVICE }.Models;

namespace @{ SERVICE }.Services
{
    public class @{ SERVICE }AutoMapperProfile:Profile
    {
        public @{ SERVICE }AutoMapperProfile()
        {
           @{ MAPPINGS }        
        }
    }
}";

        const string ServiceImpTemplate = @"
using System;
using AutoMapper;
using Cybtans.Entities;
using Cybtans.Services;
using Microsoft.Extensions.Logging;
using @{ ENTITIES_NAMESPACE };
using @{ SERVICE }.Models;

namespace @{ SERVICE }.Services
{
    public class @{ ENTITY }Service : CrudService<@{ENTITY}, @{TKEY}, @{TMESSAGE}, Get@{ENTITY}Request, GetAllRequest, GetAll@{ENTITY}Response, Update@{ENTITY}Request, Delete@{ENTITY}Request>, I@{ENTITY}Service
    {
        public @{ ENTITY }Service(IRepository<@{ENTITY}, @{TKEY}> repository, IUnitOfWork uow, IMapper mapper, ILogger<@{ENTITY}Service> logger)
            : base(repository, uow, mapper, logger)
        {

        }
    }
}";

        const string StartupRegisterTemplate = @"
using Microsoft.Extensions.DependencyInjection;
using @{SERVICE}.Services;

namespace @{SERVICE}.RestApi
{
    public static class StartupServiceRegisterExtension
    {
        public static void Register@{SERVICE}Services(this IServiceCollection services)
        {
            @{REGISTERS}
        }
    }
}";

        #endregion
    }
}
