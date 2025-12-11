using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Filters;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IAuditService
    {
        Task SaveAsync(AuditEntry entry);
    }
}
