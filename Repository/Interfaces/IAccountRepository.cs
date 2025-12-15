
    using global::UserPortalValdiationsDBContext.Models;
    using UserPortalValdiationsDBContext.Models;

    namespace UserPortalValdiationsDBContext.Repository.Interfaces
    {
        public interface IAccountRepository
        {
            bool EmailExists(string email);
            bool UsernameExists(string username);
            User? GetByCredentials(string username, string password);
            void AddUser(User user);
            void UpdateUser(User user);
            List<string> GetHobbies();
        User? GetUserWithRoleById(int userId);      //must handle Include(Role)

    }
}
