using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class AddressRepository : IAddressRepository
    {
        private readonly ApplicationDbContext _db;

        public AddressRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public List<Address> GetByUserId(int userId)
        {
            return _db.Addresses
                      .Where(a => a.UserId == userId)
                      .ToList();
        }

        public Address? GetById(int id)
        {
            return _db.Addresses.Find(id);
        }

        public void Add(Address address)
        {
            _db.Addresses.Add(address);
        }

        public void Update(Address address)
        {
            _db.Addresses.Update(address);
        }

        public void Delete(Address address)
        {
            _db.Addresses.Remove(address);
        }
    }
}
