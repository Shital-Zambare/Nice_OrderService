using Microsoft.AspNetCore.Mvc;
using OrderService.Models;
using OrderService.Services;
using StackExchange.Redis;
using System.Text.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class OrdersController : ControllerBase
    {
        public static readonly List<Models.Order> Orders = new();
        private readonly IOrderService _orderService;

        public OrdersController(IOrderService orderService)
        {
            _orderService = orderService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateOrder([FromBody] Models.Order order)
        {
            try
            {
                await _orderService.CreateOrderAsync(order);
                return CreatedAtAction(nameof(GetOrder), new { id = order.OrderId }, order);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Notification failed: {ex.Message}");
                return BadRequest("Failed to create order");
            }
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetOrder(Guid id)
        {
            try
            {
                var orderResponse = await _orderService.GetOrderByIdAsync(id);
                if(orderResponse is null)
                    return NotFound();
                return Ok(orderResponse);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error retrieving order: {ex.Message}");
                return StatusCode(500, "Internal server error");
            }
          
        }


    }

}
