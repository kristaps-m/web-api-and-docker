using System.Collections;

namespace ParseTools
{
    public interface IJsonFileFormatTools
    {
        string CreateBigHtmlStingFile(IDictionary json);
        string SimpleJsonToString(IDictionary dictionary);
    }
}