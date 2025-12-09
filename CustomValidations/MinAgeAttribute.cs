using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class MinAgeAttribute : ValidationAttribute, IClientModelValidator
    {
        public int MinAge { get; }

        public MinAgeAttribute(int age)
        {
            MinAge = age;
            ErrorMessage = $"Age must be at least {age}.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return false;
            return Convert.ToInt32(value) >= MinAge;
        }


        // CLIENT-SIDE VALIDATION SUPPORT
 
        public void AddValidation(ClientModelValidationContext context)
        {
            // Enable validation
            context.Attributes["data-val"] = "true";

            // Error message
            context.Attributes["data-val-minage"] = ErrorMessage?? "Invalid age.";

            // Pass MinAge value to jQuery adapter
            context.Attributes["data-val-minage-age"] = MinAge.ToString();
        }
    }
}
