using System;
using System.Collections.Generic;

namespace Cybtans.Proto.AST
{
    public class PrimitiveType : ITypeDeclaration
    {
        public static readonly PrimitiveType Double = new PrimitiveType("double", typeof(double));
        public static readonly PrimitiveType Float = new PrimitiveType("float", typeof(float));
        public static readonly PrimitiveType Int8 = new PrimitiveType("int8", typeof(byte));
        public static readonly PrimitiveType Int16 = new PrimitiveType("int16", typeof(short));
        public static readonly PrimitiveType Int32 = new PrimitiveType("int32", typeof(int));
        public static readonly PrimitiveType Int64 = new PrimitiveType("int64", typeof(long));
        public static readonly PrimitiveType UInt16 = new PrimitiveType("uint16", typeof(ushort));
        public static readonly PrimitiveType UInt32 = new PrimitiveType("uint32", typeof(uint));
        public static readonly PrimitiveType UInt64 = new PrimitiveType("uint64", typeof(ulong));
        public static readonly PrimitiveType Bool = new PrimitiveType("bool", typeof(bool));
        public static readonly PrimitiveType String = new PrimitiveType("string", typeof(string));
        public static readonly PrimitiveType Bytes = new PrimitiveType("bytes", typeof(byte[]));
        public static readonly PrimitiveType Datetime = new PrimitiveType("datetime", typeof(DateTime));
        public static readonly PrimitiveType Map = new PrimitiveType("map", typeof(Dictionary<,>));
        public static readonly PrimitiveType Object = new PrimitiveType("object", typeof(object));
        public static readonly PrimitiveType Void = new PrimitiveType("void", typeof(void));
        public static readonly PrimitiveType Guid = new PrimitiveType("guid", typeof(Guid));

        public PrimitiveType(string name, Type clrType)
        {
            Name = name;            
            ClrType = clrType;
        }

        public bool IsGenericDefinition => ClrType?.IsGenericTypeDefinition ?? false;

        public Type ClrType { get; }

        public string Name { get; }

        public string Package => "std";

        public bool IsBuildIn => true;

        public bool IsChecked => true;

        public bool Nullable => ClrType?.IsValueType ?? false;

        public static PrimitiveType GetPrimitiveType(Type type)
        {
            if (type == typeof(Guid)) return Guid;
            else if (type == typeof(byte)) return Int8;
            else if (type == typeof(short)) return Int16;
            else if (type == typeof(int)) return Int32;
            else if (type == typeof(long)) return Int64;
            else if (type == typeof(ushort)) return UInt16;
            else if (type == typeof(uint)) return UInt32;
            else if (type == typeof(ulong)) return UInt64;
            else if (type == typeof(float)) return Float;
            else if (type == typeof(double)) return Double;
            else if (type == typeof(bool)) return Bool;
            else if (type == typeof(string)) return String;
            else if (type == typeof(byte[])) return Bytes;
            else if (type == typeof(DateTime)) return Datetime;
            else if (type == typeof(object)) return Object;
            else if (type == typeof(void)) return Void;
            else if (type == typeof(Dictionary<,>)) return Map;
            else return null;
        }
    }
}
