namespace UserPortalValdiationsDBContext.ViewModels
{
    public class BirthdayInfo
    {
        public string Name { get; set; } = string.Empty;
        public DateTime Date { get; set; }
        public int DaysRemaining { get; set; }
       // public int DaysUntil { get; set; }
    }
}
