using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class AccountRepository : IAccountRepository
    {
        private readonly ApplicationDbContext _db;

        public AccountRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public bool EmailExists(string email)
        {
            return _db.Users.Any(u => u.Email == email);
        }

        public bool UsernameExists(string username)
        {
            return _db.Users.Any(u => u.Username == username);
        }

        public User? GetByCredentials(string username, string password)
        {
            return _db.Users.FirstOrDefault(u =>
                u.Username == username &&
                u.Password == password);
        }

        public void AddUser(User user)
        {
            _db.Users.Add(user);
        }

        public void UpdateUser(User user)
        {
            _db.Users.Update(user);
        }

        public List<string> GetHobbies()
        {
            return _db.Hobbies
                      .Select(h => h.Name)
                      .ToList();
        }
        //for roles.
        public User? GetUserWithRoleById(int userId)
        {
            return _db.Users
                .Include(u => u.Role)
                .FirstOrDefault(u => u.Id == userId);
        }

    }
}
