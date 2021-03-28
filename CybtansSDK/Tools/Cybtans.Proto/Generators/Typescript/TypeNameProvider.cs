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
                case "bytes": return "string";
                case "datetime": return "string|Date";
                case "object": return "any";
                case "void": return "void";
                case "guid": return "string";
                case "map": return "any";
                case "ByteStream": return "Blob";                
                case "google.protobuf.Timestamp": return "string|Date";
                case "google.protobuf.Duration": return "string|Date";
                case "google.protobuf.Empty": return "void";
                case "google.protobuf.BoolValue": return "boolean|undefined|null";
                case "google.protobuf.DoubleValue":
                case "google.protobuf.FloatValue": 
                case "google.protobuf.Int32Value": 
                case "google.protobuf.Int64Value": 
                case "google.protobuf.UInt32Value": 
                case "google.protobuf.UInt64Value": return "number|undefined|null";
                case "google.protobuf.StringValue": return "string";
                case "google.protobuf.BytesValue": return "string|undefined|null";
            }

            throw new InvalidOperationException($"Type {type.Name} not supported");
        }

        public static string GetTypeName(this TypeIdentifier type)
        {
            string name = type.TypeDeclaration.Name.Pascal();

            if (type.IsMap && type.GenericArgs[0].TypeDeclaration == PrimitiveType.String)
            {
                name = $"{{ [key:{GetTypeName(type.GenericArgs[0])}]: {GetTypeName(type.GenericArgs[1])} }}";
            }
            else if (type.TypeDeclaration is PrimitiveType p)
            {
                name = GetPrimitiveTypeName(p);
            }

            if (type.IsArray)
            {
                name = $"{name}[]|null";
            }
            //else if (type.IsMap || !type.TypeDeclaration.IsValueType)
            //{
            //    name += "|null";
            //}            

            return name;
        }

        public static string GetTypeName(this FieldDeclaration field)
        {
            var name = field.Type.GetTypeName();
            if (field.Option.Typecript.Partial)
            {
                name = $"Partial<{name}>";
            }

            if(!name.EndsWith("null") && ((field.Option.Optional && field.Type.TypeDeclaration.IsValueType) || field.IsNullable))
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
