using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using System.Diagnostics;
using System.Threading.Tasks;

namespace UserPortalValdiationsDBContext.Filters
{
    public class LoggingActionFilter : IAsyncActionFilter
    {
        private readonly ILogger<LoggingActionFilter> _logger;
        public LoggingActionFilter(ILogger<LoggingActionFilter> logger)
        {
            _logger = logger;
        }

        public async Task OnActionExecutionAsync(ActionExecutingContext context, ActionExecutionDelegate next)
        {
            var sw = Stopwatch.StartNew();
            var actionName = context.ActionDescriptor.DisplayName;
            _logger.LogInformation("Starting {Action}", actionName);

            if (!context.ModelState.IsValid)
            {
                _logger.LogWarning("{Action} - ModelState invalid: {Errors}", actionName,
                    context.ModelState.Values.SelectMany(v => v.Errors).Select(e => e.ErrorMessage));
            }

            var executed = await next();

            sw.Stop();
            _logger.LogInformation("Finished {Action} in {Elapsed}ms, StatusCode: {StatusCode}",
                actionName, sw.ElapsedMilliseconds, context.HttpContext.Response?.StatusCode);
        }
    }
}
