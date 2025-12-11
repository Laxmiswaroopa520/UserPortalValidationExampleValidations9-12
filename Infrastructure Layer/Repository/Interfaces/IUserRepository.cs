using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IUserRepository : IGenericRepository<User>
    {
        User? GetByUsername(string username);
        bool EmailExists(string email);
        bool UsernameExists(string username);
    }
}
