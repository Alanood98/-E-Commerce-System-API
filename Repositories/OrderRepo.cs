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

        //public IEnumerable<Order> GetAllOrder()
        //{
        //    try
        //    {

        //        return _context.Orders.ToList();
        //    }
        //    catch (Exception ex)
        //    {

        //        Console.WriteLine($"Error retrieving Orders: {ex.Message}");
        //        throw;
        //    }
        //}
        public IEnumerable<Order> GetAllOrdersForUser(int userId)
        {
            return _context.Orders
                .Include(o => o.OrderProducts)
                   
                .Where(o => o.UId == userId)
                .ToList();
        }

        public Order GetOrderById(int orderId)
        {
            return _context.Orders
                .Include(o => o.OrderProducts)
                   
                .FirstOrDefault(o => o.OrderId == orderId);
        }
    }
}
