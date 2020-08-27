using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public static class ProtoAstExtensions
    {
        public static string GetTypeName(this TypeIdentifier type)
        {
            string name = type.TypeDeclaration.Name.Pascal();

            if (type.TypeDeclaration is PrimitiveType p)
            {
                name = GetPrimitiveTypeName(p);
            }

            if (type.IsArray)
            {
                name = $"List<{name}>";
            }
            else if (type.IsMap)
            {
                name = $"{name}<{GetTypeName(type.GenericArgs[0])},{GetTypeName(type.GenericArgs[1])}>";
            }
            return name;
        }

        public static string GetTypeName(this ITypeDeclaration type)
        {
            if (type is PrimitiveType p)
            {
                return GetPrimitiveTypeName(p);
            }
            return type.Name.Pascal();
        }

        public static string GetPrimitiveTypeName(this PrimitiveType type)
        {
            switch (type.Name)
            {
                case "int8": return "byte";
                case "int16": return "short";
                case "int32": return "int";
                case "int64": return "long";
                case "uint16": return "ushort";
                case "uint32": return "uint";
                case "uint64": return "ulong";
                case "bool": return "bool";
                case "string": return "string";
                case "bytes": return "byte[]";
                case "datetime": return "DateTime";
                case "float": return "float";
                case "double": return "double";
                case "map": return "Dictionary";
                case "object": return "object";
                case "void": return "void";
                case "guid": return "Guid";
                case "decimal": return "decimal";
                case "stream": return "System.IO.Stream";
            }

            throw new InvalidOperationException($"Type {type.Name} not supported");
        }

        public static string GetReturnTypeName(this ITypeDeclaration type)
        {
            if (type == PrimitiveType.Void)
            {
                return "Task";
            }
            else
            {
                return $"Task<{type.GetTypeName()}>";
            }
        }

        public static string GetRequestTypeName(this ITypeDeclaration type, string name)
        {
            if (type == PrimitiveType.Void)
            {
                return "";
            }
            else
            {
                return $"{type.GetTypeName()} {name}";
            }
        }
    }
}
