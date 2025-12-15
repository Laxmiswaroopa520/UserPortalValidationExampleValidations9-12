using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Data; // adjust to your Data namespace
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class UserActivityService : IUserActivityService
    {
        private readonly ApplicationDbContext _db;

        public UserActivityService(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<IEnumerable<DateTime>> GetRecentLoginsAsync(int userId, int count)
        {
            var user = await _db.Users.FindAsync(userId);
            return user?.LastLoginAt != null ? new List<DateTime> { user.LastLoginAt.Value } : new List<DateTime>();
        }

        public async Task<Dictionary<string, int>> GetUserCountByDepartmentAsync()
        {
            return await _db.Users
                .GroupBy(u => u.Department)
                .Select(g => new { g.Key, Count = g.Count() })
                .ToDictionaryAsync(x => x.Key!, x => x.Count);
        }
    }
}