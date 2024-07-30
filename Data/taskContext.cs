using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using task.Models;

namespace task.Data
{
    public class taskContext : DbContext
    {
        public taskContext (DbContextOptions<taskContext> options)
            : base(options)
        {
        }

        public DbSet<task.Models.User> User { get; set; } = default!;
    }
}
