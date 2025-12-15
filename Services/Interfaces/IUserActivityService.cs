namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface IUserActivityService
    {
        Task<IEnumerable<DateTime>> GetRecentLoginsAsync(int userId, int count);

        Task<Dictionary<string, int>> GetUserCountByDepartmentAsync();
    }
}
