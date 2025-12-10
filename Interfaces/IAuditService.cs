using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Filters;

namespace UserPortalValdiationsDBContext.Services
{
    public interface IAuditService
    {
        Task SaveAsync(AuditEntry entry);
    }
}
