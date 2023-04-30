using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ParseTools.Interfaces;

namespace ParseTools.returnFormats
{
    public class JsonFormatter : ControllerBase,IFormatter
    {
        public IActionResult Format<T>(T theDictionary)
        {
            var jsonString = JsonConvert.SerializeObject(theDictionary);
            return Content(jsonString, "application/json");
        }
    }
}
