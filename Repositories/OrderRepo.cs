using E_CommerceSystem.Models;
using Microsoft.EntityFrameworkCore;

namespace E_CommerceSystem.Repositories
{
    public class OrderRepo : IOrderRepo
    {
        private readonly ApplicationDbContext _context;

        public OrderRepo(ApplicationDbContext context)
        {
            _context = context;
        }

        public void PlaceOrder(Order order)
        {
            _context.Orders.Add(order);
            _context.SaveChanges();
        }

        public IEnumerable<Order> GetAllOrdersForUser(int userId)
        {
            return _context.Orders
                .Where(o => o.UId == userId)
                .Include(o => o.OrderProducts) 
                .ToList();
        }


        public Order GetOrderById(int orderId)
        {
            return _context.Orders.Find(orderId);
              
        }
    }
}
