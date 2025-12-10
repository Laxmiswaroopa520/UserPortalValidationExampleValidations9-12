namespace UserPortalValdiationsDBContext.ViewModels
{
    public class UserProfileSidebarViewModel
    {
        public int UserId { get; set; }
        public string? Name { get; set; }
        public string? Email { get; set; }
        public string? ProfilePhotoPath { get; set; }
        public List<DateTime> RecentLogins { get; set; } = new();
        public DateTime? LastPasswordChangeAt { get; set; }
        public List<string> Roles { get; set; } = new();
        public BirthdayInfo? UpcomingBirthday { get; set; }
    }
}
