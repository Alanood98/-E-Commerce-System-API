using E_CommerceSystem.Models;
using E_CommerceSystem.Repositories;
using E_CommerceSystem.UserDTO;

namespace E_CommerceSystem.Services
{
    namespace E_CommerceSystem.Services
    {
        public class OrderService : IOrderService
        {
            private readonly IOrderRepo _orderRepo;
            private readonly IProductRepo _productRepo;

            public OrderService(IOrderRepo orderRepo, IProductRepo productRepo)
            {
                _orderRepo = orderRepo;
                _productRepo = productRepo;
            }

            public Order PlaceOrder(orderInput orderInput)
            {
                var totalAmount = 0m;

                // Validate stock and calculate total amount
                foreach (var item in orderInput.OrderItems)
                {
                   
                    if (product.Stock < item.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {product.ProductName}.");
                    }

                    totalAmount += product.ProductPrice * item.Quantity;

                    // Reduce stock
                    product.Stock -= item.Quantity;
                    _productRepo.UpdateProduct(product);
                }

                // Create and save the order
                var order = new Order
                {
                    
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    OrderProducts = orderInput.OrderItems.Select(i => new OrderProduct
                    {
                        
                        Quantity = i.Quantity
                    }).ToList()
                };

                _orderRepo.PlaceOrder(order);
                return order;
            }


            public IEnumerable<Order> GetAllOrdersForUser(int userId)
            {
                return _orderRepo.GetAllOrdersForUser(userId);
            }

            public Order GetOrderById(int orderId)
            {
                return _orderRepo.GetOrderById(orderId);
            }
        }
    }


}
