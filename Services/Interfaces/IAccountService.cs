using Microsoft.AspNetCore.Mvc.Rendering;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IAccountService
    {
        bool IsEmailExists(string email);
        bool IsUsernameExists(string username);

        void RegisterUser(User user);
        User? Login(string username, string password);
        void UpdateUser(User user);

        IEnumerable<string> GetCountries();
        IEnumerable<string> GetHobbies();
        IEnumerable<SelectListItem> GetRolesDropdown();

        User? GetUserById(int userId);          //role based  twilio,Temp Data
    }
}
