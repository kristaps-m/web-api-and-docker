namespace web_api_and_docker.Data
{
    public interface ICredentialsStorage
    {
        List<List<string>> GetAdmins();
    }
}