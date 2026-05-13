using Microsoft.AspNetCore.Mvc;

namespace ztpai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UtilsController : ControllerBase
    {
        [HttpGet("health")]
        public IActionResult CheckHealth()
        {
            return Ok(new 
            {
                apiVersion = 1,
                status = "active",
                currentLocalTime = DateTime.Now,
            });
        }
    }
}
