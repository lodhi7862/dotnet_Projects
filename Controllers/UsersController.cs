using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.IO;

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

        // POST: api/Users/upload/base64
        [HttpPost("upload/base64")]
        public async Task<IActionResult> PostUserImage([FromBody] Base64ImageRequest request)
        {
            if (string.IsNullOrEmpty(request.Base64Image))
            {
                return BadRequest("No image provided.");
            }

            var user = await _context.User.FindAsync(request.UserId);
            if (user == null)
            {
                return NotFound("User not found.");
            }

            string filePath = Path.Combine("wwwroot/images", $"{request.UserId}.png");
            byte[] imageBytes = Convert.FromBase64String(request.Base64Image);

            // Ensure the directory exists
            var directory = Path.GetDirectoryName(filePath);
            if (!Directory.Exists(directory))
            {
                Directory.CreateDirectory(directory);
            }

            await System.IO.File.WriteAllBytesAsync(filePath, imageBytes);

            // Update user record with the image filename
            user.ImageFilename = $"{request.UserId}.png";
            await _context.SaveChangesAsync();

            return Ok(new { message = "Image uploaded successfully.", imagePath = $"/images/{request.UserId}.png" });
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<User>> PostUser([FromForm] User user)
        {
            _context.User.Add(user);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetUser), new { id = user.Id }, user);
        }

        // GET: api/Users/image/5
        [HttpGet("image/{id}")]
        public async Task<IActionResult> GetImage(int id)
        {
            string filePath = Path.Combine("wwwroot/images", $"{id}.png");

            if (!System.IO.File.Exists(filePath))
            {
                return NotFound();
            }

            byte[] imageBytes = await System.IO.File.ReadAllBytesAsync(filePath);
            string base64String = Convert.ToBase64String(imageBytes);
            return Ok(new { base64Image = base64String });
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

        public class Base64ImageRequest
        {
            public string Base64Image { get; set; }
            public int UserId { get; set; }
        }
    }
}
