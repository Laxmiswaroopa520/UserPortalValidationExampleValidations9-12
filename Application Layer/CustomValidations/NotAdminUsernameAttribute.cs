using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class NotAdminUsernameAttribute : ValidationAttribute, IClientModelValidator
    {
        public NotAdminUsernameAttribute()
        {
            ErrorMessage = "Username cannot be admin.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;
            return value?.ToString().ToLower() != "admin";
          //  return value != null && value.ToLower() != "admin";
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";

            // MUST MATCH THE ADAPTER NAME EXACTLY
            context.Attributes["data-val-notadminusername"] = ErrorMessage ?? "Invalid Username.";
        }
    }
}

/*using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class NotAdminUsernameAttribute : ValidationAttribute, IClientModelValidator
    {
        public NotAdminUsernameAttribute()
        {
            ErrorMessage = "Username cannot be admin, root, or system.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            string val = value.ToString().ToLower();

            return val != "admin" && val != "root" && val != "system";
        }

        // -----------------------------
        // CLIENT-SIDE VALIDATION SUPPORT
        // -----------------------------
        // IMPORTANT: Add client-side rule

        
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";

            // Error message
            context.Attributes["data-val-notadminusername"] = ErrorMessage;

            // This enables the rule (must be a DIFFERENT name)
            context.Attributes["data-val-notadminusername-flag"] = "true";
        }


    }
}
*/