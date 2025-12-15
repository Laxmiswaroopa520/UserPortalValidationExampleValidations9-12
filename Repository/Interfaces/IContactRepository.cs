using global::UserPortalValdiationsDBContext.Models;

namespace UserPortalValdiationsDBContext.Repository.Interfaces
{
    public interface IContactRepository
    {
        List<ContactModel> GetAll();
        void Add(ContactModel contact);
    }
}

