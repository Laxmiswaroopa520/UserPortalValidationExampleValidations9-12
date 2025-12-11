using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IContactService
    {
        List<ContactModel> GetAllContacts();
        void AddContact(ContactModel model);
    }
}
