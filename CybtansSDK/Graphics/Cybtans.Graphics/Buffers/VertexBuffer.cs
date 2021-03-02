#nullable enable


namespace Cybtans.Graphics.Buffers
{
    public class VertexBuffer : ByteBuffer
    {
        VertexDefinition _vd;
        int _vertexCount;


        public VertexBuffer(VertexDefinition vd)
        {
            _vd = vd;
        }

        public VertexBuffer(VertexDefinition vd, byte[] data)
            : base(data)
        {
            _vd = vd;
            _vertexCount = data.Length / vd.Size;
        }

        protected override void OnDataSet()
        {
            _vertexCount = Length / _vd.Size;
        }

        public VertexDefinition VertexDefinition => _vd;

        public int VertexCount => _vertexCount;
       
        public unsafe BufferView<T> GetVertexBufferView<T>(string semantic, int index = 0) where T : unmanaged
        {
            return new BufferView<T>(this, _vd, semantic, index, _vertexCount);
        }

        public unsafe BufferView<T> GetVertexBufferView<T>(int offset) where T : unmanaged
        {
            return new BufferView<T>(this, offset, _vd.Size, _vertexCount);
        }

        public IndexedBufferView<T> GetIndexedBufferView<T>(IndexBuffer ib, string semantic, int index = 0) where T : unmanaged
        {
            return new IndexedBufferView<T>(this, _vd, semantic, index, ib, ib.Stride, ib.IndicesCount);
        }
    }
}
