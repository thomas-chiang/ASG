using ASG.Infrastructure.ApolloAttendences.DynamicDbSchemas;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlServerDbContexts;

public class DynamicDbContext: DbContext
{
    
    public DbSet<AttendanceHistory> AttendanceHistories { get; set; } = null!;
    
    public DynamicDbContext(string connectionString)
        : base(new DbContextOptionsBuilder<DynamicDbContext>()
            .UseSqlServer(connectionString)
            .Options)
    {
    }
    public DynamicDbContext(DbContextOptions options) : base(options)
    {
    }
}