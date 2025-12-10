using Microsoft.AspNetCore.Mvc;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.ViewModels;
using Microsoft.EntityFrameworkCore;
public class UserProfileSidebarViewComponent : ViewComponent
{
    private readonly ApplicationDbContext _db;
    private readonly IUserActivityService _userActivity;

    public UserProfileSidebarViewComponent(ApplicationDbContext db, IUserActivityService userActivity)
    {
        _db = db;
        _userActivity = userActivity;
    }

    public async Task<IViewComponentResult> InvokeAsync(int userId)
    {
        var user = await _db.Users.AsNoTracking().FirstOrDefaultAsync(u => u.Id == userId);
        if (user == null)
            return View("Default", null);

        var recent = await _userActivity.GetRecentLoginsAsync(userId, 5);

        var vm = new UserProfileSidebarViewModel
        {
            UserId = user.Id,
            Name = user.Username,
            Email = user.Email,
            ProfilePhotoPath = user.ProfilePhotoPath,
            RecentLogins = recent.ToList(),
            LastPasswordChangeAt = user.LastPasswordChangeAt,
            Roles = (user.Roles ?? "").Split(',', StringSplitOptions.RemoveEmptyEntries).ToList(),
            UpcomingBirthday = NextBirthdayInfo(user.DateOfBirth)
        };

        return View(vm);
    }

    private BirthdayInfo NextBirthdayInfo(DateTime dob)
    {
        var now = DateTime.UtcNow.Date;
        var thisYears = new DateTime(now.Year, dob.Month, dob.Day);
        var next = thisYears >= now ? thisYears : thisYears.AddYears(1);
        var days = (next - now).Days;

        return new BirthdayInfo { Date = next, DaysUntil = days };
    }
}
