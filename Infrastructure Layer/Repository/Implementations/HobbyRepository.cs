using UserPortalValdiationsDBContext.Data;
using UserPortalValdiationsDBContext.Models;
using UserPortalValdiationsDBContext.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Implementations
{
    public class HobbyRepository : GenericRepository<Hobby>, IHobbyRepository
    {
        public HobbyRepository(ApplicationDbContext db) : base(db) { }
    }
}
