using ParseTools.Interfaces;
using System.Text;

namespace ParseTools
{
    public class StringDecoding : IStringDecoding
    {
        public string FromB64ToString(string encodedString)
        {
            byte[] data = Convert.FromBase64String(encodedString);
            string decodedString = Encoding.UTF8.GetString(data);

            return decodedString;
        }
    }
}
