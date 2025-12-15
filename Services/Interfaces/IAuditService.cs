using System.Threading.Tasks;
using UserPortalValdiationsDBContext.DTO;

namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IAuditService
    {
        Task SaveAsync(AuditEntry entry);
    }
}
