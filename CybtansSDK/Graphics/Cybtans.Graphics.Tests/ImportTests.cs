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
        public void ImportCollada()
        {
            ContentImporter.Import(_scene, "Assets/sample1/sample1.dae");

            Assert.True(_scene.Root.Childrens.Count > 0);

            var visualRoot = _scene.Root.Childrens[0];

            Assert.Equal(4, visualRoot.Childrens.Count);
            Assert.Equal(2, _scene.Meshes.Count);
            Assert.Equal(1, _scene.Cameras.Count);
            Assert.Equal(1, _scene.Lights.Count);
            Assert.Equal(2, _scene.Materials.Count);
        }
    }
}
