using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ParseTools.Interfaces;
using System.Collections;

namespace ParseTools.ReturnFormats
{
    public class HtmlFormatter : ControllerBase, IFormatter
    {
        public IActionResult Format<T>(T theDictionary)
        {
            var _jsonFileFormatTools = new JsonFileFormatTools();

            if (theDictionary is IDictionary dictionary)
            {
                var htmlString = _jsonFileFormatTools.CreateBigHtmlStingFile(dictionary);

                return Content(htmlString, "text/html");
            }
            else if (theDictionary is IHeaderDictionary request)
            {
                var headerDictionary = request.ToDictionary(h => h.Key, h => h.Value.ToString());
                var htmlString = _jsonFileFormatTools.CreateBigHtmlStingFile(headerDictionary);

                return Content(htmlString, "text/html");
            }
            else
            {
                return BadRequest("Something went wrong!");
            }
        }
    }
}
