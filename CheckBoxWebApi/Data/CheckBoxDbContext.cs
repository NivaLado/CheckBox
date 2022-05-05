using CheckBoxWebApi.Models;
using Microsoft.EntityFrameworkCore;

namespace CheckBoxWebApi.Data
{
    public class CheckBoxDbContext : DbContext
    {
        public CheckBoxDbContext(DbContextOptions<CheckBoxDbContext> options)
            : base(options)
        {

        }

        public DbSet<AppUser> AppUsers { get; set; }

        public DbSet<Album> Albums { get; set; }

        public DbSet<Check> Checks { get; set; }
    }
}
