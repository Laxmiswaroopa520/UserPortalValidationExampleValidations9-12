using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UserRepository : IUserRepository
    {
        private readonly ApplicationDbContext _context;

        public UserRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public IEnumerable<User> GetAll()
            => _context.Users.ToList();

        public User? GetById(int id)
            => _context.Users.Find(id);

        public User? GetByUsername(string username)
            => _context.Users.FirstOrDefault(u => u.Username == username);

        public void Update(User user)
            => _context.Users.Update(user);

        public void Delete(User user)
            => _context.Users.Remove(user);

        // ------------------ BIRTHDAYS ------------------

        public IEnumerable<User> GetUsersWithBirthdaysInMonth(int month, int fromDay)
        {
            return _context.Users
                .Where(u => u.DateOfBirth.HasValue &&
                            u.DateOfBirth.Value.Month == month &&
                            u.DateOfBirth.Value.Day >= fromDay)
                .ToList();
        }

        // ✅ CORRECT WEEKLY BIRTHDAY LOGIC
        public IEnumerable<User> GetUsersBetweenDates(DateTime start, DateTime end)
        {
            int startDay = start.DayOfYear;
            int endDay = end.DayOfYear;

            return _context.Users
                .Where(u => u.DateOfBirth.HasValue &&
                    (
                        startDay <= endDay
                            ? u.DateOfBirth.Value.DayOfYear >= startDay &&
                              u.DateOfBirth.Value.DayOfYear <= endDay
                            : u.DateOfBirth.Value.DayOfYear >= startDay ||
                              u.DateOfBirth.Value.DayOfYear <= endDay
                    ))
                .ToList();
        }
    }
}
