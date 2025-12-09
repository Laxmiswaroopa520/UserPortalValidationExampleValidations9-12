using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services
{
    public class AddressService : IAddressService
    {
        private readonly ApplicationDbContext Db;

        public AddressService(ApplicationDbContext db)
        {
            Db = db;
        }

        // GET: List of addresses (VM)
        public List<AddressViewModel> GetAddressesByUserId(int userId)
        {
            return Db.Addresses
                .Where(a => a.UserId == userId)
                .Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Line1 = a.Line1,
                    City = a.City,
                    State = a.State,
                    Pin = a.Pin
                }).ToList();
        }

        // GET: Address by id (VM)
        public AddressViewModel GetAddressVMById(int id)
        {
            var addr = Db.Addresses.Find(id);

            if (addr == null) 
                return InvalidOperationException();

            return new AddressViewModel
            {
                Id = addr.Id,
                UserId = addr.UserId,
                Line1 = addr.Line1,
                City = addr.City,
                State = addr.State,
                Pin = addr.Pin
            };
        }

        private AddressViewModel InvalidOperationException()
        {
            throw new NotImplementedException();
        }

        // ADD new Address
        public void AddAddress(AddressViewModel vm)
        {
            var entity = new Address
            {
                UserId = vm.UserId,
                Line1 = vm.Line1,
                City = vm.City,
                State = vm.State,
                Pin = vm.Pin
            };

            Db.Addresses.Add(entity);
            Db.SaveChanges();
        }

        // UPDATE Address
        public void UpdateAddress(AddressViewModel vm)
        {
            var entity = Db.Addresses.Find(vm.Id);

            if (entity == null) return;

            entity.Line1 = vm.Line1;
            entity.City = vm.City;
            entity.State = vm.State;
            entity.Pin = vm.Pin;

            Db.SaveChanges();
        }

        // DELETE Address
        public void DeleteAddress(int id)
        {
            var addr = Db.Addresses.Find(id);
            if (addr == null) return;

            Db.Addresses.Remove(addr);
            Db.SaveChanges();
        }
    }
}
