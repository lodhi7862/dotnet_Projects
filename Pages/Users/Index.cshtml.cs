using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using task.Data;
using task.Models;

namespace task.Pages_Users
{
    public class IndexModel : PageModel
    {
        private readonly task.Data.taskContext _context;

        public IndexModel(task.Data.taskContext context)
        {
            _context = context;
        }

        // public IList<User> User { get; set; } = default!;
        public IList<User> User { get; set; } = new List<User>();

        public async Task OnGetAsync()
        {
            User = await _context.User.ToListAsync();
        }

        public async Task<IActionResult> OnPostDeleteAsync(int id)
        {
            var user = await _context.User.FindAsync(id);

            if (user == null)
            {
                return NotFound();
            }

            _context.User.Remove(user);
            await _context.SaveChangesAsync();

            return RedirectToPage();
        }



    }
}
