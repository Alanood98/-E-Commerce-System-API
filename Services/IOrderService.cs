using E_CommerceSystem.Models;
using E_CommerceSystem.UserDTO;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        Order PlaceOrder(orderInput orderInput);
        IEnumerable<Order> GetAllOrdersForUser(int userId);
        Order GetOrderById(int orderId);
    }

}
