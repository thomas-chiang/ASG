using ASG.Infrastructure.ApolloAttendances.AsiaTubeDbSchemas;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlServerDbContexts;

public class AsiaTubeDbContext: DbContext
{
    
    public DbSet<AttendanceHistory> AttendanceHistories { get; set; } = null!;
    
    public DbSet<AttendanceHistoryRecord> AttendanceHistoryRecords { get; set; } = null!;
    
    public AsiaTubeDbContext(string connectionString)
        : base(new DbContextOptionsBuilder<AsiaTubeDbContext>()
            .UseSqlServer(connectionString)
            .Options)
    {
    }
    public AsiaTubeDbContext(DbContextOptions options) : base(options)
    {
    }
}