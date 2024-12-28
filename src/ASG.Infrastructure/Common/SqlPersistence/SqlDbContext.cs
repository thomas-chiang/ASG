using ASG.Infrastructure.Gaia1001Forms.Persistence;
using ASG.Infrastructure.Gaia1001Forms.SqlSchemas;
using ASG.Infrastructure.Gaia1001Forms.Views;
using Microsoft.EntityFrameworkCore;

namespace ASG.Infrastructure.Common.SqlPersistence;

public class SqlDbContext: DbContext
{
    public DbSet<PtSyncForm> PtSyncForms { get; set; } = null!;
    
    public DbSet<PtSyncFormArchive2024> PtSyncFormsArchive2024 { get; set; } = null!;
    
    public DbSet<PtSyncFormArchive2025> PtSyncFormsArchive2025 { get; set; } = null!;
    
    public DbSet<Gaia1001Attendance> Gaia1001Attendances { get; set; }
    
    public SqlDbContext(DbContextOptions options) : base(options)
    {
    }
}