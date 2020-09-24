using APIAuthentication.Entities;
using System.Threading.Tasks;

namespace APIAuthentication.Services
{
    public interface IApiAuthenicationService
    {
        public Task<User> Authenticate(string username, string password);
    }
}
