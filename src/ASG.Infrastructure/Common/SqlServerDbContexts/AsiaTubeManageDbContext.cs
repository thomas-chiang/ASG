using ASG.Infrastructure.Common.AsiaTubeManageDbSchemas;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlServerDbContexts;

public class AsiaTubeManageDbContext : DbContext
{
    public DbSet<Company> Companies { get; set; } = null!;

    public AsiaTubeManageDbContext(DbContextOptions<AsiaTubeManageDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                                   + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTubeManageDb";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}