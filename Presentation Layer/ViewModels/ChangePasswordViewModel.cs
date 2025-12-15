//used for changing password in the side bar profile.
    using System.ComponentModel.DataAnnotations;

    namespace UserPortalValdiationsDBContext.ViewModels
    {
        public class ChangePasswordViewModel
        {
            [Required]
            public string CurrentPassword { get; set; } = "";

            [Required, MinLength(6)]
            public string NewPassword { get; set; } = "";

            [Compare("NewPassword")]
            public string ConfirmPassword { get; set; } = "";
        }
    }

