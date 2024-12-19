using E_CommerceSystem.Models;

namespace E_CommerceSystem.Repositories
{
    public interface IOrderRepo
    {
        void PlaceOrder(Order order);
        //IEnumerable<Order> GetAllOrder();
        IEnumerable<Order> GetAllOrdersForUser(int userId);
        Order GetOrderById(int orderId);
    }
}
