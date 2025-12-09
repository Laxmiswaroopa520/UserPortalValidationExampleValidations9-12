//handles user CRUD Operations
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
    }

}
