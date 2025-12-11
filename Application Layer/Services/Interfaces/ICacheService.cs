namespace UserPortalValdiationsDBContext.Services.Interfaces
{
    public interface ICacheService
    {
        void Set(string key, object value);
        object? Get(string key);
    }
}
