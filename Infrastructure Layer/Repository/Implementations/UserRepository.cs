using System.Linq;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UserRepository : GenericRepository<User>, IUserRepository
    {
        private readonly ApplicationDbContext _db;

        public UserRepository(ApplicationDbContext db) : base(db)
        {
            _db = db;
        }

        public User? GetByUsername(string username) => _db.Users.FirstOrDefault(u => u.Username == username);

        public bool EmailExists(string email) => _db.Users.Any(u => u.Email == email);

        public bool UsernameExists(string username) => _db.Users.Any(u => u.Username == username);
    }
}
