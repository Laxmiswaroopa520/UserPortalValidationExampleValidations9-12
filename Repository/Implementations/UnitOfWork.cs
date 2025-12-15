using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _context;

        public IUserRepository Users { get; }
        public IAccountRepository Accounts { get; }
        public IAddressRepository Addresses { get; }
        public IContactRepository Contacts { get; }
        public IUserActivityRepository UserActivities { get; }
        public IRoleRepository Roles { get; }

        public UnitOfWork(
            ApplicationDbContext context,
            IUserRepository userRepository,
            IAccountRepository accountRepository,
            IAddressRepository addressRepository,
            IContactRepository contactRepository,
            IUserActivityRepository userActivityRepository,
            IRoleRepository roleRepository)   
        {
            _context = context;

            Users = userRepository;
            Accounts = accountRepository;
            Addresses = addressRepository;
            Contacts = contactRepository;
            UserActivities = userActivityRepository;
            Roles = roleRepository;          
        }

        public int Save()
        {
            return _context.SaveChanges();
        }

        public void Dispose()
        {
            _context.Dispose();
        }
    }
}
