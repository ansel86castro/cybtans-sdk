using System;
using System.Buffers;
using System.Reflection;

#nullable enable

namespace Cybtans.Graphics.Buffers
{
    public class ByteBuffer : IDisposable
    {
        protected Memory<byte> _buffer;
        MemoryHandle _handler;
        int refCount;

        public ByteBuffer()
        {

        }

        public ByteBuffer(byte[] data)
        {
            SetData(data);
        }

        public ByteBuffer(ReadOnlySpan<byte> data)
        {
            SetData(data);
        }


        public int Length => _buffer.Length;

        protected virtual void OnDataSet() { }

        protected unsafe void Allocate(int bytes)
        {
            if (_handler.Pointer != null)
                _handler.Dispose();

            _buffer = new Memory<byte>(new byte[bytes]);
        }

        protected virtual void OnDataSet<T>() where T: unmanaged { }

        public void SetData(byte[] data)
        {
            _buffer = new Memory<byte>(data);
            OnDataSet();
        }

        public virtual void SetData(byte[] data, int start, int lenght)
        {
            _buffer = new Memory<byte>(data, start, lenght);
            OnDataSet();
        }

        public void SetData(ReadOnlySpan<byte> data)
        {
            var arr = data.ToArray();
            SetData(arr);
        }

        public unsafe void SetData<T>(Span<T> data)where T : unmanaged
        {
            var bytes = data.Length * sizeof(T);
            Allocate(bytes);

            fixed (T* pter = data)
            {
                void* dest = (void*)Pin();
                Buffer.MemoryCopy(pter, dest, bytes, bytes);
                UnPin();
            }

            OnDataSet<T>();
        }

        public unsafe void SetData<T>(T[] data) where T : unmanaged
        {
            var bytes = data.Length * sizeof(T);
            Allocate(bytes);

            fixed (T* pter = data)
            {
                void* dest = (void*)Pin();
                Buffer.MemoryCopy(pter, dest, bytes, bytes);
                UnPin();
            }

            OnDataSet<T>();
        }

        public unsafe IntPtr Pin()
        {
            if (_buffer.IsEmpty)
                throw new InvalidOperationException();

            if (_handler.Pointer == null)
            {
                _handler = _buffer.Pin();
            }

            refCount++;
            return new IntPtr(_handler.Pointer);
        }

        public void UnPin()
        {
            if (refCount <= 0)
                return;

            refCount--;
            if (refCount == 0)
            {
                _handler.Dispose();
                _handler = new MemoryHandle();
            }
        }

        public byte[] ToArray()
        {
            return _buffer.ToArray();
        }

        public unsafe T[] ToArray<T>()
            where T:unmanaged
        {            

            T[] arr = new T[_buffer.Length];
            var memory = new Memory<T>(arr);
            using var pin = memory.Pin();

            Pin();
            try
            {
                Buffer.MemoryCopy(_handler.Pointer, pin.Pointer, _buffer.Length, _buffer.Length);
            }
            finally 
            {
                UnPin();
            }

            return arr;
        }

        public unsafe void Dispose()
        {
            if (_handler.Pointer != null)
            {
                _handler.Dispose();
            }
            _buffer = Memory<byte>.Empty;
        }

        //public static unsafe void Copy(byte* src, byte* dest, int count)
        //{
        //    Buffer.MemoryCopy(src, dest, count, count);
        //}

        public static unsafe void Copy(void* src, void* dest, int count)
        {
            Buffer.MemoryCopy(src, dest, count, count);
        }

        public string ToBase64()
        {
            return Convert.ToBase64String(_buffer.Span, Base64FormattingOptions.None);
        }
    }
}
