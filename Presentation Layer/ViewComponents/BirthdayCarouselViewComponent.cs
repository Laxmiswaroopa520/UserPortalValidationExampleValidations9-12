using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.ViewComponents
{
    public class BirthdayCarouselViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public BirthdayCarouselViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            // var users = _userService.GetWeeklyBirthdays();
            var users = _userService.GetWeeklyBirthdays();

            return View(users);
        }
    }

}
