using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParseTools;
using ParseTools.returnFormats;
using System.Collections;
using System.Net;
using System.Text;

namespace web_api_and_docker.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        private readonly IJsonFileFormatTools _jsonFileFormatTools;

        public ApiController(IJsonFileFormatTools jsonFileFormatTools)
        {
            _jsonFileFormatTools = jsonFileFormatTools;
        }

        [Route("/")]
        [HttpGet]
        public IActionResult GetRootPath()
        {
            return Ok("Hello world!");
        }
        
        [Route("environment")]
        [HttpGet]
        public IActionResult GetEnvironmentVariables(string? format = "html")
        {
            IDictionary environment_vars = Environment.GetEnvironmentVariables();
            var formatter = FormatFactory.GetFormatter(format);
            var formattedOutput = formatter.Format(environment_vars);

            return Content(formattedOutput, GetContentType(format));
        }

        [Route("headers")]
        [HttpGet]
        public IActionResult GetHeaders(string? format = "html")
        {
            var headers = HttpContext.Request.Headers;
            var formatter = FormatFactory.GetFormatter(format);
            var formattedOutput = formatter.Format(headers);

            return Content(formattedOutput, GetContentType(format));
        }

        [Authorize]
        [Route("post")]
        [HttpPost]
        public async Task<IActionResult> Post(string? format = "html")
        {
            string body = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return BadRequest("Request body is empty or null");
            }

            var formatter = FormatFactory.GetFormatter(format);
            var bodyToIDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(body)
                    .ToDictionary(h => h.Key, h => h.Value.ToString());
            var formattedOutput = formatter.Format(bodyToIDictionary);

            return Content(formattedOutput, GetContentType(format));
        }

        [Authorize]
        [Route("post")]
        [HttpDelete]
        [HttpGet]
        [HttpHead]
        [HttpOptions]
        [HttpPatch]
        [HttpPut]
        public IActionResult NotAllowed()
        {
            return StatusCode((int)HttpStatusCode.MethodNotAllowed, "Method not allowed");
        }

        private string GetContentType(string format)
        {
            switch (format.Trim().ToLower())
            {
                case "xml":
                    return "application/xml";
                case "html":
                    return "text/html";
                case "json":
                    return "application/json";
                default:
                    return "text/html";
            }
        }
    }
}
