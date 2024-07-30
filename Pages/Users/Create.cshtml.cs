using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using task.Data;
using task.Models;

namespace task.Pages_Users
{
    public class CreateModel : PageModel
    {
        private readonly task.Data.taskContext _context;

        public CreateModel(task.Data.taskContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public User User { get; set; } = default!;

        public async Task<IActionResult> OnPostAsync()
        {
            if (User.DOB > DateTime.Today)
            {
                ModelState.AddModelError("User.DOB", "Date of Birth cannot be in the future.");
            }

            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.User.Add(User);
            await _context.SaveChangesAsync();

            return new JsonResult(new { success = true });
        }
    }
}
