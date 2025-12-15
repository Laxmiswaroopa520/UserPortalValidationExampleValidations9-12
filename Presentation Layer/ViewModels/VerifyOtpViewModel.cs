using System.ComponentModel.DataAnnotations;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class VerifyOtpViewModel
    {
        [Required]
        [StringLength(6, MinimumLength = 6)]
        public string Otp { get; set; } = string.Empty;
    }
}
