using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class NoSequentialDigitsAttribute : ValidationAttribute, IClientModelValidator
    {
        public NoSequentialDigitsAttribute()
        {
            ErrorMessage = "Password cannot contain sequential digits like 1234, 4567 or 7890.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            string v = value.ToString();

            if (v.Contains("1234")) return false;
            if (v.Contains("4567")) return false;
            if (v.Contains("7890")) return false;

            return true;
        }
        // CLIENT-SIDE VALIDATION SUPPORT  
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-noSequential"] = ErrorMessage;
        }
    }
}
