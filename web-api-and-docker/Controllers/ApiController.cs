using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParseTools;
using System.Collections;
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
            var headers = HttpContext.Request.Headers;
            var headerList = new List<KeyValuePair<string, string>>();

            foreach (var header in headers)
            {
                headerList.Add(new KeyValuePair<string, string>(header.Key, header.Value));
            }

            return Ok(headerList);
        }
    }
}
