using Microsoft.AspNetCore.Mvc;
using System.Collections;

namespace web_api_and_docker.Controllers
{
    [Route("api")]
    [ApiController]
    public class ApiController : ControllerBase
    {
        [Route("/")]
        [HttpGet]
        public IActionResult GetRootPath()
        {
            return Ok("Hello world!");
        }
        
        [Route("environment")]
        [HttpGet]
        public IActionResult GetEnvironmentVariables()
        {           
            IDictionary environment_vars = Environment.GetEnvironmentVariables();

            var variablesList = new List<KeyValuePair<object, object>>();

            foreach (DictionaryEntry variable in environment_vars)
            {
                variablesList.Add(new KeyValuePair<object, object>(variable.Key, variable.Value));
            }

            return Ok(variablesList);
        }
    }
}
