﻿using E_CommerceSystem.Models;
using E_CommerceSystem.UserDTO;

namespace E_CommerceSystem.Services
{
    public interface IOrderService
    {
        void PlaceOrder(List<OrderItemInput> orderInputs, int userId);
        IEnumerable<Order> GetAllOrdersForUser(int userId);
        Order GetOrderById(int orderId);
    }

}
