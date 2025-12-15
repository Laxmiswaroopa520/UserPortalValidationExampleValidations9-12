using UserPortalValdiationsDBContext.Enums;

namespace UserPortalValdiationsDBContext.Services
{
    public class UserCountByDepartment
    {
        // public string Department { get; set; } = string.Empty;
        // ❌ ERROR: g.Key = Departments? but model expects string because of this line only.
        public Departments? Department { get; set; }
        public int Count { get; set; }
    }
}
