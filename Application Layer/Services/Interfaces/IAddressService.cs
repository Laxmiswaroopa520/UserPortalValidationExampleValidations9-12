using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IAddressService
    {
        List<AddressViewModel> GetAddressesByUserId(int userId);
        AddressViewModel GetAddressVMById(int id);
        void AddAddress(AddressViewModel vm);
        void UpdateAddress(AddressViewModel vm);
        void DeleteAddress(int id);
    }
}
