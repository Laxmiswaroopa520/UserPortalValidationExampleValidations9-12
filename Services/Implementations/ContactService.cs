using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unit;

        public ContactService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public List<ContactModel> GetAllContacts()
        {
            return _unit.Contacts.GetAll();
        }

        public void AddContact(ContactModel model)
        {
            _unit.Contacts.Add(model);
            _unit.Save();   // ✅ single commit
        }
    }
}
