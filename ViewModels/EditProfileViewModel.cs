using System.ComponentModel.DataAnnotations;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class EditProfileViewModel
    {
        public string Username { get; set; } = "";

        [Required, EmailAddress]
        public string Email { get; set; } = "";

        [Phone]
        public string? Phone { get; set; }

        public string? ProfilePhotoPath { get; set; }
    }
}
