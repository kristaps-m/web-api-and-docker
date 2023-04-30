using Microsoft.AspNetCore.Mvc;
using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;
using System.Collections;
using Microsoft.AspNetCore.Http;
using ParseTools.Interfaces;

namespace ParseTools.returnFormats
{
    public class PdfFormatter : ControllerBase, IFormatter
    {
        public IActionResult Format<T>(T theDictionary)
        {
            var _jsonFileFormatTools = new JsonFileFormatTools();
            var document = new PdfDocument();
            string Filename = "Kristaps " + $"{DateTime.Now.ToString("dddd, dd MMMM yyyy HH-mm")}" + ".pdf";

            if (theDictionary is IDictionary dictionary)
            {
                var htmlContent = _jsonFileFormatTools.CreateBigHtmlStingFile(dictionary);
                PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
                byte[]? response = null;
                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }

                return File(response, "application/pdf", Filename);
            }
            else if (theDictionary is IHeaderDictionary request)
            {
                var headerDictionary = request.ToDictionary(h => h.Key, h => h.Value.ToString());
                var htmlContent = _jsonFileFormatTools.CreateBigHtmlStingFile(headerDictionary);
                PdfGenerator.AddPdfPages(document, htmlContent, PageSize.A4);
                byte[]? response = null;

                using (MemoryStream ms = new MemoryStream())
                {
                    document.Save(ms);
                    response = ms.ToArray();
                }
                return File(response, "application/pdf", Filename);
            }
            else
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
