using System;
using System.Collections.Generic;
using System.Linq;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;

        public UserService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public List<User> GetAllUsers() => _unit.Users.GetAll().ToList();

        public User? GetUserById(int id) => _unit.Users.GetById(id);

        public void UpdateUser(User user)
        {
            // you can choose to map only necessary fields instead of replacing whole entity
            _unit.Users.Update(user);
            _unit.Complete();
        }

        public void DeleteUser(int id)
        {
            var user = _unit.Users.GetById(id);
            if (user == null) return;
            _unit.Users.Remove(user);
            _unit.Complete();
        }

        public User? GetUserByUsername(string username) => _unit.Users.GetByUsername(username);

        public IEnumerable<object> GetUserCountByDepartment()
        {
            // keep your previous UserCountByDepartment DTO or return anonymous — prefer concrete DTO
            return _unit.Users.GetAll()
                              .GroupBy(u => u.Department)
                              .Select(g => new { Department = g.Key, Count = g.Count() })
                              .ToList();
        }

        public IEnumerable<User> GetUpcomingBirthdays()
        {
            var today = DateTime.Today;
            return _unit.Users.Find(u => u.DateOfBirth != null &&
                                         (u.DateOfBirth.Value.Month > today.Month ||
                                          (u.DateOfBirth.Value.Month == today.Month && u.DateOfBirth.Value.Day >= today.Day)))
                              .OrderBy(u => u.DateOfBirth!.Value.Month)
                              .ThenBy(u => u.DateOfBirth!.Value.Day)
                              .ToList();
        }
    }
}
