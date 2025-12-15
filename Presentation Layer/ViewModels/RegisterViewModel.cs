using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc.Rendering;
using UserPortalValdiationsDBContext.CustomValidations;
using UserPortalValdiationsDBContext.Enums;
using UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class RegisterViewModel
    {
        [Required]
        [NotAdminUsername]
        public string? Username { get; set; }

        [Required]
        [ValidEmail]
        public string? Email { get; set; }

        [Required]
        [StrongPassword]
        [NoSequentialDigits]
        public string? Password { get; set; }

        public IFormFile? ProfilePhoto { get; set; }

        [Range(1, 100)]
        [MinAge(18)]
        public int Age { get; set; }

        [PhoneNumber]
        public string? Phone { get; set; }

        [Required]
        public Departments? Department { get; set; }

        [Required]
        public string? Gender { get; set; }

        public List<string>? Hobbies { get; set; }

        [Required]
        [PastDate]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string? Country { get; set; }

        [MustBeTrue]
        public bool AcceptTerms { get; set; }

        // 🔑 ROLE (FROM DB)
        [Required(ErrorMessage = "Please select a role")]
        public int RoleId { get; set; }

        // Dropdown source
        public IEnumerable<SelectListItem>? Roles { get; set; }         //Data  Seeding..

        // 🔄 MAPPER
        public User ToUser(string? photoPath)
        {
            return new User
            {
                Username = Username!,
                Email = Email!,
                Password = Password!,

                Age = Age,
                Phone = Phone,
                Department = Department,
                Gender = Gender,
                DateOfBirth = DateOfBirth,
                Country = Country,
                Hobbies = Hobbies != null ? string.Join(",", Hobbies) : null,
                AcceptTerms = AcceptTerms,

                RoleId = RoleId,
                ProfilePhotoPath = photoPath,
                Is2FAVerified = false   // default
            };
        }
    }
}
