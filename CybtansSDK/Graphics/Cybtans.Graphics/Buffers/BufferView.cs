using System;
using System.Collections.Generic;

#nullable enable

namespace Cybtans.Graphics.Buffers
{
    public readonly struct BufferView
    {
        readonly IntPtr pter;
        readonly int stride;
        readonly int offset;
        public BufferView(IntPtr baseAddr, VertexDefinition vd, string semantic, int usageIndex)
        {
            pter = baseAddr;
            offset = vd.OffsetOf(semantic, usageIndex);
            stride = vd.Size;
        }

        public unsafe IntPtr this[int index]
        {
            get
            {
                return pter + index * stride + offset;
            }
        }
        public unsafe IntPtr this[uint index]
        {
            get
            {
                return (IntPtr)((uint)pter + index * (uint)stride + (uint)offset);
            }
        }

        public static BufferView PositionReader(IntPtr baseAddr, VertexDefinition vd)
        {
            return new BufferView(baseAddr, vd, VertexSemantic.Position, 0);
        }
        public static BufferView NormalReader(IntPtr baseAddr, VertexDefinition vd)
        {
            return new BufferView(baseAddr, vd, VertexSemantic.Normal, 0);
        }
        public static BufferView TangentReader(IntPtr baseAddr, VertexDefinition vd)
        {
            return new BufferView(baseAddr, vd, VertexSemantic.Tangent, 0);
        }
        public static BufferView Texture0Reader(IntPtr baseAddr, VertexDefinition vd)
        {
            return new BufferView(baseAddr, vd, VertexSemantic.TextureCoordinate, 0);
        }
    }

    public unsafe readonly struct IndexBufferView
    {
        readonly byte* pter;
        readonly int stride;
        readonly int count;

        public IndexBufferView(IntPtr baseAddr, bool sixteenBits, int count)
        {
            pter = (byte*)baseAddr;
            stride = sixteenBits ? 2 : 4;
            this.count = count;
        }

        public int Count { get { return count; } }

        public int this[int index]
        {
            get
            {
                return stride == 2 ? *(ushort*)(pter + index * stride) : *(int*)(pter + index * stride);
            }
            set
            {
                if (stride == 2)
                    *(ushort*)(pter + index * stride) = (ushort)value;
                else
                    *(int*)(pter + index * stride) = value;

            }
        }
    }

    public unsafe readonly struct BufferView<T> : IEnumerable<T>, IDisposable
        where T : unmanaged
    {
        readonly byte* pter;
        readonly int stride;
        readonly int count;
        readonly ByteBuffer? _buffer;

        public BufferView(ByteBuffer buffer, int count)
        {
            pter = (byte*)buffer.Pin();
            this.count = count;
            stride = sizeof(T);
            _buffer = buffer;
        }

        public BufferView(ByteBuffer buffer, int offet, int stride, int count)
        {
            pter = (byte*)buffer.Pin() + offet;
            this.stride = stride;
            this.count = count;
            _buffer = buffer;
            _buffer = buffer;
        }

        public BufferView(IntPtr pter, VertexDefinition vd, string semantic, int usageIndex, int count)
        {
            this.pter = (byte*)pter + vd.OffsetOf(semantic, usageIndex);
            stride = vd.Size;
            this.count = count;
            _buffer = null;
        }

        public BufferView(ByteBuffer buffer, VertexDefinition vd, string semantic, int usageIndex, int count)
        {
            pter = (byte*)buffer.Pin() + vd.OffsetOf(semantic, usageIndex);
            stride = vd.Size;
            this.count = count;
            _buffer = buffer;
        }

        public T this[int index]
        {
            get
            {

                return *(T*)(pter + index * stride);
            }
            set
            {
                *(T*)(pter + index * stride) = value;
            }

        }

        public byte* BasePter { get { return pter; } }

        public T* Pter => (T*)pter;

        public int Stride { get { return stride; } }

        public int Count { get { return count; } }

        public unsafe IntPtr GetPtr(int index)
        {
            return (IntPtr)(pter + index * stride);
        }


        public void SetValue(int index, ref T value)
        {
            *(T*)(pter + index * stride) = value;
        }

        #region IEnumerable<T> Members

        public AttributeEnumerator GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        public void Dispose()
        {
            _buffer?.UnPin();
        }

        #endregion

        public struct AttributeEnumerator : IEnumerator<T>
        {
            int index;
            BufferView<T> reader;

            public AttributeEnumerator(BufferView<T> reader)
            {
                this.reader = reader;
                index = -1;
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return reader[index]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                index = -1;
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return reader[index]; }
            }

            public bool MoveNext()
            {
                index++;
                if (index >= reader.Count)
                    return false;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion
        }

    }

    public unsafe readonly struct IndexedBufferView<T> : IEnumerable<T>, IDisposable
        where T : unmanaged
    {
        readonly byte* vertexPter;
        readonly int vstride;

        readonly byte* indicesPter;
        readonly int istride;
        readonly int icount;
        readonly IndexBuffer _indices;
        readonly VertexBuffer _vertexes;

        public IndexedBufferView(VertexBuffer vertexes, int vstride, IndexBuffer indices, int istride, int icount)
        {
            vertexPter = (byte*)vertexes.Pin();
            this.vstride = vstride;
            indicesPter = (byte*)indices.Pin();
            this.istride = istride;
            this.icount = icount;
            _indices = indices;
            _vertexes = vertexes;
        }

        public IndexedBufferView(VertexBuffer vertexes, VertexDefinition vd, string semantic, int usageIndex, IndexBuffer indices, int istride, int icount)
        {
            vertexPter = (byte*)vertexes.Pin() + vd.OffsetOf(semantic, usageIndex);
            vstride = vd.Size;
            indicesPter = (byte*)indices.Pin();
            this.istride = istride;
            this.icount = icount;
            _indices = indices;
            _vertexes = vertexes;
        }

        public int Count { get { return icount; } }

        public T this[int index]
        {
            get
            {
                index = istride == 2 ? *(ushort*)(indicesPter + index * istride) :
                                        *(int*)(indicesPter + index * istride);

                return *(T*)(vertexPter + index * vstride);
            }
            set
            {
                index = istride == 2 ? *(ushort*)(indicesPter + index * istride) :
                                        *(int*)(indicesPter + index * istride);

                *(T*)(vertexPter + index * vstride) = value;
            }
        }

        public IntPtr GetPtr(int index)
        {
            index = istride == 2 ? *(ushort*)(indicesPter + index * istride) :
                                       *(int*)(indicesPter + index * istride);

            return (IntPtr)(vertexPter + index * vstride);
        }

        public void GetValue(int index, out T value)
        {
            index = istride == 2 ? *(ushort*)(indicesPter + index * istride) :
                                      *(int*)(indicesPter + index * istride);

            value = *(T*)(vertexPter + index * vstride);
        }

        public void SetValue(int index, ref T value)
        {
            index = istride == 2 ? *(ushort*)(indicesPter + index * istride) :
                                        *(int*)(indicesPter + index * istride);

            *(T*)(vertexPter + index * vstride) = value;

        }

        #region IEnumerable<T> Members

        public AttributeEnumerator GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        IEnumerator<T> IEnumerable<T>.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        #endregion

        #region IEnumerable Members

        System.Collections.IEnumerator System.Collections.IEnumerable.GetEnumerator()
        {
            return new AttributeEnumerator(this);
        }

        public void Dispose()
        {
            _vertexes.UnPin();
            _indices.UnPin();
        }

        #endregion

        public struct AttributeEnumerator : IEnumerator<T>
        {
            int index;
            IndexedBufferView<T> reader;

            public AttributeEnumerator(IndexedBufferView<T> reader)
            {
                this.reader = reader;
                index = -1;
            }

            #region IEnumerator<T> Members

            public T Current
            {
                get { return reader[index]; }
            }

            #endregion

            #region IDisposable Members

            public void Dispose()
            {
                index = -1;
            }

            #endregion

            #region IEnumerator Members

            object System.Collections.IEnumerator.Current
            {
                get { return reader[index]; }
            }

            public bool MoveNext()
            {
                index++;
                if (index >= reader.Count)
                    return false;
                return true;
            }

            public void Reset()
            {
                index = -1;
            }

            #endregion
        }
    }
}
