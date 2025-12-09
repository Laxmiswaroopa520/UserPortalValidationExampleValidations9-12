namespace UserPortalValdiationsDBContext.CustomValidations
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

    public class PastDateAttribute : ValidationAttribute, IClientModelValidator
    {
        public override bool IsValid(object? value)
        {
            if (value == null)
                return false;

            DateTime date = (DateTime)value;

            return date <= DateTime.Today;
        }

        public void AddValidation(ClientModelValidationContext context)
        {
            // Enables jQuery client-side validation
            context.Attributes.Add("data-val", "true");
            context.Attributes.Add("data-val-pastdate", ErrorMessage ?? "Date cannot be in the future.");

            // Compare with today's date on client side
            context.Attributes.Add("data-val-pastdate-today", DateTime.Today.ToString("yyyy-MM-dd"));
        }
    }

}
