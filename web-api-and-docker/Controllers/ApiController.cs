using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
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
        private readonly IFormatFactory _formatFactory;

        public ApiController(IFormatFactory formatFactory)
        {
            _formatFactory = formatFactory;
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
            var formatter = _formatFactory.GetFormatter(format);
            var formattedOutputAsIactionResult = formatter.Format(environment_vars);

            return formattedOutputAsIactionResult;
        }

        [Route("headers")]
        [HttpGet]
        public IActionResult GetHeaders(string? format = "html")
        {
            var headers = HttpContext.Request.Headers;
            var formatter = _formatFactory.GetFormatter(format);
            var formattedOutputAsIactionResult = formatter.Format(headers);

            return formattedOutputAsIactionResult;
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

            var formatter = _formatFactory.GetFormatter(format);
            var bodyToIDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(body)
                    .ToDictionary(h => h.Key, h => h.Value.ToString());
            var formattedOutputAsIactionResult = formatter.Format(bodyToIDictionary);

            return formattedOutputAsIactionResult;
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
    }
}
