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

            public void PlaceOrder(List<OrderItemInput> orderInputs, int userId)
            {
                var totalAmount = 0m;
                var orderProducts = new List<OrderProduct>();

                foreach (var orderInput in orderInputs)
                {
                    var product = _productRepo.GetProductById(orderInput.ProductId);
                    if (product == null || product.Stock < orderInput.Quantity)
                    {
                        throw new InvalidOperationException($"Insufficient stock for product {orderInput.ProductId}.");
                    }

                    totalAmount += product.ProductPrice * orderInput.Quantity;
                    product.Stock -= orderInput.Quantity;
                    _productRepo.UpdateProduct(product);

                    orderProducts.Add(new OrderProduct
                    {
                        ProductId = product.ProductId,
                        Quantity = orderInput.Quantity
                    });
                }

                var order = new Order
                {
                    UId = userId,
                    OrderDate = DateTime.Now,
                    TotalAmount = totalAmount,
                    OrderProducts = orderProducts
                };

                _orderRepo.PlaceOrder(order);
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
