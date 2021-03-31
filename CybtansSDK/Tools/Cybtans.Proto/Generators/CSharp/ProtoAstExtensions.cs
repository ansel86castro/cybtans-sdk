using Cybtans.Proto.AST;
using Cybtans.Proto.Utils;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Cybtans.Proto.Generators.CSharp
{
    public static class ProtoAstExtensions
    {
        public static string GetFieldName(this FieldDeclaration d)
        {
            return d.Name.Pascal();
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
                case "ByteStream": return "System.IO.Stream";
                case "google.protobuf.Timestamp": return "DateTime?";
                case "google.protobuf.Duration": return "TimeSpan?";
                case "google.protobuf.Empty": return "void";
                case "google.protobuf.BoolValue": return "bool?";
                case "google.protobuf.DoubleValue": return "double?";
                case "google.protobuf.FloatValue": return "float?";
                case "google.protobuf.Int32Value": return "int?";
                case "google.protobuf.Int64Value": return "long?";
                case "google.protobuf.UInt32Value": return "uint?";                
                case "google.protobuf.UInt64Value": return "ulong?";
                case "google.protobuf.StringValue": return "string?";
                case "google.protobuf.BytesValue": return "byte[]?";

            }

            throw new InvalidOperationException($"Type {type.Name} not supported");
        }

        public static string GetReturnTypeName(this ITypeDeclaration type)
        {
            if (PrimitiveType.Void.Equals(type))
            {
                return "Task";
            }
            else
            {
                return $"Task<{type.GetTypeName()}>";
            }
        }

        public static string GetFullReturnTypeName(this ITypeDeclaration type)
        {
            if (PrimitiveType.Void.Equals(type))
            {
                return "Task";
            }
            else
            {
                return $"Task<mds::{type.GetTypeName()}>";
            }
        }

        public static string GetControllerReturnTypeName(this ITypeDeclaration type)
        {
            if (PrimitiveType.Void.Equals(type))
            {
                return "Task";
            }
            else if (type.HasStreams())
            {
                return "async Task<IActionResult>";
            }            
            else
            {
                return $"Task<mds::{type.GetTypeName()}>";
            }
        }


        public static string GetRequestTypeName(this ITypeDeclaration type, string name)
        {
            if (PrimitiveType.Void.Equals(type))
            {
                return "";
            }
            else
            {
                return $"{type.GetTypeName()} {name}";
            }
        }

        public static string GetFullRequestTypeName(this ITypeDeclaration type, string name)
        {
            if (PrimitiveType.Void.Equals(type))
            {
                return "";
            }
            else if (type is MessageDeclaration)
            {
                return $"mds::{type.GetTypeName()} {name}";
            }
            else
            {                
                return $"{type.GetTypeName()} {name}";
            }
        }
        

        //public static bool HasStreams(this ITypeDeclaration type)
        //{
        //    if (type == PrimitiveType.Stream)
        //        return true;

        //    var msg = type as MessageDeclaration;
        //    if (msg == null)
        //        return false;

        //    foreach (var field in msg.Fields)
        //    {
        //        if (field.FieldType == PrimitiveType.Stream)
        //            return true;
        //    }

        //    if (msg.Fields.Any(x => x.FieldType is MessageDeclaration && x.FieldType.HasStreams()))
        //        throw new InvalidOperationException($"Streams are only allowed a the root message in  {msg.Name}");

        //    return false;
        //}

        public static string GetProtobufName(this IUserDefinedType type)
        {
            var name = type.SourceName ?? type.Name;
            name = name.Pascal();

            if(type.DeclaringMessage != null)
            {
                return $"{type.DeclaringMessage.GetProtobufName()}.Types.{name}";
            }

            return name;
        }
    }
}
