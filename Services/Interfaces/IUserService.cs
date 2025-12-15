/*using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IUserService
    {
        // 🔹 CRUD
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        void UpdateUser(User user);
        void DeleteUser(int id);

        // 🔹 Dashboard / Charts
        IEnumerable<UserCountByDepartment> GetUserCountByDepartment();
        IEnumerable<UserCountByDepartment> GetUserCountByDepartmentChart();

        // 🔹 Birthdays
        IEnumerable<User> GetUpcomingBirthdays();
        IEnumerable<User> GetWeeklyBirthdays();

        // 🔹 Sidebar
        UserProfileSidebarViewModel? GetUserProfileSidebar(string username);

        // 🔹 Profile & Password
        EditProfileViewModel? GetEditProfileData(string username);
        bool UpdateUserProfile(string username, EditProfileViewModel vm);
        bool ChangePassword(string username, ChangePasswordViewModel vm);
    }
}
*/

using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IUserService
    {
        // 🔹 CRUD
        List<User> GetAllUsers();
        User? GetUserById(int id);
        User? GetUserByUsername(string username);
        void UpdateUser(User user);
        void DeleteUser(int id);

        // 🔹 Dashboard / Charts
        IEnumerable<UserCountByDepartment> GetUserCountByDepartment();
        IEnumerable<UserCountByDepartment> GetUserCountByDepartmentChart();

        // 🔹 Birthdays
        IEnumerable<User> GetUpcomingBirthdays();
        IEnumerable<User> GetWeeklyBirthdays();

        // 🔹 Sidebar
        UserProfileSidebarViewModel? GetUserProfileSidebar(string username);

        // 🔹 Profile & Password
        EditProfileViewModel? GetEditProfileData(string username);
        bool UpdateUserProfile(string username, EditProfileViewModel vm);
        bool ChangePassword(string username, ChangePasswordViewModel vm);
    }
}
