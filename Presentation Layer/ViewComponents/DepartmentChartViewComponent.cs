//Department BAR Chart + Dynamic Filtering
//1.Department Chart with Dynamic Filtering(when you click on graph it will filter the users from the database table..
using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.ViewComponents
{
    public class DepartmentChartViewComponent : ViewComponent
    {
        private readonly IUserService _userService;

        public DepartmentChartViewComponent(IUserService userService)
        {
            _userService = userService;
        }

        public IViewComponentResult Invoke()
        {
            var data = _userService.GetUserCountByDepartmentChart();
            return View(data);
        }
    }

}


/*Department bar chart loads

Clicking a bar filters user table

All existing features continue working

No enum or casting issues
*/