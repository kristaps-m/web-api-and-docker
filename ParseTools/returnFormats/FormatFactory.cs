using ParseTools.Interfaces;

namespace ParseTools.ReturnFormats
{
    public class FormatFactory: IFormatFactory
    {
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
