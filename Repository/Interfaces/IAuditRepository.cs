using UserPortalValdiationsDBContext.Models;
using System.Threading.Tasks;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IAuditRepository
    {
        Task AddAsync(AuditLog auditLog);
    }
}
