using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class AddressService : IAddressService
    {
        private readonly IUnitOfWork _unit;

        public AddressService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public List<AddressViewModel> GetAddressesByUserId(int userId)
        {
            return _unit.Addresses
                        .GetByUserId(userId)
                        .Select(a => new AddressViewModel
                        {
                            Id = a.Id,
                            UserId = a.UserId,
                            Line1 = a.Line1,
                            City = a.City,
                            State = a.State,
                            Pin = a.Pin
                        })
                        .ToList();
        }

        public AddressViewModel GetAddressVMById(int id)
        {
            var addr = _unit.Addresses.GetById(id);
            if (addr == null) 
                return throw new KeyNotFoundException($"Address {id} not found.");

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

            _unit.Addresses.Add(entity);
            _unit.Complete();
        }

        public void UpdateAddress(AddressViewModel vm)
        {
            var entity = _unit.Addresses.GetById(vm.Id);
            if (entity == null) return;

            entity.Line1 = vm.Line1;
            entity.City = vm.City;
            entity.State = vm.State;
            entity.Pin = vm.Pin;

            _unit.Addresses.Update(entity);
            _unit.Complete();
        }

        public void DeleteAddress(int id)
        {
            var addr = _unit.Addresses.GetById(id);
            if (addr == null) return;

            _unit.Addresses.Remove(addr);
            _unit.Complete();
        }
    }
}
