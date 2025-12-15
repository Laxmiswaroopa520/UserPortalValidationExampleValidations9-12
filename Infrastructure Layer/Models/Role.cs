//Data Seeding for role based access and authorization..
namespace UserPortalValdiationsDBContext.Models
{
    public class Role
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;  // Admin, User, Manager
    }
}
