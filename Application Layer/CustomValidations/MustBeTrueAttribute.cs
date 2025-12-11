using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.ComponentModel.DataAnnotations;

namespace UserPortalValdiationsDBContext.CustomValidations
{

    public class MustBeTrueAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object? value)
        {
            return value is bool b && b == true;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-mustbetrue", ErrorMessage ?? "You must accept this.");
        }
    }
}