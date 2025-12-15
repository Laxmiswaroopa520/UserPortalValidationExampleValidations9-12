using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;
using System.Text.RegularExpressions;

namespace UserPortalValdiationsDBContext.CustomValidations
{
    public class ValidEmailAttribute : ValidationAttribute, IClientModelValidator
    {
        public ValidEmailAttribute()
        {
            ErrorMessage = "Please enter a valid email address.";
        }

        public override bool IsValid(object value)
        {
            if (value == null) return true;

            string email = value?.ToString();

            string pattern = @"^(?:[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+(?:\.[A-Za-z0-9!#$%&'*+/=?^_`{|}~-]+)*)" +
                   @"@[A-Za-z0-9-]+(?:\.[A-Za-z]{2,10})+$";

            return Regex.IsMatch(email, pattern);
        }
        //adding client side validation..
        public void AddValidation(ClientModelValidationContext context)
        {
            context.Attributes["data-val"] = "true";
            context.Attributes["data-val-validemail"] = ErrorMessage;
        }
    }
}


/*@gmail.com

john.@gmail.com

john..doe@gmail.com

john@

john@-domain.com

john@gmail.c

john@gmail.abcdefghijk

john@domain..com

john@@gmail.com

john doe@gmail.com
*/




/*SUMMARY (Simple Explanation)

This regex checks for a properly structured email with:

Valid username part

Can include letters (A–Z, a–z), numbers (0–9)

Allows these special characters:
! # $ % & ' * + / = ? ^ _ { | } ~ -`

Allows dots, but not at the beginning or end, and not repeated.

Exactly one @ symbol

Valid domain name

Letters, numbers, or hyphens

Cannot start or end with hyphen

Valid domain extension

One or more extensions (like .com, .co.in, .org)

Each extension must be 2 to 10 alphabets

⭐ Examples of VALID emails (these will pass)

✔ john.doe@gmail.com
✔ abc123@company.org
✔ user_name@my-domain.in
✔ contact.support@service.co.in
✔ x@y.io
✔ dev-team@project-info.tech

❌ Examples of INVALID emails (these will fail)

✘ @gmail.com — missing username
✘ john..doe@gmail.com — double dots not allowed
✘ john@-domain.com — domain cannot start with -
✘ john@domain.c — extension too short
✘ john@domain.abcdefghijk — extension too long
✘ john.@gmail.com — username can't end with a dot
✘ john@ — missing domain

🟦 In ONE Line:

This regex ensures the email has a valid username, exactly one @, a correct domain name, and a proper domain extension.
*/