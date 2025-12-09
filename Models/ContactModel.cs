using UserPortalValdiationsDBContext.CustomValidations;

namespace UserPortalValdiationsDBContext.Models
{
    public class ContactModel
    {
        public int Id { get; set; }
        public string? Message { get; set; }
        [ValidEmail]
        public string? Email { get; set; }
    }
}
