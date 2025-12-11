using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IUnitOfWork _unit;

        public AccountController(IAccountService accountService, IUnitOfWork unit)
        {
            _accountService = accountService;
            _unit = unit;
        }

        [HttpGet]
        [ServiceFilter(typeof(ResultCacheFilter))]
        public IActionResult Register()
        {
            LoadDropdowns();
            return View(new RegisterViewModel());
        }

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult Register(RegisterViewModel model)
        {
            LoadDropdowns();

            if (_accountService.IsEmailExists(model.Email!))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            if (_accountService.IsUsernameExists(model.Username!))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            var newUser = model.ToUser();
            newUser.ProfilePhotoPath = "/images/default-profile.png";
            newUser.LastLoginAt = DateTime.Now;
            newUser.LastPasswordChangeAt = DateTime.Now;
            newUser.Roles = "User";

            _accountService.RegisterUser(newUser);
            return RedirectToAction("Login");
        }

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        [ServiceFilter(typeof(ActionValidationFilter))]
        public IActionResult Login(LoginViewModel model)
        {
            var user = _accountService.Login(model.Username!, model.Password!);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            user.LastLoginAt = DateTime.Now;
            _accountService.UpdateUser(user);

            return RedirectToAction("Index", "Dashboard");
        }

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private void LoadDropdowns()
        {
            ViewBag.Countries = Enum.GetNames(typeof(CountryEnum)).ToList();

            // Hobby list now comes from Repository → UoW
            ViewBag.HobbiesList = _unit.Hobbies.GetAll()
                .Select(h => h.Name)
                .ToList();
        }
    }
}













/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;
namespace UserPortalValdiationsDBContext.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService AccountService;
        private readonly ApplicationDbContext Context;

        public AccountController(IAccountService accountService, ApplicationDbContext context)
        {
            AccountService = accountService;
            Context = context;
        }

        // ---------------------------
        // REGISTER (GET)
        // ---------------------------
        public IActionResult Register()
        {
            LoadDropdowns();
            return View(new RegisterViewModel());
        }

        // ---------------------------
        // REGISTER (POST)
        // ---------------------------
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            if (AccountService.IsEmailExists(model.Email!))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            if (AccountService.IsUsernameExists(model.Username!))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            try
            {
                var newUser = model.ToUser();
                newUser.ProfilePhotoPath = "/images/default-profile.png";
                newUser.LastLoginAt = DateTime.Now;
                newUser.LastPasswordChangeAt = DateTime.Now;
                newUser.Roles = "User";

                AccountService.RegisterUser(newUser);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Registration failed: " + ex.Message);
                return View(model);
            }
        }

        // ---------------------------
        // LOGIN (GET)
        // ---------------------------
        public IActionResult Login()
        {
            return View();
        }

        // ---------------------------
        // LOGIN (POST)
        // ---------------------------
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var user = AccountService.Login(model.Username!, model.Password!);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            user.LastLoginAt = DateTime.Now;
            AccountService.UpdateUser(user);

            return RedirectToAction("Index", "Dashboard");
        }

        // ---------------------------
        // LOGOUT
        // ---------------------------
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        // ---------------------------
        // LOAD DROPDOWNS
        // ---------------------------
        private void LoadDropdowns()
        {
            ViewBag.Countries = Enum.GetNames(typeof(CountryEnum)).ToList();
            ViewBag.HobbiesList = Context.Hobbies.Select(h => h.Name).ToList();
        }
    }
}

*/






































/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    public class AccountController : Controller
    {
        private readonly IAccountService AccountService;
        private readonly ApplicationDbContext Context;
        public AccountController(IAccountService accountService,ApplicationDbContext context)
        {
            AccountService = accountService;
            Context = context;
        }

        // GET: REGISTER
        public IActionResult Register()
        {
            LoadDropdowns();
            return View(new RegisterViewModel());
        }

        // POST: REGISTER
        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            LoadDropdowns(); // Must reload for POST

            if (!ModelState.IsValid)
            {
                return View(model);
            }

            if (AccountService.IsEmailExists(model.Email!))
            {
                ModelState.AddModelError("Email", "Email already exists.");
                return View(model);
            }

            if (AccountService.IsUsernameExists(model.Username!))
            {
                ModelState.AddModelError("Username", "Username already exists.");
                return View(model);
            }

            try
            {
                User newUser = model.ToUser();
                AccountService.RegisterUser(newUser);
                return RedirectToAction("Login");
            }
            catch (Exception ex)
            {
                ModelState.AddModelError("", "Registration failed: " + ex.Message);
                return View(model);
            }
        }

        // GET: LOGIN
        public IActionResult Login()
        {
            return View();
        }

        // POST: LOGIN
        [HttpPost]
        public IActionResult Login(LoginViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            //  var user = AccountService.Login(model.Username, model.Password);
            var user = AccountService.Login(model.Username!, model.Password!);              //tell that compiler values are not null.(!)


            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            return RedirectToAction("Index", "Dashboard");
        }

        // LOGOUT
        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            return RedirectToAction("Login", "Account");
        }

        private void LoadDropdowns()
        {
            // Always use ENUM for both GET and POST
            ViewBag.Countries = Enum.GetNames(typeof(CountryEnum)).ToList();                // getting drop down enum for countries.
               // Fetch hobbies from database
            ViewBag.HobbiesList = Context.Hobbies
                                 .Select(h => h.Name)
                                 .ToList();
        }
    }
}






*/







/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    public class AccountController : Controller
    {
       
        private readonly IAccountService AccountService;

        public AccountController(IAccountService accountService)
        {
            AccountService = accountService;
        }

        // GET: REGISTER
        public IActionResult Register()
        {
            LoadDropdowns();
            return View(new RegisterViewModel());
        }

        /* // POST: REGISTER
         [HttpPost]
         public IActionResult Register(RegisterViewModel model)
         {
             LoadDropdowns();

             if (!ModelState.IsValid)
                 return View(model);

             if (AccountService.IsEmailExists(model.Email))
             {
                 ModelState.AddModelError("Email", "Email already exists.");
                 return View(model);
             }

             if (AccountService.IsUsernameExists(model.Username))
             {
                 ModelState.AddModelError("Username", "Username already exists.");
                 return View(model);
             }

             var user = new User
             {
                 Username = model.Username!,
                 Email = model.Email!,
                 Password = model.Password!,
                 Age = model.Age,
                 Phone = model.Phone,
                 Gender = model.Gender,
                 DateOfBirth = model.DateOfBirth,
                 Country = model.Country,
                 Hobbies = model.Hobbies != null ? string.Join(",", model.Hobbies) : "",
                 AcceptTerms = model.AcceptTerms
             };

             AccountService.RegisterUser(user);

             return RedirectToAction("Login");
         }*/
/*  [HttpPost]              //changed used automapper concept here.,
  public IActionResult Register(RegisterViewModel model)
  {
      LoadDropdowns();

      if (!ModelState.IsValid)
          return View(model);

      if (AccountService.IsEmailExists(model.Email!))
      {
          ModelState.AddModelError("Email", "Email already exists.");
          return View(model);
      }

      if (AccountService.IsUsernameExists(model.Username!))
      {
          ModelState.AddModelError("Username", "Username already exists.");
          return View(model);
      }

      // Use ViewModel to map to User entity
      User newUser = model.ToUser();
      AccountService.RegisterUser(newUser);

      return RedirectToAction("Login");
  }
  */

/*  [HttpPost]
  public IActionResult Register(RegisterViewModel model)
  {
      // Load dropdowns for country etc.
      LoadDropdowns();

      // Check ModelState
      if (!ModelState.IsValid)
      {
          // Collect all validation errors
          var errors = ModelState.Values.SelectMany(v => v.Errors)
                                        .Select(e => e.ErrorMessage)
                                        .ToList();
          ViewBag.Errors = errors;

          // Password field will be cleared automatically by MVC
          return View(model);
      }

      // Check if email exists
      if (AccountService.IsEmailExists(model.Email!))
      {
          ModelState.AddModelError("Email", "Email already exists.");
          return View(model);
      }

      // Check if username exists
      if (AccountService.IsUsernameExists(model.Username!))
      {
          ModelState.AddModelError("Username", "Username already exists.");
          return View(model);
      }

      try
      {
          // Map ViewModel to User entity
          User newUser = model.ToUser();

          // Register the user
          AccountService.RegisterUser(newUser);

          // Redirect to login on success
          return RedirectToAction("Login");
      }
      catch (Exception ex)
      {
          // Handle unexpected errors during registration
          ModelState.AddModelError("", "Registration failed: " + ex.Message);
          return View(model);
      }
  }




  // GET: LOGIN
  public IActionResult Login()
  {
      return View();
  }

  // POST: LOGIN
  [HttpPost]
  public IActionResult Login(LoginViewModel model)
  {
      if (!ModelState.IsValid)
          return View(model);

      var user = AccountService.Login(model.Username, model.Password);

      if (user == null)
      {
          ModelState.AddModelError("", "Invalid username or password.");
          return View(model);
      }

      return RedirectToAction("Index", "Dashboard");
  }
  //logout
  [HttpPost]
  public async Task<IActionResult> Logout()
  {
      await HttpContext.SignOutAsync();
      return RedirectToAction("Login", "Account");
  }


  private void LoadDropdowns()
  {
      ViewBag.Countries = new List<string>
      {
          "India", "USA", "UK", "Canada", "Australia"
      };

      ViewBag.HobbiesList = new List<string>
      {
          "Sports", "Music", "Reading", "Travel", "Photography"
      };
  }
}
}
*/