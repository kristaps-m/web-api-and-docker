using Microsoft.AspNetCore.Mvc;

namespace ParseTools.Interfaces
{
    public interface IFormatter
    {
        IActionResult Format<T>(T data);
    }
}
