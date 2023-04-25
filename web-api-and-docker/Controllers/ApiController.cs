using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParseTools;
using ParseTools.returnFormats;
using System.Collections;
using System.Net;
using System.Text;
using iTextSharp.text;
using iTextSharp.text.pdf;
using Newtonsoft.Json;
using System.Reflection.Metadata;
using Document = System.Reflection.Metadata.Document;
using Newtonsoft.Json.Linq;

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

        //[Route("test")]
        //[HttpGet]
        //public ActionResult ConvertJsonToPdf()
        //{
        //    string json = "{ 'name': 'John Doe', 'age': 30, 'city': 'New York' }";
        //    dynamic data = JsonConvert.DeserializeObject(json);

        //    MemoryStream workStream = new MemoryStream();
        //    Document document = new Document();
        //    //PdfWriter.GetInstance(document, Response.OutputStream);
        //    PdfWriter.GetInstance(document, workStream).CloseStream = false;
        //    document.Open();

        //    PdfPTable table = new PdfPTable(2);
        //    table.AddCell("Field");
        //    table.AddCell("Value");

        //    foreach (var property in data)
        //    {
        //        table.AddCell(property.Name);
        //        table.AddCell(property.Value.ToString());
        //    }

        //    document.Add(table);
        //    document.Close();

        //    Response.ContentType = "application/pdf";
        //    Response.AddHeader("content-disposition", "attachment;filename=converted.pdf");
        //    Response.Cache.SetCacheability(HttpCacheability.NoCache);
        //    Response.Write(document);
        //    Response.End();

        //    return new EmptyResult();
        //}

        [Route("test")]
        [HttpPost]
        public async Task<HttpResponseMessage> ConvertJsonToPdf(JObject data)
        {
            // Deserialize the JSON data into a dynamic object
            dynamic jsonData = data;

            // Create a new MemoryStream to hold the PDF data
            MemoryStream stream = new MemoryStream();

            // Create a new PdfWriter to write the PDF to the MemoryStream
            PdfWriter writer = new PdfWriter(stream);

            // Create a new PdfDocument using the PdfWriter
            PdfDocument pdf = new PdfDocument(writer);

            // Create a new document to hold the PDF content
            Document document = new Document(pdf);

            // Loop through the properties of the JSON data and add them to the PDF
            foreach (var property in jsonData)
            {
                Paragraph paragraph = new Paragraph($"{property.Key}: {property.Value}");
                document.Add(paragraph);
            }

            // Close the document
            document.Close();

            // Create a new HttpResponseMessage to hold the PDF data
            HttpResponseMessage response = new HttpResponseMessage();
            response.Content = new ByteArrayContent(stream.ToArray());

            // Set the content type and content disposition headers
            response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
            response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
            {
                FileName = "output.pdf"
            };

            // Return the HttpResponseMessage
            return response;
        }
        //[HttpPost]
        //public async Task<HttpResponseMessage> ConvertJsonToPdf(JObject data)
        //{
        //    // Deserialize the JSON data into a dynamic object
        //    dynamic jsonData = data;

        //    // Create a new MemoryStream to hold the PDF data
        //    MemoryStream stream = new MemoryStream();

        //    // Create a new PDF document
        //    PdfDocument pdf = new PdfDocument(new PdfWriter(stream));

        //    // Create a new document to hold the PDF content
        //    Document document = new Document(pdf);

        //    // Loop through the properties of the JSON data and add them to the PDF
        //    foreach (var property in jsonData)
        //    {
        //        Paragraph paragraph = new Paragraph($"{property.Key}: {property.Value}");
        //        document.Add(paragraph);
        //    }

        //    // Close the document
        //    document.Close();

        //    // Create a new HttpResponseMessage to hold the PDF data
        //    HttpResponseMessage response = new HttpResponseMessage();
        //    response.Content = new ByteArrayContent(stream.ToArray());

        //    // Set the content type and content disposition headers
        //    response.Content.Headers.ContentType = new MediaTypeHeaderValue("application/pdf");
        //    response.Content.Headers.ContentDisposition = new ContentDispositionHeaderValue("attachment")
        //    {
        //        FileName = "output.pdf"
        //    };

        //    // Return the HttpResponseMessage
        //    return response;
        //}

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
