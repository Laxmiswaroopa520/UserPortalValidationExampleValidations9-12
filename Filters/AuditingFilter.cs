using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Services;
using UserPortalValdiationsDBContext.Interfaces;

namespace UserPortalValdiationsDBContext.Filters
{
    public class AuditingFilter : IAsyncActionFilter
    {
        private readonly IAuditService _audit;
        private readonly ILogger<AuditingFilter> _logger;

        public AuditingFilter(IAuditService auditService, ILogger<AuditingFilter> logger)
        {
            _audit = auditService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var user = context.HttpContext.User?.Identity?.Name ?? "Anonymous";
            var action = context.ActionDescriptor.DisplayName;
            var started = DateTime.UtcNow;

            var executed = await next();

            var ended = DateTime.UtcNow;
            var audit = new AuditEntry
            {
                UserName = user,
                Action = action,
                StartedAt = started,
                EndedAt = ended,
                HttpMethod = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
                Success = executed.Exception == null
            };

            try
            {
                await _audit.SaveAsync(audit);
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Failed to persist audit entry");
            }
        }
    }

    // small DTO, can move to Models/AuditEntry.cs
    public class AuditEntry
    {
        public string? UserName { get; set; }
        public string? Action { get; set; }
        public DateTime StartedAt { get; set; }
        public DateTime EndedAt { get; set; }
        public string? HttpMethod { get; set; }
        public string? Path { get; set; }
        public bool Success { get; set; }
    }
}
