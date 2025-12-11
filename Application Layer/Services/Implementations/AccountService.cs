using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class AccountService : IAccountService
    {
        private readonly IUnitOfWork _unit;

        public AccountService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public bool IsEmailExists(string email) => _unit.Users.EmailExists(email);

        public bool IsUsernameExists(string username) => _unit.Users.UsernameExists(username);

        public void RegisterUser(User user)
        {
            _unit.Users.Add(user);
            _unit.Complete();
        }

        public void UpdateUser(User user)
        {
            _unit.Users.Update(user);
            _unit.Complete();
        }

        public User? Login(string username, string password)
        {
            var user = _unit.Users.GetByUsername(username);
            if (user != null && user.Password == password) return user;
            return null;
        }
    }
}
