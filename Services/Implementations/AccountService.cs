using Microsoft.AspNetCore.Mvc.Rendering;
using UserPortalValdiationsDBContext.Enums;
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

        public bool IsEmailExists(string email)
            => _unit.Accounts.EmailExists(email);

        public bool IsUsernameExists(string username)
            => _unit.Accounts.UsernameExists(username);

        public void RegisterUser(User user)
        {
            _unit.Accounts.AddUser(user);
            _unit.Save();
        }

        public User? Login(string username, string password)
            => _unit.Accounts.GetByCredentials(username, password);

        public void UpdateUser(User user)
        {
            _unit.Accounts.UpdateUser(user);
            _unit.Save();
        }

        // ✅ FIXED RETURN TYPES
        public IEnumerable<string> GetCountries()
            => Enum.GetNames(typeof(CountryEnum));

        public IEnumerable<string> GetHobbies()
            => _unit.Accounts.GetHobbies();

        public IEnumerable<SelectListItem> GetRolesDropdown()
        {
            return _unit.Roles.GetAllRoles()
                .Select(r => new SelectListItem
                {
                    Value = r.Id.ToString(),
                    Text = r.Name
                });
        }

        // ✅ FIXED: no DbContext here
        public User? GetUserById(int userId)
        {
            return _unit.Accounts.GetUserWithRoleById(userId);
        }
    }
}
