using System;
using System.ComponentModel.DataAnnotations;
using UserPortalValdiationsDBContext.CustomValidations;

namespace UserPortalValdiationsDBContext.Models
{
    public class User
    {
        public int Id { get; set; }

        // USERNAME
        [Required]
        [NotAdminUsername(ErrorMessage = "Admin username not allowed.")]
        public string? Username { get; set; }

        // EMAIL
        [Required]
        [ValidEmail]
        public string? Email { get; set; }

        // PASSWORD
        [Required]
        [StrongPassword]
        [NoSequentialDigits]
        public string? Password { get; set; }

        // AGE
        [Range(1, 100, ErrorMessage = "Age must be between 1 and 100.")]
        [MinAge(18, ErrorMessage = "Minimum allowed age is 18.")]
        public int Age { get; set; }

        // PHONE NUMBER
        [PhoneNumber]
       [NoSequentialDigits]
     public string? Phone { get; set; }

        // GENDER
        [Required]
        public string? Gender { get; set; }  // Male, Female, Other

        // DATE OF BIRTH
        [Required]
        [PastDate(ErrorMessage = "Date of birth cannot be in the future.")]
        public DateTime DateOfBirth { get; set; }

        // COUNTRY
        [Required(ErrorMessage = "Please select a country.")]
        public string? Country { get; set; }

        // HOBBIES (comma-separated for DB storage)
        public string? Hobbies { get; set; }

        // TERMS ACCEPTANCE (not stored usually, optional)
        // Added only if you want to save it
        public bool AcceptTerms { get; set; }
    }
}
