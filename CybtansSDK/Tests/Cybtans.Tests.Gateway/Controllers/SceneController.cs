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

namespace Cybtans.Tests.Gateway.Controllers
{
    [Route("api/scene")]
    [ApiController]
    public class SceneController: ControllerBase
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
        public async Task<ActionResult<ShaderProgramCollection>> GetPrograms()
        {
            ShaderProgramRepository repository = new ShaderProgramRepository();
            await repository.LoadFromDirectory("Assets/Shaders");

            return repository.ToDto();
        }
    }
}
