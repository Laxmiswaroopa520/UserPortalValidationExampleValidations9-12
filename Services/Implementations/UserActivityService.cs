using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class UserActivityService : IUserActivityService
    {
        private readonly IUnitOfWork _unit;

        public UserActivityService(IUnitOfWork unit)
        {
            _unit = unit;
        }

        public async Task<IEnumerable<DateTime>> GetRecentLoginsAsync(
            int userId, int count)
        {
            var lastLogin = await _unit.UserActivities
                .GetLastLoginAsync(userId);

            if (!lastLogin.HasValue)
                return Enumerable.Empty<DateTime>();

            // Ready for future expansion (login history table)
            return new List<DateTime> { lastLogin.Value };
        }

        public async Task<Dictionary<string, int>>
            GetUserCountByDepartmentAsync()
        {
            var data = await _unit.UserActivities
                .GetUserCountByDepartmentAsync();

            return data.ToDictionary(
                x => x.Department?.ToString() ?? "Unknown",
                x => x.Count
            );
        }
    }
}
