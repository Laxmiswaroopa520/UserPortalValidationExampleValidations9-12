
    using global::UserPortalValdiationsDBContext.Models;
  //  using UserPortalValdiationsDBContext.Models;
namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IUserRepository
    {
        IEnumerable<User> GetAll();
        User? GetById(int id);
        User? GetByUsername(string username);

        void Update(User user);
        void Delete(User user);

        // 🎂 Birthday Queries
        IEnumerable<User> GetUsersWithBirthdaysInMonth(int month, int fromDay);
        IEnumerable<User> GetUsersBetweenDates(DateTime start, DateTime end);
    }
}
