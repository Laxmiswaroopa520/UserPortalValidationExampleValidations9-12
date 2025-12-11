using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace UserPortalValdiationsDBContext.Filters
{
    // ensures ModelState is valid; if not - returns the view with same model or BadRequest for APIs
    public class ActionValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ModelState.IsValid)
            {
                if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                }
                else
                {
                    // If action has argument named "model", try to return the same view with that model
                    var model = context.ActionArguments.Values.FirstOrDefault();
                    context.Result = new ViewResult
                    {
                        ViewData = context.Controller is Microsoft.AspNetCore.Mvc.Controller ctrl ? ctrl.ViewData : new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(new Microsoft.AspNetCore.Mvc.ModelBinding.EmptyModelMetadataProvider(), context.ModelState),
                        TempData = context.Controller is Microsoft.AspNetCore.Mvc.Controller control ? control.TempData : null,
                        ViewName = null // fall back to default view
                    };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
