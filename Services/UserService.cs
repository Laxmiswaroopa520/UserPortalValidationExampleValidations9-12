// Handles user CRUD operations
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;


namespace UserPortalValdiationsDBContext.Services
{
    public class UserService : IUserService
    {
        private readonly ApplicationDbContext Db;

        public UserService(ApplicationDbContext db)
        {
            Db = db;
        }

        public List<User> GetAllUsers() => Db.Users.ToList();

       // public User GetUserById(int id) => Db.Users.Find(id);
        public User? GetUserById(int id) => Db.Users.Find(id);


        /*  public void UpdateUser(User user)
          {
              var existing = Db.Users.Find(user.Id);
              if (existing != null)
              {
                  existing.Username = user.Username;
                  existing.Email = user.Email;
                  existing.Age = user.Age;
                  existing.Phone = user.Phone;
                  existing.Password = user.Password;
                  Db.Users.Update(existing);
                  Db.SaveChanges();
              }
          }
        */
        //update user
        public void UpdateUser(User user)
        {
            Db.Attach(user);
            Db.Entry(user).State = Microsoft.EntityFrameworkCore.EntityState.Modified;
            Db.SaveChanges();
        }

        public void DeleteUser(int id)
        {
            var user = Db.Users.Find(id);
            if (user != null)
            {
                Db.Users.Remove(user);
                Db.SaveChanges();
            }
        }

        public User? GetUserByUsername(string username)
        {
            return Db.Users.FirstOrDefault(u => u.Username == username);
        }

        //for dept count (View Component)
        public IEnumerable<UserCountByDepartment> GetUserCountByDepartment()
        {
            return Db.Users
                .GroupBy(u => u.Department)
                .Select(g => new UserCountByDepartment
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }
        //for view model(View Component)
        public IEnumerable<User> GetUpcomingBirthdays()
        {
            var today = DateTime.Today;

            // Use DateOfBirth instead of BirthDate
            return Db.Users
                     .Where(u => u.DateOfBirth != null &&
                                 u.DateOfBirth.Month == today.Month &&
                                 u.DateOfBirth.Day >= today.Day)
                     .OrderBy(u => u.DateOfBirth.Day)
                     .ToList();
        }



    }
}
