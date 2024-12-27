using ASG.Infrastructure.Gaia1001Forms.SqlSchemas;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.Persistence;

public class SqlDbContext: DbContext
{
    public DbSet<PTSyncForm> PTSyncForms { get; set; } = null!;
    
    public SqlDbContext(DbContextOptions options) : base(options)
    {
    }
}