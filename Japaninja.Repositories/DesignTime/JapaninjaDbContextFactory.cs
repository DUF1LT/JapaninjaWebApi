using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Design;

namespace Japaninja.Repositories.DesignTime;

public class JapaninjaDesignTimeDbContextFactory : IDesignTimeDbContextFactory<JapaninjaDbContext>
{
    private const string DesignTimeConnectionString = "Server=(localdb)\\mssqllocaldb;Database=Japaninja;Trusted_Connection=True;Integrated Security=True;";

    public JapaninjaDbContext CreateDbContext(string[] args)
    {
        var optionsBuilder = new DbContextOptionsBuilder<JapaninjaDbContext>().UseSqlServer(DesignTimeConnectionString);

        return new JapaninjaDbContext(optionsBuilder.Options);
    }
}