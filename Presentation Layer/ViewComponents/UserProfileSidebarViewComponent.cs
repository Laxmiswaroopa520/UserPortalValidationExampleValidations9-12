using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Services.Interfaces;

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
        {
            return Content("No user logged in.");
        }

        var user = _userService.GetUserByUsername(userId);
        if (user == null)
        {
            return Content("User not found.");
        }

        return View(user);
    }
}
