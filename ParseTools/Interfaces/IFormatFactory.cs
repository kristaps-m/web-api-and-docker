namespace ParseTools.Interfaces
{
    public interface IFormatFactory
    {
        IFormatter GetFormatter(string format);
    }
}
