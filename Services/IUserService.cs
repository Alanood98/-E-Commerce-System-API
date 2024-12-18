using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IUserService
    {
         

     
        
        //string HashPassword(string password);


        //void AddUser(User newUser);

        void RegisterUser(User user);
        User GetUserById(int id);
        User Login(string email, string password);

    }
}
