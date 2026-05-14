using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ztpai.Services;

namespace ztpai.Controllers
{
    [Route("log")]
    [ApiController]
    public class LoggingController(ILoggingService loggingService) : ControllerBase
    {
        [HttpPost]
        public IActionResult Log(string message)
        {
            loggingService.Log(message);
            return Ok();
        }
    }
}
