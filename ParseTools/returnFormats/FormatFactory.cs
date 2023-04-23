using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml;

namespace ParseTools.returnFormats
{
    public class FormatFactory
    {
        public static IFormatter GetFormatter(string format)
        {
            switch (format.Trim().ToLower())
            {
                case "xml":
                    return new XmlFormatter();
                case "html":
                    return new HtmlFormatter();
                case "json":
                    return new JsonFormatter();
                default:
                    return new HtmlFormatter();
            }
        }
    }
}
