using Microsoft.AspNetCore.Mvc;

namespace ParseTools.returnFormats
{
    public interface IFormatter
    {
        IActionResult Format<T>(T data);
    }
}
