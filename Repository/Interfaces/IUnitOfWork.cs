using System;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepository Users { get; }
        IAccountRepository Accounts { get; }
        IAddressRepository Addresses { get; }
        IContactRepository Contacts { get; }
        IUserActivityRepository UserActivities { get; }
        IRoleRepository Roles { get; }      // role based 

        int Save();
    }
}
