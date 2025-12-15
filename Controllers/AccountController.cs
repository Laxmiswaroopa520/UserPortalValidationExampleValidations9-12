using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services.Implementations;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    [ServiceFilter(typeof(ActionValidationFilter))]
    public class AccountController : Controller
    {
        private readonly IAccountService _accountService;
        private readonly IAuthService _authService;
        private readonly IUserService _userService;
        private readonly ISmsService _smsService;

        public AccountController(
            IAccountService accountService,
            IAuthService authService,
            IUserService userService,
            ISmsService smsService)
        {
            _accountService = accountService;
            _authService = authService;
            _userService = userService;
            _smsService = smsService;
        }

        // ---------------- REGISTER ----------------

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Roles = _accountService.GetRolesDropdown()
            };

            LoadDropdowns();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            model.Roles = _accountService.GetRolesDropdown();
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            if (_accountService.IsEmailExists(model.Email!))
            {
                ModelState.AddModelError("Email", "Email already exists");
                return View(model);
            }

            if (_accountService.IsUsernameExists(model.Username!))
            {
                ModelState.AddModelError("Username", "Username already exists");
                return View(model);
            }

            // ---------- PROFILE PHOTO ----------
            string profilePhotoPath = "/images/default-profile.png";

            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                var uploadDir = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/uploads/users");
                Directory.CreateDirectory(uploadDir);

                var fileName = Guid.NewGuid() + Path.GetExtension(model.ProfilePhoto.FileName);
                var filePath = Path.Combine(uploadDir, fileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                model.ProfilePhoto.CopyTo(stream);

                profilePhotoPath = "/uploads/users/" + fileName;
            }

            var user = new User
            {
                Username = model.Username!,
                Email = model.Email!,
                Password = model.Password!,
                Age = model.Age,
                Gender = model.Gender,
                Country = model.Country,
                DateOfBirth = model.DateOfBirth,
                Phone = model.Phone,
                Department = model.Department,
                Hobbies = model.Hobbies != null ? string.Join(",", model.Hobbies) : null,
                AcceptTerms = model.AcceptTerms,
                RoleId = model.RoleId,
                ProfilePhotoPath = profilePhotoPath,
                Is2FAVerified = false,
                LastLoginAt = DateTime.Now,
                LastPasswordChangeAt = DateTime.Now
            };

            _accountService.RegisterUser(user);
            return RedirectToAction("Login");
        }

        // ---------------- LOGIN ----------------

        [HttpGet]
        public IActionResult Login() => View();

        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = _accountService.Login(model.Username!, model.Password!);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password");
                return View(model);
            }

            user.LastLoginAt = DateTime.Now;
            _accountService.UpdateUser(user);

            // ---- ADMIN OTP ----
            if (user.Role?.Name == "Admin")
            {
                await SendOtpAsync(user);
                TempData["UserId"] = user.Id;
                return RedirectToAction("VerifyOtp");
            }

            // ---- MANAGER OTP + IP ----
            if (user.Role?.Name == "Manager")
            {
                await SendOtpAsync(user);
                user.LastLoginIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                _accountService.UpdateUser(user);

                TempData["UserId"] = user.Id;
                return RedirectToAction("VerifyOtp");
            }

            // ---- NORMAL USER ----
            await _authService.SignInAsync(user, HttpContext);
            return RedirectToAction("Index", "Dashboard");
        }

        // ---------------- OTP ----------------

        [HttpGet]
        public IActionResult VerifyOtp() => View();

        /*  [HttpPost]
          public async Task<IActionResult> VerifyOtp(string otp)
          {
              if (!TempData.TryGetValue("UserId", out var userIdObj))
                  return RedirectToAction("Login");

              var user = _accountService.GetUserById(Convert.ToInt32(userIdObj));

              if (user == null ||
                  user.TwoFactorCode != otp ||
                  user.TwoFactorExpiry < DateTime.Now)
              {
                  ModelState.AddModelError("", "Invalid or expired OTP");
                  return View();
              }

              user.Is2FAVerified = true;
              user.TwoFactorCode = null;
              user.TwoFactorExpiry = null;
              _accountService.UpdateUser(user);

              var claims = new List<Claim>
              {
                  new Claim("Is2FAVerified", "true"),
                  new Claim(ClaimTypes.Role, user.Role!.Name)
              };

              if (!string.IsNullOrEmpty(user.LastLoginIP))
                  claims.Add(new Claim("LoginIP", user.LastLoginIP));

              var identity = new ClaimsIdentity(claims, "login");
              await HttpContext.SignInAsync(new ClaimsPrincipal(identity));

              return RedirectToAction("Index", "Dashboard");
          }
        */
        [HttpPost]
        public async Task<IActionResult> VerifyOtp(VerifyOtpViewModel model)
        {
            if (!ModelState.IsValid)
                return View(model);

            var otp = model.Otp;

            if (!TempData.TryGetValue("UserId", out var userIdObj))
                return RedirectToAction("Login");

            var user = AccountService.GetUserById(Convert.ToInt32(userIdObj));

            if (user == null ||
                user.TwoFactorCode != otp ||
                user.TwoFactorExpiry < DateTime.Now)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View(model);
            }

            // success logic...
        }

        // ---------------- LOGOUT ----------------

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await _authService.SignOutAsync(HttpContext);
            return RedirectToAction("Login");
        }

        // ---------------- EDIT PROFILE ----------------

        [HttpGet]
        public IActionResult EditProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var vm = _userService.GetEditProfileData(username);
            return vm == null ? NotFound() : View(vm);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel vm)
        {
            if (!_userService.UpdateUserProfile(User.Identity!.Name!, vm))
            {
                ModelState.AddModelError("", "Unable to update profile");
                return View(vm);
            }

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // ---------------- CHANGE PASSWORD ----------------

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel vm)
        {
            if (!_userService.ChangePassword(User.Identity!.Name!, vm))
            {
                ModelState.AddModelError("", "Current password is incorrect");
                return View(vm);
            }

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // ---------------- RESULT CACHE ----------------

        [HttpGet]
        [ServiceFilter(typeof(ResultCacheFilter))]
        public IActionResult GetCountriesCached()
        {
            Thread.Sleep(5000);
            return Ok(new
            {
                Time = DateTime.Now.ToString("HH:mm:ss"),
                Data = _accountService.GetCountries()
            });
        }

        // ---------------- HELPERS ----------------

        private async Task SendOtpAsync(User user)
        {
            var otp = Random.Shared.Next(100000, 999999).ToString();
            user.TwoFactorCode = otp;
            user.TwoFactorExpiry = DateTime.Now.AddMinutes(5);
            user.Is2FAVerified = false;

            if (!string.IsNullOrEmpty(user.Phone))
                await _smsService.SendSmsAsync(user.Phone, $"Your OTP is {otp}");

            _accountService.UpdateUser(user);
        }

        private void LoadDropdowns()
        {
            ViewBag.Countries = _accountService.GetCountries();
            ViewBag.HobbiesList = _accountService.GetHobbies();
        }

        // ---------------- AUTHORIZATION ----------------

        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard() => View();

        [Authorize(Policy = "Admin2FA")]
        public IActionResult Admin2FADashboard() => View();

        [Authorize(Policy = "ManagerExtraCheck")]
        public IActionResult ManagerDashboard() => View();
    }
}






/*Global Filter is used for :
 * ✅ Best for:

Model validation

Authorization checks

Logging

Auditing

❌ Avoid if:

Some actions don’t need validation (GET requests mostly)*/










































/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services.Interfaces;
using UserPortalValdiationsDBContext.ViewModels;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    [ServiceFilter(typeof(ActionValidationFilter))] // Centralized validation
    public class AccountController : Controller
    {
        private readonly IAccountService AccountService;
        private readonly IAuthService AuthService;
        private readonly IUserService UserService;
        private readonly ISmsService SmsService;

        public AccountController(
            IAccountService accountService,
            IAuthService authService,
            IUserService userService, ISmsService smsService)
        {
            AccountService = accountService;
            AuthService = authService;
            UserService = userService;
            SmsService = smsService;
        }

        // ------------------ REGISTER ------------------

        [HttpGet]
        public IActionResult Register()
        {
            var model = new RegisterViewModel
            {
                Roles = AccountService.GetRolesDropdown()       // for roles 
            };

            LoadDropdowns();
            return View(model);
        }

        [HttpPost]
        public IActionResult Register(RegisterViewModel model)
        {
            // 🔁 Reload dropdowns (VERY IMPORTANT)
            model.Roles = AccountService.GetRolesDropdown();
            LoadDropdowns();

            if (!ModelState.IsValid)
                return View(model);

            // Business validations
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

            string profileImagePath = "/images/default-profile.png";

            // Image upload
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/users");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName =
                    Guid.NewGuid() + Path.GetExtension(model.ProfilePhoto.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using var stream = new FileStream(filePath, FileMode.Create);
                model.ProfilePhoto.CopyTo(stream);

                profileImagePath = "/uploads/users/" + uniqueFileName;
            }

            // MAP VIEWMODEL → ENTITY
            var newUser = new User
            {
                Username = model.Username!,
                Email = model.Email!,
                Password = model.Password!,

                Age = model.Age,
                Gender = model.Gender,
                Country = model.Country,
                DateOfBirth = model.DateOfBirth,
                Phone = model.Phone,
                Department = model.Department,
                Hobbies = model.Hobbies != null ? string.Join(",", model.Hobbies) : null,
                AcceptTerms = model.AcceptTerms,

                RoleId = model.RoleId,                 //  DB ROLE
                ProfilePhotoPath = profileImagePath,

                Is2FAVerified = false,
                LastLoginAt = DateTime.Now,
                LastPasswordChangeAt = DateTime.Now
            };

            AccountService.RegisterUser(newUser);
            return RedirectToAction("Login");
        }


        // ------------------ LOGIN ------------------

        [HttpGet]
        public IActionResult Login() => View();

        /* [HttpPost]
         public async Task<IActionResult> Login(LoginViewModel model)
         {
             //  TEMPORARY: Force exception to test Error page
            // throw new Exception("TEST EXCEPTION: Login failed unexpectedly");

             var user = AccountService.Login(model.Username!, model.Password!);

             if (user == null)
             {
                 ModelState.AddModelError("", "Invalid username or password.");
                 return View(model);
             }

             user.LastLoginAt = DateTime.Now;
             AccountService.UpdateUser(user);

             await AuthService.SignInAsync(user, HttpContext);
             return RedirectToAction("Index", "Dashboard");

         }
        ----end of Comment..
        [HttpPost]
        public async Task<IActionResult> Login(LoginViewModel model)
        {
            var user = AccountService.Login(model.Username!, model.Password!);

            if (user == null)
            {
                ModelState.AddModelError("", "Invalid username or password.");
                return View(model);
            }

            user.LastLoginAt = DateTime.Now;
            AccountService.UpdateUser(user);

            // Admin login
            if (user.Role.Name == "Admin")
            {
                await SendOtpAsync(user);           //reusable method call for both manager and admin
                TempData["UserId"] = user.Id;
                return RedirectToAction("VerifyOtp");
            }

            // Manager login   (OTP+IP Check)
            if (user.Role.Name == "Manager")
            {
                await SendOtpAsync(user);       //reusable method call
                user.LastLoginIP = HttpContext.Connection.RemoteIpAddress?.ToString();
                AccountService.UpdateUser(user);

                TempData["UserId"] = user.Id;             // Store user temporarily   //used TempData  //used in VerifyOTP Method..
                return RedirectToAction("VerifyOtp");
            }
            // Normal user login
            await AuthService.SignInAsync(user, HttpContext);
            return RedirectToAction("Index", "Dashboard");
}

        /*Temp Data: Perfect for things like OTP verification, where you need to temporarily store UserId from login to the OTP page.

        TempData is automatically cleared after it is read, so it won’t linger in memory.

        // ------------------ LOGOUT ------------------

        [HttpPost]
        public async Task<IActionResult> Logout()
        {
            await AuthService.SignOutAsync(HttpContext);
            return RedirectToAction("Login");
        }

        // ------------------ EDIT PROFILE ------------------

        [HttpGet]
        public IActionResult EditProfile()
        {
            var username = User.Identity?.Name;
            if (string.IsNullOrEmpty(username))
                return RedirectToAction("Login");

            var vm = UserService.GetEditProfileData(username);
            if (vm == null)
                return NotFound();

            return View(vm);
        }

        [HttpPost]
        public IActionResult EditProfile(EditProfileViewModel vm)
        {
            var result = UserService.UpdateUserProfile(User.Identity!.Name!, vm);

            if (!result)
            {
                ModelState.AddModelError("", "Unable to update profile");
                return View(vm);
            }

            TempData["Success"] = "Profile updated successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // ------------------ CHANGE PASSWORD ------------------

        [HttpGet]
        public IActionResult ChangePassword() => View();

        [HttpPost]
        public IActionResult ChangePassword(ChangePasswordViewModel vm)
        {
            var result = UserService.ChangePassword(User.Identity!.Name!, vm);

            if (!result)
            {
                ModelState.AddModelError("", "Current password is incorrect");
                return View(vm);
            }

            TempData["Success"] = "Password changed successfully!";
            return RedirectToAction("Index", "Dashboard");
        }

        // ------------------ HELPERS ------------------

        private void LoadDropdowns()
        {
            ViewBag.Countries = AccountService.GetCountries();
            ViewBag.HobbiesList = AccountService.GetHobbies();
        }
     
        // result cache
        [HttpGet]
        [ServiceFilter(typeof(ResultCacheFilter))]
        public IActionResult GetCountriesCached()
        {
            // Simulate expensive operation
            Thread.Sleep(5000); // 5 seconds delay

            var countries = AccountService.GetCountries();

            return Ok(new
            {
                Time = DateTime.Now.ToString("HH:mm:ss"),
                Data = countries
            });
        }

        //OTP

        //verify OTP
        [HttpGet]
        public IActionResult VerifyOtp() => View();


        [HttpPost]
        public async Task<IActionResult> VerifyOtp(string otp)
        {
            if (!TempData.TryGetValue("UserId", out var userIdObj))
                return RedirectToAction("Login");           //// if missing, go back

            var userId = Convert.ToInt32(userIdObj);
            var user = AccountService.GetUserById(userId);

            if (user == null || user.TwoFactorCode != otp || user.TwoFactorExpiry < DateTime.Now)
            {
                ModelState.AddModelError("", "Invalid or expired OTP.");
                return View();
            }

            user.Is2FAVerified = true;
            user.TwoFactorCode = null;
            user.TwoFactorExpiry = null;
            AccountService.UpdateUser(user);

            // Add claims for 2FA verification
            var claims = new List<Claim>
    {
        new Claim("Is2FAVerified", "true"),
        new Claim(ClaimTypes.Role, user.Role.Name)
    };
            if (!string.IsNullOrEmpty(user.LastLoginIP))
                claims.Add(new Claim("LoginIP", user.LastLoginIP));

            var identity = new ClaimsIdentity(claims, "login");
            var principal = new ClaimsPrincipal(identity);

            await HttpContext.SignInAsync(principal);

            return RedirectToAction("Index", "Dashboard");
        }


        // Helper method to reduce the same code twice for both code for Admin and Manager:
        private async Task SendOtpAsync(User user)
        {
            var otp = new Random().Next(100000, 999999).ToString();
            user.TwoFactorCode = otp;
            user.TwoFactorExpiry = DateTime.Now.AddMinutes(5);   //after 5 minutes it will expire
            user.Is2FAVerified = false;

            await SmsService.SendSmsAsync(user.Phone!, $"Your OTP is {otp}");
            AccountService.UpdateUser(user);
        }




        [Authorize(Roles = "Admin")]
        public IActionResult AdminDashboard() => View();

        [Authorize(Policy = "Admin2FA")]
        public IActionResult Admin2FADashboard() => View();

        [Authorize(Policy = "ManagerExtraCheck")]
        public IActionResult ManagerDashboard() => View();

    }
}





*/




















/*using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Filters;
using UserPortalValdiationsDBContext.Interfaces;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.ViewModels;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Controllers
{
    [ServiceFilter(typeof(LoggingActionFilter))]
    [ServiceFilter(typeof(AuditingFilter))]
    [ServiceFilter(typeof(ErrorHandlingFilter))]
    public class AccountController : Controller
    {
        private readonly IAccountService AccountService;      
        private readonly IAuthService AuthService;
        private readonly IUserService UserService;

        public AccountController(IAccountService accountService, IAuthService authService, IUserService userService)
        {
            AccountService = accountService;
            AuthService = authService;
            UserService = userService;
        }


        // ------------------ REGISTER ------------------

        [HttpGet]
        public IActionResult Register()
        {
            LoadDropdowns();
            return View(new RegisterViewModel());
        }

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

            string profileImagePath = "/images/default-profile.png";

            //  IMAGE UPLOAD LOGIC
            if (model.ProfilePhoto != null && model.ProfilePhoto.Length > 0)
            {
                string uploadsFolder = Path.Combine(
                    Directory.GetCurrentDirectory(),
                    "wwwroot/uploads/users");

                if (!Directory.Exists(uploadsFolder))
                    Directory.CreateDirectory(uploadsFolder);

                string uniqueFileName =
                    Guid.NewGuid() + Path.GetExtension(model.ProfilePhoto.FileName);

                string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                using (var stream = new FileStream(filePath, FileMode.Create))
                {
                    model.ProfilePhoto.CopyTo(stream);
                }

                profileImagePath = "/uploads/users/" + uniqueFileName;
            }

            var newUser = new User
            {
                Username = model.Username,
                Email = model.Email,
                Password = model.Password,
                Age = model.Age,
                Gender = model.Gender,
                Country = model.Country,
                DateOfBirth = model.DateOfBirth,
                Phone = model.Phone,
                Hobbies = model.Hobbies != null ? string.Join(",", model.Hobbies) : "",
                Department = model.Department,
                AcceptTerms = model.AcceptTerms,
                Roles = model.Roles,

             //profile path
                ProfilePhotoPath = profileImagePath,

                LastLoginAt = DateTime.Now,
                LastPasswordChangeAt = DateTime.Now
            };

            AccountService.RegisterUser(newUser);

            return RedirectToAction("Login");
        }

        // ------------------ LOGIN ------------------

        [HttpGet]
        public IActionResult Login() => View();

        /* [HttpPost]
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
        */
/*[HttpPost]              // add authentication and claims in hte authservice 
public async Task<IActionResult> Login(LoginViewModel model)
{
    if (!ModelState.IsValid)
        return View(model);

    User? user = AccountService.Login(model.Username!, model.Password!);

    if (user == null)
    {
        ModelState.AddModelError("", "Invalid username or password.");
        return View(model);
    }

    user.LastLoginAt = DateTime.Now;
    AccountService.UpdateUser(user);

    await AuthService.SignInAsync(user, HttpContext);   // Moved to service

    return RedirectToAction("Index", "Dashboard");
}
--------------end of comment
[HttpPost]
public async Task<IActionResult> Login(LoginViewModel model)
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

    await AuthService.SignInAsync(user, HttpContext);

    return RedirectToAction("Index", "Dashboard");
}


// ------------------ LOGOUT ------------------

/* [HttpPost]
 public async Task<IActionResult> Logout()
 {
     await HttpContext.SignOutAsync();
     return RedirectToAction("Login");
 }
---end of comment2

[HttpPost]
public async Task<IActionResult> Logout()
{
    await AuthService.SignOutAsync(HttpContext);
    return RedirectToAction("Login");
}


// ------------------ LOAD DROPDOWNS ------------------

private void LoadDropdowns()
{
    ViewBag.Countries = AccountService.GetCountries();
    ViewBag.HobbiesList = AccountService.GetHobbies();
}

//used for edit profile in the side bar..

[HttpGet]
public IActionResult EditProfile()
{
    var username = User.Identity?.Name;
    if (string.IsNullOrEmpty(username))
        return RedirectToAction("Login");

    var vm = UserService.GetEditProfileData(username);
    if (vm == null)
        return NotFound();

    return View(vm);
}

[HttpPost]
public IActionResult EditProfile(EditProfileViewModel vm)
{
    if (!ModelState.IsValid)
        return View(vm);

    var result = UserService.UpdateUserProfile(User.Identity!.Name!, vm);

    if (!result)
    {
        ModelState.AddModelError("", "Unable to update profile");
        return View(vm);
    }

    TempData["Success"] = "Profile updated successfully!";
    return RedirectToAction("Index", "Dashboard");
}

[HttpGet]
public IActionResult ChangePassword()
{
    return View();
}

[HttpPost]
public IActionResult ChangePassword(ChangePasswordViewModel vm)
{
    if (!ModelState.IsValid)
        return View(vm);

    var result = UserService.ChangePassword(User.Identity!.Name!, vm);

    if (!result)
    {
        ModelState.AddModelError("", "Current password is incorrect");
        return View(vm);
    }

    TempData["Success"] = "Password changed successfully!";
    return RedirectToAction("Index", "Dashboard");
}


}
}

*/


































