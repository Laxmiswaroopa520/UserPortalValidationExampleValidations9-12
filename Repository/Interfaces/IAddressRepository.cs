using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IAddressRepository
    {
        List<Address> GetByUserId(int userId);
        Address? GetById(int id);
        void Add(Address address);
        void Update(Address address);
        void Delete(Address address);
    }
}
