using Cybtans.Serialization;
using Cybtans.Services.Utils;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using Xunit;

namespace Cybtans.Tests.Serialization
{
    public class TestModel
    {
        public string Name { get; set; }

        public Stream Stream { get; set; }

       
    }

    public class SerializationStreamTest
    {
        public SerializationStreamTest()
        {

        }

        public static byte[] CreateRandomBuffer(int size)
        {
            Random rand = new Random();
            var buffer = new byte[size];
            rand.NextBytes(buffer);
            return buffer;
        }

        [Fact]
        public void SerializeStream()
        {
            const int size = 10 * 1024 * 1024;
            var dataBuffer = CreateRandomBuffer(size);
            var data = new TestModel
            {
                Name = "Test",
                Stream = new MemoryStream(dataBuffer)
            };

            var buffer = BinaryConvert.Serialize(data);
            Assert.NotNull(buffer);

            var result = BinaryConvert.Deserialize<TestModel>(buffer);
            Assert.Equal(result.Name, data.Name);
            Assert.NotNull(result.Stream);
            Assert.Equal(result.Stream.Length, size);

            var memory = result.Stream as MemoryStream;
            var memoryBuffer = memory.ToArray();

            for (int i = 0; i < size; i++)
            {
                Assert.Equal(dataBuffer[i], memoryBuffer[i]);
            }
            
        }
    }
}
