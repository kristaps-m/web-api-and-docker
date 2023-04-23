using Microsoft.AspNetCore.Http;
using Newtonsoft.Json;
using System.Collections;
using System.Xml;
using System.Xml.Linq;

namespace ParseTools.returnFormats
{
    public class XmlFormatter : IFormatter
    {
        public string Format<T>(T theDictionary)
        {
            if (theDictionary is IDictionary dictionary)
            {
                var _jsonFileFormatTools = new JsonFileFormatTools();
                string bigJsonString = _jsonFileFormatTools.SimpleJsonToString(dictionary).Replace("\\", "\\\\");
                XmlDocument xmlDocument = JsonConvert.DeserializeXmlNode(bigJsonString, "root");

                return xmlDocument.OuterXml;
            }
            else if (theDictionary is IHeaderDictionary headers)
            {
                var xml = new XElement("headers", headers.Select(h => new XElement(h.Key.Replace(':', '_'), h.Value)));

                return xml.ToString();
            }
            else
            {    
                return "Something went wrong!";
            }
        }
    }
}
