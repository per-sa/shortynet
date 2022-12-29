using shortynet.Models;
using Microsoft.EntityFrameworkCore;

namespace shortynet.Data
{
    public class ShortyDbContext : DbContext
    {
        public ShortyDbContext(DbContextOptions options) : base(options)
        {
        }

        public DbSet<Shortener> Shorteners { get; set; }
    }
}