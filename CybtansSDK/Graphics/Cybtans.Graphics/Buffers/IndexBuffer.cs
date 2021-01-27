#nullable enable


using Cybtans.Serialization;
using System;

namespace Cybtans.Graphics.Buffers
{
    public class IndexBuffer : ByteBuffer
    {
        public int Stride { get; private set; }

        public int IndicesCount { get; private set; }      

        public bool Is16BitsIndices => Stride == 2;

        public IndexBuffer() { }

        public IndexBuffer(int stride)
        {
            Stride = stride;
        }

        public IndexBuffer(byte[] data, int stride) : base(data)
        {
            Stride = stride;
            IndicesCount = data.Length / stride;
        }

        protected override void OnDataSet()
        {
            IndicesCount = Length / Stride;
        }

        protected override unsafe void OnDataSet<T>()
        {
            Stride = sizeof(T);
            OnDataSet();
        }

        public IndexBufferView GetIndexBufferView()
        {
            return new IndexBufferView(Pin(), Stride == 2, Length / Stride);
        }

       
    }
}
