using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace UserPortalValdiationsDBContext.Services
{
    public interface IUserActivityService
    {
        Task<IEnumerable<DateTime>> GetRecentLoginsAsync(int userId, int count);
        Task<Dictionary<string, int>> GetUserCountByDepartmentAsync();
    }
}