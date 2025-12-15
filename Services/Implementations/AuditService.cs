using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UserPortalValdiationsDBContext.DTO;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Services.Implementations
{
    public class AuditService : IAuditService
    {
        private readonly IAuditRepository _repository;
        private readonly ILogger<AuditService> _logger;

        public AuditService(IAuditRepository repository, ILogger<AuditService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        public async Task SaveAsync(AuditEntry entry)
        {
            try
            {
                var log = new AuditLog
                {
                    Action = entry.Action,
                    EntityName = entry.Path,
                    Timestamp = DateTime.UtcNow
                };

                await _repository.AddAsync(log);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to save audit log to DB");
            }
        }
    }
}
