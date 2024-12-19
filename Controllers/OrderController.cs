using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using E_CommerceSystem.Models;
using E_CommerceSystem.Services;
using E_CommerceSystem.UserDTO;
using E_CommerceSystem.Utils;

namespace E_CommerceSystem.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/orders")]
    public class OrderController : ControllerBase
    {
        private readonly IOrderService _orderService;

        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }


        [HttpPost]
        [AllowAnonymous]
        public IActionResult PlaceOrder([FromBody] List<OrderItemInput> orderInputs)
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                _orderService.PlaceOrder(orderInputs, userId);
                return Ok(new { Message = "Orders placed successfully." });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { Error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }

        [HttpGet]

        public IActionResult GetAllOrdersForUser()
        {
            try
            {
                var userId = int.Parse(User.FindFirst(System.Security.Claims.ClaimTypes.NameIdentifier)?.Value);
                var orders = _orderService.GetAllOrdersForUser(userId);
                return Ok(orders);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }



        [HttpGet("{id}")]
        [AllowAnonymous]
        public IActionResult GetOrderById(int id)
        {
            try
            {
                var order = _orderService.GetOrderById(id);
                if (order == null)
                {
                    return NotFound(new { Error = "Order not found." });
                }
                return Ok(order);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = ex.Message });
            }
        }
    }
}
