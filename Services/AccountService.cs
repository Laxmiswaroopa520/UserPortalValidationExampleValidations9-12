using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Services
{
    public class AccountService : IAccountService
    {
        
        private readonly ApplicationDbContext DbContext;

        public AccountService(ApplicationDbContext db)
        {
            DbContext = db;
        }

        public bool IsEmailExists(string email)
        {
            return DbContext.Users.Any(u => u.Email == email);
        }

        public bool IsUsernameExists(string username)
        {
            return DbContext.Users.Any(u => u.Username == username);
        }

        public void RegisterUser(User user)
        {
            DbContext.Users.Add(user);
            DbContext.SaveChanges();
        }

        //code for update user in account controller   :already present in UserService
        public void UpdateUser(User user)
        {
            DbContext.Users.Update(user);
            DbContext.SaveChanges();
        }


        public User? Login(string username, string password) => DbContext.Users
                .FirstOrDefault(x => x.Username == username && x.Password == password);
    }
}
