using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IAddressRepository : IGenericRepository<Address>
    {
        IEnumerable<Address> GetByUserId(int userId);
    }
}
