using APIAuthentication.Entities;
using System.Threading.Tasks;

namespace APIAuthentication.Services
{
    /// <summary>
    /// Designed to stub a production authentication service that would retrieve valid credentials from e.g. a database or CI/CI pipeline secrets.
    /// </summary>
    public class DummyAuthenticationService : IApiAuthenicationService
    {

        private readonly string _username = "testuser";
        private readonly string _password = "password1";

        public async Task<User> Authenticate(string username, string password)
        {
            if (string.IsNullOrEmpty(username) || string.IsNullOrEmpty(password))
            {
                return null;
            }

            if (username != this._username || password != this._password)
            {
                return null;
            }

            return new User()
            {
                Username = username,
                Password = password
            };
        }
    }
}
