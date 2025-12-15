using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUnitOfWork _unit;

        public UserService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        // -------- CRUD --------

        public List<User> GetAllUsers()
            => _unit.Users.GetAll().ToList();

        public User? GetUserById(int id)
            => _unit.Users.GetById(id);

        public User? GetUserByUsername(string username)
            => _unit.Users.GetByUsername(username);

        public void UpdateUser(User user)
        {
            _unit.Users.Update(user);
            _unit.Save();
        }

        public void DeleteUser(int id)
        {
            var user = _unit.Users.GetById(id);
            if (user == null) return;

            _unit.Users.Delete(user);
            _unit.Save();
        }

        // -------- DASHBOARD --------

        public IEnumerable<UserCountByDepartment> GetUserCountByDepartment()
        {
            return _unit.Users.GetAll()
                .GroupBy(u => u.Department)
                .Select(g => new UserCountByDepartment
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }

        public IEnumerable<UserCountByDepartment> GetUserCountByDepartmentChart()
            => GetUserCountByDepartment();

        // -------- BIRTHDAYS --------

        public IEnumerable<User> GetUpcomingBirthdays()
        {
            var today = DateTime.Today;
            return _unit.Users.GetUsersWithBirthdaysInMonth(today.Month, today.Day);
        }

     
        public IEnumerable<User> GetWeeklyBirthdays()
        {
            var today = DateTime.Today;
            return _unit.Users.GetUsersBetweenDates(today, today.AddDays(7));
        }


        // -------- SIDEBAR --------
        public UserProfileSidebarViewModel? GetUserProfileSidebar(string username)
        {
            var user = _unit.Users.GetByUsername(username);
            if (user == null) return null;

            return new UserProfileSidebarViewModel
            {
                UserId = user.Id,
                Name = user.Username,
                Email = user.Email,
                ProfilePhotoPath = user.ProfilePhotoPath,

                Roles = user.Role != null
                    ? new List<string> { user.Role.Name }
                    : new List<string> { "User" }
            };
        }


        // -------- PROFILE --------

        public EditProfileViewModel? GetEditProfileData(string username)
        {
            var user = _unit.Users.GetByUsername(username);
            if (user == null) return null;

            return new EditProfileViewModel
            {
                Username = user.Username!,
                Email = user.Email!,
                Phone = user.Phone,
                ProfilePhotoPath = user.ProfilePhotoPath
            };
        }

        public bool UpdateUserProfile(string username, EditProfileViewModel vm)
        {
            var user = _unit.Users.GetByUsername(username);
            if (user == null) return false;

            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.ProfilePhotoPath = vm.ProfilePhotoPath;

            _unit.Save();
            return true;
        }

        public bool ChangePassword(string username, ChangePasswordViewModel vm)
        {
            var user = _unit.Users.GetByUsername(username);
            if (user == null || user.Password != vm.CurrentPassword)
                return false;

            user.Password = vm.NewPassword;
            user.LastPasswordChangeAt = DateTime.Now;

            _unit.Save();
            return true;
        }
    }
}



































/*using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IUnitOfWork _unit;

        public UserService(IUserRepository userRepository, IUnitOfWork unit)
        {
            _userRepository = userRepository;
            _unit = unit;
        }

        // ---------------- CRUD ----------------

        public List<User> GetAllUsers()
            => _userRepository.GetAll().ToList();

        public User? GetUserById(int id)
            => _userRepository.GetById(id);

        public User? GetUserByUsername(string username)
            => _userRepository.GetByUsername(username);

        public void UpdateUser(User user)
        {
            _userRepository.Update(user);
            _unit.Save();
        }

        public void DeleteUser(int id)
        {
            var user = _userRepository.GetById(id);
            if (user == null) return;

            _userRepository.Delete(user);
            _unit.Save();
        }

        // ---------------- DEPARTMENT COUNTS ----------------

        public IEnumerable<UserCountByDepartment> GetUserCountByDepartment()
        {
            return _userRepository.GetAll()
                .GroupBy(u => u.Department)
                .Select(g => new UserCountByDepartment
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }

        public IEnumerable<UserCountByDepartment> GetUserCountByDepartmentChart()
        {
            return _userRepository.GetAll()
                .Where(u => u.Department != null)
                .GroupBy(u => u.Department)
                .Select(g => new UserCountByDepartment
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToList();
        }

        // ---------------- BIRTHDAYS ----------------

        public IEnumerable<User> GetUpcomingBirthdays()
        {
            var today = DateTime.Today;

            return _userRepository.GetAll()
                .Where(u => u.DateOfBirth != null &&
                            u.DateOfBirth.Month == today.Month &&
                            u.DateOfBirth.Day >= today.Day)
                .OrderBy(u => u.DateOfBirth.Day)
                .ToList();
        }

        public IEnumerable<User> GetWeeklyBirthdays()
        {
            var today = DateTime.Today;
            var endDate = today.AddDays(7);

            return _userRepository.GetAll()
                .Where(u => u.DateOfBirth != null &&
                            u.DateOfBirth >= today &&
                            u.DateOfBirth <= endDate)
                .OrderBy(u => u.DateOfBirth)
                .ToList();
        }

        // ---------------- SIDEBAR ----------------

        public UserProfileSidebarViewModel? GetUserProfileSidebar(string username)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) return null;

            return new UserProfileSidebarViewModel
            {
                UserId = user.Id,
                Name = user.Username,
                Email = user.Email,
                ProfilePhotoPath = user.ProfilePhotoPath,
                Roles = user.Roles?.Split(',').ToList() ?? new(),
                LastPasswordChangeAt = user.LastPasswordChangeAt,
                RecentLogins = user.LastLoginAt != null
                    ? new List<DateTime> { user.LastLoginAt.Value }
                    : new(),
                UpcomingBirthday = user.DateOfBirth != null
                    ? new BirthdayInfo
                    {
                        Name = user.Username ?? "",
                        Date = user.DateOfBirth,
                        DaysRemaining = (user.DateOfBirth.Date - DateTime.Today).Days
                    }
                    : null
            };
        }

        // ---------------- PROFILE ----------------

        public EditProfileViewModel? GetEditProfileData(string username)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) return null;

            return new EditProfileViewModel
            {
                Username = user.Username!,
                Email = user.Email!,
                Phone = user.Phone,
                ProfilePhotoPath = user.ProfilePhotoPath
            };
        }

        public bool UpdateUserProfile(string username, EditProfileViewModel vm)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) return false;

            user.Email = vm.Email;
            user.Phone = vm.Phone;
            user.ProfilePhotoPath = vm.ProfilePhotoPath;

            _userRepository.Update(user);
            _unit.Save();

            return true;
        }

        public bool ChangePassword(string username, ChangePasswordViewModel vm)
        {
            var user = _userRepository.GetByUsername(username);
            if (user == null) return false;

            if (user.Password != vm.CurrentPassword)
                return false;

            user.Password = vm.NewPassword;
            user.LastPasswordChangeAt = DateTime.Now;

            _userRepository.Update(user);
            _unit.Save();

            return true;
        }
    }
}
*/