using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _db;

        public IUserRepository Users { get; private set; }
        public IAddressRepository Addresses { get; private set; }
        public IContactRepository Contacts { get; private set; }
        public IHobbyRepository Hobbies { get; private set; }

        public UnitOfWork(ApplicationDbContext db)
        {
            _db = db;
            Users = new UserRepository(db);
            Addresses = new AddressRepository(db);
            Contacts = new ContactRepository(db);
            Hobbies = new HobbyRepository(db);
        }

        public int Complete() => _db.SaveChanges();

        public Task<int> CompleteAsync() => _db.SaveChangesAsync();
    }
}
