using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class ContactRepository : IContactRepository
    {
        private readonly ApplicationDbContext _context;

        public ContactRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public List<ContactModel> GetAll()
        {
            return _context.Contacts.ToList();
        }

        public void Add(ContactModel contact)
        {
            _context.Contacts.Add(contact);
        }
    }
}
