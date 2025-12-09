using System.ComponentModel.DataAnnotations;

namespace UserPortalValdiationsDBContext.ViewModels
{
    public class AddressViewModel
    {
        public int Id { get; set; }

        public int UserId { get; set; }

        [Required]
        public string? Line1 { get; set; }

        [Required]
        public string? City { get; set; }

        [Required]
        public string? State { get; set; }

        [Required]
        public string? Pin { get; set; }
    }
}
