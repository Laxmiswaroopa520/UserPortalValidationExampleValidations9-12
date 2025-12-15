//for roles 
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IRoleRepository
    {
        List<Role> GetAllRoles();
    }
}
