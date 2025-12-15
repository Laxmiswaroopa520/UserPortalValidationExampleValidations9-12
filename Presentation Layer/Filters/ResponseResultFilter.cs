using Microsoft.AspNetCore.Mvc.Filters;

namespace UserPortalValdiationsDBContext.Filters
{
    // Example: add headers or wrap responses
    public class ResponseResultFilter : IResultFilter
    {
        public void OnResultExecuting(ResultExecutingContext context)
        {
            context.HttpContext.Response.Headers["X-App-Version"] = "1.0.0";
        }

        public void OnResultExecuted(ResultExecutedContext context) { }
    }
}
