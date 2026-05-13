using Microsoft.AspNetCore.Mvc;

namespace ztpai.Controllers
{
    public record HealthResponseDTO(string apiVersion, string status, DateTime currentTime);
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult CheckHealth()
        {
            var response = new HealthResponseDTO("v1.0", "healthy", DateTime.Now);
            return Ok(response);
        }
    }
}
