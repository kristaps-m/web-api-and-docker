using web_api_and_docker.Data;
using Microsoft.AspNetCore.Authentication;
using Microsoft.Extensions.Options;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using System.Text.Encodings.Web;
using ParseTools;

namespace web_api_and_docker.Handler
{
    public class BasicAuthenticationHandler : AuthenticationHandler<AuthenticationSchemeOptions>
    {
        private readonly ICredentialsStorage _credentialsStorage;
        private readonly IStringDecoding _stringDecoding;

        public BasicAuthenticationHandler(IOptionsMonitor<AuthenticationSchemeOptions> options,
            ILoggerFactory logger, UrlEncoder encoder, ISystemClock clock,
            ICredentialsStorage credentialsStorage, IStringDecoding stringDecoding) :
            base(options, logger, encoder, clock)
        {
            _credentialsStorage = credentialsStorage;
            _stringDecoding = stringDecoding;
        }

        protected async override Task<AuthenticateResult> HandleAuthenticateAsync()
        {
            if (!Request.Headers.ContainsKey("Authorization"))
            {
                return AuthenticateResult.Fail("No header found");
            }

            var headerValue = AuthenticationHeaderValue.Parse(Request.Headers["Authorization"]);
            var bytes = Convert.FromBase64String(headerValue.Parameter);
            string credentials = Encoding.UTF8.GetString(bytes);
            if (!string.IsNullOrEmpty(credentials))
            {
                string[] array = credentials.Split(':');
                string username = array[0];
                string password = array[1];
                var user = _credentialsStorage.GetAdmins()
                    .FirstOrDefault(i => i[0] == username && _stringDecoding.FromB64ToString(i[1]) == password);

                if (user == null)
                {
                    return AuthenticateResult.Fail("UnAuthorized");
                }

                // Generate Ticket
                var claim = new[] { new Claim(ClaimTypes.Name, username) };
                var identity = new ClaimsIdentity(claim, Scheme.Name);
                var principal = new ClaimsPrincipal(identity);
                var ticket = new AuthenticationTicket(principal, Scheme.Name);
                return AuthenticateResult.Success(ticket);
            }
            else
            {
                return AuthenticateResult.Fail("UnAuthorized");
            }
        }
    }
}
