using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;
using System.Threading.Tasks;
using System.Collections.Generic;

namespace task.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly taskContext _context;

        public UsersController(taskContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<User>>> GetUsers()
        {
            return await _context.User.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<User>> GetUser(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            return user;
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        [HttpGet("image/{id}")]
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
            return Ok(new { base64Image = base64String });
        }
        // POST: api/Users/upload/base64
        [HttpPost("upload/base64")]
        public async Task<IActionResult> UploadBase64Image([FromBody] Base64ImageRequest request)
        {
            if (string.IsNullOrEmpty(request.Base64Image))
            {
                return BadRequest("No image data provided.");
            }

            // Decode the Base64 string
            var imageBytes = Convert.FromBase64String(request.Base64Image);

            // Generate a unique filename
            var fileName = $"{Guid.NewGuid()}.png";
            
            // Define the path to save the file
            var filePath = Path.Combine("wwwroot/images", fileName);

            // Ensure the directory exists
            if (!Directory.Exists("wwwroot/images"))
            {
                Directory.CreateDirectory("wwwroot/images");
            }

            // Save the file
            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            return Ok(new { filePath, message = "File uploaded successfully." });
        }

        public class Base64ImageRequest
        {
            public required string Base64Image { get; set; }
        }


        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromForm] User user)
        {
            if (id != user.Id)
            {
                return BadRequest();
            }

            _context.Entry(user).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }


        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.User.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool UserExists(int id)
        {
            return _context.User.Any(e => e.Id == id);
        }
    }
}
