using UserPortalValdiationsDBContext.Enums;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IUserActivityRepository
    {
        Task<DateTime?> GetLastLoginAsync(int userId);

        Task<List<(Departments? Department, int Count)>>
            GetUserCountByDepartmentAsync();
    }
}

/*Note:

We return enum, not string

Service decides how to display it
*/