using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System;
using System.Threading.Tasks;
using UserPortalValdiationsDBContext.DTO;   // updated namespace
using UserPortalValdiationsDBContext.Services.Interfaces;

namespace UserPortalValdiationsDBContext.Filters
{
    public class AuditingFilter : IAsyncActionFilter            //it implenets asyncaction filters so it runs before and after method exectures..
    {
        private readonly IAuditService _auditService;
        private readonly ILogger<AuditingFilter> _logger;

        public AuditingFilter(
            IAuditService auditService,
            ILogger<AuditingFilter> logger)
        {
            _auditService = auditService;
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(
            ActionExecutingContext context,
            ActionExecutionDelegate next)
        {
            var startedAt = DateTime.UtcNow;            //notes start time

            var executedContext = await next();             //controller runs,service run and database calls happen

            var endedAt = DateTime.UtcNow;

            var audit = new AuditEntry              //building the audit record
            {
                UserName = context.HttpContext.User?.Identity?.Name ?? "Anonymous",
                Action = context.ActionDescriptor.DisplayName,
                StartedAt = startedAt,
                EndedAt = endedAt,
                HttpMethod = context.HttpContext.Request.Method,
                Path = context.HttpContext.Request.Path,
                Success = executedContext.Exception == null
            };

            try
            {
                await _auditService.SaveAsync(audit);           //saves audit data safely
            }
            catch (Exception ex)
            {
                _logger.LogWarning(ex, "Audit logging failed");
            }
        }
    }
}

/*🧠 High-Level Purpose

The purpose of this Auditing Filter is to:

 Track user activity
 Record important actions
 Measure execution time
 Detect failures or exceptions
Support security, compliance, and debugging*/