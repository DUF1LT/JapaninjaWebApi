using Microsoft.EntityFrameworkCore;

namespace Japaninja.Repositories;

public class JapaninjaDbContext : DbContext
{
    public JapaninjaDbContext(DbContextOptions<JapaninjaDbContext> options)
        : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
    }
}