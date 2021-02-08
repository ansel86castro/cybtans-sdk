using Cybtans.Graphics.Models;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.IO;
using Cybtans.Graphics.Importers;
using Cybtans.Graphics;
using Cybtans.Graphics.Shading;
using Microsoft.AspNetCore.StaticFiles;

namespace Cybtans.Tests.Gateway.Controllers
{
    [Route("api/scene")]
    [ApiController]
    public class SceneController: Controller
    {
        public SceneController()
        {

        }

        [HttpGet("{name}")]        
        public async Task<ActionResult<SceneDto>> Get(string name ="sample1")
        {
            var file = $"Assets/{name}/{name}.dae";
            if (!System.IO.File.Exists(file))
                return NotFound();

            var dto = await Task.Run(() =>
            {
                Scene scene = new Scene(name);
                ContentImporter.Import(scene, file);
                return scene.ToDto();
            });

            return dto;
        }

        [HttpGet("programs")]
        public async Task<ActionResult<EffectsManagerDto>> GetPrograms()
        {
            EffectsManager effectManager = new EffectsManager();
            await effectManager.LoadEffectsFromDirectory("Assets/Effects");

            return effectManager.ToDto();
        }

        [HttpGet("texture/{scene}/{name}")]
        public ActionResult GetTexture(string scene, string name)
        {
            var file = $"Assets/{scene}/{name}";
            if (!System.IO.File.Exists(file))
                return NotFound();

            var provider = new FileExtensionContentTypeProvider();
            string contentType;
            if (!provider.TryGetContentType(file, out contentType))
            {
                contentType = "application/octet-stream";
            }

            var fi = new FileInfo(file);
            return File(fi.OpenRead(), contentType);
        }

    }
}
