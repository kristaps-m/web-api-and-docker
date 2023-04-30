using System.Collections;

namespace ParseTools.Interfaces
{
    public interface IJsonFileFormatTools
    {
        string CreateBigHtmlStingFile(IDictionary json);
        string SimpleJsonToString(IDictionary dictionary);
    }
}