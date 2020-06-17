using Cybtans.Entities;
using Cybtans.Proto.AST;
using Cybtans.Proto.Generators;
using Cybtans.Proto.Utils;
using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices.ComTypes;
using System.Text;
using System.Threading.Tasks.Sources;
using static Cybtans.Proto.Generator.TemplateManager;

namespace Cybtans.Proto.Generator
{
    public class MessageGenerator : IGenerator
    {
        public bool CanGenerate(string value)
        {
            return value == "messages" || value == "m";
        }

        public bool Generate(string[] args)
        {
            if (args == null || args.Length == 0 || !CanGenerate(args[0]))
                return false;

            string outputFilename = null;
            string assemblyFilename = null;
            HashSet<string> models = new HashSet<string>();

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
                        outputFilename = value;
                        break;
                    case "-assembly":
                        assemblyFilename = value;
                        break;
                    case "-models":
                        models.Add(value);
                        while (++i < args.Length)
                        {
                            if (args[i].StartsWith("-"))
                            {
                                i--;
                                break;
                            }

                            models.Add(args[i]);
                        }
                        break;
                }
            }

            if (outputFilename == null || assemblyFilename == null)
            {
                PrintHelp();
                return false;
            }
               
            var loadAssemblyPath = Path.GetDirectoryName(assemblyFilename);
            if (string.IsNullOrEmpty(loadAssemblyPath))
            {
                loadAssemblyPath = Environment.CurrentDirectory;
            }

            var assembly = Assembly.Load(File.ReadAllBytes(assemblyFilename)); 
            AppDomain.CurrentDomain.AssemblyResolve += delegate (object sender, ResolveEventArgs args)
            {
                var i = args.Name.IndexOf(',');
                var name = args.Name.Substring(0, i);
                string assemplyPath = Path.Combine(loadAssemblyPath, name + ".dll");
                if (!File.Exists(assemplyPath))
                    return null;  // Don't throw an exception if the assembly doesn't exist. Return null.
                return Assembly.Load(File.ReadAllBytes(assemplyPath));
            };

            GenerateMessages(outputFilename, models, assembly.ExportedTypes);
            return true;
        }

        public void PrintHelp()
        {
            Console.WriteLine("Messages options are:");
            Console.WriteLine("m : Generates protobuff message definitions");            
            Console.WriteLine("-o : The output filename");
            Console.WriteLine("-assembly : The models assembly");
            Console.WriteLine("-models :The list of models root to generate");
            Console.WriteLine("Example: ServiceGenerator m -o Service1/Protos/Models.proto -assembly Services1.Data.dll -models Customer Order Product Invoice");
        }    

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

        private HashSet<Type> GenerateMessages(string outputFilename, HashSet<string> models, IEnumerable<Type> types)
        {
            CodeWriter codeWriter = new CodeWriter();
            codeWriter.Append("syntax = \"proto3\";").AppendLine(2);

            var generated = new HashSet<Type>();
            foreach (var type in types)
            {
                if (generated.Contains(type))
                {
                    continue;
                }

                if (type.IsEnum)
                {
                    GenerateEnum(type, codeWriter, generated);
                }
                else
                {

                    var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);

                    if ((models.Count > 0 && !models.Contains(type.Name))
                        || type.IsValueType
                        || type.IsAbstract
                        || type.IsInterface
                        || attr == null)
                        continue;

                    GenerateMessage(type, codeWriter, generated);
                }

            }

            var path = Path.GetDirectoryName(outputFilename);
            if (!string.IsNullOrEmpty(path) && path != "." && path != "..")
            {
                Directory.CreateDirectory(path);
            }

            GenerateServices(codeWriter, generated);

            File.WriteAllText(outputFilename, codeWriter.ToString());
            return generated;
        }

        private void GenerateEnum(Type type, CodeWriter codeWriter, HashSet<Type> generated)
        {
            if (generated.Contains(type))
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

        private void GenerateMessage(Type type, CodeWriter codeWriter, HashSet<Type> generated)
        {
            var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
            if (attr == null || generated.Contains(type))
                return;

            HashSet<string> excluded = attr.GetExcluded();

            generated.Add(type);

            codeWriter.Append($"message { GetTypeName(type) } {{");
            codeWriter.AppendLine();

            var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            var counter = 1;
            List<Type> types = new List<Type>();

            foreach (var p in props)
            {
                if (excluded.Contains(p.Name))
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
                if (!isPrimitive && propertyType.GetCustomAttribute<GenerateMessageAttribute>(true) == null)
                {
                    continue;
                }

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

                if (!isPrimitive && !generated.Contains(propertyType))
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
                    GenerateMessage(t, codeWriter, generated);
                }
                else if (t.IsEnum)
                {
                    GenerateEnum(t, codeWriter, generated);
                }
            }
        }

        private void GenerateServices(CodeWriter codeWriter, HashSet<Type> types)
        {            
            codeWriter.AppendLine(2);

            var template = GetRawTemplate("ProtoServices.tpl");

            foreach (var type in types)
            {                
                var attr = type.GetCustomAttribute<GenerateMessageAttribute>(true);
                if (attr == null || !attr.GenerateCrudService || !type.IsClass)
                    continue;

                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                var IdProp = props.FirstOrDefault(x =>
                {
                    var name = x.Name.ToLower();
                    return name == "id" || name == $"{type.Name.ToLower()}id" || name == $"{type.Name.ToLower()}_id";
                });

                if (IdProp == null)
                    continue;

                codeWriter.AppendLine(2);
                codeWriter.Append(TemplateProcessor.Process(template, new
                {
                    ENTITY = type.Name.Pascal(),
                    ID_TYPE = GetTypeName(IdProp.PropertyType),
                    ID = IdProp.Name.Camel(),
                    ENTITYDTO = GetTypeName(type)
                }));

            }
        }
    }
}
