using Cybtans.Graphics.Importers;
using System;
using Xunit;

namespace Cybtans.Graphics.Tests
{
    public class ImportTests
    {
        Scene _scene;
        public ImportTests()
        {
            _scene = new Scene();
        }

        [Fact]
        public void ImportScene1()
        {
            ContentImporter.Import(_scene, "Assets/sample1/sample1.dae");

            Assert.True(_scene.Root.Childrens.Count > 0);

            var visualRoot = _scene.Root.Childrens[0];

            Assert.Equal(3, visualRoot.Childrens.Count);
            Assert.Equal(1, _scene.Meshes.Count);
            Assert.Equal(1, _scene.Cameras.Count);
            Assert.Equal(1, _scene.Lights.Count);
            Assert.Equal(2, _scene.Materials.Count);
            Assert.NotEmpty(_scene.Textures);
        }

        [Fact]
        public void ImportScene2()
        {
            ContentImporter.Import(_scene, "Assets/sample2/sample2.dae");

            Assert.True(_scene.Root.Childrens.Count > 0);

            var visualRoot = _scene.Root.Childrens[0];

            Assert.Equal(3, visualRoot.Childrens.Count);
            Assert.Equal(1, _scene.Meshes.Count);
            Assert.Equal(1, _scene.Cameras.Count);
            Assert.Equal(1, _scene.Lights.Count);
            Assert.Equal(2, _scene.Materials.Count);
            Assert.NotEmpty(_scene.Textures);
        }
    }
}
