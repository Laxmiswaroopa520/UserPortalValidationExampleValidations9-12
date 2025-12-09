using System.ComponentModel.DataAnnotations;
using UserPortalValdiationsDBContext.CustomValidations;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class EditUserViewModel
    {
        public int Id { get; set; }

        [Required]
        [NotAdminUsername]
        public string? Username { get; set; }

        [Required]
        [EmailAddress]
        public string? Email { get; set; }

        [PhoneNumber]
        public string? Phone { get; set; }

        [MinAge(18)]
        public int Age { get; set; }
    }
}
