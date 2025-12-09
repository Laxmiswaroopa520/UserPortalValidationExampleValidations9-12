using System.ComponentModel.DataAnnotations;
using System.Text.RegularExpressions;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class PhoneNumberAttribute : ValidationAttribute, IClientModelValidator
    {
        public PhoneNumberAttribute()
        {
            ErrorMessage = "Phone number must be a valid 10-digit Indian number (starting with 6-9).";
        }

        public override bool IsValid(object? value)
        {
            if (value == null)
                return true;

            string phone = value.ToString();

            return Regex.IsMatch(phone, @"^[6-9]\d{9}$");
        }

        // CLIENT-SIDE VALIDATION SUPPORT
      
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-phonenumber"] = ErrorMessage;
        }
    }
}
