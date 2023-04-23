using Microsoft.AspNetCore.Http;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ParseTools.returnFormats
{
    public class HtmlFormatter : IFormatter
    {
        public string Format<T>(T theDictionary)
        {
            var _jsonFileFormatTools = new JsonFileFormatTools();

            if (theDictionary is IDictionary dictionary)
            {
                return _jsonFileFormatTools.CreateBigHtmlStingFile(dictionary);
            }
            else if (theDictionary is IHeaderDictionary request)
            {
                var headerDictionary = request.ToDictionary(h => h.Key, h => h.Value.ToString());

                return _jsonFileFormatTools.CreateBigHtmlStingFile(headerDictionary);
            }
            else
            {
                return "Something went wrong";
            }
        }
    }
}
