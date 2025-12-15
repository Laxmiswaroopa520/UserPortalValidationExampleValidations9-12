//Because it implements IActionFilter, it runs at: Before the action method executes::(OnActionExecuting)
//So the action never runs if the model is invalid.
/// // ensures ModelState is valid; if not - returns the view with same model or BadRequest for APIs
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Microsoft.AspNetCore.Mvc.ModelBinding;

namespace UserPortalValdiationsDBContext.Filters
{
    public class ActionValidationFilter : IActionFilter
    {
        public void OnActionExecuting(ActionExecutingContext context)
        {
            // ✅ Skip GET requests
            if (context.HttpContext.Request.Method == "GET")
                return;

            if (!context.ModelState.IsValid)
            {
                // ✅ API requests
                if (context.HttpContext.Request.Path.StartsWithSegments("/api"))
                {
                    context.Result = new BadRequestObjectResult(context.ModelState);
                    return;
                }

                // ✅ MVC requests
                var controller = context.Controller as Controller;

                if (controller != null)
                {
                    context.Result = new ViewResult
                    {
                        ViewName = context.ActionDescriptor.RouteValues["action"],
                        ViewData = new Microsoft.AspNetCore.Mvc.ViewFeatures.ViewDataDictionary(
                            new EmptyModelMetadataProvider(),
                            context.ModelState)
                        {
                            Model = context.ActionArguments.Values.FirstOrDefault()
                        },
                        TempData = controller.TempData
                    };
                }
            }
        }

        public void OnActionExecuted(ActionExecutedContext context) { }
    }
}
















/*
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
*/

/*👉 This filter automatically checks model validation before any controller action runs and handles invalid data in a centralized way.

So instead of writing this in every action:

if (!ModelState.IsValid)
{
    return View(model);
}


You write it once, in this filter.*/

//Eliminates Repeated Code
//if (!ModelState.IsValid)
//{
//   return View(model);
//}
//instead of writing this everywhere use this filter instead

//3.
/*ou can confidently say:

“I used an Action Filter to centralize model validation logic, reduce duplication across controllers, and handle both MVC and API validation responses consistently.”

🔥 That’s a senior-level explanation.*/


/*4. When SHOULD We Use ActionValidationFilter?

Use it for:

Register (POST)

Login (POST)

EditProfile (POST)

ChangePassword (POST)

👉 Basically any action that uses ViewModels + ModelState
*/