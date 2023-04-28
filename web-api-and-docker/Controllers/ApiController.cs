using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using ParseTools;
using ParseTools.returnFormats;
using System.Collections;
using System.Net;
using System.Net.Http.Headers;
using System.Text;
using System.Reflection.Metadata;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
//using Document = System.Reflection.Metadata.Document;

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

        // testing returning env_vars with also PDF !
        [HttpGet("generatepdf")]
        public IActionResult GeneratePDF(string format, string test)
        {
            IDictionary environment_vars = Environment.GetEnvironmentVariables();
            var formatter = FormatFactory.GetFormatter(format);
            var formattedOutput = formatter.Format(environment_vars);

            return Content(formattedOutput, GetContentType(format));
            
            var document = new PdfDocument();
            string htmlContent = "<h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1>";
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + test + ".pdf";
            
            return File(response, "application/pdf", Filename);
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
