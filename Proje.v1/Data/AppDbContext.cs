using Microsoft.EntityFrameworkCore;
using Proje.v1.Models;

namespace Proje.v1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) 
        {
        
        }

        public DbSet<Note>Notes { get; set; }
    }
}
