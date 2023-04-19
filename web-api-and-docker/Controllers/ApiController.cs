using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParseTools;
using System.Collections;
using System.Net;
using System.Text;
using System.Xml;
using System.Xml.Linq;

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
        public IActionResult GetEnvironmentVariables()
        {
            var format = Request.Query["format"].ToString().ToLower();
            IDictionary environment_vars = Environment.GetEnvironmentVariables();

            if (format == "json")
            {
                return Ok(environment_vars);
            }
            else if (format == "xml")
            {
                string bigJsonString = _jsonFileFormatTools.SimpleJsonToString(environment_vars).Replace("\\", "\\\\");
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(bigJsonString, "root");

                return Content(xmlDocument.OuterXml, "application/xml");
            }
            else
            {
                var html = _jsonFileFormatTools.CreateBigHtmlStingFile(environment_vars);

                return Content(html, "text/html");
            }
        }

        [Route("headers")]
        [HttpGet]
        public IActionResult GetHeaders()
        {
            var format = Request.Query["format"].ToString().ToLower();
            var headers = HttpContext.Request.Headers;

            if (format == "json")
            {
                return Ok(headers);
            }
            else if (format == "xml")
            {
                var xml = new XElement("headers", headers.Select(h => new XElement(h.Key.Replace(':', '_'), h.Value)));

                return Content(xml.ToString(), "application/xml");
            }
            else
            {
                var headerDictionary = headers.ToDictionary(h => h.Key, h => h.Value.ToString());
                var html = _jsonFileFormatTools.CreateBigHtmlStingFile(headerDictionary);

                return Content(html, "text/html");
            }
        }

        [Authorize]
        [Route("post")]
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            var format = Request.Query["format"].ToString().ToLower();
            string body = await new StreamReader(Request.Body, Encoding.UTF8).ReadToEndAsync();

            if (string.IsNullOrWhiteSpace(body))
            {
                return BadRequest("Request body is empty or null");
            }

            if (format == "json")
            {
                return Content(body, "application/json");
            }
            else if (format == "xml")
            {
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(body, "root");

                return Content(xmlDocument.OuterXml, "application/xml");
            }
            else
            {
                var bodyToIDictionary = JsonConvert.DeserializeObject<IDictionary<string, string>>(body)
                    .ToDictionary(h => h.Key, h => h.Value.ToString());

                var html = _jsonFileFormatTools.CreateBigHtmlStingFile(bodyToIDictionary);

                return Content(html, "text/html");
            }
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
