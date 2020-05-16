using System;
using System.Collections.Generic;

namespace CybtansSdk.Proto.AST
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

        public bool Nullable => ClrType.IsValueType;

    }
}
