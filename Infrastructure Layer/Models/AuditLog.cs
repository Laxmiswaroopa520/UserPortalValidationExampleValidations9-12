// EF Core entity for DB persistence
namespace UserPortalValdiationsDBContext.Models
{
    public class AuditLog
    {
        public int Id { get; set; }
        public string? Action { get; set; }
        public string? EntityName { get; set; }
        public DateTime Timestamp { get; set; }
    }
}
