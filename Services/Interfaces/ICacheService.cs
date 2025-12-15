namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface ICacheService
    {
        void Set(string key, object value, TimeSpan? expiration = null);
        object? Get(string key);
        void Remove(string key);
    }
}
