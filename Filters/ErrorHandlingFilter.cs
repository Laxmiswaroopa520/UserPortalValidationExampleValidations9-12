using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;

namespace UserPortalValdiationsDBContext.Filters
{
    public class ErrorHandlingFilter : IExceptionFilter
    {
        private readonly ILogger<ErrorHandlingFilter> _logger;
        public ErrorHandlingFilter(ILogger<ErrorHandlingFilter> logger)
        {
            _logger = logger;
        }

        public void OnException(ExceptionContext context)
        {
            var ex = context.Exception;
            _logger.LogError(ex, "Unhandled exception in {Path}", context.HttpContext.Request.Path);

            // For API: return JSON, for MVC: you might redirect to an Error view
            if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Result = new ObjectResult(new { error = "An error occurred. Please contact admin." })
                {
                    StatusCode = 500
                };
            }
            else
            {
                var routeData = new Microsoft.AspNetCore.Routing.RouteValueDictionary(new
                {
                    controller = "Home",
                    action = "Error"
                });
                context.Result = new RedirectToRouteResult(routeData);
            }

            context.ExceptionHandled = true;
        }
    }
}
