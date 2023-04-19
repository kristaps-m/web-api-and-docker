namespace web_api_and_docker.Data
{
    public class CredentialsStorage : ICredentialsStorage
    {
        private List<List<string>> Admins = new() { 
            // pw = BOOK2000store
            new() { "kristapsmitins", "Qk9PSzIwMDBzdG9yZQ==" },
            // pw = demo
            new() { "demo", "ZGVtbw==" }
        };

        public List<List<string>> GetAdmins()
        {
            return Admins;
        }
    }
}
