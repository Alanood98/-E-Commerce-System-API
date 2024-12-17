using E_CommerceSystem.Models;

namespace E_CommerceSystem.Services
{
    public interface IUserService
    {
         void AddUser(User newUser);
        void DeleteUser(User currentUser, int userId);
        User GetUser(string email, string password);
        //void AddNewProduct(User currentUser, Product product);
        //void DeleteProduct(User currentUser, int productId);
        //void UpdateProduct(User currentUser, Product product);
        //void AddReview(User currentUser, int productId, int rating, string comment);
        void ReturnProduct(User currentUser, int productId);
        void MakeOrder(User currentUser, Order orderDetails = null);


    }
}
