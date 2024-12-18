using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IUserRepo
    {
        void AddUser(User user);
        User GetUserById(int id);
        IEnumerable<User> GetAllUsers();
        public User GetUSer(string email, string password);
        public User GetUSerByEmail(string email);




    }
}
