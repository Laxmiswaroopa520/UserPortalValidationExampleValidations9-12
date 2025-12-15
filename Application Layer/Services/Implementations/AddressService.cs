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
<<<<<<<< HEAD:Services/Implementations/AddressService.cs
            return _unit.Addresses.GetByUserId(userId)
                .Select(a => new AddressViewModel
                {
                    Id = a.Id,
                    UserId = a.UserId,
                    Line1 = a.Line1,
                    City = a.City,
                    State = a.State,
                    Pin = a.Pin
                }).ToList();
========
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
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Implementations/AddressService.cs
        }

        public AddressViewModel GetAddressVMById(int id)
        {
<<<<<<<< HEAD:Services/Implementations/AddressService.cs
            var addr = _unit.Addresses.GetById(id)
                ?? throw new InvalidOperationException("Address not found");
========
            var addr = _unit.Addresses.GetById(id);
            if (addr == null) 
                return throw new KeyNotFoundException($"Address {id} not found.");
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Implementations/AddressService.cs

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
            _unit.Addresses.Add(new Address
            {
                UserId = vm.UserId,
                Line1 = vm.Line1,
                City = vm.City,
                State = vm.State,
                Pin = vm.Pin
            });

<<<<<<<< HEAD:Services/Implementations/AddressService.cs
            _unit.Save();
========
            _unit.Addresses.Add(entity);
            _unit.Complete();
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Implementations/AddressService.cs
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
<<<<<<<< HEAD:Services/Implementations/AddressService.cs
            _unit.Save();
========
            _unit.Complete();
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Implementations/AddressService.cs
        }

        public void DeleteAddress(int id)
        {
<<<<<<<< HEAD:Services/Implementations/AddressService.cs
            var entity = _unit.Addresses.GetById(id);
            if (entity == null) return;

            _unit.Addresses.Delete(entity);
            _unit.Save();
========
            var addr = _unit.Addresses.GetById(id);
            if (addr == null) return;

            _unit.Addresses.Remove(addr);
            _unit.Complete();
>>>>>>>> 434e4f68b63bd11e3b7b38789add7bc53265a4dc:Application Layer/Services/Implementations/AddressService.cs
        }
    }
}
