//use new custom validations

using System.ComponentModel.DataAnnotations;
using UserPortalValdiationsDBContext.CustomValidations;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [NotAdminUsername(ErrorMessage = "Admin username not allowed.")]
        public string? Username { get; set; }

        [Required]
        [ValidEmail]
        public string? Email { get; set; }

        [Required]
        [StrongPassword]
        [NoSequentialDigits]
        public string? Password { get; set; }

        // AGE MUST BE BETWEEN 1–100
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
        [MinAge(18, ErrorMessage = "Minimum allowed age is 18.")]
        [RegularExpression(@"^\d{1,3}$", ErrorMessage = "Age must be a whole number. Decimals are not allowed.")]

        public int Age { get; set; }

        [PhoneNumber]
        public string? Phone { get; set; } = "9985995256";

        // GENDER - RADIO BUTTONS
        [Required]
        public string? Gender { get; set; } // Male, Female, Other

                                                                // HOBBIES - CHECKBOXES
        public List<string>? Hobbies { get; set; }

                                                          // DATE OF BIRTH – DATE PICKER + NO FUTURE DATE
        [Required]
        [PastDate(ErrorMessage = "Date of birth cannot be in the future.")]
        public DateTime DateOfBirth { get; set; }

        // DROPDOWN LIST FOR COUNTRY
        [Required(ErrorMessage = "Please select a country.")]
        public string? Country { get; set; }

        // REQUIRED CHECKBOX
        [MustBeTrue(ErrorMessage = "You must accept terms and conditions.")]
        public bool AcceptTerms { get; set; }


        // Converts ViewModel to User entity    (add a mapper)
        public User ToUser()
        {
            return new User
            {
                Username = this.Username!,
                Email = this.Email!,
                Password = this.Password!,
                Age = this.Age,
                Phone = this.Phone,
                Gender = this.Gender,
                DateOfBirth = this.DateOfBirth,
                Country = this.Country,
                Hobbies = this.Hobbies != null ? string.Join(",", this.Hobbies) : string.Empty,
                AcceptTerms = this.AcceptTerms
            };
        }
    }
}
    
