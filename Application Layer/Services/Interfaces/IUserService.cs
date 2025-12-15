using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services;
namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IUserService
    {
        List<User> GetAllUsers();
        User? GetUserById(int id);
        void UpdateUser(User user);
        void DeleteUser(int id);
        User? GetUserByUsername(string username);
        IEnumerable<UserCountByDepartment> GetUserCountByDepartment(); // now matches the moved class view components..
        IEnumerable<User> GetUpcomingBirthdays();               //for  view components..

    }
}
