using ParseTools.Interfaces;

namespace ParseTools.returnFormats
{
    public class FormatFactory: IFormatFactory
    {
        //public static IFormatter GetFormatter(string format)
        //{
        //    switch (format.Trim().ToLower())
        //    {
        //        case "xml":
        //            return new XmlFormatter();
        //        case "html":
        //            return new HtmlFormatter();
        //        case "json":
        //            return new JsonFormatter();
        //        case "pdf":
        //            return new PdfFormatter();
        //        default:
        //            return new HtmlFormatter();
        //    }
        //}

        public IFormatter GetFormatter(string format)
        {
            switch (format.Trim().ToLower())
            {
                case "xml":
                    return new XmlFormatter();
                case "html":
                    return new HtmlFormatter();
                case "json":
                    return new JsonFormatter();
                case "pdf":
                    return new PdfFormatter();
                default:
                    return new HtmlFormatter();
            }
        }
    }
}
