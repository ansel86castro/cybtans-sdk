#nullable enable

using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;

namespace Cybtans.Serialization
{
    public class BinarySerializer
    {
        public const string MEDIA_TYPE = "application/x-cybtans";
        public static DateTime EPOCH = new DateTime(1970, 1, 1, 0, 0, 0, DateTimeKind.Utc);
        public static Encoding DefaultEncoding { get; set; } = Encoding.UTF8;

        readonly Encoding _encoding;
        readonly Encoder _encoder;
        readonly byte[] _buffer = new byte[256];
        Memory<byte> _memory;
        Dictionary<Type, TypeCache>? _typeCache;

        public enum Types : byte
        {
            TYPE_NONE = 0x01,
            TYPE_TRUE = 0x02,
            TYPE_FALSE = 0x03,

            TYPE_INT_8 = 0x10,
            TYPE_UINT_8 = 0x11,
            TYPE_INT_16 = 0x12,
            TYPE_UINT_16 = 0x13,
            TYPE_INT_32 = 0x14,
            TYPE_UINT_32 = 0x15,
            TYPE_INT_64 = 0x16,
            TYPE_UINT_64 = 0x17,

            TYPE_FLOAT = 0x20,
            TYPE_DOUBLE = 0x21,

            TYPE_STRING_8 = 0x30,
            TYPE_STRING_16 = 0x31,
            TYPE_STRING_32 = 0x32,

            TYPE_BINARY_8 = 0x33,
            TYPE_BINARY_16 = 0x34,
            TYPE_BINARY_32 = 0x35,            

            TYPE_DATETIME = 0x40,

            TYPE_ARRAY_8 = 0x50,
            TYPE_ARRAY_16 = 0x51,
            TYPE_ARRAY_32 = 0x52,

            TYPE_MAP_8 = 0x60,
            TYPE_MAP_16 = 0x61,
            TYPE_MAP_32 = 0x62,

            TYPE_CODED_MAP_8 = 0x67,
            TYPE_CODED_MAP_16 = 0x68,
            TYPE_CODED_MAP_32 = 0x69,

            TYPE_DECIMAL = 0x70,

            TYPE_LIST_8 = 0x71,
            TYPE_LIST_16 = 0x72,
            TYPE_LIST_32 = 0x73,
            TYPE_GUID = 0x74,

            TYPE_STREAM_8 = 0x80,
            TYPE_STREAM_16 = 0x81,
            TYPE_STREAM_32 = 0x82
        };

        public BinarySerializer() : this(DefaultEncoding)
        {

        }

        public BinarySerializer(Encoding encoding)
        {
            _encoding = encoding;
            _encoder = encoding.GetEncoder();            
            _memory = _buffer.AsMemory();
        }

        #region Serialize

        public byte[] Serialize(object obj)
        {
            using var memStream = new MemoryStream();
            Serialize(memStream, obj);

            return memStream.ToArray();
        }

        public unsafe void Serialize(Stream stream, object obj)
        {
            if (obj == null)
            {
                stream.WriteByte((byte)Types.TYPE_NONE);
                return;
            }

            var type = obj.GetType();
            type = Nullable.GetUnderlyingType(type) ?? type;

            switch (obj)
            {
                case bool boolValue:
                    stream.WriteByte(boolValue ? (byte)Types.TYPE_TRUE : (byte)Types.TYPE_FALSE);
                    break;
                case byte byteV:
                    WriteInteger(stream, byteV);
                    break;
                case sbyte sByteV:
                    WriteInteger(stream, sByteV);
                    break;
                case short shortV:
                    WriteInteger(stream, shortV);
                    break;
                case ushort ushortV:
                    WriteInteger(stream, ushortV);
                    break;
                case int intV:
                    WriteInteger(stream, intV);
                    break;
                case uint uintV:
                    WriteInteger(stream, uintV);
                    break;
                case long longV:
                    WriteInteger(stream, longV);
                    break;
                case ulong ulongV:
                    WriteInteger(stream, ulongV);
                    break;
                case decimal decV:
                    WriteDecimal(stream, decV);
                    break;
                case float floatV:
                    WriteFloat(stream, floatV);
                    break;
                case double doubleV:
                    WriteDouble(stream, doubleV);
                    break;
                case char ch:
                    WriteChar(stream, ch);
                    break;
                case string str:
                    WriteString(stream, str);
                    break;
                case byte[] bytes:
                    WriteLenght(stream, bytes.Length, Types.TYPE_BINARY_8, Types.TYPE_BINARY_16, Types.TYPE_BINARY_32);
                    stream.Write(bytes, 0, bytes.Length);
                    break;
                case DateTime dateTime:
                    stream.WriteByte((byte)Types.TYPE_DATETIME);
                    TimeSpan span = dateTime - EPOCH;
                    WriteNumber(span.Ticks, stream);
                    break;
                case Guid guid:
                    WriteGuid(stream, guid);                    
                    break;
                case Array array:
                    WriteLenght(stream, array.Length, Types.TYPE_ARRAY_8, Types.TYPE_ARRAY_16, Types.TYPE_ARRAY_32);
                    WriteArray(stream, array);
                    break;
                case ICollection collection:
                    if (collection is IDictionary dict)
                    {
                        WriteLenght(stream, dict.Count, Types.TYPE_MAP_8, Types.TYPE_MAP_16, Types.TYPE_MAP_32);
                        WriteMap(stream, dict);
                    }
                    else if(collection is IList list)
                    {
                        WriteLenght(stream, list.Count, Types.TYPE_LIST_8, Types.TYPE_LIST_16, Types.TYPE_LIST_32);
                        WriteList(stream, list);
                    }
                    else
                    {
                        WriteLenght(stream, collection.Count, Types.TYPE_LIST_8, Types.TYPE_LIST_16, Types.TYPE_LIST_32);
                        WriteEnumerable(stream, collection);
                    }
                    break;                
                case IReflectorMetadataProvider accesorProvider:
                    WriteObject(stream, accesorProvider);
                    break;
                case Stream st:
                    if (st.Length > int.MaxValue)
                        throw new InvalidOperationException("Can not serialize more than 2GB of data for an stream");
                    WriteLenght(stream, (int)st.Length, Types.TYPE_STREAM_8, Types.TYPE_STREAM_16, Types.TYPE_STREAM_32);
                    WriteStream(stream, st);
                    break;
                default:
                    if (type.IsEnum)
                    {
                        WriteInteger(stream, System.Convert.ToInt32(obj));
                    }
                    else
                    {
                        WriteObject(stream, obj, type);
                    }
                    break;
            }
        }

        private void WriteObject(Stream stream, object obj, Type type)
        {
            var props = GetPropertiesList(type);

            WriteLenght(stream, props.Length, Types.TYPE_MAP_8, Types.TYPE_MAP_16, Types.TYPE_MAP_32);

            foreach (var p in props)
            {
                if (!p.CanRead)
                    continue;
                
                WriteString(stream, p.Name);                
                Serialize(stream, p.GetValue(obj));
            }
        }

        private void WriteObject(Stream stream, IReflectorMetadataProvider obj)
        {
            var accesor = obj.GetAccesor();
            var props = accesor.GetPropertyCodes();

            WriteLenght(stream, props.Length, Types.TYPE_CODED_MAP_8, Types.TYPE_CODED_MAP_16, Types.TYPE_CODED_MAP_32);

            foreach (var code in props)
            {                
                WriteInteger(stream, code);
                Serialize(stream, accesor.GetValue(obj, code));
            }
        }

        private unsafe void WriteString(Stream stream, string value)
        {                       
            var bytesCount = _encoding.GetByteCount(value);
           
            WriteLenght(stream, bytesCount, Types.TYPE_STRING_8, Types.TYPE_STRING_16, Types.TYPE_STRING_32);

            if(bytesCount <= _buffer.Length)
            {
                var numBytes =_encoding.GetBytes(value, 0, value.Length, _buffer, 0);
                stream.Write(_buffer, 0, numBytes);
            }
            else
            {
                var chars = value.AsSpan();
                var bytes = _memory.Span;

                int loops = (bytesCount / _buffer.Length) + (bytesCount % _buffer.Length > 0 ? 1 : 0);
                            
                int totalChars = 0;
                for (int i = 0; i < loops; i++)
                {
                    _encoder.Convert(chars.Slice(totalChars), bytes, i == (loops - 1), out var charsUsed, out var bytesWritten, out var completed);

                    stream.Write(bytes.Slice(0, bytesWritten));
                    totalChars += charsUsed;

                    if (completed)
                        break;
                }   
            }                      
        }

        private unsafe void WriteChar(Stream stream, char ch)
        {
            var chars = new ReadOnlySpan<char>(&ch, 1);
            var bytes = _memory.Span;
            int numBytes = _encoding.GetBytes(chars, bytes);

            stream.Write(bytes.Slice(0, numBytes));
        }

        private void WriteArray(Stream stream, Array array)
        {
            var len = array.Length;
            for (int i = 0; i < len; i++)
            {
                Serialize(stream, array.GetValue(i));
            }            
        }

        private void WriteList(Stream stream, IList list)
        {
            var len = list.Count;
            for (int i = 0; i < len; i++)
            {
                Serialize(stream, list[i]);
            }
        }

        private void WriteEnumerable(Stream stream, IEnumerable collection)
        {
            foreach (var item in collection)
            {
                Serialize(stream, item);
            }            
        }

        protected void WriteMap(Stream stream, IDictionary map)
        {
            foreach (DictionaryEntry item in map)
            {
                Serialize(stream, item.Key);
                Serialize(stream, item.Value);
            }
        }

        private static void WriteInteger(Stream stream, long value)
        {
            if (value >= sbyte.MinValue && value <= sbyte.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_INT_8);
                stream.WriteByte((byte)value);
            }
            else if (value >= byte.MinValue && value <= byte.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_8);
                stream.WriteByte((byte)value);
            }
            else if (value >= short.MinValue && value <= short.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_INT_16);
                WriteNumber((short)value, stream);
            }
            else if (value >= ushort.MinValue && value <= ushort.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_16);
                WriteNumber((ushort)value, stream);
            }
            else if (value >= int.MinValue && value <= int.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_INT_32);
                WriteNumber((int)value, stream);
            }
            else if (value >= uint.MinValue && value <= uint.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_32);
                WriteNumber((uint)value, stream);
            }
            else
            {
                stream.WriteByte((byte)Types.TYPE_INT_64);
                WriteNumber(value, stream);
            }
        }

        private static void WriteInteger(Stream stream, ulong value)
        {
            if (value <= byte.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_8);
                stream.WriteByte((byte)value);
            }
            else if (value <= ushort.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_16);
                WriteNumber((ushort)value, stream);
            }
            else if (value <= uint.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_32);
                WriteNumber((int)value, stream);
            }
            else if (value <= uint.MaxValue)
            {
                stream.WriteByte((byte)Types.TYPE_UINT_32);
                WriteNumber((uint)value, stream);
            }
            else
            {
                stream.WriteByte((byte)Types.TYPE_UINT_64);
                WriteNumber(value, stream);
            }
        }

        private static unsafe void WriteNumber<T>(T number, Stream stream)
            where T : unmanaged
        {
            var size = sizeof(T);
            byte* pBytes = (byte*)&number;

            if (size > 1)
            {
                Convert(size, pBytes);
            }

            var span = new ReadOnlySpan<byte>(pBytes, size);
            stream.Write(span);
        }

        private static void WriteFloat(Stream stream, float floatV)
        {
            int i = (int)floatV;
            if (i == floatV)
            {
                WriteInteger(stream, i);
            }
            else
            {
                stream.WriteByte((byte)Types.TYPE_FLOAT);
                WriteNumber(floatV, stream);
            }
        }

        private static void WriteDouble(Stream stream, double doubleV)
        {
            float floatV = (float)doubleV;
            if (floatV == doubleV)
            {
                WriteFloat(stream, floatV);
            }
            else
            {
                stream.WriteByte((byte)Types.TYPE_DOUBLE);
                WriteNumber(doubleV, stream);
            }
        }

        private static unsafe void WriteDecimal(Stream stream, decimal decimalV)
        {
            double doubleV = (double)decimalV;
            if ((decimal)doubleV == decimalV)
            {
                WriteDouble(stream, doubleV);
            }
            else
            {
                stream.WriteByte((byte)Types.TYPE_DECIMAL);
                WriteNumber(decimalV, stream);
            }
        }


        private static unsafe void Convert(int size, byte* pBytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                int i = 0, j = size - 1;
                while (i < j)
                {
                    var temp = pBytes[i];
                    pBytes[i] = pBytes[j];
                    pBytes[j] = temp;

                    i++;
                    j--;
                }
            }
        }

        private static void Convert(int size, in Span<byte> pBytes)
        {
            if (BitConverter.IsLittleEndian)
            {
                int i = 0, j = size - 1;
                while (i < j)
                {
                    var temp = pBytes[i];
                    pBytes[i] = pBytes[j];
                    pBytes[j] = temp;

                    i++;
                    j--;
                }
            }
        }

        private void WriteLenght(Stream stream, int length, Types type1, Types type2, Types type3)
        {
            if (length >= byte.MinValue && length <= byte.MaxValue)
            {
                stream.WriteByte((byte)type1);
                WriteNumber((byte)length, stream);
            }
            else if (length >= ushort.MinValue && length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)type2);
                WriteNumber((ushort)length, stream);
            }
            else
            {
                stream.WriteByte((byte)type3);
                WriteNumber(length, stream);
            }
        }

        private void WriteLenght(Stream stream, long length, Types type1, Types type2, Types type3, Types type4)
        {
            if (length >= byte.MinValue && length <= byte.MaxValue)
            {
                stream.WriteByte((byte)type1);
                WriteNumber((byte)length, stream);
            }
            else if (length >= ushort.MinValue && length <= ushort.MaxValue)
            {
                stream.WriteByte((byte)type2);
                WriteNumber((ushort)length, stream);
            }
            else if (length >= int.MinValue && length <= int.MaxValue)
            {
                stream.WriteByte((byte)type3);
                WriteNumber((int)length, stream);
            }
            else
            {
                stream.WriteByte((byte)type4);
                WriteNumber(length, stream);
            }
        }

        private unsafe void WriteGuid(Stream stream, Guid guid)
        {
            stream.WriteByte((byte)Types.TYPE_GUID);
            Span<byte> span = stackalloc byte[16];

            if (!guid.TryWriteBytes(span))
                throw new InvalidOperationException("Unable to serialize Guid");

            stream.Write(span);
        }

        private unsafe void WriteStream(Stream stream, Stream value)
        {            
            for (int bytes = value.Read(_buffer,0, _buffer.Length); bytes > 0; bytes = value.Read(_buffer, 0, _buffer.Length))
            {
                stream.Write(_buffer, 0, bytes);
            }
        }
        #endregion

        #region Deserialize

        public T Deserialize<T>(Stream stream) => (T)Deserialize(stream, typeof(T));

        public T Deserialize<T>(byte[] bytes) => (T)Deserialize(bytes, typeof(T));

        public object? Deserialize(byte[] bytes) => Deserialize(bytes, null);

        public object? Deserialize(Stream stream) => Deserialize(stream, null);

        public object? Deserialize(byte[] bytes, Type? type)
        {
            using var stream = new MemoryStream(bytes);
            return Deserialize(stream, type);
        }

        public object? Deserialize(Stream stream, Type? type)
        {
            var typeByte = stream.ReadByte();
            if (typeByte == -1) return null;
            var typeCode = (Types)typeByte;

            object? value;
            switch (typeCode)
            {
                case Types.TYPE_NONE: return null;
                case Types.TYPE_TRUE: value = true; break;
                case Types.TYPE_FALSE: value = false; break;
                case Types.TYPE_INT_8: value = (sbyte)stream.ReadByte(); break;
                case Types.TYPE_UINT_8: value = (byte)stream.ReadByte(); break;
                case Types.TYPE_INT_16: value = ReadNumber<short>(stream); break;
                case Types.TYPE_UINT_16: value = ReadNumber<ushort>(stream); break;
                case Types.TYPE_INT_32: value = ReadNumber<int>(stream); break;
                case Types.TYPE_UINT_32: value = ReadNumber<uint>(stream); break;
                case Types.TYPE_INT_64: value = ReadNumber<long>(stream); break;
                case Types.TYPE_UINT_64: value = ReadNumber<ulong>(stream); break;
                case Types.TYPE_FLOAT: value = ReadNumber<float>(stream); break;
                case Types.TYPE_DOUBLE: value = ReadNumber<double>(stream); break;
                case Types.TYPE_STRING_8: value = ReadString8(stream); break;
                case Types.TYPE_STRING_16: value = ReadString16(stream); break;
                case Types.TYPE_STRING_32: value = ReadString32(stream); break;
                case Types.TYPE_BINARY_8: value = ReadBinary8(stream); break;
                case Types.TYPE_BINARY_16: value = ReadBinary16(stream); break;
                case Types.TYPE_BINARY_32: value = ReadBinary32(stream); break;
                case Types.TYPE_STREAM_8: value = ReadStream8(stream); break;
                case Types.TYPE_STREAM_16: value = ReadStream16(stream); break;
                case Types.TYPE_STREAM_32: value = ReadStream32(stream); break;
                case Types.TYPE_DATETIME: value = ReadDateTime(stream); break;
                case Types.TYPE_GUID: value = ReadGuid(stream); break;
                case Types.TYPE_ARRAY_8: value = ReadArray8(stream, type);break;
                case Types.TYPE_ARRAY_16:value = ReadArray16(stream, type);break;
                case Types.TYPE_ARRAY_32:value = ReadArray32(stream, type);break;
                case Types.TYPE_LIST_8:value = ReadList8(stream, type);break;
                case Types.TYPE_LIST_16:value = ReadList16(stream, type);break;
                case Types.TYPE_LIST_32:value = ReadList32(stream, type);break;
                case Types.TYPE_MAP_8:value = ReadMap8(stream, type);break;
                case Types.TYPE_MAP_16:value = ReadMap16(stream, type);break;
                case Types.TYPE_MAP_32:value = ReadMap32(stream, type);break;                
                case Types.TYPE_CODED_MAP_8:
                case Types.TYPE_CODED_MAP_16:
                case Types.TYPE_CODED_MAP_32:
                    value = ReadCodedMap(stream, typeCode, type);
                    break;
                case Types.TYPE_DECIMAL:
                    value = ReadNumber<decimal>(stream);
                    break;
                default:
                    throw new NotSupportedException($"TYPE CODE {typeByte} not supported");
            }
         
            return BinaryConvert.ConvertTo(type, value);
        }

        private unsafe T ReadNumber<T>(Stream stream)
            where T : unmanaged
        {
            var size = sizeof(T);
            byte* ptr = stackalloc byte[sizeof(T)];
            var span = new Span<byte>(ptr, sizeof(T));

            int bytesRead = stream.Read(span);
            if (bytesRead != size)
                throw new InvalidOperationException("Invalid Size");

            Convert(size, span);

            return *(T*)ptr;
        }

        private unsafe string ReadString8(Stream stream) => ReadString(stream, ReadNumber<byte>(stream));
        private unsafe string ReadString16(Stream stream) => ReadString(stream, ReadNumber<ushort>(stream));
        private unsafe string ReadString32(Stream stream) => ReadString(stream, ReadNumber<int>(stream));
        private unsafe string ReadString(Stream stream, int length)
        {            
            if (length <= _buffer.Length)
            {
                stream.Read(_buffer, 0, length);

                return _encoding.GetString(_buffer, 0, length);
            }
            
            byte[] buff = new byte[length];
            stream.Read(buff, 0, length);
            return _encoding.GetString(buff);
        }

        private byte[] ReadBinary8(Stream stream) => ReadBinary(stream, ReadNumber<byte>(stream));
        private byte[] ReadBinary16(Stream stream) => ReadBinary(stream, ReadNumber<ushort>(stream));
        private byte[] ReadBinary32(Stream stream) => ReadBinary(stream, ReadNumber<int>(stream));
        private Stream ReadStream8(Stream stream) => ReadStream(stream, ReadNumber<byte>(stream));
        private Stream ReadStream16(Stream stream) => ReadStream(stream, ReadNumber<ushort>(stream));
        private Stream ReadStream32(Stream stream) => ReadStream(stream, ReadNumber<int>(stream));

        private byte[] ReadBinary(Stream stream, int length)
        {
            byte[] buff = new byte[length];
            stream.Read(buff, 0, length);
            return buff;
        }

        private Stream ReadStream(Stream stream, int length)
        {
            return new MemoryStream(ReadBinary(stream, length));
        }

        private DateTime ReadDateTime(Stream stream)
        {
            var value = ReadNumber<long>(stream);
            return EPOCH.AddTicks(value);
        }
               
        private ICollection ReadArray8(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<byte>(stream), arrayType, true);
        private ICollection ReadArray16(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<ushort>(stream), arrayType, true);
        private ICollection ReadArray32(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<int>(stream), arrayType, true);
     
        private ICollection ReadList8(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<byte>(stream), arrayType, false);
        private ICollection ReadList16(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<ushort>(stream), arrayType, false);
        private ICollection ReadList32(Stream stream, Type? arrayType) => ReadCollection(stream, ReadNumber<int>(stream), arrayType, false);

        private Array ReadArray(Stream stream, int length, Type? type)
        {
            Type? elementType = null;
            if (type != null)
            {
                if (!type.IsArray)
                    throw new InvalidOperationException($"Type {type} is not an array");

                elementType = type.GetElementType();
            }

            var array = Array.CreateInstance(elementType ?? typeof(object), length);
            for (int i = 0; i < length; i++)
            {
                var value = Deserialize(stream, elementType == typeof(object) ? null : elementType);
                array.SetValue(value, i);
            }

            return array;
        }

        private IList ReadList(Stream stream, int length, Type? listType)
        {
            Type? type = null;
            if (listType != null)
            {
                type = listType.GetGenericArguments()[0];
            }

            IList list = (IList)Activator.CreateInstance(listType ?? typeof(List<object>), length);
            for (int i = 0; i < length; i++)
            {
                var value = Deserialize(stream, type == typeof(object) ? null : type);
                list.Add(value);
            }

            return list;
        }

        private ICollection ReadCollection(Stream stream, int length, Type? type, bool arrayCode)
        {
            IList list;
            Type? elementType = null;

            if (type == null)
            {
                if (arrayCode)
                {
                    var array = new object?[length];
                    for (int i = 0; i < length; i++)
                    {
                        array[i] = Deserialize(stream, null);
                    }

                    return array;
                }
                else
                {
                    list = new List<object?>(length);
                    for (int i = 0; i < length; i++)
                    {
                        list.Add(Deserialize(stream, null));
                    }

                    return list;
                }
            }
            else if (type.IsArray)
            {
                elementType = type.GetElementType();
                var array = Array.CreateInstance(elementType, length);
                for (int i = 0; i < length; i++)
                {
                    array.SetValue(Deserialize(stream, elementType), i);
                }

                return array;
            }
            else if (type.IsInterface)
            {
                elementType = type.GetGenericArguments()[0];
                list = (IList)Activator.CreateInstance(typeof(List<>).MakeGenericType(elementType), length);
                for (int i = 0; i < length; i++)
                {
                    list.Add(Deserialize(stream, elementType));
                }

                return list;
            }
            else
            {
                elementType = type.GetGenericArguments()[0];
                list = (IList)Activator.CreateInstance(type, length);
                for (int i = 0; i < length; i++)
                {
                    list.Add(Deserialize(stream, elementType));
                }

                return list;
            }
        }

        private object ReadMap8(Stream stream, Type? type) => ReadMap(stream, ReadNumber<byte>(stream), type);
        private object ReadMap16(Stream stream, Type? type) => ReadMap(stream, ReadNumber<ushort>(stream), type);
        private object ReadMap32(Stream stream, Type? type) => ReadMap(stream, ReadNumber<int>(stream), type);

        private object ReadMap(Stream stream, int count, Type? type)
        {
            object obj;

            if (type == null || typeof(IDictionary).IsAssignableFrom(type))
            {
                if (type?.IsGenericType ?? false)
                {
                    if (type.GetGenericTypeDefinition() == typeof(Dictionary<,>))
                    {
                        var dictionary = (IDictionary)Activator.CreateInstance(type, count);
                        var genArgs = type.GetGenericArguments();
                        for (int i = 0; i < count; i++)
                        {
                            var key = Deserialize(stream, genArgs[0] == typeof(object) ? null : genArgs[0]);
                            var item = Deserialize(stream, genArgs[1] == typeof(object) ? null : genArgs[1]);
                            dictionary.Add(key, item);
                        }

                        obj = dictionary;
                    }
                    else if (type.GetGenericTypeDefinition() == typeof(ReadOnlyDictionary<,>))
                    {
                        var genArgs = type.GetGenericArguments();
                        var internalDicType = typeof(Dictionary<,>).MakeGenericType(genArgs);
                        var itemDic = ReadMap(stream, count, internalDicType);
                        obj = (IDictionary)Activator.CreateInstance(type, itemDic);
                    }
                    else
                    {
                        throw new InvalidOperationException($"Can not deserialize {type}");
                    }

                    return obj;
                }
                else if (type == null)
                {
                    Dictionary<object, object?> dic = new Dictionary<object, object?>();
                    for (int i = 0; i < count; i++)
                    {
                        var key = (string?)Deserialize(stream, typeof(string));
                        if (key != null)
                        {
                            var item = Deserialize(stream, null);
                            dic.Add(key, item);
                        }
                    }

                    return dic;
                }
                else
                {
                    throw new InvalidOperationException($"Can not deserialize {type}");
                }

            }

            var props = GetPropertiesMap(type);


            obj = Activator.CreateInstance(type);

            for (int i = 0; i < count; i++)
            {
                var name = (string?)Deserialize(stream, null);
                if (name != null && props.TryGetValue(name, out var p))
                {
                    var value = Deserialize(stream, p.PropertyType);
                    if (p.CanWrite)
                    {
                        p.SetValue(obj, value);
                    }
                }
                else
                {
                    Deserialize(stream, null);
                }
            }

            return obj;
        }

        private object ReadCodedMap(Stream stream, Types typeCode, Type? type)
        {                    
            int count = 0;
            switch (typeCode)
            {
                case Types.TYPE_CODED_MAP_8: count = stream.ReadByte(); break;
                case Types.TYPE_CODED_MAP_16: count = ReadNumber<ushort>(stream); break;
                case Types.TYPE_CODED_MAP_32: count = ReadNumber<int>(stream); break;
            }

            if (type == null)
            {
                return ReadMap(stream, count, null);
            }

            var item = (IReflectorMetadataProvider)Activator.CreateInstance(type);
            IReflectorMetadata accesor = item.GetAccesor();

            for (int i = 0; i < count; i++)
            {
                int code = 0;
                var keyCode = (Types)stream.ReadByte();
                switch (keyCode)
                {
                    case Types.TYPE_INT_8: code = stream.ReadByte(); break;
                    case Types.TYPE_UINT_8: code = stream.ReadByte(); break;
                    case Types.TYPE_INT_16: code = ReadNumber<short>(stream); break;
                    case Types.TYPE_UINT_16: code = ReadNumber<ushort>(stream); break;
                    case Types.TYPE_INT_32: code = ReadNumber<int>(stream); break;
                }

                var propertyType = accesor.GetPropertyType(code);
                var value = Deserialize(stream, propertyType);
                accesor.SetValue(item, code, value);
            }

            return item;
        }

       
        private unsafe Guid ReadGuid(Stream stream)
        {
            Span<byte> span = stackalloc byte[16];          
            stream.Read(span);
            return new Guid(span);
        }

        private Dictionary<string, PropertyInfo> GetPropertiesMap(Type type)
        {
            if(_typeCache == null)
            {
                _typeCache = new Dictionary<Type, TypeCache>();
            }

            if (!_typeCache.TryGetValue(type, out var cache))
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                cache = new TypeCache(type) { PropertyMap = props.ToDictionary(x => x.Name) };
                _typeCache.Add(type, cache);
            }
            else if (cache.PropertyMap == null)
            {
                cache.PropertyMap = cache.Properties.ToDictionary(x => x.Name);
            }

            return cache.PropertyMap;
        }

        private PropertyInfo[] GetPropertiesList(Type type)
        {
            if (_typeCache == null)
            {
                _typeCache = new Dictionary<Type, TypeCache>();
            }

            if (!_typeCache.TryGetValue(type, out var cache))
            {
                var props = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
                cache = new TypeCache(type) { Properties = props };
                _typeCache.Add(type, cache);
            }
            else if(cache.Properties == null)
            {
                cache.Properties = type.GetProperties(BindingFlags.Public | BindingFlags.Instance);
            }

            return cache.Properties;
        }

        #endregion

        class TypeCache
        {
            public Type Type;

            public Dictionary<string, PropertyInfo>? PropertyMap;
            public PropertyInfo[]? Properties;

            public TypeCache(Type type)
            {
                Type = type;
            }
        }
    }    
}
