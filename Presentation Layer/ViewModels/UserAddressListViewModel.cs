namespace UserPortalValdiationsDBContext.ViewModels
{
    public class UserAddressListViewModel
    {
        public int UserId { get; set; }
        public string? Username { get; set; }

        public List<AddressViewModel>? Addresses { get; set; }
    }
}
