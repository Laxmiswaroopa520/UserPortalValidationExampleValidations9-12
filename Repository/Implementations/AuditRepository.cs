using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class AuditRepository : IAuditRepository
    {
        private readonly ApplicationDbContext _db;

        public AuditRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task AddAsync(AuditLog auditLog)
        {
            _db.AuditLogs.Add(auditLog);
            await _db.SaveChangesAsync();
        }
    }
}
