



using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    public class DashboardController : Controller
    {
        private readonly IUserService UserService;
        private readonly IAddressService AddressService;

        public DashboardController(IUserService userService, IAddressService addressService)
        {
            UserService = userService;
            AddressService = addressService;
        }

        [ServiceFilter(typeof(ResultCacheFilter))]  // Cache Dashboard index page for performance
        public IActionResult Index()
        {
            var users = UserService.GetAllUsers();

            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                var name = HttpContext.User.Identity.Name;
                var loggedUser = UserService.GetUserByUsername(name!);

                if (loggedUser != null)
                {
                    loggedUser.LastLoginAt = DateTime.Now;
                    UserService.UpdateUser(loggedUser);
                }
            }

            return View(users);
        }

        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult Edit(int id)
        {
            var user = UserService.GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult Edit(User user)
        {
            user.LastPasswordChangeAt = DateTime.Now;
            UserService.UpdateUser(user);
            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            UserService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        public IActionResult AddAddress(int userId) => View(new AddressViewModel { UserId = userId });

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult AddAddress(AddressViewModel model)
        {
            AddressService.AddAddress(model);
            TempData["msg"] = "Address added successfully!";
            return RedirectToAction("UserAddresses", new { userId = model.UserId });
        }

        public IActionResult EditAddress(int id)
        {
            var vm = AddressService.GetAddressVMById(id);
            if (vm == null) return NotFound();
            return View(vm);
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult EditAddress(AddressViewModel model)
        {
            AddressService.UpdateAddress(model);
            return RedirectToAction("UserAddresses", new { userId = model.UserId });
        }

        public IActionResult DeleteAddress(int id)
        {
            AddressService.DeleteAddress(id);
            return RedirectToAction("Index");
        }

        public IActionResult UserAddresses(int userId)
        {
            var user = UserService.GetUserById(userId);
            if (user == null) return NotFound();

            var addresses = AddressService.GetAddressesByUserId(userId);

            var vm = new UserAddressListViewModel
            {
                UserId = userId,
                Username = user.Username,
                Addresses = addresses
            };

            return View(vm);
        }

        public IActionResult Birthdays()
        {
            var users = UserService.GetAllUsers();
            var upcoming = users
                .Where(u => u.DateOfBirth.Month >= DateTime.Today.Month)
                .OrderBy(u => u.DateOfBirth.Month)
                .ThenBy(u => u.DateOfBirth.Day)
                .ToList();

            return View(upcoming);
        }
    }
}

























/*using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    public class DashboardController : Controller
    {
        private readonly IUserService UserService;
        private readonly IAddressService AddressService;

        public DashboardController(IUserService userService, IAddressService addressService)
        {
            UserService = userService;
            AddressService = addressService;
        }

        // ---------------------------
        // DASHBOARD HOME
        // ---------------------------
        public IActionResult Index()
        {
            var users = UserService.GetAllUsers();

            // Update "LastLoginAt" of current logged-in user (OPTIONAL)
            if (HttpContext.User.Identity!.IsAuthenticated)
            {
                var name = HttpContext.User.Identity.Name;
                var loggedUser = UserService.GetUserByUsername(name!);

                if (loggedUser != null)
                {
                    loggedUser.LastLoginAt = DateTime.Now;
                    UserService.UpdateUser(loggedUser);
                }
            }

            return View(users);
        }

        // ---------------------------
        // USER OPERATIONS
        // ---------------------------
        public IActionResult Edit(int id)
        {
            var user = UserService.GetUserById(id);
            if (user == null) return NotFound();

            return View(user);
        }

        [HttpPost]
        public IActionResult Edit(User user)
        {
            if (!ModelState.IsValid)
                return View(user);

            user.LastPasswordChangeAt = DateTime.Now; // Track password change
            UserService.UpdateUser(user);

            return RedirectToAction("Index");
        }

        public IActionResult Delete(int id)
        {
            UserService.DeleteUser(id);
            return RedirectToAction("Index");
        }

        // ---------------------------
        // ADDRESSES
        // ---------------------------
        public IActionResult AddAddress(int userId)
        {
            return View(new AddressViewModel { UserId = userId });
        }

        [HttpPost]
        public IActionResult AddAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AddressService.AddAddress(model);

            TempData["msg"] = "Address added successfully!";
            return RedirectToAction("UserAddresses", new { userId = model.UserId });
        }

        public IActionResult EditAddress(int id)
        {
            var vm = AddressService.GetAddressVMById(id);
            if (vm == null) return NotFound();

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditAddress(AddressViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            AddressService.UpdateAddress(model);

            return RedirectToAction("UserAddresses", new { userId = model.UserId });
        }

        public IActionResult DeleteAddress(int id)
        {
            AddressService.DeleteAddress(id);
            return RedirectToAction("Index");
        }

        public IActionResult UserAddresses(int userId)
        {
            var user = UserService.GetUserById(userId);
            if (user == null) return NotFound();

            var addresses = AddressService.GetAddressesByUserId(userId);

            var vm = new UserAddressListViewModel
            {
                UserId = userId,
                Username = user.Username,
                Addresses = addresses
            };

            return View(vm);
        }

        // ---------------------------
        // VIEW UPCOMING BIRTHDAYS PAGE
        // ---------------------------
        public IActionResult Birthdays()
        {
            var users = UserService.GetAllUsers();

            var upcoming = users
                .Where(u => u.DateOfBirth.Month >= DateTime.Today.Month)
                .OrderBy(u => u.DateOfBirth.Month)
                .ThenBy(u => u.DateOfBirth.Day)
                .ToList();

            return View(upcoming);
        }
    }
}















*/




























