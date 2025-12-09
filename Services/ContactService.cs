using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Services
{
    public class ContactService : IContactService
    {
        private readonly ApplicationDbContext Db;

        public ContactService(ApplicationDbContext db)
        {
            Db = db;
        }

        public List<ContactModel> GetAllContacts()
        {
            return Db.Contacts.ToList();
        }

        public void AddContact(ContactModel model)
        {
            Db.Contacts.Add(model);
            Db.SaveChanges();
        }
    }
}
