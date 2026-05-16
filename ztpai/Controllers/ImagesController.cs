using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ztpai.Services;

namespace ztpai.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController(IProductImageService minioService) : ControllerBase
    {
        [Authorize(Roles = "Admin")]
        [HttpPost("upload")]
        public async Task<IActionResult> Upload([FromForm] IFormFile file)
        {
            var fileName = await minioService.UploadProductImageAsync(file);
            return Ok(new { fileName });
        }

        [Authorize(Roles = "Admin")]
        [HttpDelete("{fileName}")]
        public async Task<IActionResult> Delete(string fileName)
        {
            await minioService.DeleteProductImageAsync(fileName);
            return NoContent();
        }

        [HttpGet("{fileName}")]
        public async Task<IActionResult> Get(string fileName)
        {
            var (stream, contentType) = await minioService.GetProductImageAsync(fileName);
            return File(stream, contentType);
        }
    }
}
