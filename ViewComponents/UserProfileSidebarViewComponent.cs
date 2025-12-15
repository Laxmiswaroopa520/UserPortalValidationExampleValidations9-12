using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.ViewComponents
{
    public class UserProfileSidebarViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public UserProfileSidebarViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke(string? userId)
        {
            if (string.IsNullOrEmpty(userId))
                return Content("");

            var vm = _userService.GetUserProfileSidebar(userId);

            if (vm == null)
                return Content("");

            return View(vm);
        }
    }
}
