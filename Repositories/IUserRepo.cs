using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IUserRepo
    {
        void AddUser(User user);
        User GetUSer(string email, string password);
        User UpdateUser(User user);
        User DeleteUser(User user);

    }
}
