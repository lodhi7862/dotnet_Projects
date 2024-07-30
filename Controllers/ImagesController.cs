using Microsoft.AspNetCore.Mvc;
using System.IO;
using System.Threading.Tasks;

namespace task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {
        // GET: api/Images/5
        [HttpGet("{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            // Assuming the image is stored at this path
            string filePath = Path.Combine("wwwroot/images", $"{id}.png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return Ok(base64String);
        }
    }
}
