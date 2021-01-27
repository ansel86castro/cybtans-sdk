using Cybtans.Graphics.Models;
using Cybtans.Math;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;

namespace Cybtans.Graphics.Buffers
{
    public static class VertexSemantic
    {
        public const string PositionTransformed = "POSITIONH";
        public const string Position = "POSITION";
        public const string Normal = "NORMAL";
        public const string Tangent = "TANGENT";
        public const string Color = "COLOR";
        public const string TextureCoordinate = "TEXCOORD";
        public const string BlendIndices = "BLEND_INDICES";
        public const string BlendWeights = "BLEND_WEIGHTS";
        public const string OcclutionFactor = "OCC_FACTOR";
        public const string PointSize = "POINT_SIZE";        
    }

    public class VertexDefinition
    {
        private VertexElement[] _elements;
        private int _size;

        public VertexDefinition(VertexElement[] elements)
        {
            _elements = elements;
            _size = CalculateSize(elements);
        }        

        public IEnumerable<VertexElement> Elements => _elements;

        public int Size => _size;

        public VertexDefinition(Type vertexType)
        {
            var fields = vertexType.GetFields(BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
            Array.Sort(fields, (f1, f2) => ((int)Marshal.OffsetOf(vertexType, f1.Name)).CompareTo((int)Marshal.OffsetOf(vertexType, f2.Name)));
            var offsets = fields.Select(f => (short)Marshal.OffsetOf(vertexType, f.Name)).ToArray();

            _elements = new VertexElement[fields.Length];

            for (int i = 0; i < fields.Length; i++)
            {
                VertexElementAttribute attr;
                try
                {
                    attr = (VertexElementAttribute)fields[i].GetCustomAttributes(typeof(VertexElementAttribute), false)[0];
                }
                catch (IndexOutOfRangeException)
                {
                    throw new InvalidOperationException("The field" + fields[i].Name + " do not have a defined VertexElementAttribute");
                }

                var format = GetTypeFormat(fields[i].FieldType, out var size);

                _elements[i] = new VertexElement(attr.Stream,
                                              attr.Offset < 0 ? offsets[i] : attr.Offset,
                                              attr.Format == VertexElementFormat.Unused ? format : attr.Format,
                                              attr.Semantic,
                                              attr.UsageIndex,
                                              size);
            }

            _size = Marshal.SizeOf(vertexType);
        }

        int CalculateSize(VertexElement[] elements)
        {
            int size = 0;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                VertexElement e = elements[i];
                if (e.Format != VertexElementFormat.Unused)
                {
                    size += SizeOfElement(e.Format, e.Size);
                }
            }
            return size;
        }

        public static int GetSize(VertexElement[] elements)
        {
            int size = 0;
            for (int i = 0; i < elements.Length - 1; i++)
            {
                VertexElement e = elements[i];
                if (e.Format != VertexElementFormat.Unused)
                {
                    size += SizeOfElement(e.Format, e.Size);
                }
            }
            return size;
        }

        public static VertexElementFormat GetTypeFormat(Type type, out int size)
        {
            if (type == typeof(float))
            {
                size = 1;
                return VertexElementFormat.Float;
            }
            if (type == typeof(Vector2))
            {
                size = 2;
                return VertexElementFormat.Float;
            }
            if (type == typeof(Vector3))
            {
                size = 3;
                return VertexElementFormat.Float;
            }
            if (type == typeof(Vector4) || type == typeof(Color4))
            {
                size = 4;
                return VertexElementFormat.Float;
            }
            if (type == typeof(int) || type == typeof(uint))
            {
                size = 1;
                return VertexElementFormat.Int;
            }
            if (type == typeof(byte))
            {
                size = 1;
                return VertexElementFormat.Byte;
            }


            throw new ArgumentOutOfRangeException("type");

        }

        public static int SizeOfElement(VertexElementFormat type, int size)
        {
            switch (type)
            {
                case VertexElementFormat.Int:
                case VertexElementFormat.Float:
                case VertexElementFormat.UInt:
                    return sizeof(float) * size;

                case VertexElementFormat.UShort:
                case VertexElementFormat.Short:
                    return sizeof(short) * size;

                case VertexElementFormat.Byte:
                case VertexElementFormat.UByte:
                    return size;
            }
            throw new ArgumentOutOfRangeException("type");
        }

        public int OffsetOf(string semantic, int usageIndex)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i].Semantic == semantic && _elements[i].UsageIndex == usageIndex)
                {
                    return _elements[i].Offset;
                }
            }

            return -1;
        }

        public int SizeOf(string semantic, int usageIndex)
        {
            for (int i = 0; i < _elements.Length; i++)
            {
                if (_elements[i].Semantic == semantic && _elements[i].UsageIndex == usageIndex)
                {
                    return SizeOfElement(_elements[i].Format, _elements[i].Size);
                }
            }

            return -1;
        }

        public static VertexDefinition GetDefinition<T>()where T : unmanaged
        {
            return new VertexDefinition(typeof(T));
        }

        public Models.VertexDefinitionDto ToDto()
        {
            return new Models.VertexDefinitionDto
            {
                Size = Size,
                Elements = Elements.Select(x => x.ToDto()).ToList()
            };
        }
    }

    public class VertexElement
    {
        public short Offset { get; set; }
        public short Stream { get; set; }
        public string Semantic { get; set; }
        public byte UsageIndex { get; set; }
        public VertexElementFormat Format { get; set; }
        public int Size { get; set; }

        public VertexElement() { }

        public VertexElement(short stream, short offset, VertexElementFormat format, string semantic, byte usageIndex, int size)
        {
            Stream = stream;
            Offset = offset;
            Format = format;
            Semantic = semantic;
            UsageIndex = usageIndex;
            Size = size;
        }

        public VertexElementDto ToDto()
        {
            return new VertexElementDto
            {
                Offset = Offset,
                Stream = Stream,
                Semantic = Semantic,
                UsageIndex = UsageIndex,
                Size = Size,
                Format = (Models.VertexElementFormat)Format
            };
        }
    }

    public enum VertexElementFormat
    {
        Unused,
        Byte,
        UByte,
        Short,
        UShort,
        Int,
        UInt,
        Float,
        Double
    }

    [AttributeUsage(AttributeTargets.Field)]
    public class VertexElementAttribute : Attribute
    {
        public VertexElementAttribute()
        {
            Format = VertexElementFormat.Unused;
            Offset = -1;
        }
        public VertexElementAttribute(string semantic, byte usageIndex = 0, short stream = 0, VertexElementFormat type = VertexElementFormat.Unused, short offset = -1)
        {
            Semantic = semantic;
            UsageIndex = usageIndex;
            Stream = stream;
            Format = type;
            Offset = offset;
        }

        public short Offset { get; set; }
        public short Stream { get; set; }
        public string Semantic { get; set; }
        public byte UsageIndex { get; set; }
        public VertexElementFormat Format { get; set; }

    }

}
