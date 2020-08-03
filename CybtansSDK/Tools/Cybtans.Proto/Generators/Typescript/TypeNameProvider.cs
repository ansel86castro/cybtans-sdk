using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Text;

namespace Cybtans.Proto.Generators.Typescript
{
    public static class TypeNameProvider
    {
        public static string GetPrimitiveTypeName(this PrimitiveType type)
        {
            switch (type.Name)
            {
                case "int8":
                case "int16":
                case "int32":
                case "int64":
                case "uint16":
                case "uint32":
                case "uint64":
                case "float":
                case "decimal":
                case "double": return "number";
                case "bool": return "boolean";
                case "string": return "string";
                case "bytes": return "number[]";
                case "datetime": return "string|Date";
                case "object": return "any";
                case "void": return "void";
                case "guid": return "string";
                case "map": return "any";
            }

            throw new InvalidOperationException($"Type {type.Name} not supported");
        }

        public static string GetTypeName(this TypeIdentifier type)
        {
            string name = type.TypeDeclaration.Name.Pascal();

            if (type.TypeDeclaration is PrimitiveType p)
            {
                name = GetPrimitiveTypeName(p);
            }

            if (type.IsArray)
            {
                name = $"{name}[]|null";
            }
            else if (type.IsMap)
            {
                name += "|null";
            }
            else if (type.TypeDeclaration is MessageDeclaration)
            {
                name += "|null";
            }

            return name;
        }

        public static string GetTypeName(this FieldDeclaration field)
        {
            var name = field.Type.GetTypeName();
            if(!name.EndsWith("null") && field.Option.Optional && field.Type.TypeDeclaration.Nullable)
            {
                name += "|null";
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



    }
}
