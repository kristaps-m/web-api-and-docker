using System.Collections;

namespace ParseTools.returnFormats
{
    public interface IFormatter
    {
        string Format<T>(T environment_vars);
    }
}
