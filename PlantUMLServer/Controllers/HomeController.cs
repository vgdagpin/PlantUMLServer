using Microsoft.AspNetCore.Mvc;
using PlantUml.Net;
using System.Net;
using System.Text;
using System.Text.Json;

namespace PlantUMLServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class HomeController : ControllerBase
    {
        private readonly IPlantUmlRenderer plantUmlRenderer;

        public HomeController(IPlantUmlRenderer plantUmlRenderer)
        {
            this.plantUmlRenderer = plantUmlRenderer;
        }

        [HttpGet]
        [Route("/")]
        public IActionResult Index()
        {
            return new ContentResult()
            {
                Content = JsonSerializer.Serialize(new[]
                {
                    new
                    {
                        Type = "Get",
                        Url = "/Render/{base64data}",
                        Body = (string?)null
                    },
                    new
                    {
                        Type = "Post",
                        Url = "/RenderFromBase64",
                        Body = (string?)"base64 data"
                    },
                    new
                    {
                        Type = "Post",
                        Url = "/RenderFromPlain",
                        Body = (string?)"uml data"
                    }
                }),
                ContentType = "application/json",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
        
        [HttpGet]
        [Route("/Render/{base64data}")]
        public async Task<IActionResult> Render(string base64data)
        {
            var bData = Convert.FromBase64String(base64data);
            var strData = Encoding.UTF8.GetString(bData);

            var result = await plantUmlRenderer.RenderAsync(strData, OutputFormat.Svg);

            return new ContentResult()
            {
                Content = Encoding.UTF8.GetString(result),
                ContentType = "image/svg+xml",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPost]
        [Route("/RenderFromBase64")]
        public async Task<IActionResult> RenderFromBase64()
        {
            var stream = new StreamReader(Request.Body);
            var reqBody = await stream.ReadToEndAsync();

            var bData = Convert.FromBase64String(reqBody);
            var strData = Encoding.UTF8.GetString(bData);

            var result = await plantUmlRenderer.RenderAsync(strData, OutputFormat.Svg);

            return new ContentResult()
            {
                Content = Encoding.UTF8.GetString(result),
                ContentType = "text/xml",
                StatusCode = (int)HttpStatusCode.OK
            };
        }

        [HttpPost]
        [Route("/RenderFromPlain")]
        public async Task<IActionResult> RenderFromPlain()
        {
            var stream = new StreamReader(Request.Body);
            var reqBody = await stream.ReadToEndAsync();

            var result = await plantUmlRenderer.RenderAsync(reqBody, OutputFormat.Svg);

            return new ContentResult()
            {
                Content = Encoding.UTF8.GetString(result),
                ContentType = "text/xml",
                StatusCode = (int)HttpStatusCode.OK
            };
        }
    }
}
