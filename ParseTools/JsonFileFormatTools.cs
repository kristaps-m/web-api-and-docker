using System.Collections;

namespace ParseTools
{
    public class JsonFileFormatTools : IJsonFileFormatTools
    {
        private string htmlTableFirstPart = @"<!DOCTYPE html>
            <html>
            <head>
            <style>
            table {
              font-family: arial, sans-serif;
              border-collapse: collapse;
              width: 100%;
            }
            td, th {
              border: 1px solid #dddddd;
              text-align: left;
              padding: 8px;
            }
            tr:nth-child(even) {
              background-color: #dddddd;
            }
            </style>
            </head>
            <body>
            <h2>HTML Table!</h2>
            <table>
              <tr>
                <th>Key</th>
                <th>Value</th>
              </tr>
            ";
        private string htmlTableSEndPart = @"</table></body></html>";

        public string CreateBigHtmlStingFile(IDictionary json)
        {
            var midleHtmlString = "";

            foreach (DictionaryEntry data in json)
            {
                midleHtmlString += $"<tr>" +
                    $"<td>{data.Key}</td>" +
                    $"<td>{data.Value}</td>" +
                    $"</tr>";
            }

            return htmlTableFirstPart + midleHtmlString + htmlTableSEndPart;
        }

        public string SimpleJsonToString(IDictionary dictionary)
        {
            if (dictionary == null)
                throw new ArgumentNullException("dictionary");

            var result = new List<string>();

            foreach (DictionaryEntry data in dictionary)
            {
                result.Add($"\"{data.Key}\"" + ":" + $"\"{data.Value}\"");
            }

            return "{" + string.Join(",", result) + "}";
        }
    }
}
