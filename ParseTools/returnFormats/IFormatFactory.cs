namespace ParseTools.returnFormats
{
    public interface IFormatFactory
    {
        IFormatter GetFormatter(string format);
    }
}
