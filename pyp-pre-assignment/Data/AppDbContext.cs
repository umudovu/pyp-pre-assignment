using Microsoft.EntityFrameworkCore;
using pyp_pre_assignment.Entities;

namespace pyp_pre_assignment.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Commerce>? Commerces { get; set; }
    }
}
