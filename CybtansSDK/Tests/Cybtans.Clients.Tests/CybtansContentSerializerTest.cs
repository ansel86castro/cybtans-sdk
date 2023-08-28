using Cybtans.Common;
using System.ComponentModel.DataAnnotations;

namespace Cybtans.Clients.Tests
{
    public class CybtansContentSerializerTest
    {
        public class Model
        {
            public int Id { get; set; }

            public string Name { get; set; }

            public string Description { get; set; }            
            public DateTime Time { get; set; }
        }

        [Fact]
        public void ToHttpContent()
        {
            var model = new Model
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                Time = DateTime.UtcNow
            };
            var serializer  = new CybtansContentSerializer();
            using var memory = serializer.ToMemory(model);
            var content = new ReadOnlyMemoryContent(memory.Memory);
            Assert.NotNull(content);
        }

        [Fact]
        public async Task FromStreamAsync()
        {
            var model = new Model
            {
                Id = 1,
                Name = "Name",
                Description = "Description",
                Time = DateTime.UtcNow
            };
            var serializer = new CybtansContentSerializer();
            using var memory = serializer.ToMemory(model);
            var content = new ReadOnlyMemoryContent(memory.Memory);
            var stream = await content.ReadAsStreamAsync();

            var obj = await serializer.FromStreamAsync<Model>(stream);
            Assert.NotNull(obj);
            Assert.Equal(model.Id, obj.Id);
            Assert.Equal(model.Name, obj.Name);
            Assert.Equal(model.Description, obj.Description);
            Assert.Equal(model.Time, obj.Time);
        }
    }
}