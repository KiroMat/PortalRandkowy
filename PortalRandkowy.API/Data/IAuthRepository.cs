using System.Threading.Tasks;
using PortalRandkowy.API.Models;

namespace PortalRandkowy.API.Data
{
    public interface IAuthRepository
    {
         Task<User> Login(string userName, string passwort);
         Task<User> Register(User user, string password);
         Task<bool> UserExist(string userName);
    }
}