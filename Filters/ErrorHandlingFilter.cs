using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.Mvc.ViewFeatures;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using System;

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

            // Generate unique error reference id
            var errorId = Guid.NewGuid().ToString();

            // Log full exception with error id
            _logger.LogError(
                ex,
                "Unhandled exception occurred. ErrorId: {ErrorId}, Path: {Path}",
                errorId,
                context.HttpContext.Request.Path
            );

            // API requests → JSON response
            if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
            {
                context.Result = new ObjectResult(new
                {
                    message = "An unexpected error occurred.",
                    errorId = errorId
                })
                {
                    StatusCode = 500
                };
            }
            else
            {
                // MVC requests → Shared Error Page
                context.Result = new ViewResult
                {
                    ViewName = "~/Views/Shared/Error.cshtml",
                    ViewData = new ViewDataDictionary(
                        new EmptyModelMetadataProvider(),
                        context.ModelState)
                    {
                        { "ErrorId", errorId }
                    }
                };
            }

            // Mark exception as handled
            context.ExceptionHandled = true;
        }
    }
}



//for checking error handling example is inside login method(post) in the account controller.


/*Exception thrown
⬇
ErrorHandlingFilter catches it
⬇
Generates unique ErrorId
⬇
Logs error + ErrorId
⬇
Shows Shared Error page
⬇
User sees ErrorId
Admin finds error using logs
*/


/*“We use a centralized shared error page with a clean UI and a unique error reference ID.
This helps users understand the issue without exposing sensitive details, while allowing support teams to trace the exact error through logs.
*/

/*“I use an Exception Filter to globally handle unhandled runtime errors, log them securely, and return safe responses for both MVC and API requests. This avoids exposing sensitive details and keeps controllers clean.”


✔ Executes only on exceptions
✔ Centralized error handling
✔ Logs errors for developers
✔ Safe UI & API responses
✔ Works perfectly with your controller stack
*/










/*using Microsoft.AspNetCore.Mvc;
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
*/



