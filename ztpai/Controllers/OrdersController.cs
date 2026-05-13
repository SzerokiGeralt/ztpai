using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;
using ztpai.DTO;
using ztpai.Models;
using ztpai.Services;

namespace ztpai.Controllers
{
    [Route("api/{controller}")]
    [ApiController]
    public class OrdersController(IOrderService orderService) : ControllerBase
    {
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateOrder(CreateOrderRequestDTO request)
        {
            var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            if (string.IsNullOrWhiteSpace(userIdClaim) || !Guid.TryParse(userIdClaim, out var userId))
            {
                return Unauthorized();
            }

            var result = await orderService.CreateOrderAsync(userId, request);
            if (!result.Success)
            {
                return BadRequest(new { message = result.Error });
            }

            return Ok(result);
        }

        [Authorize(Roles = "Admin")]
        [HttpPost("update")]
        public async Task<IActionResult> UpdateOrderStatus(int orderId, OrderStatus status)
        {
            var isUpdated = await orderService.UpdateOrderStatusAsync(orderId, status);
            if (!isUpdated)
            {
                return NotFound(new { message = $"Not found order id = {orderId}" });
            }

            return NoContent();
        }
    }
}
