using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class ContactRepository : GenericRepository<ContactModel>, IContactRepository
    {
        public ContactRepository(ApplicationDbContext db) : base(db) { }
    }
}
