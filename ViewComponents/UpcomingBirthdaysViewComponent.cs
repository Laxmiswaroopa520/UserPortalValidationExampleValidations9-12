using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Interfaces;

namespace UserPortalValdiationsDBContext.ViewComponents
{
    public class UpcomingBirthdaysViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public UpcomingBirthdaysViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            // Assuming IUserService has a method to get users with upcoming birthdays
            var upcomingBirthdays = _userService.GetUpcomingBirthdays();
            return View(upcomingBirthdays); // Default view: /Views/Shared/Components/UpcomingBirthdays/Default.cshtml
        }
    }
}
