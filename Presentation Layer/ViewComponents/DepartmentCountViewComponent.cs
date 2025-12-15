using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.ViewComponents
{
    public class DepartmentCountViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public DepartmentCountViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            var data = _userService.GetUserCountByDepartment();
            return View(data);
        }
    }
}
