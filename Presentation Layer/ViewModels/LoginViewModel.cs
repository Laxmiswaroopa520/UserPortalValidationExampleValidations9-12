using System.ComponentModel.DataAnnotations;
using UserPortalValdiationsDBContext.CustomValidations;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class LoginViewModel
    {
        [Required]
        [NotAdminUsername(ErrorMessage = "Admin username not allowed.")]
        public string? Username { get; set; }

        [Required]
        [NoSequentialDigits]
        [StrongPassword]
        public string? Password { get; set; }
    }
}
