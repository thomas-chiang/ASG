using ASG.Infrastructure.ApolloAttendances.AsiaTubeDbSchemas;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlServerDbContexts;

public class AsiaTubeDbContext : DbContext
{
    public DbSet<AttendanceHistory> AttendanceHistories { get; set; } = null!;

    public DbSet<AttendanceHistoryRecord> AttendanceHistoryRecords { get; set; } = null!;

    public AsiaTubeDbContext(DbContextOptions<AsiaTubeDbContext> options) : base(options)
    {
    }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        if (!optionsBuilder.IsConfigured)
        {
            var connectionString = @"Server=sea-asia-tube-sqlsrv.database.windows.net;"
                                   + "Authentication=Active Directory Interactive; Encrypt=True; Database=AsiaTubeDB";

            optionsBuilder.UseSqlServer(connectionString);
        }
    }
}