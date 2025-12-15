// DTO for filter → service communication
namespace UserPortalValdiationsDBContext.DTO
{
    
        public class AuditEntry             //used for auditing filter service..
        {
            public string? UserName { get; set; }
            public string? Action { get; set; }
            public DateTime StartedAt { get; set; }
            public DateTime EndedAt { get; set; }
            public string? HttpMethod { get; set; }
            public string? Path { get; set; }
            public bool Success { get; set; }
        }
    }

