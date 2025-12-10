namespace UserPortalValdiationsDBContext.ViewComponents
{
    using global::UserPortalValdiationsDBContext.Services;
    using Microsoft.AspNetCore.Mvc;
    using System.Threading.Tasks;
    using UserPortalValdiationsDBContext.Services;

   
        public class DepartmentCountViewComponent : ViewComponent
        {
            private readonly IUserActivityService _svc;

            public DepartmentCountViewComponent(IUserActivityService svc)
            {
                _svc = svc;
            }

            public async Task<IViewComponentResult> InvokeAsync()
            {
                var counts = await _svc.GetUserCountByDepartmentAsync();
                return View(counts);
            }
        }
    }
