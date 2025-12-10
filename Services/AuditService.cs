using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Filters;

namespace UserPortalValdiationsDBContext.Services
{
    public class AuditService : IAuditService
    {
        private readonly ILogger<AuditService> _logger;
        public AuditService(ILogger<AuditService> logger)
        {
            _logger = logger;
        }

        public Task SaveAsync(AuditEntry entry)
        {
            // For POC: write to log. For production: persist to DB table.
            _logger.LogInformation("AUDIT: {User} {Action} {Path} {Started} - {Ended} Success:{Success}",
                entry.UserName, entry.Action, entry.Path, entry.StartedAt, entry.EndedAt, entry.Success);

            return Task.CompletedTask;
        }
    }
}
