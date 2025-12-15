using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public UserActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastLoginAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.LastLoginAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<(Departments? Department, int Count)>> GetUserCountByDepartmentAsync()
        {
            // Step 1: Query anonymous type
            var result = await _context.Users
                .GroupBy(u => u.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .ToListAsync();

            // Step 2: Project to tuple after materialization
            return result.Select(x => (x.Department, x.Count)).ToList();
        }
    }
}
















/*
using Microsoft.EntityFrameworkCore;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class UserActivityRepository : IUserActivityRepository
    {
        private readonly ApplicationDbContext _context;

        public UserActivityRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<DateTime?> GetLastLoginAsync(int userId)
        {
            return await _context.Users
                .Where(u => u.Id == userId)
                .Select(u => u.LastLoginAt)
                .FirstOrDefaultAsync();
        }

        public async Task<List<(Departments? Department, int Count)>>
            GetUserCountByDepartmentAsync()
        {
            return await _context.Users
                .GroupBy(u => u.Department)
                .Select(g => new
                {
                    Department = g.Key,
                    Count = g.Count()
                })
                .Select(x => (x.Department, x.Count))//error here
                .ToListAsync();
        }
    }
}
Ah! I see the exact problem. The error:

An expression tree may not contain a tuple literal.


happens because Entity Framework Core cannot translate tuple literals like (x.Department, x.Count) directly into SQL in a LINQ-to-Entities query. EF Core can only translate anonymous types or entity types into SQL.

We need to return a list of a concrete type or an anonymous type, then project to a tuple after ToListAsync().
*/