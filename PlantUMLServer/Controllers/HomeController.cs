using Microsoft.AspNetCore.Mvc;
using PlantUml.Net;
using System.IO;
using System.Net;
using System.Reflection;
using System.Text;

namespace PlantUMLServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        [HttpGet]
        [Route("/")]
        public async Task<IActionResult> Index()
        {
            var vsixPath = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var path = Path.Combine(vsixPath, "PlantUML", "plantuml.jar");

            var r = new RendererFactory()
                .CreateRenderer(new PlantUmlSettings
                {
                    JavaPath = path
                });

            var d = @"@startuml
Bob -> Alice : hello
@enduml";

            var temp = await r.RenderAsync(d, OutputFormat.Svg);

            return new ContentResult()
            {
                Content = Encoding.UTF8.GetString(temp),
                ContentType = "image/svg+xml",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
