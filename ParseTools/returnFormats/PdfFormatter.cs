using PdfSharpCore;
using PdfSharpCore.Pdf;
using TheArtOfDev.HtmlRenderer.PdfSharp;

namespace ParseTools.returnFormats
{
    public class PdfFormatter : IFormatter
    {
        public string Format<T>(T theDictionary)
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
            string Filename = "Invoice_" + test + ".pdf";

            return File(response, "application/pdf", Filename);
        }
    }
}
