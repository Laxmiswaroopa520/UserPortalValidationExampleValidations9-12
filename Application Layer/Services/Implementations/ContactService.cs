using System.Collections.Generic;
using System.Linq;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class ContactService : IContactService
    {
        private readonly IUnitOfWork _unit;

        public ContactService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public List<ContactModel> GetAllContacts() => _unit.Contacts.GetAll().ToList();

        public void AddContact(ContactModel model)
        {
            _unit.Contacts.Add(model);
            _unit.Complete();
        }
    }
}
