using Microsoft.VisualBasic.CompilerServices;
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.IO;

namespace Cybtans.Proto.AST
{
    public class PrimitiveType : ITypeDeclaration, IEquatable<PrimitiveType>
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
        public static readonly PrimitiveType Decimal = new PrimitiveType("decimal", typeof(decimal));
        public static readonly PrimitiveType Stream = new PrimitiveType("ByteStream", typeof(Stream));
        public static readonly PrimitiveType TimeStamp = new PrimitiveType("google.protobuf.Timestamp", typeof(DateTime));
        public static readonly PrimitiveType Duration = new PrimitiveType("google.protobuf.Duration", typeof(TimeSpan));
        public static readonly PrimitiveType BoolValue = new PrimitiveType("google.protobuf.BoolValue", typeof(bool?));
        public static readonly PrimitiveType DoubleValue = new PrimitiveType("google.protobuf.DoubleValue", typeof(double?));
        public static readonly PrimitiveType FloatValue = new PrimitiveType("google.protobuf.FloatValue", typeof(float?));
        public static readonly PrimitiveType Int32Value = new PrimitiveType("google.protobuf.Int32Value", typeof(int?));
        public static readonly PrimitiveType Int64Value = new PrimitiveType("google.protobuf.Int64Value", typeof(long?));
        public static readonly PrimitiveType UInt32Value = new PrimitiveType("google.protobuf.UInt32Value", typeof(uint?));
        public static readonly PrimitiveType UInt64Value = new PrimitiveType("google.protobuf.UInt64Value", typeof(ulong?));
        public static readonly PrimitiveType StringValue = new PrimitiveType("google.protobuf.StringValue", typeof(string));
        public static readonly PrimitiveType BytesValue = new PrimitiveType("google.protobuf.BytesValue", typeof(byte[]));
        public static readonly PrimitiveType Empty = new PrimitiveType("google.protobuf.Empty", typeof(void));

        Type _genericDefinition;
        public PrimitiveType(string name, Type clrType)
        {
            Name = name;            
            ClrType = clrType;

            if(ClrType.IsGenericType)
            {
                _genericDefinition = ClrType.GetGenericTypeDefinition();
            }
        }

        public bool IsGenericType => ClrType.IsGenericType;

        public bool IsNullableValue => _genericDefinition == typeof(Nullable<>);

        public Type GenericDefinition => _genericDefinition;

        public bool IsGenericDefinition => ClrType?.IsGenericTypeDefinition ?? false;

        public Type ClrType { get; }

        public string Name { get; }

        public string Package => "std";

        public bool IsBuildIn => true;

        public bool IsChecked => true;

        public bool IsValueType => ClrType?.IsValueType ?? false;

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
            else if (type == typeof(decimal)) return Decimal;
            else if (type == typeof(Stream)) return Stream;
            else return null;
        }

        public override bool Equals(object obj)
        {
            return Equals(obj as PrimitiveType);
        }        

        public bool Equals([AllowNull] PrimitiveType other)
        {
            if (other == null) return false;

            if (System.Object.ReferenceEquals(this, other))
                return true;

            return ClrType == other.ClrType;
        }
    }
}
