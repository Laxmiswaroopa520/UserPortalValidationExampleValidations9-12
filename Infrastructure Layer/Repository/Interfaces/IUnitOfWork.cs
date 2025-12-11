using System.Threading.Tasks;
using UserPortalValdiationsDBContext.Infrastructure_Layer.Repository.Interfaces;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IUnitOfWork
    {
        IUserRepository Users { get; }
        IAddressRepository Addresses { get; }
        IContactRepository Contacts { get; }

        int Complete();
        Task<int> CompleteAsync();
    }
}
