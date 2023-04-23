using Newtonsoft.Json;

namespace ParseTools.returnFormats
{
    public class JsonFormatter : IFormatter
    {
        public string Format<T>(T environment_vars)
        {
            return JsonConvert.SerializeObject(environment_vars);
        }
    }
}
