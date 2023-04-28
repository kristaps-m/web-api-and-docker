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

        // return pdf :)
        [HttpGet("generatepdf")]
        public async Task<IActionResult> GeneratePDF(string InvoiceNo)
        {
            var document = new PdfDocument();
            string htmlContent = "<h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1><h1>WARAAAAAAAAAAAAAAAP</h1>";
            PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
            byte[]? response = null;
            using (MemoryStream ms = new MemoryStream())
            {
                document.Save(ms);
                response = ms.ToArray();
            }
            string Filename = "Invoice_" + InvoiceNo + ".pdf";
            //string imgeurl = "data:image/png;base64, " + Getbase64string() + "";

            //string[] copies = { "Customer copy", "Comapny Copy" };
            //for (int i = 0; i < copies.Length; i++)
            //{
            //    InvoiceHeader header = await this._container.GetAllInvoiceHeaderbyCode(InvoiceNo);
            //    List<InvoiceDetail> detail = await this._container.GetAllInvoiceDetailbyCode(InvoiceNo);
            //    string htmlcontent = "<div style='width:100%; text-align:center'>";
            //    htmlcontent += "<img style='width:80px;height:80%' src='" + imgeurl + "'   />";
            //    htmlcontent += "<h2>" + copies[i] + "</h2>";
            //    htmlcontent += "<h2>Welcome to Nihira Techiees</h2>";



            //    if (header != null)
            //    {
            //        htmlcontent += "<h2> Invoice No:" + header.InvoiceNo + " & Invoice Date:" + header.InvoiceDate + "</h2>";
            //        htmlcontent += "<h3> Customer : " + header.CustomerName + "</h3>";
            //        htmlcontent += "<p>" + header.DeliveryAddress + "</p>";
            //        htmlcontent += "<h3> Contact : 9898989898 & Email :ts@in.com </h3>";
            //        htmlcontent += "<div>";
            //    }



            //    htmlcontent += "<table style ='width:100%; border: 1px solid #000'>";
            //    htmlcontent += "<thead style='font-weight:bold'>";
            //    htmlcontent += "<tr>";
            //    htmlcontent += "<td style='border:1px solid #000'> Product Code </td>";
            //    htmlcontent += "<td style='border:1px solid #000'> Description </td>";
            //    htmlcontent += "<td style='border:1px solid #000'>Qty</td>";
            //    htmlcontent += "<td style='border:1px solid #000'>Price</td >";
            //    htmlcontent += "<td style='border:1px solid #000'>Total</td>";
            //    htmlcontent += "</tr>";
            //    htmlcontent += "</thead >";

            //    htmlcontent += "<tbody>";
            //    if (detail != null && detail.Count > 0)
            //    {
            //        detail.ForEach(item =>
            //        {
            //            htmlcontent += "<tr>";
            //            htmlcontent += "<td>" + item.ProductCode + "</td>";
            //            htmlcontent += "<td>" + item.ProductName + "</td>";
            //            htmlcontent += "<td>" + item.Qty + "</td >";
            //            htmlcontent += "<td>" + item.SalesPrice + "</td>";
            //            htmlcontent += "<td> " + item.Total + "</td >";
            //            htmlcontent += "</tr>";
            //        });
            //    }
            //    htmlcontent += "</tbody>";

            //    htmlcontent += "</table>";
            //    htmlcontent += "</div>";

            //    htmlcontent += "<div style='text-align:right'>";
            //    htmlcontent += "<h1> Summary Info </h1>";
            //    htmlcontent += "<table style='border:1px solid #000;float:right' >";
            //    htmlcontent += "<tr>";
            //    htmlcontent += "<td style='border:1px solid #000'> Summary Total </td>";
            //    htmlcontent += "<td style='border:1px solid #000'> Summary Tax </td>";
            //    htmlcontent += "<td style='border:1px solid #000'> Summary NetTotal </td>";
            //    htmlcontent += "</tr>";
            //    if (header != null)
            //    {
            //        htmlcontent += "<tr>";
            //        htmlcontent += "<td style='border: 1px solid #000'> " + header.Total + " </td>";
            //        htmlcontent += "<td style='border: 1px solid #000'>" + header.Tax + "</td>";
            //        htmlcontent += "<td style='border: 1px solid #000'> " + header.NetTotal + "</td>";
            //        htmlcontent += "</tr>";
            //    }
            //    htmlcontent += "</table>";
            //    htmlcontent += "</div>";

            //    htmlcontent += "</div>";

            //    PdfGenerator.AddPdfPages(document, htmlcontent, PageSize.A4);
            //}
            //byte[]? response = null;
            //using (MemoryStream ms = new MemoryStream())
            //{
            //    document.Save(ms);
            //    response = ms.ToArray();
            //}
            //string Filename = "Invoice_" + InvoiceNo + ".pdf";
            return File(response, "application/pdf", Filename);
        }

        //[HttpPost("test")]
        //public async Task<HttpResponseMessage> ConvertJsonToPdf() // JObject data
        //{
        //    //string json = "{ 'name': 'John Doe', 'age': 30, 'city': 'New York' }";
        //    string json = "{  'version': {'major': 0,'minor': 0,'build': 0,'revision': 0,'majorRevision': 0,'minorRevision': 0  },  'content': {'headers': [{'key': 'string','value': [          'string'                    ]      }    ]  },  'statusCode': 100,  'reasonPhrase': 'string',  'headers': [{'key': 'string','value': ['string']    }  ],  'trailingHeaders': [    {                'key': 'string',      'value': [        'string'      ]    }  ],  'requestMessage': {'version': {'major': 0,      'minor': 0,      'build': 0,      'revision': 0,      'majorRevision': 0,      'minorRevision': 0},    'versionPolicy': 0,    'content': {'headers': [{'key': 'string','value': [            'string']        }      ]    },    'method': {                    'method': 'string'    },    'requestUri': 'string',    'headers': [      {                    'key': 'string',        'value': [          'string'        ]      }    ],    'options': {                    'additionalProp1': 'string',      'additionalProp2': 'string',      'additionalProp3': 'string'    }            },  'isSuccessStatusCode': true}";
        //    dynamic data = JsonConvert.DeserializeObject(json);

        //    // Deserialize the JSON data into a dynamic object
        //    dynamic jsonData = data;

        //    // Create a new MemoryStream to hold the PDF data
        //    MemoryStream stream = new MemoryStream();

        //    // Create a new PdfWriter to write the PDF to the MemoryStream
        //    PdfWriter writer = new PdfWriter(stream);

        //    // Create a new PdfDocument using the PdfWriter
        //    PdfDocument pdf = new PdfDocument(writer);

        //    // Create a new Document to hold the PDF content
        //    iText.Layout.Document document = new iText.Layout.Document(pdf);

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




        //[HttpPost]
        //public async Task<HttpResponseMessage> ConvertJsonToPdf(JObject data)
        //{
        //    // Deserialize the JSON data into a dynamic object
        //    dynamic jsonData = data;

        //    // Create a new MemoryStream to hold the PDF data
        //    MemoryStream stream = new MemoryStream();

        //    // Create a new PdfWriter to write the PDF to the MemoryStream
        //    PdfWriter writer = new PdfWriter(stream);

        //    // Create a new PdfDocument using the PdfWriter
        //    PdfDocument pdf = new PdfDocument(writer);

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
