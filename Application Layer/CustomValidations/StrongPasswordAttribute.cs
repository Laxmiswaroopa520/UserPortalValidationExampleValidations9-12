using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.RegularExpressions;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class StrongPasswordAttribute : ValidationAttribute, IClientModelValidator
    {
        public StrongPasswordAttribute()
        {
            ErrorMessage = "Password must be at least 8 characters, include uppercase, lowercase, digit, special character, no spaces, and no sequential digits.";
        }

        public override bool IsValid(object? value)
        {
            if (value == null) return true;

            string password = value?.ToString();

            // 1. Check length
            if (password.Length < 8) return false;

            // 2. No spaces
            if (password.Contains(" ")) return false;

            // 3. Uppercase, lowercase, digit, special character
            if (!Regex.IsMatch(password, @"[A-Z]")) return false;
            if (!Regex.IsMatch(password, @"[a-z]")) return false;
            if (!Regex.IsMatch(password, @"\d")) return false;              //must contain atleast one digit..
            if (!Regex.IsMatch(password, @"[!@#$%^&*(),.?""':{}|<>]")) return false;

            // 4. No sequential digits
            string[] sequential = { "1234", "4567", "7890" };
            foreach (var seq in sequential)
            {
                if (password.Contains(seq)) return false;
            }

            return true;
        }

        // Client-side support
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-strongpassword"] = ErrorMessage??"Invlaid Password";
        }
    }
}
