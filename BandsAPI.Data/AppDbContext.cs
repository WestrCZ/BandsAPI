using Microsoft.EntityFrameworkCore;
using BandsAPI.Data.Entities;
namespace BandsAPI.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions options) : base(options)
    {
        
    }
    public DbSet<Author> Authors { get; set; }
    public DbSet<Song> Songs { get; set; }
}
