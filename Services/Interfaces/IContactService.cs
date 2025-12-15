using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Interfaces
{
    public interface IContactService
    {
        List<ContactModel> GetAllContacts();
        void AddContact(ContactModel model);
    }
}
