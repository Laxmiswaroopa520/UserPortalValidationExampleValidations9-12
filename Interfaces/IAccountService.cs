using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Interfaces
{
    public interface IAccountService
    {
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);
        void RegisterUser(User user);
        User? Login(string username, string password);
        void UpdateUser(User user);
        

    }
}
