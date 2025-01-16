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

    public virtual async Task<string> GetCompanyDbConnectionString(Guid companyId)
    {
        var companyCode = await Companies
            .Where(c => c.CompanyId == companyId)
            .Select(c => c.CompanyCode)
            .FirstOrDefaultAsync();
        if (string.IsNullOrEmpty(companyCode))
            throw new InvalidOperationException($"Company with ID {companyId} not found.");

        var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                               + $"Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTube{companyCode}";
        try
        {
            using (var comConnection = new SqlConnection(connectionString))
            {
                await comConnection.OpenAsync();
            }
        }
        catch
        {
            connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                               + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTubeDB";
        }

        return connectionString;
    }
}