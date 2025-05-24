using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace OrderService.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController : ControllerBase
    {
        [HttpPost]
        public IActionResult notify([FromBody] object orderSummary)
        {
            Console.WriteLine("Notification received: " + JsonSerializer.Serialize(orderSummary));
            return Ok();
        }

    }
}
